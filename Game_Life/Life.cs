using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Life
{
    class Life //Жизнь клеток, правила жизни
    {
        int[,] Field; //0 - пусто, 1 - живой, 2 - умерает, -1 - рождается
        int[,] Sum; // Sum[x,y] - сколько инфузорий правее и ниже клетки x,y
        int w, h;


        public Life(int w, int h)
        {
            this.w = w;
            this.h = h;
            Field = new int[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Field[x, y] = 0;
                }
            }
        }

        public int Change_Cells_Type(int x, int y)
        {
            Field[x, y] = Field[x, y] == 0 ? 1 : 0;
            return Field[x, y];
        }

        public int Get_Field(int x, int y)
        {
            if (x < 0 || x >= w)
                return 0;
            if (y < 0 || y >= h)
                return 0;
            return Field[x, y];
        }

        public int Get_Sum(int x, int y)
        {
            if (x < 0 || x >= w)
                return 0;
            if (y < 0 || y >= h)
                return 0;
            return Sum[x, y];
        }

        private int Get_Live_Cells_Count(int x, int y) //Проверяем окрестности инфузории на наличие других.
        {
            int sum = 0;
            for (int sx = -1; sx <= 1; sx++)
                for (int sy = -1; sy <= 1; sy++)
                    if (Get_Field(x + sx, y + sy) > 0)
                        sum++;
            return sum;
        }

        private void Prepare()
        {
            for (int x = w - 1; x >= 0; x--)
                for (int y = h - 1; h >= 0; y--)
                {
                    Sum[x, y] = Get_Field(x, y) +
                                Get_Sum(x + 1, y) +
                                Get_Sum(x, y - 1) -
                                Get_Sum(x + 1, y + 1);
                }
        }

        private int Around2(int x, int y) //Динамическое прог-е. Кол-во инфузорий областями, не вычисляя их число. Проходится 4 элемента, а не 9 (как в Around).
        {
            return Get_Sum(x, y) -
                   Get_Sum(x + 1, y) -
                   Get_Sum(x, y + 1) +
                   Get_Sum(x + 1, y + 1);
        }

        public void Mark_Cells_For_Life_Or_Die() //Отмечаем где родятся и умрут инфузории.
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int a = Get_Live_Cells_Count(x, y);
                    if (Field[x, y] == 1)
                    {
                        if (a <= 2)
                            Field[x, y] = 2; //От одиночества
                        if (a >= 5)
                            Field[x, y] = 2; //Пересыщение
                    }
                    else
                    {
                        if (a == 3)
                            Field[x, y] = -1; //Родится
                    }
                }
            }
        }

        public void Place_Life_And_Clean_Cells()  //Убираем умерших и размещаем родившихся.
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (Field[x, y] == -1)
                        Field[x, y] = 1;
                    else if (Field[x, y] == 2)
                        Field[x, y] = 0;
                }
            }
        }
    }
}