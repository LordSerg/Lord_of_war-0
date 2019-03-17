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

namespace Lord_of_War_0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }

        bool glmsentr = false;//при заходе мыши на територию жлконтролера
        bool glmsclck = false;//при нажатии мыши на територию жлконтролера
        bool tlclck = false;
        double x0, y0, x1, y1, x, y;
        double z = 0.00217;
        bool[,] selected;
        int[,] busyness;/*если эта переменная = 0, то в данной точке ничего нет;
                                              = 1, то в данной точке растет дерево;
                                              = 2, то в данной точке есть золото;
                                              = 3, то в данной точке присутствует (стоит/бежит/лежит/парит и тд) юнит;
                                              = 4, то в данной точке стоит house01
                                              = 5, то в данной точке стоит house02
                                              = 6, то в данной точке стоит house03
                                              ...
                                              = 47, то в данной точке стоит house44
        */
        double o=0.1;//оx=oy=0.1 => 20*20//
        int o1;
        int [][]unit;//масив юнитов
        int n1,n2;//размеры карты

        private void Form1_Load(object sender, EventArgs e)
        {
            //читаем карту и заносим все в массивы и переменные
            string path = System.IO.Path.GetFullPath("map1.txt");
            string[] s1 = File.ReadAllLines(path);
            string[] s2;
            s2 = s1[0].Split(' ');
            bool q = int.TryParse(s2[0], out n1);
            q = int.TryParse(s2[1], out n2);
            busyness = new int[n1, n2];
            for (int i=1;i<n1+1;i++)//по строкам
            {
                s2 = s1[i].Split(' ');
                for (int j = 0; j < n2; j++)//в строках 
                {
                    busyness[i - 1, j] = int.Parse(s2[j]);
                }
            }
            //
            glControl1.Visible = true;
            glControl1.SwapBuffers();
            timer1.Interval = 1;
            timer1.Enabled = true;
            o1 = Convert.ToInt32((1 / o) * 2);
            selected = new bool[o1,o1];
            for (int i = 0; i < o1; i++)
                for (int j = 0; j < o1; j++)
                    selected[i, j] = false;
            Cursor.Position = new Point(0, 0);
        }
      
        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x0 = (MousePosition.X - glControl1.Location.X) * z - 1;
                y0 = (-MousePosition.Y + glControl1.Location.Y) * z + 1;
                glmsclck = true;
            }
            if(e.Button == MouseButtons.Right)
            {
                selected[0, 0] = !selected[0, 0];
            }
            if(e.Button==MouseButtons.Middle)
            {
                selected[5, 5] = !selected[5, 5];
            }
            //if(e.Delta>0)//прокрутка мыши
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
        }
        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            glmsclck = false;
            int i1 = 0, j1 = 0;
            for (int i5 = 0; i5 < o1; i5++)
                for (int j5 = 0; j5 < o1; j5++)
                    selected[i5, j5] = false;
            i1 = 0;
            for (double i = -1; i < 1; i += o)
            {
                j1 = 0;
                for (double j = -1; j < 1; j += o)
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

                    if (j1 < o1-1)
                        j1++;
                }
                if (i1 < o1-1)
                    i1++;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tlclck = !tlclck;
            pictureBox2.Visible = tlclck;
            pictureBox3.Visible = tlclck;
            pictureBox4.Visible = tlclck;
            pictureBox5.Visible = tlclck;
            pictureBox6.Visible = tlclck;
            pictureBox7.Visible = tlclck;
            pictureBox9.Visible = tlclck;
            pictureBox10.Visible = tlclck;
            pictureBox11.Visible = tlclck;
            pictureBox12.Visible = tlclck;
            pictureBox13.Visible = tlclck;
            pictureBox14.Visible = tlclck;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            x = (MousePosition.X - glControl1.Location.X) * z - 1;
            y = (-MousePosition.Y + glControl1.Location.Y) * z + 1;
            GL.Begin(PrimitiveType.Lines);
            rete(Color.White, Color.Red);//сеть
            GL.End();            
            if (glmsentr ==true&& glmsclck == true)
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
            else if(glmsentr==false)
            {
                GL.ClearColor(Color.Black);
                GL.Clear(ClearBufferMask.ColorBufferBit);
            }
            GL.Flush();
            glControl1.SwapBuffers();
        }

        private void glControl1_MouseEnter(object sender, EventArgs e)
        {
            glmsentr = true;
        }

        private void glControl1_MouseLeave(object sender, EventArgs e)
        {
            glmsentr = false;
        }
        void rete(Color col1,Color col2)//квадратная сеть с квадратом размерами (о*о) 
        {
            int i1 = 0, j1 = 0;
            for (double i = -1; i <= 1; i += o)
            {
                j1 = 0;
                for (double j = -1; j <= 1; j += o)
                {
                    if ((i < x && i + o > x) && (j < y && j + o > y))
                    {
                        GL.Color3(col2);
                        GL.Vertex2(i + o / 20, j + o / 20);
                        GL.Vertex2(i + o - o / 20, j + o / 20);
                        GL.Vertex2(i + o - o / 20, j + o / 20);
                        GL.Vertex2(i + o - o / 20, j + o - o / 20);
                        GL.Vertex2(i + o - o / 20, j + o - o / 20);
                        GL.Vertex2(i + o / 20, j + o - o / 20);
                        GL.Vertex2(i + o / 20, j + o - o / 20);
                        GL.Vertex2(i + o / 20, j + o / 20);
                    }
                    else if (selected[i1, j1] == false)
                    {
                        GL.Color3(col1);
                        GL.Vertex2(i, j);
                        GL.Vertex2(i + o, j);
                        GL.Vertex2(i + o, j);
                        GL.Vertex2(i + o, j + o);
                        GL.Vertex2(i + o, j + o);
                        GL.Vertex2(i, j + o);
                        GL.Vertex2(i, j + o);
                        GL.Vertex2(i, j);
                    }
                    else
                    {
                        GL.Color3(Color.Green);
                        GL.Vertex2(i + o / 20, j + o / 20);
                        GL.Vertex2(i + o - o / 20, j + o / 20);
                        GL.Vertex2(i + o - o / 20, j + o / 20);
                        GL.Vertex2(i + o - o / 20, j + o - o / 20);
                        GL.Vertex2(i + o - o / 20, j + o - o / 20);
                        GL.Vertex2(i + o / 20, j + o - o / 20);
                        GL.Vertex2(i + o / 20, j + o - o / 20);
                        GL.Vertex2(i + o / 20, j + o / 20);
                    }
                    if (j1 < o1-1)
                        j1++;
                }
                if (i1 < o1-1)
                    i1++;
            }
        }

    }
}
