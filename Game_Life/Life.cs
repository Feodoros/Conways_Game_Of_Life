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
        int[,] Pattern;
        int counter_generations = 0;

        public Life(int w, int h)
        {
            this.w = w;
            this.h = h;
            Pattern = new int[w, h];
            Field = new int[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Field[x, y] = 0;
                    Pattern[x, y] = 0;
                }
            }
        }

        public int Change_Cells_Type(int x, int y)
        {
            Field[x, y] = Field[x, y] == 0 ? 1 : 0;
            return Field[x, y];
        }

        public void Scan_Pattern()
        {
            Clear_Pattern_Panel();
            for (int x1 = 0; x1 < w; x1++)
            {
                for (int y1 = 0; y1 < h; y1++)
                {
                    if (Get_Field(x1, y1) == 1 && Get_Field(x1, y1 + 1) == 1 && Get_Field(x1, y1 + 2) == 1 && (Get_Live_Cells_Count(x1, y1) + Get_Live_Cells_Count(x1, y1 + 1) + Get_Live_Cells_Count(x1, y1 + 2) <= 7))
                    {
                        Pattern[x1, y1] = 1;
                        Pattern[x1, y1 + 1] = 1;
                        Pattern[x1, y1 + 2] = 1;
                    }

                    if (Get_Field(x1, y1) == 1 && Get_Field(x1 + 1, y1) == 1 && Get_Field(x1 + 2, y1) == 1 && (Get_Live_Cells_Count(x1, y1) + Get_Live_Cells_Count(x1 + 1, y1) + Get_Live_Cells_Count(x1 + 2, y1) <= 7))
                    {

                        Pattern[x1, y1] = 1;
                        Pattern[x1 + 1, y1] = 1;
                        Pattern[x1 + 2, y1] = 1;
                    }

                    if (Get_Field(x1, y1) == 1 && Get_Field(x1 - 1, y1 + 1) == 1 && Get_Field(x1 - 1, y1 + 2) == 1 && Get_Field(x1 + 1, y1 + 1) == 1 && Get_Field(x1 + 1, y1 + 2) == 1 && Get_Field(x1, y1 + 3) == 1 &&
                        (Get_Live_Cells_Count(x1, y1) + Get_Live_Cells_Count(x1 - 1, y1 + 1) + Get_Live_Cells_Count(x1 - 1, y1 + 2) + Get_Live_Cells_Count(x1 + 1, y1 + 1) + Get_Live_Cells_Count(x1 + 1, y1 + 2) + Get_Live_Cells_Count(x1, y1 + 3)) <= 18)
                    {
                        Pattern[x1, y1] = 1;
                        Pattern[x1 - 1, y1 + 1] = 1;
                        Pattern[x1 - 1, y1 + 2] = 1;
                        Pattern[x1, y1 + 3] = 1;
                        Pattern[x1 + 1, y1 + 1] = 1;
                        Pattern[x1 + 1, y1 + 2] = 1;
                    }

                    if (Get_Field(x1, y1) == 1 && Get_Field(x1 + 1, y1 - 1) == 1 && Get_Field(x1 + 2, y1 - 1) == 1 && Get_Field(x1 + 3, y1) == 1 && Get_Field(x1 + 2, y1 + 1) == 1 && Get_Field(x1 + 1, y1 + 1) == 1 &&
                        (Get_Live_Cells_Count(x1, y1) + Get_Live_Cells_Count(x1 + 1, y1 - 1) + Get_Live_Cells_Count(x1 + 2, y1 - 1) + Get_Live_Cells_Count(x1 + 3, y1) + Get_Live_Cells_Count(x1 + 2, y1 + 1) + Get_Live_Cells_Count(x1 + 1, y1 + 1)) <= 18)
                    {
                        Pattern[x1, y1] = 1;
                        Pattern[x1 + 1, y1 - 1] = 1;
                        Pattern[x1 + 2, y1 - 1] = 1;
                        Pattern[x1 + 3, y1] = 1;
                        Pattern[x1 + 1, y1 + 1] = 1;
                        Pattern[x1 + 2, y1 + 1] = 1;
                    }

                    if (Get_Field(x1, y1) == 1 && Get_Field(x1 + 1, y1 + 1) == 1 && Get_Field(x1 + 1, y1 + 2) == 1 && Get_Field(x1, y1 + 2) == 1 && Get_Field(x1 - 1, y1 + 2) == 1 && (
                        Get_Live_Cells_Count(x1, y1) + Get_Live_Cells_Count(x1 + 1, y1 + 1) + Get_Live_Cells_Count(x1 + 1, y1 + 2) + Get_Live_Cells_Count(x1, y1 + 2) + Get_Live_Cells_Count(x1 - 1, y1 + 2)) <= 15)
                    {
                        Pattern[x1, y1] = 1;
                        Pattern[x1, y1 + 2] = 1;
                        Pattern[x1 + 1, y1 + 2] = 1;
                        Pattern[x1 - 1, y1 + 2] = 1;
                        Pattern[x1 + 1, y1 + 1] = 1;
                    }

                    if (Get_Field(x1, y1) == 1 && Get_Field(x1 + 1, y1) == 1 && Get_Field(x1, y1 + 1) == 1 && Get_Field(x1 + 1, y1 + 1) == 1 &&
                        (Get_Live_Cells_Count(x1, y1) + Get_Live_Cells_Count(x1 + 1, y1) + Get_Live_Cells_Count(x1, y1 + 1) + Get_Live_Cells_Count(x1 + 1, y1 + 1)) <= 16)
                    {
                        Pattern[x1, y1] = 1;
                        Pattern[x1 + 1, y1] = 1;
                        Pattern[x1, y1 + 1] = 1;
                        Pattern[x1 + 1, y1 + 1] = 1;
                    }

                    if (Get_Field(x1, y1) == 1 && Get_Field(x1 + 1, y1 - 1) == 1 && Get_Field(x1 + 1, y1 + 1) == 1 && Get_Field(x1 + 2, y1) == 1 &&
                        (Get_Live_Cells_Count(x1, y1) + Get_Live_Cells_Count(x1 + 1, y1 - 1) + Get_Live_Cells_Count(x1 + 1, y1 + 1) + Get_Live_Cells_Count(x1 + 2, y1) <= 12))
                    {
                        Pattern[x1, y1] = 1;
                        Pattern[x1 + 1, y1 - 1] = 1;
                        Pattern[x1 + 1, y1 + 1] = 1;
                        Pattern[x1 + 2, y1] = 1;
                    }

                }
            }
        }

        private void Clear_Pattern_Panel()
        {
            for (int x1 = 0; x1 < w; x1++)
                for (int y1 = 0; y1 < h; y1++)
                    Pattern[x1, y1] = 0;
        }

        public int Get_Field(int x, int y)
        {
            if (x < 0 || x >= w)
                return 0;
            if (y < 0 || y >= h)
                return 0;
            return Field[x, y];
        }

        public int Get_Field_Pattern(int x1, int y1)
        {
            if (x1 < 0 || x1 >= w)
                return 0;
            if (y1 < 0 || y1 >= h)
                return 0;
            return Pattern[x1, y1];
        }

        public int Get_Sum(int x, int y)
        {
            if (x < 0 || x >= w)
                return 0;
            if (y < 0 || y >= h)
                return 0;
            return Sum[x, y];
        }

        public int Get_Live_Cells_Count(int x, int y) //Проверяем окрестности инфузории на наличие других.
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

        public int Counter_Live_Cells()
        {
            int count = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (Get_Field(x, y) == 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public int Counter_Generations()
        {
            if (Counter_Live_Cells() != 0)
                counter_generations++;
            return counter_generations;
        }
    }
}