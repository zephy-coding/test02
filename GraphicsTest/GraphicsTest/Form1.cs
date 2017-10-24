using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GraphicsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            Graphics a = e.Graphics;
            a.FillEllipse(Brushes.Red, 5, 5, kkk, kkk);
        }
        int kkk = 10;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics a = e.Graphics;
            a.FillEllipse(Brushes.Red, 60, 60, 45, 45);
            a.FillRectangle(Brushes.Tomato, 130, 130, 30, 20);
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.HideSelection = false;
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                // Copy the text from TextBox1 to RichTextBox1, add a CRLF after 
                // the copied text, and keep the caret in view.
                richTextBox.SelectedText = textBox1.Text + "\r\n";
                //richTextBox.ScrollToCaret();
            }
        }
    }
}
