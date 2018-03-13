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
        bool flag, flag1= true;
        double xEX = 1;
        int box_w = 20;
        int box_h = 20;
        int w, h;
        Label[,] Lab, Lab1;
        Life life;

        public delegate void Life_Event();
        public event Life_Event Begining_Of_Turn;                  //Помечаем родившихся и умерших (Step 1)
        public event Life_Event End_Of_Turn;                       //Убираем умерших и ставим родившихся (Step 2). 
        public event Life_Event Count_And_Percent_Of_Cells_Event;  //Количество живых клеток на данный момент, их прогресс/регресс с прошлым ходом, счетчик поколений.
        public event Life_Event Paint_Patterns;                    //Отрисовка паттернов (устойчивых кластеров) на отдельной панели.

        Color color_none = Color.Transparent;                      //Мертвая.
        Color color_live = Color.Black;                            //Живая.
        Color color_born = Color.Yellow;                           //Рождается (на след. ходу -> Живая).
        Color color_died = Color.Red;                              //Умирает (на след. ходу -> Мертвая).
        
        public Form1()
        {
            InitializeComponent();
            Init_labels();
            Begining_Of_Turn += life.Mark_Cells_For_Life_Or_Die;
            End_Of_Turn += life.Place_Life_And_Clean_Cells;
            Count_And_Percent_Of_Cells_Event += Count_And_Percent_Of_Live_Cells_Now;
            Paint_Patterns += life.Scan_Pattern;
            Paint_Patterns += Paint_Pattern;
            
        }

        private void Init_labels() //Заполняем массивом лэйблов панель (размещаем пустые клетки).
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

        private void Button1_Click(object sender, EventArgs e)  //Step1.
        {
            Begining_Of_Turn();
            Paint();
        }

        private void Button2_Click(object sender, EventArgs e)  //Step2.
        {
            End_Of_Turn();
            Count_And_Percent_Of_Cells_Event();
            Paint();            
        }

        private void button3_Click(object sender, EventArgs e)  //Two Steps together.
        {
            Begining_Of_Turn();
            End_Of_Turn();
            Count_And_Percent_Of_Cells_Event();
            Paint();            
        }

        private void button4_Click(object sender, EventArgs e)  //Auto Mode.
        {
            timer.Enabled = !timer.Enabled;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Begining_Of_Turn();
            End_Of_Turn();
            Count_And_Percent_Of_Cells_Event();
            Paint();
        }

        private void button5_Click(object sender, EventArgs e) //Нахождение и отрисовка паттернов.
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            Paint_Patterns();            
        }

        private void Paint() //Отрисовка клеток (родятся/умрут/живая/пустая).
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

        private void Paint_Pattern()  //Отрисовка паттернов на отдельную панель.
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    switch (life.Get_Field_Pattern(x, y))
                    {
                        case 0: Lab1[x, y].BackColor = color_none; break;
                        case 1: Lab1[x, y].BackColor = color_live; break;
                    }
                }
        }
        
        private void Count_And_Percent_Of_Live_Cells_Now() //Счетчик живых клеток, их прогресс(регресс), счетчик поколений.
        {
            int x1 = (life.Counter_Live_Cells());
            textBox1.Text = ("Живых клеток: " + x1);

            double x = (life.Counter_Live_Cells());
            double percent = 100;
            if (x != 0)
            {
                if (xEX == 1 && flag)
                {
                    percent = 100;
                }
                else
                {
                    percent = Math.Round((x / xEX) * 100, 2);
                }
                textBox3.Text = ("Поколение номер: " + life.Counter_Generations());
                textBox2.Text = ("Прогресс/Регресс колонии: " + percent + "%");
            }
            else
            {
                textBox2.Text = ("Прогресс/Регресс колонии: " + 0 + "%");
                if (flag1)
                {
                    flag1 = false;
                    MessageBox.Show("Колония пережила " + life.Counter_Generations() + " поколений. Жизнь кончена.", "Количество поколений?");
                }
            }
            xEX = x;
            flag = false;

        }

        private void Draw_Cell_Borders() //Отрисовка границ клеток.
        {
            w = (panel.Width - 1) / box_w;
            h = (panel.Height - 1) / box_h;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    panel.Controls.Remove(Lab[x, y]);
                    panel1.Controls.Remove(Lab1[x, y]);
                    Add_Label(x, y);
                    Add_Label_Pattern(x, y);
                }
            Paint();
        }


        //Побочные методы//

        private void Label1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = ((Label)sender).Location.X / box_w;
            int y = ((Label)sender).Location.Y / box_h;
            int color = life.Change_Cells_Type(x, y);
            Lab[x, y].BackColor = color == 1 ? color_live : color_none;

        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox1 = (CheckBox)sender;
            Draw_Cell_Borders();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Add_Label(int x, int y)
        {
            Lab[x, y] = new Label
            {
                Location = new Point(x * box_w, y * box_h),
                Size = new Size(box_w + 1, box_h + 1),
                Parent = panel
            };

            if (checkBox1.Checked)
                Lab[x, y].BorderStyle = BorderStyle.FixedSingle;
            else
                Lab[x, y].BorderStyle = BorderStyle.None;

            Lab[x, y].MouseClick += new MouseEventHandler(this.Label1_MouseClick);
        }

        private void Add_Label_Pattern(int x, int y)
        {
            Lab1[x, y] = new Label
            {
                Location = new Point(x * box_w, y * box_h),
                Size = new Size(box_w + 1, box_h + 1),
                Parent = panel1
            };
            if (checkBox1.Checked)
                Lab1[x, y].BorderStyle = BorderStyle.FixedSingle;
            else
                Lab1[x, y].BorderStyle = BorderStyle.None;
        }
    }
}
