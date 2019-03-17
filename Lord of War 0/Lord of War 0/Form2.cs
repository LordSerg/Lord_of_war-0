using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform.Windows;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;


namespace Lord_of_War_0
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        //1)нарисовать объекты улучшения (жизнь, атака и т. д.)
        //2)придумать наконец то окончательную цепочку последовательности улучшений для КАЖДОЙ расы

        int num_of_units;
        int[][] unit;//масив юнитов
        int n1, n2;//размеры карты: n1 - ширина; n2 - высота;
        bool glmsentr = false;//при заходе мыши на територию жлконтролера
        bool glmsclck = false;//при нажатии мыши на територию жлконтролера
        bool tlclck = false;//постройки показать/убрать
        double x0, y0, x1, y1, x, y;
        double z = 0.002075;
        bool[,] selected;
        double o = 0.1;//оx=oy=0.1 => 20*20//!!!никогда не применять дробь!!!// возможные размеры : 0.1/0.08/0.05/0.04/0.02/0.01
        double[,] busyness;/*если эта переменная = -1, то в данной точке есть вода; 
                                              = 0, то в данной точке ничего нет;
                                              = 1, то в данной точке растет дерево;
                                              = 2, то в данной точке есть золото;
                                              = 3, то в данной точке есть руда;
                                              = 4, то в данной точке стоит house01
                                              = 5, то в данной точке стоит house02
                                              = 6, то в данной точке стоит house03
                                              ...
                                              = 47, то в данной точке стоит house44
                                              от 100 и далее - юнит (каждый юнит обозначается уникальным номером и записаны в блокнот после карты)

        по поводу зданий, чьи габариты больше, чем 1 клетка игрового поля:
                                              после номера здания ставить "." и порядковый номер детали здания, например
                                              4.1 4.4 4.7  (здание 3х3 кл)
                                              4.2 4.5 4.8
                                              4.3 4.6 4.9
        */
        long o1;
        double gx,gy;//
        //Random r = new Random();
        Bitmap main = new Bitmap("Main.png");
        private void Form2_Load(object sender, EventArgs e)
        {//разкодируем карту (пока что нет)
            //читаем карту и заносим все в массивы и переменные
            string path = System.IO.Path.GetFullPath(/*"map2.txt"*/@"C:\Users\moskalenko_s\Desktop\My games\Lord of War 0\Maps\map4.txt");//
            string[] s1 = File.ReadAllLines(path);
            string[] s2;
            num_of_units = 0;//для подсчета юнитов на !НАЧАЛЬНОЙ КАРТЕ!(карта,которая читается из файла)
            s2 = s1[0].Split(' ');
            bool q = int.TryParse(s2[0], out n1);//n1 - ширина
            q = int.TryParse(s2[1], out n2);//n2 - высота
            busyness = new double[n2, n1];
            for (int i = 1; i < n2 + 1; i++)//по строкам
            {
                s2 = s1[i].Split(' ');
                for (int j = 0; j < n1; j++)//в строках 
                {
                    if (double.Parse(s2[j]) < 100)
                        busyness[i - 1, j] = double.Parse(s2[j]);
                    else//если это юнит, то 
                    {
                        num_of_units++;
                        busyness[i - 1, j] = double.Parse(s2[j]);
                    }
                }
            }
            //считав карту надо занести все сведения о юнитах, которые присутствуют на карте
            //будем считывать оставшуюся часть файла;
            //в файле все параметры будут указаны в правильной последовательности через пробел
            unit = new int[num_of_units][];
            for (int i = 0; i < num_of_units; i++)//у каждого юнита есть свои свойства (10 свойств)
            {
                unit[i] = new int[10];
                s2 = s1[n2 + 1 + i].Split(' ');
                for (int j = 0; j < 10; j++)//каждый массив отвечает за каждое свойство: порядковый номер, раса, клан которому пренадлежит, уровень, жизнь, атака, защита, скорость_земля, скорость_вода, скорость_воздух
                {//+
                    unit[i][j] = int.Parse(s2[j]);
                }
            }
            /*//чтение инф-ы про юнита
            label1.Text = "жизнь = " + unit[0][4];
            label5.Text = "атака = " + unit[0][5];
            label6.Text = "защита = " + unit[0][6];
            label7.Text = "ур. = " + unit[0][3];
            label8.Text = "скорость_земля = " + unit[0][7];
            label9.Text = "скорость_вода = " + unit[0][8];
            label10.Text = "скорость_воздух = " + unit[0][9];*/
            //устанавливаем масштаб наблюдаемой карты:
            glControl1.Visible = true;
            glControl1.SwapBuffers();
            timer1.Interval = 1;
            timer1.Enabled = true;
            o1 = Convert.ToInt64(2/o);
            //selected = new bool[o1, o1];
            //for (int i = 0; i < o1; i++)
            //    for (int j = 0; j < o1; j++)
            //        selected[i, j] = false;
            Cursor.Position = new Point(this.Width/2, this.Height/2);//устанавливаем курсор по центру
            //
            vScrollBar1.Maximum = n1 - Convert.ToInt32(o1);
            hScrollBar1.Maximum = n2 - Convert.ToInt32(o1);
            gx = o * -hScrollBar1.Value;
            gy = o * -vScrollBar1.Value;
            //
        }

        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//выделение
            {
                x0 = (MousePosition.X - glControl1.Location.X) * z - 1;
                y0 = (-MousePosition.Y + glControl1.Location.Y) * z + 1;
                glmsclck = true;
            }
            if (e.Button == MouseButtons.Right)//направление юнита
            {
                //selected[0, 0] = !selected[0, 0];
            }
            if (e.Button == MouseButtons.Middle)
            {
                //selected[5, 5] = !selected[5, 5];
            }
            //if(e.Delta>0)//прокрутка колесика
            //{
            //    for (int i = 0; i < o1; i++)
            //    {
            //        selected[0,i]=true;
            //    }
            //}
            //if (e.Delta < 0)
            //{
            //    for (int i = o1-1; i >= 0; i--)
            //    {
            //        selected[0, i] = true;
            //    }
            //}
            //for(;;)//бесконечный цикл
            //{

            //}
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            gx = o * -hScrollBar1.Value;
            label1.Text = (hScrollBar1.Value + " " + vScrollBar1.Value).ToString();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            gy = o * -vScrollBar1.Value;
            label1.Text = (hScrollBar1.Value + " " + vScrollBar1.Value).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.ShowDialog();
            //this.label1.Text = f.Data;
            o = Convert.ToDouble(f.Data);            
            o1 = Convert.ToInt64(2/o);
            vScrollBar1.Maximum = n1 - Convert.ToInt32(o1);
            hScrollBar1.Maximum = n2 - Convert.ToInt32(o1);
            vScrollBar1.Minimum = 0;
            hScrollBar1.Minimum = 0;
            gx = o * -hScrollBar1.Value;
            gy = o * -vScrollBar1.Value;
        }

        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            glmsclck = false;
            /*int i1 = 0, j1 = 0;
            for (int i5 = 0; i5 < o1; i5++)
                for (int j5 = 0; j5 < o1; j5++)
                    selected[i5, j5] = false;
            i1 = 0;
            for (double i = -1+o; i < 1; i += o)
            {
                j1 = 0;
                for (double j = 1-o; j > -1; j -= o)
                {
                    if (x0 > x1 && y0 > y1)//III
                    {
                        if ((i > x1 && i < x0) && (j > y1 && j < y0))
                            selected[i1, j1] = true;
                    }
                    if (x0 > x1 && y0 < y1)//II
                    {
                        if ((i > x1 && i < x0) && (j > y0 && j < y1))
                            selected[i1, j1] = true;
                    }
                    if (x0 < x1 && y0 > y1)//VI
                    {
                        if ((i > x0 && i < x1) && (j > y1 && j < y0))
                            selected[i1, j1] = true;
                    }
                    if (x0 < x1 && y0 < y1)//I
                    {
                        if ((i > x0 && i < x1) && (j > y0 && j < y1))
                            selected[i1, j1] = true;
                    }

                    if (j1 < o1 - 1)
                        j1++;
                }
                if (i1 < o1 - 1)
                    i1++;
            }*/
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tlclck = !tlclck;            
            pictureBox3.Visible = tlclck;
            pictureBox4.Visible = tlclck;
            pictureBox5.Visible = tlclck;
            pictureBox6.Visible = tlclck;
            pictureBox7.Visible = tlclck;
            pictureBox8.Visible = tlclck;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            x = (MousePosition.X - glControl1.Location.X) * z - 1;
            y = (-MousePosition.Y + glControl1.Location.Y) * z + 1;
            //GL.Begin(PrimitiveType.Lines);
            rete(Color.SandyBrown, Color.Blue);//сеть
            //GL.End();
            if (glmsentr == true && glmsclck == true)//выделитель
            {
                x1 = (MousePosition.X - glControl1.Location.X) * z - 1;
                y1 = (-MousePosition.Y + glControl1.Location.Y) * z + 1;
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.White);
                GL.Vertex2(x0, y0);
                GL.Vertex2(x1, y0);
                GL.Vertex2(x1, y0);
                GL.Vertex2(x1, y1);
                GL.Vertex2(x1, y1);
                GL.Vertex2(x0, y1);
                GL.Vertex2(x0, y1);
                GL.Vertex2(x0, y0);
                GL.End();
            }
            else if (glmsentr == false)
            {
                //GL.ClearColor(Color.Black);
                //GL.Clear(ClearBufferMask.ColorBufferBit);
            }

            glControl1.SwapBuffers();
            GL.Flush();

            //передвижение по карте:
            if (MousePosition.X < 5)
                if (hScrollBar1.Value > 0)
                {
                    hScrollBar1.Value--;
                    Thread.Sleep(70);
                }
            if (MousePosition.X > this.Width - 5)
                if (hScrollBar1.Value < hScrollBar1.Maximum)
                {
                    hScrollBar1.Value++;
                    Thread.Sleep(70);
                }
            if (MousePosition.Y < 5)
                if (vScrollBar1.Value > 0)
                {
                    vScrollBar1.Value--;
                    Thread.Sleep(70);
                }
            if (MousePosition.Y > this.Height - 5)
                if (vScrollBar1.Value < vScrollBar1.Maximum)
                {
                    vScrollBar1.Value++;
                    Thread.Sleep(70);
                }
            gx = o * -hScrollBar1.Value;
            gy = o * -vScrollBar1.Value;
        }

        private void glControl1_MouseEnter(object sender, EventArgs e)
        {
            glmsentr = true;
        }
        private void glControl1_MouseLeave(object sender, EventArgs e)
        {
            glmsentr = false;
        }

        void rete(Color col1, Color col2)//квадратная сеть с квадратом (ячейкой) размерами (о*о)
        {
            
            
            //j1,i1 - маркеры для массива "занятости";j2,i2 - маркеры для массива "выделенности"
            for (double i = -1 + gx, i1 = 0/*, i2 = 0*/; i <= 1; i += o, i1++/*, i2++*/)
            {

                for (double j = 1 - gy, j1 = 0/*, j2 = 0*/; j >= -1; j -= o, j1++/*, j2++*/)
                {
                    //if ()
                    //{
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Color3(col1);
                    GL.Vertex2(i, j);
                    GL.Vertex2(i + o, j);
                    GL.Vertex2(i + o, j - o);
                    GL.Vertex2(i, j - o);
                    GL.Vertex2(i, j);
                    GL.End();
                    //}
                    /*if (j1 < o1&&i1<o1)//(надо сделать так, чтобы выделялось только определенное кол-во юнитов или один рудник/шахта/дерево/здание)
                    {
                        if (selected[Convert.ToInt32(i1), Convert.ToInt32(j1)] == false)//отмечаем выделенные клетки
                        {
                            GL.Color3(Color.Green);
                            GL.Vertex2(i + o / 20, j - o / 20);
                            GL.Vertex2(i + o - o / 20, j - o / 20);
                            GL.Vertex2(i + o - o / 20, j - o / 20);
                            GL.Vertex2(i + o - o / 20, j - o + o / 20);
                            GL.Vertex2(i + o - o / 20, j - o + o / 20);
                            GL.Vertex2(i + o / 20, j - o + o / 20);
                            GL.Vertex2(i + o / 20, j - o + o / 20);
                            GL.Vertex2(i + o / 20, j - o / 20);
                        }
                        else
                        {
                            GL.Color3(col1);
                            GL.Vertex2(i, j);
                            GL.Vertex2(i + o, j);
                            GL.Vertex2(i + o, j);
                            GL.Vertex2(i + o, j - o);
                            GL.Vertex2(i + o, j - o);
                            GL.Vertex2(i, j - o);
                            GL.Vertex2(i, j - o);
                            GL.Vertex2(i, j);
                        }
                    }*/
                    if (j1 < n1 && i1 < n2)
                    {
                        /*if (busyness[Convert.ToInt32(i2), Convert.ToInt32(j2)] == 0)
                        {
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Color3(col1);
                            GL.Vertex2(i, j);
                            GL.Vertex2(i + o, j);
                            //GL.Vertex2(i + o, j);
                            GL.Vertex2(i + o, j - o);
                            //GL.Vertex2(i + o, j - o);
                            GL.Vertex2(i, j - o);
                            //GL.Vertex2(i, j - o);
                            GL.Vertex2(i, j);
                            GL.End();
                        }                        
                        else*/
                        if (Math.Floor(busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)]) == 1)//отмечаем месность
                        {//дерево
                            tree_draw(Convert.ToInt32(((busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] - (Math.Truncate(busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)])) % 10) * 10)), i, j);
                        }
                        else if (busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] == 2)
                        {
                            //шахта
                            /**/
                            //Bitmap bitmap = new Bitmap("Main.png");
                            int texture;
                            //GL.ClearColor(Color.White);
                            GL.Enable(EnableCap.Texture2D);

                            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

                            GL.GenTextures(1, out texture);
                            GL.BindTexture(TextureTarget.Texture2D, texture);

                            BitmapData data = main.LockBits(new System.Drawing.Rectangle(0, 0, main.Width, main.Height),
                                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                            main.UnlockBits(data);

                            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


                            //GL.Clear(ClearBufferMask.ColorBufferBit);
                            GL.MatrixMode(MatrixMode.Modelview);
                            GL.LoadIdentity();
                            GL.BindTexture(TextureTarget.Texture2D, texture);

                            GL.Begin(BeginMode.Quads);
                            //GL.Color3(col1);
                            GL.TexCoord2(0, 0);
                            GL.Vertex2(i, j);
                            GL.TexCoord2(1, 0);
                            GL.Vertex2(i + o, j);
                            GL.TexCoord2(1, 1);
                            GL.Vertex2(i + o, j - o);
                            GL.TexCoord2(0, 1);
                            GL.Vertex2(i, j - o);
                            GL.End();
                        }
                        else if (busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] == 100)
                        {//юнит
                            /*GL.Begin(PrimitiveType.Lines);
                            GL.Color3(Color.Red);
                            GL.Vertex2(i + o / 10, j + o / 10);
                            GL.Vertex2(i + o - o / 10, j + o / 10);
                            GL.Vertex2(i + o - o / 10, j + o / 10);
                            GL.Vertex2(i + o - o / 10, j + o - o / 10);
                            GL.Vertex2(i + o - o / 10, j + o - o / 10);
                            GL.Vertex2(i + o / 10, j + o - o / 10);
                            GL.Vertex2(i + o / 10, j + o - o / 10);
                            GL.Vertex2(i + o / 10, j + o / 10);
                            GL.End();*/

                            //if (unit_state() == 0)
                                unit_draw0(i, j);
                        }
                        else if (busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] == 101)
                        {
                            unit_draw1(i, j);
                        }
                        else if(busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] == 102)
                        {
                            unit_draw2(i,j);
                        }
                        
                    }
                    if ((i < x && i + o > x) && (j - o < y && j > y))//отмечаем курсор
                    {
                        GL.Begin(PrimitiveType.Lines);
                        GL.Color3(col2);
                        GL.Vertex2(i + o / 20, j - o + o / 20);
                        GL.Vertex2(i + o - o / 20, j - o + o / 20);
                        GL.Vertex2(i + o - o / 20, j - o + o / 20);
                        GL.Vertex2(i + o - o / 20, j - o + o - o / 20);
                        GL.Vertex2(i + o - o / 20, j - o + o - o / 20);
                        GL.Vertex2(i + o / 20, j - o + o - o / 20);
                        GL.Vertex2(i + o / 20, j - o + o - o / 20);
                        GL.Vertex2(i + o / 20, j - o + o / 20);
                        GL.End();
                    }
                }

            }
        }

        void tree_draw(int kind,double i,double j)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.LightGreen);
            GL.Vertex2(i + o / 20, j - o / 20);
            GL.Vertex2(i + o - o / 20, j - o / 20);
            GL.Vertex2(i + o - o / 20, j - o / 20);
            GL.Vertex2(i + o - o / 20, j - o + o / 20);
            GL.Vertex2(i + o - o / 20, j - o + o / 20);
            GL.Vertex2(i + o / 20, j - o + o / 20);
            GL.Vertex2(i + o / 20, j - o + o / 20);
            GL.Vertex2(i + o / 20, j - o / 20);
            GL.End();
            if (kind==0)
            {
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.Green);
                GL.Vertex2(i + o * 0.5, j - o * 0.1);
                //GL.Vertex2(i + o * 0.2, j - o * 0.5);
                GL.Vertex2(i + o * 0.2, j - o * 0.5);
                //GL.Vertex2(i + o * 0.3, j - o * 0.5);
                GL.Vertex2(i + o * 0.3, j - o * 0.5);
                //GL.Vertex2(i + o * 0.1, j - o * 0.8);
                GL.Vertex2(i + o * 0.1, j - o * 0.8);
                //GL.Vertex2(i + o * 0.4, j - o * 0.8);
                GL.Vertex2(i + o * 0.6, j - o * 0.8);
                //GL.Vertex2(i + o * 0.9, j - o * 0.8);
                GL.Vertex2(i + o * 0.9, j - o * 0.8);
                //GL.Vertex2(i + o * 0.7, j - o * 0.5);
                GL.Vertex2(i + o * 0.7, j - o * 0.5);
                //GL.Vertex2(i + o * 0.8, j - o * 0.5);
                GL.Vertex2(i + o * 0.8, j - o * 0.5);
                GL.Vertex2(i + o * 0.5, j - o * 0.1);
                GL.End();
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.Brown);
                GL.Vertex2(i + o * 0.4, j - o * 0.8);
                GL.Vertex2(i + o * 0.4, j - o * 0.9);
                //GL.Vertex2(i + o * 0.4, j - o * 0.9);
                GL.Vertex2(i + o * 0.6, j - o * 0.9);
                //GL.Vertex2(i + o * 0.6, j - o * 0.9);
                GL.Vertex2(i + o * 0.6, j - o * 0.8);
                GL.End();
            }
            if(kind==1)
            {
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.Green);
                GL.Vertex2(i + o * 0.5, j - o * 0.05);
                GL.Vertex2(i + o * 0.3, j - o * 0.1);
                //GL.Vertex2(i + o * 0.3, j - o * 0.1);
                GL.Vertex2(i + o * 0.2, j - o * 0.2);
                //GL.Vertex2(i + o * 0.2, j - o * 0.2);
                GL.Vertex2(i + o * 0.1, j - o * 0.4);
                //GL.Vertex2(i + o * 0.1, j - o * 0.4);
                GL.Vertex2(i + o * 0.2, j - o * 0.6);
                //GL.Vertex2(i + o * 0.2, j - o * 0.6);
                GL.Vertex2(i + o * 0.3, j - o * 0.7);
                //GL.Vertex2(i + o * 0.3, j - o * 0.7);
                GL.Vertex2(i + o * 0.4, j - o * 0.7);
                GL.Vertex2(i + o * 0.6, j - o * 0.7);
                GL.Vertex2(i + o * 0.7, j - o * 0.7);
                //GL.Vertex2(i + o * 0.7, j - o * 0.7);
                GL.Vertex2(i + o * 0.8, j - o * 0.6);
                //GL.Vertex2(i + o * 0.8, j - o * 0.6);
                GL.Vertex2(i + o * 0.9, j - o * 0.4);
                //GL.Vertex2(i + o * 0.9, j - o * 0.4);
                GL.Vertex2(i + o * 0.8, j - o * 0.2);
                //GL.Vertex2(i + o * 0.8, j - o * 0.2);
                GL.Vertex2(i + o * 0.7, j - o * 0.1);
                //GL.Vertex2(i + o * 0.7, j - o * 0.1);
                GL.Vertex2(i + o * 0.5, j - o * 0.05);
                GL.End();
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.Brown);
                GL.Vertex2(i + o * 0.4, j - o * 0.7);
                GL.Vertex2(i + o * 0.4, j - o * 0.9);
                //GL.Vertex2(i + o * 0.4, j - o * 0.9);
                GL.Vertex2(i + o * 0.6, j - o * 0.9);
                //GL.Vertex2(i + o * 0.6, j - o * 0.9);
                GL.Vertex2(i + o * 0.6, j - o * 0.7);
                GL.End();
            }
            if(kind==2)
            {
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.Green);
                GL.Vertex2(i + o * 0.5, j - o * 0.05);
                GL.Vertex2(i + o * 0.3, j - o * 0.1);
                GL.Vertex2(i + o * 0.1, j - o * 0.3);
                GL.Vertex2(i, j - o * 0.5);
                GL.Vertex2(i, j - o * 0.6);
                GL.Vertex2(i + o * 0.1, j - o * 0.7);
                GL.Vertex2(i + o * 0.2, j - o * 0.7);
                GL.Vertex2(i + o * 0.3, j - o * 0.6);
                GL.Vertex2(i + o * 0.6, j - o * 0.6);
                GL.Vertex2(i + o * 0.8, j - o * 0.7);
                GL.Vertex2(i + o * 0.9, j - o * 0.6);
                GL.Vertex2(i + o, j - o * 0.4);
                GL.Vertex2(i + o * 0.9, j - o * 0.2);
                GL.Vertex2(i + o * 0.7, j - o * 0.1);
                GL.Vertex2(i + o * 0.5, j - o * 0.05);
                GL.End();
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.Brown);
                GL.Vertex2(i + o * 0.5, j - o * 0.7);
                GL.Vertex2(i + o * 0.4, j - o * 0.6);
                GL.Vertex2(i + o * 0.3, j - o * 0.6);
                GL.Vertex2(i + o * 0.4, j - o * 0.7);
                GL.Vertex2(i + o * 0.4, j - o * 0.9);
                GL.Vertex2(i + o * 0.5, j - o * 0.9);
                GL.Vertex2(i + o * 0.5, j - o * 0.8);
                GL.Vertex2(i + o * 0.6, j - o * 0.6);
                GL.Vertex2(i + o * 0.55, j - o * 0.6);                
                GL.End();
            }            
        }

        void unit_draw0(double i,double j)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Black);
            GL.Vertex2(i + o * 0.3, j - o);
            GL.Vertex2(i + o * 0.5, j - o * 0.7);

            GL.Vertex2(i + o * 0.5, j - o * 0.7);
            GL.Vertex2(i + o * 0.5, j - o * 0.4);

            GL.Vertex2(i + o * 0.5, j - o * 0.4);
            GL.Vertex2(i + o * 0.3, j - o * 0.4);

            GL.Vertex2(i + o * 0.3, j - o * 0.4);
            GL.Vertex2(i + o * 0.3, j - o * 0.6);

            GL.Vertex2(i + o * 0.3, j - o * 0.6);
            GL.Vertex2(i + o * 0.1, j - o * 0.8);

            GL.Vertex2(i + o * 0.5, j - o * 0.4);
            GL.Vertex2(i + o * 0.7, j - o * 0.4);

            GL.Vertex2(i + o * 0.7, j - o * 0.4);
            GL.Vertex2(i + o * 0.7, j - o * 0.6);

            GL.Vertex2(i + o * 0.7, j - o * 0.6);
            GL.Vertex2(i + o * 0.6, j - o * 0.8);

            GL.Vertex2(i + o * 0.5, j - o * 0.4);
            GL.Vertex2(i + o * 0.5, j - o * 0.3);

            GL.Vertex2(i + o * 0.5, j - o * 0.3);
            GL.Vertex2(i + o * 0.4, j - o * 0.2);

            GL.Vertex2(i + o * 0.4, j - o * 0.2);
            GL.Vertex2(i + o * 0.5, j - o * 0.1);

            GL.Vertex2(i + o * 0.5, j - o * 0.1);
            GL.Vertex2(i + o * 0.6, j - o * 0.2);

            GL.Vertex2(i + o * 0.6, j - o * 0.2);
            GL.Vertex2(i + o * 0.5, j - o * 0.3);

            GL.Vertex2(i + o * 0.5, j - o * 0.7);
            GL.Vertex2(i + o * 0.7, j - o);
            GL.End();
        }

        void unit_draw1(double i, double j)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Black);
            //1-2
            GL.Vertex2(i + o * 0.35, j - o);
            GL.Vertex2(i + o * 0.45, j - o * 0.6);
            //2-3
            GL.Vertex2(i + o * 0.45, j - o * 0.6);
            GL.Vertex2(i + o * 0.55, j - o * 0.55);
            //4-5
            GL.Vertex2(i + o * 0.5, j - o * 0.575);
            GL.Vertex2(i + o * 0.5, j - o * 0.3);
            //5-6
            GL.Vertex2(i + o * 0.5, j - o * 0.3);
            GL.Vertex2(i + o * 0.3, j - o * 0.7);
            //5-7
            GL.Vertex2(i + o * 0.5, j - o * 0.3);
            GL.Vertex2(i + o * 0.7, j - o * 0.4);
            //7-3
            GL.Vertex2(i + o * 0.7, j - o * 0.4);
            GL.Vertex2(i + o * 0.55, j - o * 0.55);
            //3-8
            GL.Vertex2(i + o * 0.55, j - o * 0.55);
            GL.Vertex2(i + o * 0.6, j - o * 0.6);
            //5-9
            GL.Vertex2(i + o * 0.5, j - o * 0.3);
            GL.Vertex2(i + o * 0.5, j - o * 0.2);
            //9-10
            GL.Vertex2(i + o * 0.5, j - o * 0.2);
            GL.Vertex2(i + o * 0.4, j - o * 0.1);
            //10-11
            GL.Vertex2(i + o * 0.4, j - o * 0.1);
            GL.Vertex2(i + o * 0.5, j);
            //11-12
            GL.Vertex2(i + o * 0.5, j);
            GL.Vertex2(i + o * 0.6, j - o * 0.1);
            //12-9
            GL.Vertex2(i + o * 0.6, j - o * 0.1);
            GL.Vertex2(i + o * 0.5, j - o * 0.2);
            //3-13
            GL.Vertex2(i + o * 0.55, j - o * 0.55);
            GL.Vertex2(i + o * 0.6, j - o * 0.8);
            //13-14
            GL.Vertex2(i + o * 0.6, j - o * 0.8);
            GL.Vertex2(i + o * 0.6, j - o);
            GL.End();
        }

        void unit_draw2(double i, double j)
        {//c молотом
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Black);
            //1-2
            GL.Vertex2(i + o * 0.5, j - o);
            GL.Vertex2(i + o * 0.5, j - o * 0.6);
            //2-3
            GL.Vertex2(i + o * 0.5, j - o * 0.6);
            GL.Vertex2(i + o * 0.55, j - o * 0.6);
            //3-17
            GL.Vertex2(i + o * 0.55, j - o * 0.6);
            GL.Vertex2(i + o * 0.6, j - o * 0.8);
            //17-18
            GL.Vertex2(i + o * 0.6, j - o * 0.8);
            GL.Vertex2(i + o * 0.7, j - o);
            //4-5
            GL.Vertex2(i + o * 0.525, j - o * 0.6);
            GL.Vertex2(i + o * 0.5, j - o * 0.4);
            //5-6
            GL.Vertex2(i + o * 0.5, j - o * 0.4);
            GL.Vertex2(i + o * 0.55, j - o * 0.25);
            //6-7
            GL.Vertex2(i + o * 0.55, j - o * 0.25);
            GL.Vertex2(i + o * 0.35, j - o * 0.2);
            //7-8
            GL.Vertex2(i + o * 0.35, j - o * 0.2);
            GL.Vertex2(i + o * 0.4, j - o * 0.3);
            //8-9
            GL.Vertex2(i + o * 0.4, j - o * 0.3);
            GL.Vertex2(i + o * 0.4, j - o * 0.4);
            //6-10
            GL.Vertex2(i + o * 0.55, j - o * 0.25);
            GL.Vertex2(i + o * 0.65, j - o * 0.3);
            //10-11
            GL.Vertex2(i + o * 0.65, j - o * 0.3);
            GL.Vertex2(i + o * 0.7, j - o * 0.5);
            //11-12
            GL.Vertex2(i + o * 0.7, j - o * 0.5);
            GL.Vertex2(i + o * 0.65, j - o * 0.7);
            //6-13
            GL.Vertex2(i + o * 0.55, j - o * 0.25);
            GL.Vertex2(i + o * 0.55, j - o * 0.2);
            //13-14
            GL.Vertex2(i + o * 0.55, j - o * 0.2);
            GL.Vertex2(i + o * 0.45, j - o * 0.1);
            //14-15
            GL.Vertex2(i + o * 0.45, j - o * 0.1);
            GL.Vertex2(i + o * 0.55, j);
            //15-16
            GL.Vertex2(i + o * 0.55, j);
            GL.Vertex2(i + o * 0.65, j - o * 0.1);
            //16-13
            GL.Vertex2(i + o * 0.65, j - o * 0.1);
            GL.Vertex2(i + o * 0.55, j - o * 0.2);
            //19-20
            GL.Vertex2(i + o * 0.2, j - o * 0.3);
            GL.Vertex2(i + o * 0.75, j - o * 0.16);
            //21-22
            GL.Vertex2(i + o * 0.7, j - o * 0.3);
            GL.Vertex2(i + o * 0.7, j - o * 0.05);
            //22-23
            GL.Vertex2(i + o * 0.7, j - o * 0.05);
            GL.Vertex2(i + o * 0.8, j - o * 0.05);
            //23-24
            GL.Vertex2(i + o * 0.8, j - o * 0.05);
            GL.Vertex2(i + o * 0.9, j - o * 0.1);
            //24-25
            GL.Vertex2(i + o * 0.9, j - o * 0.1);
            GL.Vertex2(i + o * 0.8, j - o * 0.1);
            //25-22
            GL.Vertex2(i + o * 0.8, j - o * 0.1);
            GL.Vertex2(i + o * 0.7, j - o * 0.05);
            //25-26
            GL.Vertex2(i + o * 0.8, j - o * 0.1);
            GL.Vertex2(i + o * 0.8, j - o * 0.35);
            //24-26
            GL.Vertex2(i + o * 0.9, j - o * 0.1);
            GL.Vertex2(i + o * 0.8, j - o * 0.35);
            //26-21
            GL.Vertex2(i + o * 0.8, j - o * 0.35);
            GL.Vertex2(i + o * 0.7, j - o * 0.3);
            GL.End();
        }

    
        /*int unit_state()
        {


            return 0;
        }*/

        public static int load_texture(string path)
        {//загружаем текстуру картинки
            int id = GL.GenTexture();
            
            try
            {
                
                GL.BindTexture(TextureTarget.Texture2D, id);
                Bitmap bmp = new Bitmap(path);
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bmp.UnlockBits(data); 
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            catch { }
           
            return id;
        }

        abstract class Unit
        {
            public bool superposibles;
            public int level;
            public int live;
            public int earth_speed;
            public int water_speed;
            public int air_speed;
            public int protection;
            public int attack;
            public With_me with_me = new With_me();
            public On_me on_me = new On_me();
        }

        class Human : Unit
        {
            public Human(bool s, int l, int hp, int e_s, int w_s, int a_s, int p, int a, With_me w)
            {
                superposibles = s;
                level = l;
                live = hp;
                earth_speed = e_s;
                water_speed = w_s;
                air_speed = a_s;
                protection = p;
                attack = a;
                with_me = w;
                if(superposibles)
                {

                }
            }

        }

        abstract class Artefacts
        {
            //
        }

        class On_me
        {
            public int[] rings = new int[8];
            public bool head;
            public int right_hand;
            public int left_hand;
            public int body;
        }

        class With_me
        {
            public int[] slots = new int[5];
            public With_me()
            {
                for (int i = 0; i < 5; i++)
                    slots[i] = 0;
            }

            public With_me(int[] s1)
            {
                for (int i = 0; i < s1.Length; i++)
                    slots[0] = s1[i];
            }
        }

    }
}
