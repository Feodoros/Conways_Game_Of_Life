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
        Random rand = new Random();
        Label[,] Lab, Lab1;
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
            Lab1 = new Label[w, h];
            life = new Life(w, h);
            Lab = new Label[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    Add_Label_Pattern(x, y);
                    Add_Label(x, y);
                }
        }


        private void Label1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = ((Label)sender).Location.X / box_w;
            int y = ((Label)sender).Location.Y / box_h;
            int color = life.Change_Cells_Type(x, y);
            Lab[x, y].BackColor = color == 1 ? color_live : color_none;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            life.Mark_Cells_For_Life_Or_Die();
            Paint();
        }

        private void Paint()
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

        private void Paint_Pattern()
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    int m = rand.Next(0, 4);
                    switch (life.Get_Field_Pattern(x, y))
                    {
                        case 0: Lab1[x, y].BackColor = color_none; break;
                        case 1: Lab1[x, y].BackColor = color_live; break;
                    }
                }
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            life.Place_Life_And_Clean_Cells();
            Paint();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            life.Mark_Cells_For_Life_Or_Die();
            life.Place_Life_And_Clean_Cells();
            Paint();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer.Enabled = !timer.Enabled;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            life.Mark_Cells_For_Life_Or_Die();
            life.Place_Life_And_Clean_Cells();
            Paint();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            life.Scan_Pattern();
            Paint_Pattern();
        }

        private void Add_Label(int x, int y)
        {
            Lab[x, y] = new Label
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(x * box_w, y * box_h),
                Size = new Size(box_w + 1, box_h + 1),
                Parent = panel
            };
            Lab[x, y].MouseClick += new MouseEventHandler(this.Label1_MouseClick);
        }

        private void Add_Label_Pattern(int x, int y)
        {
            Lab1[x, y] = new Label
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(x * box_w, y * box_h),
                Size = new Size(box_w + 1, box_h + 1),
                Parent = panel1
            };
        }
    }
}
