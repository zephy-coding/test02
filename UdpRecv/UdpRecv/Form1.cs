using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace UdpRecv
{
    public partial class Form1 : Form
    {
        const int sendPort = 6688;
        const int recvPort = 6699;
        Thread rcvThread;

        public Form1()
        {
            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void recvPacket()
        {
            IPEndPoint remoteEP = null;
            UdpClient remoteSocket = new UdpClient(recvPort);
            int tbIndex = 0;
            byte[] totalBuff = null;
            
            while (true)
            {
                byte[] buff = remoteSocket.Receive(ref remoteEP);
                if (totalBuff == null)
                {
                    totalBuff = new byte[buff.Length];
                }
                else
                {
                    Array.Resize<byte>(ref totalBuff, tbIndex + buff.Length);
                }
                Array.Copy(buff, 0, totalBuff, tbIndex, buff.Length);
                if (buff.Length != 1024)
                    break;
                tbIndex += 1024;

            }
            MemoryStream ms = new MemoryStream();
            ms.Write(totalBuff, 0, totalBuff.Length);
            ms.Position = 0;
            BinaryFormatter BF = new BinaryFormatter();
            Bitmap pic = (Bitmap)BF.Deserialize(ms);
            remoteSocket.Close();
            pictureBox1.Image = pic;
            ms.Close();
        }
        private void btnRcv_Click(object sender, EventArgs e)
        {
            rcvThread = new Thread(new ThreadStart(recvPacket));
            rcvThread.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            rcvThread.Join();
        }
    }
}
