using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omok
{
    public partial class Form1 : Form
    {
        List<double> coord = new List<double>();
        bool isBlackturn = true;
        Stack<PictureBox> stone_Stack = new Stack<PictureBox>();
        Image whiteStone = Image.FromFile("resourse\\white-go-stone32.png");
        Image blackStone = Image.FromFile("resourse\\black-go-stone32.png");
        Image whiteTurn = Image.FromFile("resourse\\white-go-stone48.png");
        Image blackTurn = Image.FromFile("resourse\\black-go-stone48.png");
        Cursor white_cursor, black_cursor;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int k = 0; k <19; k++)
            {
                coord.Add(23.0 + ((755.0 - 23.0) / 18.0)*(double)k);
            }
            turn_pic.Image = blackTurn;
            Bitmap blackc = (Bitmap)blackStone.Clone();
            

            black_cursor = new Cursor(((Bitmap)blackStone.Clone()).GetHicon());
            
            white_cursor = new Cursor(((Bitmap)whiteStone).GetHicon());
            baduk_Pane.MouseMove += new MouseEventHandler(mouseCoordinate);
            baduk_Pane.MouseClick += new MouseEventHandler(stoneDrop);
            baduk_Pane.Cursor = black_cursor;
        }
        void stoneDrop(object sender, MouseEventArgs ee)
        {
            draw_Stone(ee.Location);
        }
        void mouseCoordinate(object sender, MouseEventArgs ee)
        {
            //this.Text = ee.Location.ToString();

        }
        void draw_Stone(Point coodinate)
        {
            List<double> calcX = new List<double>();
            List<double> calcY = new List<double>();
            foreach (double d in coord)
            {
                calcX.Add(Math.Abs(d - coodinate.X));
                calcY.Add(Math.Abs(d - coodinate.Y));
            }
            
            PictureBox pb = new PictureBox();
            pb.Location = new Point((int)coord[calcX.IndexOf(calcX.Min())] - 16, (int)coord[calcY.IndexOf(calcY.Min())] - 16);
            
            if (isBlackturn)
            {
                pb.Image = blackStone;
                turn_pic.Image = whiteTurn;
                baduk_Pane.Cursor = white_cursor;
            }
            else
            {
                pb.Image = whiteStone;
                turn_pic.Image = blackTurn;
                baduk_Pane.Cursor = black_cursor;
            }
            pb.BackColor = Color.Transparent;
            pb.Size = new Size(whiteStone.Width, whiteStone.Height);
            this.Controls.Add(pb);
            baduk_Pane.SendToBack();
            pb.Parent = baduk_Pane;
            stone_Stack.Push(pb);
            isBlackturn = !isBlackturn;
        }

        private void btn_Undo_Click(object sender, EventArgs e)
        {
            try
            {
                if (stone_Stack.Count > 0)
                {
                    PictureBox pb = stone_Stack.Pop();
                    pb.Dispose();
                    isBlackturn = !isBlackturn;
                    if (isBlackturn)
                    {
                        baduk_Pane.Cursor = black_cursor;
                        turn_pic.Image = blackTurn;
                    }
                    else
                    {
                        baduk_Pane.Cursor = white_cursor;
                        turn_pic.Image = whiteTurn;
                    }
                }
            }
            catch(InvalidOperationException ec){ string temp = ec.StackTrace; }
            finally
            {
                
            }
        }
    }
}
