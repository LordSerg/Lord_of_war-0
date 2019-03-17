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
using System.Threading;

namespace Map_Wizard_LOW0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool start = false;
        int map_width=-1, map_height;
        string map_path, map_name;
        double[,] busyness;
        int num_of_units;
        int[][] unit;
        bool glms_down = false, glms_enter;
        int monstr_index=-1;
        double o;
        double gx, gy;
        Random r;
        double z = 0.00225;
        double x, y;
        int size_of_mouse=1;
        StreamWriter st;
        //1)нарисовать объекты улучшения (жизнь, атака и т. д.)
        //2)придумать наконец то окончательную цепочку последовательности улучшений для КАЖДОЙ расы

            //!!!!!!!!!!!!!!!!!!!!tabControl!!!!!!!!!!!!!!!!!!!!!!!

        private void Form1_Load(object sender, EventArgs e)
        {
            //добавляем список рас:
            comboBox2.Text = "Human";
            comboBox2.Items.Add("Human");
            comboBox2.Items.Add("Reptile");
            //comboBox2.Items.Add("");

            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color.White);
            GL.Vertex2(0.5, 0.5);
            GL.Vertex2(0.5, -0.5);
            GL.Vertex2(-0.5, -0.5);
            GL.Vertex2(-0.5, 0.5);
            GL.End();
            glControl1.SwapBuffers();
            timer1.Interval = 1;
            timer1.Enabled = true;
            if (start == false)
            {
                toolTip1.SetToolTip(glControl1, "You should choose or create map to edit it");
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
            }
            toolTip1.SetToolTip(checkBox1, "Game rete");
            toolTip1.SetToolTip(checkBox2, "Busyness rete");
            toolTip1.SetToolTip(trackBar1, "Scale of the map");
            trackBar1.Visible = false;
            label2.Visible = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
            radioButton7.Enabled = false;
            radioButton8.Enabled = false;
            radioButton9.Enabled = false;
            radioButton10.Enabled = false;
            radioButton11.Enabled = false;
            radioButton12.Enabled = false;
            groupBox2.Visible = false;
        }

        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            if (MessageBox.Show("All unsaved maps will be deleted", "Are you sure you want to exit?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            if (start)
            {//редакция карты происходит только если start==true
                x = (MousePosition.X - glControl1.Location.X) * z - 1;
                y = (-MousePosition.Y + glControl1.Location.Y) * z + 1;
                rete(Color.SandyBrown);
                if (checkBox1.Checked == true)//сетка в клеточку
                {
                    rete1();
                }
                //if (checkBox2.Checked == true)//сетка "занятости"
                //{
                //    rete2();
                //}
                
                //if (glms_down && comboBox1.Text == "Monsters")
                //{
                //    //selected_monster(monstr_index);
                //}
                //передвижение по карте:
                if (MousePosition.X < 5)
                    if (hScrollBar1.Value > 0)
                    {
                        hScrollBar1.Value--;
                        Thread.Sleep(70);
                    }
                if (MousePosition.X > this.Width - 25)
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
                //vScrollBar1.Maximum = map_width - Convert.ToInt32(2/o);
                //hScrollBar1.Maximum = map_height - Convert.ToInt32(2/o);
                gx = o * -hScrollBar1.Value;
                gy = o * -vScrollBar1.Value;
            }
            

            glControl1.SwapBuffers();
        }

        void rete(Color col1)
        {
            for (double i = -1 + gx, i1 = 0/*, i2 = 0*/; i <= 1; i += o, i1++/*, i2++*/)
            {

                for (double j = 1 - gy, j1 = 0/*, j2 = 0*/; j >= -1; j -= o, j1++/*, j2++*/)
                {
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Color3(col1);
                    GL.Vertex2(i, j);
                    GL.Vertex2(i + o, j);
                    GL.Vertex2(i + o, j - o);
                    GL.Vertex2(i, j - o);
                    GL.Vertex2(i, j);
                    GL.End();
                    if (j1 < map_width && i1 < map_height)
                    {
                        if(glms_down && tabControl1.SelectedIndex==4 && glms_enter)//резинка
                        {
                            if (radioButton16.Checked)
                                if ((i < x && i + o > x) && (j - o < y && j > y))//1x1
                                {
                                    busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 0;
                                    size_of_mouse = 1;
                                }
                            if (radioButton15.Checked)
                                if ((i < x && i + 2 * o > x) && (j - 2 * o < y && j > y))//2x2
                                {
                                    busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 0;
                                    size_of_mouse = 2;
                                }
                            if (radioButton14.Checked)
                                if ((i - o < x && i + 2 * o > x) && (j - 2 * o < y && j + o > y))//3x3
                                {
                                    busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 0;
                                    size_of_mouse = 3;
                                }
                        }

                        if (glms_down && tabControl1.SelectedIndex == 1 && glms_enter)//рисуем горы и деревья
                        {
                            if (radioButton1.Checked)//деревья
                            {
                                if (radioButton8.Checked)//1x1
                                    if ((i < x && i + o > x) && (j - o < y && j > y))
                                    {
                                        r = new Random();
                                        int rr = r.Next(0, 3);
                                        if (rr == 0)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1;
                                        if (rr == 1)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1.1;
                                        if (rr == 2)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1.2;
                                        size_of_mouse = 1;
                                    }
                                if(radioButton7.Checked)//2x2
                                    if((i < x && i + 2*o > x) && (j - 2*o < y && j > y))
                                    {
                                        r = new Random();
                                        int rr = r.Next(0, 3);
                                        if (rr == 0)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1;
                                        if (rr == 1)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1.1;
                                        if (rr == 2)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1.2;
                                        size_of_mouse = 2;
                                    }
                                if (radioButton6.Checked)//3x3
                                    if ((i-o < x && i + 2 * o > x) && (j - 2 * o < y && j+o > y))
                                    {
                                        r = new Random();
                                        int rr = r.Next(0, 3);
                                        if (rr == 0)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1;
                                        if (rr == 1)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1.1;
                                        if (rr == 2)
                                            busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] = 1.2;
                                        size_of_mouse = 3;
                                    }
                            }
                            if(radioButton2.Checked)//горы
                            {

                            }
                        }
                        if (Math.Floor(busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)]) == 1)
                        {
                            tree_draw(Convert.ToInt32(((busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)] - (Math.Truncate(busyness[Convert.ToInt32(i1), Convert.ToInt32(j1)])) % 10) * 10)), i, j);
                        }
                        if (size_of_mouse == 1)//1x1
                            if ((i < x && i + o > x) && (j - o < y && j > y))//отмечаем курсор
                            {
                                GL.Begin(PrimitiveType.Lines);
                                GL.Color3(Color.Blue);
                                GL.Vertex2(i + o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o / 20);
                                GL.Vertex2(i + o - o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o + o / 20);
                                GL.End();
                            }
                        if(size_of_mouse==2)//2x2
                            if((i < x && i + 2 * o > x) && (j - 2 * o < y && j > y))
                            {
                                GL.Begin(PrimitiveType.Lines);
                                GL.Color3(Color.Blue);
                                GL.Vertex2(i + o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o / 20);
                                GL.Vertex2(i + o - o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o + o / 20);
                                GL.End();
                            }
                        if (size_of_mouse == 3)//3x3
                            if ((i - o < x && i + 2 * o > x) && (j - 2 * o < y && j + o > y))
                            {
                                GL.Begin(PrimitiveType.Lines);
                                GL.Color3(Color.Blue);
                                GL.Vertex2(i + o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o + o / 20);
                                GL.Vertex2(i + o - o / 20, j - o / 20);
                                GL.Vertex2(i + o - o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o / 20);
                                GL.Vertex2(i + o / 20, j - o + o / 20);
                                GL.End();
                            }
                    }
                }
            }
        }

        void rete1()
        {//сеть в клеточку
            for (double i = -1 + gx; i <= 1; i += o)
            {

                for (double j = 1 - gy; j >= -1; j -= o)
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color3(Color.Black);
                    GL.Vertex2(i, j);
                    GL.Vertex2(i + o, j);
                    GL.Vertex2(i + o, j);
                    GL.Vertex2(i + o, j - o);
                    GL.Vertex2(i + o, j - o);
                    GL.Vertex2(i, j - o);
                    GL.Vertex2(i, j - o);
                    GL.Vertex2(i, j);
                    GL.End();
                }
            }
        }

        void rete2()
        {//сеть "занятости"

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {//создаем карту
            Form2 f = new Form2();
            f.ShowDialog();
            if (f.answer() == true)
            {
                map_name = f.Map_Name();
                map_path = f.Map_Path();
                map_width = f.Map_Width();
                map_height = f.Map_Height();
                this.Text = f.Name();
                start = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                toolTip1.SetToolTip(glControl1, map_name);
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
                radioButton7.Enabled = true;
                radioButton8.Enabled = true;
                radioButton9.Enabled = true;
                radioButton10.Enabled = true;
                radioButton11.Enabled = true;
                radioButton12.Enabled = true;
                first_step(true);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {//открываем существующий файл
            OpenFileDialog op = new OpenFileDialog();
            op.InitialDirectory = Path.GetFullPath(@"Maps");
            if(op.ShowDialog()==DialogResult.OK)
            {
                map_path = op.FileName;
                char ch = '\\';
                string[] s123 = op.FileName.Split(ch);
                string s1 = s123[s123.Length - 1];
                map_name =s1;
                s123 = s1.Split('.');//для красоты выводим название карты на форму без ".txt"
                s1 = s123[0];
                for (int i = 1; i < s123.Length - 1; i++)
                {
                    if (s123.Length > i + 1)
                        s1 += "." + s123[i];
                }                
                this.Text = s1;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                start = true;
                toolTip1.SetToolTip(glControl1, map_name);
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
                radioButton7.Enabled = true;
                radioButton8.Enabled = true;
                radioButton9.Enabled = true;
                radioButton10.Enabled = true;
                radioButton11.Enabled = true;
                radioButton12.Enabled = true;
                first_step(false);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //для сохранения файла требуется 1)скопирывать данные; 2)удалить карту; 3)создать карту с этим же именем и занести туда измененные данные
            System.IO.StreamWriter file = new System.IO.StreamWriter(map_path, false);//очищаем
            file.WriteLine("");
            file.Close();
            st = new StreamWriter(map_path);
            st.WriteLine(map_width + " " + map_height);
            for (int i = 0; i < map_height; i++)//заполняем все накопленные значения из массива busyness
            {
                for (int j = 0; j < map_width; j++)
                {
                    if (busyness[i, j] < 100)
                        st.Write(busyness[i, j] + " ");
                    else
                    {
                        num_of_units++;
                    }
                }
                st.WriteLine();
            }
            //далее заполняем информацию про юнитов
            for(int i=0;i<num_of_units;i++)
            {
                for (int j = 0; j < 10; j++)
                    st.Write(unit[i][j] + " ");
            }
            st.Close();
        }
       
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void glControl1_MouseEnter(object sender, EventArgs e)
        {
            glms_enter = true;
        }
        private void glControl1_MouseLeave(object sender, EventArgs e)
        {
            glms_enter = false;
        }
        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            glms_down = start;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel5.Enabled = radioButton1.Checked;
            panel4.Enabled = !radioButton1.Checked;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            gx = o * -hScrollBar1.Value;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            gy = o * -vScrollBar1.Value;
        }


        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            glms_down = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if(trackBar1.Value==1)
            {
                label2.Text = "5 x 5";
                o = 0.4;
            }
            if (trackBar1.Value == 2)
            {
                label2.Text = "10 x 10";
                o = 0.2;
            }
            if (trackBar1.Value == 3)
            {
                label2.Text = "20 x 20";
                o = 0.1;
            }
            if (trackBar1.Value == 4)
            {
                label2.Text = "40 x 40";
                o = 0.05;
            }
            vScrollBar1.Maximum = map_width - Convert.ToInt32(2 / o);
            hScrollBar1.Maximum = map_height - Convert.ToInt32(2 / o);
            gx = o * -hScrollBar1.Value;
            gy = o * -vScrollBar1.Value;
        }

        void first_step(bool new_map)//очищаем все и считываем/создаем карту
        {
            if (new_map == false)//если new_map = false, то надо прочитать карту
            {
                try//"try" - на случай если это не карта или этот файл пуст.
                {                    
                    string[] s = File.ReadAllLines(map_path);
                    string[] s1;
                    num_of_units = 0;
                    s1 = s[0].Split(' ');
                    map_width = int.Parse(s1[0]);
                    map_height = int.Parse(s1[1]);
                    busyness = new double[map_height, map_width];
                    for (int i = 1; i < map_height + 1; i++)//по строкам
                    {
                        s1 = s[i].Split(' ');
                        for (int j = 0; j < map_width; j++)//в строках 
                        {
                            if (double.Parse(s1[j]) < 100)//проверка на наличие юнитов
                                busyness[i - 1, j] = double.Parse(s1[j]);
                            else//если это юнит, то 
                            {
                                num_of_units++;
                                busyness[i - 1, j] = double.Parse(s1[j]);
                            }
                        }
                    }

                    unit = new int[num_of_units][];//количество разных юнитов

                    for (int i = 0; i < num_of_units; i++)
                    {
                        unit[i] = new int[10];
                        s1 = s[map_height + 1 + i].Split(' ');
                        for (int j = 0; j < 10; j++)//у каждого юнита есть свои свойства (10 свойств)
                        {
                            unit[i][j] = int.Parse(s1[j]);
                        }
                    }
                    start = true;
                    trackBar1.Visible = true;
                    label2.Visible = true;
                }
                catch(Exception a)
                {
                    MessageBox.Show(a.ToString(),"Oooops!");
                }
            }
            else//если это новая карта, то открываем поток и записываем туда все значения
            {
                st = new StreamWriter(map_path);
                st.WriteLine(map_width + " " + map_height);
                busyness = new double[map_height, map_width];
                for(int i=0;i<map_height;i++)//заполняем все нулями и заносим в массив busyness
                {
                    for (int j = 0; j < map_width; j++)
                    {
                        busyness[i, j] = 0;
                        st.Write(busyness[i,j] + " ");
                    }
                    st.WriteLine();
                }
                st.Close();
                start = true;
                trackBar1.Visible = true;
                label2.Visible = true;
            }
            trackBar1.Value = 2;
            label2.Text = "10 x 10";
            o = 0.2;//5x5  <->0.4 <->trackBar=1,
                    //10x10<->0.2 <->trackBar=2,
                    //20x20<->0.1 <->trackBar=3,
                    //40x40<->0.05<->trackBar=4;
            vScrollBar1.Maximum = map_width - Convert.ToInt32(2/o);
            hScrollBar1.Maximum = map_height - Convert.ToInt32(2/o);
            gx = o * -hScrollBar1.Value;
            gy = o * -vScrollBar1.Value;
            tabControl1.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //генерируем нового человека
            With_me w = new With_me();
            Unit h = new Human(false, 0, 5, 4, 1, 0, 2, 3, w);
        }


        private void button1_Click(object sender, EventArgs e)
        {

            Form3 f = new Form3();
            int er;
            if (int.TryParse(Microsoft.VisualBasic.Interaction.InputBox("Input the level of your unit (from 1 till 30)"), out er) == false)
            {
                MessageBox.Show("Please input correctly!", "Oops...");
            }
            else
            {
                if (er > 0 && er < 31)
                {
                    f.Text = er.ToString();
                    if (comboBox2.Text == "Human")
                    {
                        f.Text += " 1";
                        f.ShowDialog();
                    }
                    else if (comboBox2.Text == "Reptile")
                    {
                        f.Text += " 2";
                        f.ShowDialog();
                    }
                    else if (comboBox2.Text == "Coleopter")
                    {
                        f.Text += " 3";
                        f.ShowDialog();
                    }
                    else
                        MessageBox.Show("Please chose the rase of your unit.");
                }
                else
                    MessageBox.Show("Unit's level shouldn't be more than 30 and less than 0!");
            }

        }

        void tree_draw(int kind, double i, double j)
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
            if (kind == 0)
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
            if (kind == 1)
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
            if (kind == 2)
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
            public With_me with_me =new With_me();
            public On_me on_me=new On_me();
        }

        class Human:Unit
        {
            public Human(bool s, int l, int hp, int e_s, int w_s, int a_s, int p, int a,With_me w)
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
            }

        }

        abstract class Artefacts
        {
            
        }

        class On_me
        {
            public int[] rings=new int [8];
            public bool head;
            public int right_hand;
            public int left_hand;
            public int body;
        }

        class With_me
        {
            public int[] slots=new int[5];
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
