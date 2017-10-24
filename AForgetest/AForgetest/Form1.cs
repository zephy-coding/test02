using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;
using NAudio.Wave;

namespace AForgetest
{
    public partial class Form1 : Form
    {
        delegate void SetVoidCallback();
        double sendedPackets = 0;
        double recvedPackets = 0;
        const int form_width = 480;
        const int form_height = 440;
        FilterInfoCollection videoDevices;
        VideoCaptureDevice webcamSource;
        ToolTip toolTip1 = new ToolTip();
        BinaryFormatter binaryFM = new BinaryFormatter();
        Thread recvThread;
        Thread tcpThread;
        UdpClient localSocket = new UdpClient(info.recvPORT);
        UdpClient voiceLocalSocket = new UdpClient(info.recvVoicePORT);
        TcpListener tcpL = new TcpListener(new IPEndPoint(0, info.respPORT));

        WaveIn waveIn = null;
        BufferedWaveProvider provider = null;
        WaveOut waveOut = null;
        INetworkChatCodec selectedCodec, playCodec;
        [ImportMany(typeof(INetworkChatCodec))]
        public IEnumerable<INetworkChatCodec> Codecs { get; set; }

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            localipLabel.Text += GetLocalIPAddress();
            createCamComboboxes();
            createMicComboboxes();
            recvThread = new Thread(new ThreadStart(recvPacket));
            recvThread.Start();
            tcpThread = new Thread(new ThreadStart(tcpListen));
            tcpThread.Start();
            ThreadPool.QueueUserWorkItem(voiceReceive);
            S_timer.Start();
        }

        private void voiceReceive(object state)
        {
            IPEndPoint remoteEPrecv = null;
            while (true)
            {
                byte[] b = voiceLocalSocket.Receive(ref remoteEPrecv);
                if ((b.Length == 1) && (b[0] == 127))
                {
                    byte[] b2 = voiceLocalSocket.Receive(ref remoteEPrecv);
                    playCodec = ((CodecComboItem)(soundCodecBox.Items[(int)b2[0]])).Codec;
                    waveOut = new WaveOut();
                    provider = new BufferedWaveProvider(playCodec.RecordFormat);
                    waveOut.Init(provider);
                    waveOut.Play();
                }
                byte[] decoded = playCodec.Decode(b, 0, b.Length);
                provider.AddSamples(decoded, 0, decoded.Length);
            }
        }

