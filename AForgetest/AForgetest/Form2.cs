﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using AForgetest;

namespace AForgetest
{
    public partial class Form2 : Form
    {
        Form1 MainForm = null;
        Thread tcpConnectThread = null;
        private void btnAsk_Click(object sender, EventArgs e)
        {
            try
            {
                if (info.remoteEP == null)
                    info.remoteEP = new IPEndPoint(IPAddress.Parse(iptextBox.Text), info.recvPORT);
                else
                    info.remoteEP.Address = IPAddress.Parse(iptextBox.Text);
                tcpConnectThread = new Thread(new ThreadStart(tcpConnect));
                tcpConnectThread.Start();
            }
            catch(FormatException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        void tcpConnect()
        {
            TcpClient respSend_socket = new TcpClient(new IPEndPoint(IPAddress.Parse(MainForm.GetLocalIPAddress()), 0));
            respSend_socket.Connect(IPAddress.Parse(iptextBox.Text), info.respPORT);
            this.MainForm.btnConnect.BackColor = Color.LightPink;
            this.MainForm.btnConnect.Text = "연결됨";
            respSend_socket.Close();
            Close();
        }

        public Form2()
        {
            InitializeComponent();
        }
        public Form2(Form1 f1)
        {
            MainForm = f1;
            InitializeComponent();
        }
    }
    public class info
    {
        public static readonly int respPORT = 6788;
        public static readonly int recvPORT = 6789;
        public static IPEndPoint remoteEP;
        public static UdpClient sending_Socket = new UdpClient();
        public static TcpClient respSend_Socket;
    }
}
