using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenShot_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {   
            InitializeComponent();
        }
        int multi_shot_counter = 0;
        Rectangle bounds;
        List<Bitmap> bmpList;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;
            bmpList = new List<Bitmap>();
            btn_multi.PreviewKeyDown += new PreviewKeyDownEventHandler(control_PreviewKeyDown);
            btn_Save.PreviewKeyDown += new PreviewKeyDownEventHandler(control_PreviewKeyDown);
            KeyPreview = true;
            KeyDown += new KeyEventHandler(keh);
        }
        void keh(object sender, KeyEventArgs e)
        {
            if(e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        this.Location = new Point(Location.X, Location.Y + 10);
                        break;
                    case Keys.Up:
                        this.Location = new Point(Location.X, Location.Y - 10);
                        break;
                    case Keys.Right:
                        this.Location = new Point(Location.X + 10, Location.Y);
                        break;
                    case Keys.Left:
                        this.Location = new Point(Location.X - 10, Location.Y);
                        break;
                }
                return;
            }
            switch (e.KeyData)
            {
                case Keys.Down:
                    this.Location = new Point(Location.X, Location.Y + 1);
                    break;
                case Keys.Up:
                    this.Location = new Point(Location.X, Location.Y - 1);
                    break;
                case Keys.Right:
                    this.Location = new Point(Location.X + 1, Location.Y);
                    break;
                case Keys.Left:
                    this.Location = new Point(Location.X - 1, Location.Y);
                    break;
            }

        }
        void control_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if(multi_shot_counter == 0)
            {
                bounds = this.Bounds;
                bounds.X += 8;
                bounds.Width -= 16;
                bounds.Y += 61;
                bounds.Height -= 69;

                Bitmap single_btm = new Bitmap(bounds.Width, bounds.Height);
                Graphics single_g = Graphics.FromImage(single_btm);
                single_g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Png Image|*.png|JPeg Image|*.jpg|Gif Image|*.gif|Bitmap Image|*.bmp";
                sfd.ShowDialog();

                if (sfd.FileName != "")
                {
                    FileStream fs = (FileStream)sfd.OpenFile();
                    switch (sfd.FilterIndex)
                    {
                        case 1:
                            single_btm.Save(fs, ImageFormat.Png);
                            break;

                        case 2:
                            single_btm.Save(fs, ImageFormat.Jpeg);
                            break;

                        case 3:
                            single_btm.Save(fs, ImageFormat.Gif);
                            break;

                        case 4:
                            single_btm.Save(fs, ImageFormat.Bmp);
                            break;
                    }
                    fs.Close();
                }
            }
            else
            {
                int bmpHeight = 0;
                int bmpWidth = 0;
                foreach(Bitmap b in bmpList)
                {
                    bmpHeight += b.Height;
                    if (b.Width > bmpWidth)
                        bmpWidth = b.Width;
                }
                Bitmap multi_btm = new Bitmap(bmpWidth, bmpHeight);
                Graphics multi_g = Graphics.FromImage(multi_btm);
                int hh = 0;
                foreach(Bitmap b in bmpList)
                {
                    multi_g.DrawImage(b, 0, hh);
                    hh += b.Height;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Png Image|*.png|JPeg Image|*.jpg|Gif Image|*.gif|Bitmap Image|*.bmp";
                sfd.ShowDialog();

                if (sfd.FileName != "")
                {
                    FileStream fs = (FileStream)sfd.OpenFile();
                    switch (sfd.FilterIndex)
                    {
                        case 1:
                            multi_btm.Save(fs, ImageFormat.Png);
                            break;

                        case 2:
                            multi_btm.Save(fs, ImageFormat.Jpeg);
                            break;

                        case 3:
                            multi_btm.Save(fs, ImageFormat.Gif);
                            break;

                        case 4:
                            multi_btm.Save(fs, ImageFormat.Bmp);
                            break;
                    }
                    fs.Close();
                }
                btn_multi.Text = "누적";
                multi_shot_counter = 0;
                bmpList.Clear();
            }
        }
        private void btn_multi_Click(object sender, EventArgs e)
        {
            bounds = this.Bounds;
            bounds.X += 8;
            bounds.Width -= 16;
            bounds.Y += 61;
            bounds.Height -= 69;

            Bitmap single_btm = new Bitmap(bounds.Width, bounds.Height);
            Graphics single_g = Graphics.FromImage(single_btm);
            single_g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            bmpList.Add(single_btm);
            multi_shot_counter++;
            btn_multi.Text = multi_shot_counter.ToString();
        }
    }
}
