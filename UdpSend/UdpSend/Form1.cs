using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace UdpSend
{
    public partial class Form1 : Form
    {
        const int Chunk_Size = 1024;
        const int sendPort = 6688;
        const int recvPort = 6699;
        byte[] buff;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap rawFile = new Bitmap("IMG_2939.jpg");
            BinaryFormatter BF = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            BF.Serialize(ms, rawFile);
            ms.Position = 0;
            buff = new byte[ms.Length];
            ms.Read(buff, 0, buff.Length);
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
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint desEP = new IPEndPoint(IPAddress.Parse(msgBox.Text), recvPort);
                UdpClient mySocket = new UdpClient();
                //UdpClient desSocket = new UdpClient(desEP);
                int remain_size = buff.Length;
                int src_index = 0;
                int cnt = 0;
                int sendbyte = 0;
                while (0 < remain_size)
                {
                    cnt++;
                    if(remain_size > Chunk_Size)
                    {
                        byte[] sd = new byte[Chunk_Size];
                        Array.Copy(buff, src_index, sd, 0, Chunk_Size);
                        sendbyte += mySocket.Send(sd, sd.Length, desEP);
                        remain_size -= Chunk_Size;
                        src_index += Chunk_Size;
                    }
                    else
                    {
                        byte[] sd = new byte[remain_size];
                        Array.Copy(buff, src_index, sd, 0, remain_size);
                        sendbyte += mySocket.Send(sd, sd.Length, desEP);
                        remain_size -= Chunk_Size;
                        src_index += remain_size;
                    }
                }
                //msgBox.Text = sendbyte.ToString();
                mySocket.Close();
            }
            catch (FormatException ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

    }
}
