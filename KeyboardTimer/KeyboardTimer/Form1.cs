using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;


namespace KeyboardTimer
{
    public partial class Form1 : Form
    {
        IKeyboardMouseEvents KME;
        
        bool locker = true;
        DateTime Dstart, Dstop;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            KME = Hook.GlobalEvents();
            KME.KeyDown += On_KeyDown;
            KME.KeyUp += On_KeyUp;
        }
        private void On_KeyDown(object sender, KeyEventArgs e)
        {
            if (locker)
                Dstart = DateTime.Now;
            locker = false;
        }
        private void On_KeyUp(object sender, KeyEventArgs e)
        {
            Dstop = DateTime.Now;
            TimeSpan Sub = Dstop - Dstart;
            string k = "[" + e.KeyCode.ToString() + "]";
            time_Label.Text = k + Sub.ToString();
            locker = true;
        }

    }
}
