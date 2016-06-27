using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;


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
        TcpListener tcpL = new TcpListener(new IPEndPoint(0, info.respPORT));

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            localipLabel.Text += GetLocalIPAddress();
            createCamComboboxes();
            webcamSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            recvThread = new Thread(new ThreadStart(recvPacket));
            recvThread.Start();
            tcpThread = new Thread(new ThreadStart(tcpListen));
            tcpThread.Start();
            S_timer.Start();
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
        int cnt = 0;
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
            cnt += info.sending_Socket.Send(buff, buff.Length, info.remoteEP);
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
                return;
            }
            webcamSource.VideoResolution = webcamSource.VideoCapabilities[modeListBox.SelectedIndex];
            webcamSource.Start();
            isRunning = true;
            btnStart.Text = "Stop";
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
        public bool isForm2_opened = false;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if(isForm2_opened == false)
            { 
                Form2 connect_Form = new Form2(this);
                connect_Form.Show();
                isForm2_opened = true;
            }
        }
        
        private void S_timer_Tick(object sender, EventArgs e)
        {
            speedLabel.Text = "Sending Speed : " + Math.Round((sendedPackets / 1024.0),2).ToString() + " kbyte/s ";
            speedLabel.Text += "Receiving Speed : " + Math.Round((recvedPackets / 1024.0), 2).ToString() + " kbyte/s";
            sendedPackets = 0;
            recvedPackets = 0;
        }
    }
}