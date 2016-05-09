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

namespace AForgetest
{
    public partial class Form2 : Form
    {
        private void btnAsk_Click(object sender, EventArgs e)
        {
            try
            {
                info.remoteEP = new IPEndPoint(IPAddress.Parse(iptextBox.Text), info.recvPORT);
                Close();
            }
            catch(FormatException ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        public Form2()
        {
            InitializeComponent();
        }
    }
    public class info
    {
        public static readonly int repoPORT = 6788;
        public static readonly int recvPORT = 6789;
        public static IPEndPoint remoteEP;
        public static UdpClient local_Socket = new UdpClient();
    }
}