        void createMicComboboxes()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(INetworkChatCodec).Assembly));
            CompositionContainer _container = new CompositionContainer(catalog);
            _container.SatisfyImportsOnce(this);

            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var tmp = WaveIn.GetCapabilities(i);
                micListBox.Items.Add(tmp.ProductName);
            }
            if (micListBox.Items.Count > 0)
            {
                micListBox.SelectedIndex = 0;
            }
            PopulateCodecsCombo(Codecs);
        }
        private void PopulateCodecsCombo(IEnumerable<INetworkChatCodec> codecs)
        {
            var sorted = from codec in codecs
                         where codec.IsAvailable
                         orderby codec.BitsPerSecond ascending
                         select codec;

            foreach (var codec in codecs)
            {
                string bitRate = codec.BitsPerSecond == -1 ? "VBR" : String.Format("{0:0.#}kbps", codec.BitsPerSecond / 1000.0);
                string text = String.Format("{0} ({1})", codec.Name, bitRate);
                soundCodecBox.Items.Add(new CodecComboItem { Text = text, Codec = codec });
            }
            soundCodecBox.SelectedIndex = 0;
        }
        
        class CodecComboItem
        {
            public string Text { get; set; }
            public INetworkChatCodec Codec { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }
        void createCamComboboxes()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in videoDevices)
            {
                camListBox.Items.Add(fi.Name);
            }
            if (videoDevices.Count > 0)
            {
                camListBox.SelectedIndex = 0;
                webcamSource = new VideoCaptureDevice(videoDevices[camListBox.SelectedIndex].MonikerString);
                webcamSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                foreach (var capability in webcamSource.VideoCapabilities)
                {
                    modeListBox.Items.Add(capability.FrameSize.ToString() + ":" + capability.MaximumFrameRate.ToString() + ":" + capability.BitCount.ToString());
                }
                modeListBox.SelectedIndex = 0;
                
                //mode콤보 툴팁만들기
                //modeListBox.DrawMode = DrawMode.OwnerDrawFixed;
                //modeListBox.DrawItem += modeListBox_DrawItem;
                //modeListBox.DropDownClosed += modeListBox_DropDownClosed;
            }
        }
        void tcpListen()
        {
            tcpL.Start();
            tcpL.AcceptTcpClient();
            MessageBox.Show("클라이언트 접속예정");
        }
        void recvPacket()
        {
            IPEndPoint remoteEPrecv = null;
            while (true)
            {
                byte[] buff = localSocket.Receive(ref remoteEPrecv);
                if((buff.Length == 1) && (buff[0] == 127))
                {
                    pictureBox.Image = null;
                    this.Width = form_width;
                    this.Height = form_height;
                    this.pictureBox.Width = 417;
                    this.pictureBox.Height = 250;
                    this.pictureBox.Left = 24;
                    changer_trigger = false;
                    codecInfoNeed = true;
                }
                else
                {
                    recvedPackets += (double)buff.Length;
                    MemoryStream ms = new MemoryStream();
                    ms.Write(buff, 0, buff.Length);
                    ms.Position = 0;
                    Image pic = (Image)binaryFM.Deserialize(ms);
                    pictureBox.Image = pic;
                    if (changer_trigger == false)
                    {
                        formSize_changer();
                    }
                }
            }
        }
        void stopMsgSend()
        {
            byte[] buff = new byte[1];
            buff[0] = 127;
            info.sending_Socket.Send(buff, buff.Length, info.remoteEP);
        }
        bool changer_trigger = false;
        void formSize_changer()
        {
            if (pictureBox.InvokeRequired)
            {
                SetVoidCallback d = new SetVoidCallback(formSize_changer);
                Invoke(d);
            }
            else
            {
                if ((pictureBox.Image.Width + 40) > form_width)
                {

                    this.Width = pictureBox.Image.Width + 40;
                    this.Height = pictureBox.Image.Height + 180;
                    this.pictureBox.Width = pictureBox.Image.Width;
                    this.pictureBox.Height = pictureBox.Image.Height;
                    this.pictureBox.Left = (this.Width - this.pictureBox.Width - 16) / 2;
                }
            }
            changer_trigger = true;
        }
        
        void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //pictureBox.Image = (Image)eventArgs.Frame.Clone();
            MemoryStream ms = new MemoryStream();
            eventArgs.Frame.Save(ms, ImageFormat.Jpeg);
            Image img = Image.FromStream(ms);
            ms = new MemoryStream();
            binaryFM.Serialize(ms, img);
            ms.Position = 0;
            byte[] buff = new byte[ms.Length];
            ms.Read(buff, 0, buff.Length);
            sendedPackets += (double)buff.Length;
            info.sending_Socket.Send(buff, buff.Length, info.remoteEP);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(webcamSource != null)
                webcamSource.Stop();
            localSocket.Close();
            recvThread.Abort();
            tcpL.Stop();
            tcpThread.Abort();
            info.sending_Socket.Close();
        }

        private void modeListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) { return; } // added this line thanks to Andrew's comment
            string text = modeListBox.GetItemText(modeListBox.Items[e.Index]);
            e.DrawBackground();
            using (SolidBrush br = new SolidBrush(e.ForeColor))
            { e.Graphics.DrawString(text, e.Font, br, e.Bounds); }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { toolTip1.Show(text, modeListBox, e.Bounds.Right, e.Bounds.Bottom); }
            e.DrawFocusRectangle();
        }
        private void modeListBox_DropDownClosed(object sender, EventArgs e)
        {
            toolTip1.Hide(modeListBox);
        }
        bool isRunning = false;
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                webcamSource.Stop();
                pictureBox.Image = null;
                this.Width = form_width;
                this.Height = form_height;
                pictureBox.Width = 417;
                pictureBox.Height = 250;
                isRunning = false;
                btnStart.Text = "Start";
                changer_trigger = false;
                stopMsgSend();

                waveIn.DataAvailable -= waveSource_DataAvailable;
                waveIn.StopRecording();
                waveIn = null;
                provider = null;
                return;
            }
            webcamSource.VideoResolution = webcamSource.VideoCapabilities[modeListBox.SelectedIndex];
            webcamSource.Start();

            waveIn = new WaveIn();
            waveIn.DeviceNumber = micListBox.SelectedIndex;
            selectedCodec = ((CodecComboItem)soundCodecBox.SelectedItem).Codec;
            waveIn.WaveFormat = selectedCodec.RecordFormat;
            waveIn.BufferMilliseconds = 50;
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveIn.StartRecording();

            isRunning = true;
            btnStart.Text = "Stop";
        }
        bool codecInfoNeed = true;
        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (codecInfoNeed)
            {
                byte[] buff2 = new byte[1];
                buff2[0] = 127;
                info.voiceSend_Socket.Send(buff2, buff2.Length, info.remoteVoiceEP);

                byte[] buff = new byte[1];
                buff[0] = (byte)soundCodecBox.SelectedIndex;
                info.voiceSend_Socket.Send(buff, buff.Length, info.remoteVoiceEP);
                codecInfoNeed = false;
            }

            byte[] encoded = selectedCodec.Encode(e.Buffer, 0, e.BytesRecorded);
            info.voiceSend_Socket.Send(encoded, encoded.Length, info.remoteVoiceEP);
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            Form2 connect_Form = new Form2(this);
            connect_Form.ShowDialog();
        }
        
        private void S_timer_Tick(object sender, EventArgs e)
        {
            speedLabel.Text = "Sending Speed : " + Math.Round((sendedPackets / 1024.0), 2).ToString() + " kbyte/s ";
            speedLabel.Text += "Receiving Speed : " + Math.Round((recvedPackets / 1024.0), 2).ToString() + " kbyte/s";
            sendedPackets = 0;
            recvedPackets = 0;
        }
    }
}