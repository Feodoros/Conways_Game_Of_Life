using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Life
{
    public partial class Form1 : Form
    {
        int box_w = 20;
        int box_h = 20;
        int w, h;

        Label[,] Lab;
        Life life;

        Color color_none = Color.White;
        Color color_live = Color.Black;
        Color color_born = Color.Yellow;
        Color color_died = Color.Red;

        public Form1()
        {
            InitializeComponent();
            Init_labels();
        }

        private void Init_labels()
        {
            w = (panel.Width - 1) / box_w;
            h = (panel.Height - 1) / box_h;

            life = new Life(w, h);
            Lab = new Label[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    Add_Label(x, y);
        }

        private void Label1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = ((Label)sender).Location.X / box_w;
            int y = ((Label)sender).Location.Y / box_h;
            int color = life.Turn(x, y);
            Lab[x, y].BackColor = color == 1 ? color_live : color_none;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            life.Step1();
            refresh();
        }

        private void refresh()
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    switch (life.Get_Field(x, y))
                    {
                        case 0: Lab[x, y].BackColor = color_none; break;
                        case 1: Lab[x, y].BackColor = color_live; break;
                        case 2: Lab[x, y].BackColor = color_died; break;
                        case -1: Lab[x, y].BackColor = color_born; break;

                    }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            life.Step2();
            refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            life.Step1();
            life.Step2();
            refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            life.Step1();
            life.Step2();
            refresh();
        }

        private void Add_Label(int x, int y)
        {
            Lab[x, y] = new Label();
            Lab[x, y].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Lab[x, y].Location = new System.Drawing.Point(x * box_w, y * box_h);
            Lab[x, y].Size = new System.Drawing.Size(box_w + 1, box_h + 1);
            Lab[x, y].Parent = panel;
            Lab[x, y].MouseClick += new System.Windows.Forms.MouseEventHandler(this.Label1_MouseClick);
        }
    }
}
