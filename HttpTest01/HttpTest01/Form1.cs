using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing;

namespace HttpTest01
{
    public partial class Form1 : Form
    {
        Socket sock;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Graphics a = this.CreateGraphics();
            a.DrawEllipse(Pens.Red, 10, 10, 30, 30);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            IPAddress ipAddr = Dns.GetHostEntry(addressBox.Text).AddressList[0];
            EndPoint serverEP = new IPEndPoint(ipAddr, 80);
            sock.Connect(serverEP);
            String request = "GET / HTTP/1.0\r\nHost: " + addressBox.Text + "\r\n\r\n";
            byte[] sbuffer = Encoding.UTF8.GetBytes(request);
            sock.Send(sbuffer);
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                byte[] rbuffer = new byte[4096];
                int nRecv = sock.Receive(rbuffer);
                if(nRecv == 0)
                {
                    break;
                }
                ms.Write(rbuffer, 0, nRecv);
            }
            sock.Close();
            string response = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            contentsBox.Text = response;
            File.WriteAllText("page.html", response);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
