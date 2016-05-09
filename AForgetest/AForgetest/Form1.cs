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
using AForgetest;

namespace AForgetest
{
    public partial class Form1 : Form
    {
        const int form_width = 480;
        const int form_height = 440;
        FilterInfoCollection videoDevices;
        VideoCaptureDevice webcamSource;
        ToolTip toolTip1 = new ToolTip();
        BinaryFormatter binaryFM = new BinaryFormatter();
        Thread recvThread;
        UdpClient localSocket = new UdpClient(info.recvPORT);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            localipLabel.Text += GetLocalIPAddress();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo fi in videoDevices)
            {
                camListBox.Items.Add(fi.Name);
            }
            if(videoDevices != null)
            {
                camListBox.SelectedIndex = 0;
                webcamSource = new VideoCaptureDevice(videoDevices[camListBox.SelectedIndex].MonikerString);
                foreach (var capability in webcamSource.VideoCapabilities)
                {
                    modeListBox.Items.Add(capability.FrameSize.ToString() + ":" + capability.MaximumFrameRate.ToString() + ":" + capability.BitCount.ToString());
                }
                modeListBox.SelectedIndex = 0;
                webcamSource.NewFrame += new NewFrameEventHandler(video_NewFrame);


                //mode콤보 툴팁만들기
                modeListBox.DrawMode = DrawMode.OwnerDrawFixed;
                modeListBox.DrawItem += modeListBox_DrawItem;
                modeListBox.DropDownClosed += modeListBox_DropDownClosed;
            }
            
            recvThread = new Thread(new ThreadStart(recvPacket));
            recvThread.Start();

        }
        void recvPacket()
        {
            IPEndPoint remoteEPrecv = null;
            while (true)
            {
                byte[] buff = localSocket.Receive(ref remoteEPrecv);
                MemoryStream ms = new MemoryStream();
                ms.Write(buff, 0, buff.Length);
                ms.Position = 0;
                Image pic = (Image)binaryFM.Deserialize(ms);
                pictureBox.Image = pic;
            }
            
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
            cnt += info.local_Socket.Send(buff, buff.Length, info.remoteEP);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            info.local_Socket.Close();
            localSocket.Close();
            recvThread.Interrupt();
            webcamSource.Stop();
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
                return;
            }
            webcamSource.VideoResolution = webcamSource.VideoCapabilities[modeListBox.SelectedIndex];
            
            if((webcamSource.VideoResolution.FrameSize.Width + 40) > form_width)
            {
                this.Width = webcamSource.VideoResolution.FrameSize.Width + 40;
                this.Height = webcamSource.VideoResolution.FrameSize.Height + 180;
                this.pictureBox.Width = webcamSource.VideoResolution.FrameSize.Width;
                this.pictureBox.Height = webcamSource.VideoResolution.FrameSize.Height;
                this.pictureBox.Left = (this.Width - this.pictureBox.Width - 16) / 2;
            }
            webcamSource.Start();
            isRunning = true;
            btnStart.Text = "Stop";
        }

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null)
                return;
            MemoryStream ms = new MemoryStream();
            pictureBox.Image.Save(ms, ImageFormat.Jpeg);
            this.Text = ms.Length.ToString();
            
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
            Form2 connect_Form = new Form2();
            connect_Form.Show();
        }
    }
}
