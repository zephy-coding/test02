using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using NReco.VideoConverter;

namespace FFmpegPractice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            byte[] buff = new byte[] { 11, 12};
            MemoryStream ms = new MemoryStream();
            ms.Write(buff, 0, 2);
            this.Text = ms.Length.ToString();
        }
    }
}
