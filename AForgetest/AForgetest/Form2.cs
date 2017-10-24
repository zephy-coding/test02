using System;
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
        private void btnAsk_Click(object sender, EventArgs e)
        {
            try
            {
                if (info.remoteEP == null)
                    info.remoteEP = new IPEndPoint(IPAddress.Parse(iptextBox.Text), info.recvPORT);
                else
                    info.remoteEP.Address = IPAddress.Parse(iptextBox.Text);
                if (info.remoteVoiceEP == null)
                    info.remoteVoiceEP = new IPEndPoint(IPAddress.Parse(iptextBox.Text), info.recvVoicePORT);
                else
                    info.remoteVoiceEP.Address = IPAddress.Parse(iptextBox.Text);
                
                TcpClient respSend_socket = new TcpClient(new IPEndPoint(IPAddress.Parse(MainForm.GetLocalIPAddress()), 0));
                
                if (respSend_socket.ConnectAsync(IPAddress.Parse(iptextBox.Text), info.respPORT).Wait(2000))
                {
                    this.MainForm.btnConnect.BackColor = Color.LightPink;
                    this.MainForm.btnConnect.Text = "연결됨";
                    respSend_socket.Close();
                    Close();
                }
                respSend_socket.Close();
            }
            
            catch(AggregateException ff)
            {
                MessageBox.Show(ff.Message);
            }
            catch (FormatException ee)
            {
                MessageBox.Show(ee.Message);
            }
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

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            

        }
    }
    public class info
    {
        public static readonly int respPORT = 6788;
        public static readonly int recvPORT = 6789;
        public static readonly int recvVoicePORT = 6790;
        public static IPEndPoint remoteEP;
        public static IPEndPoint remoteVoiceEP;
        public static UdpClient sending_Socket = new UdpClient();
        public static UdpClient voiceSend_Socket = new UdpClient();
        public static TcpClient respSend_Socket;
    }
}
