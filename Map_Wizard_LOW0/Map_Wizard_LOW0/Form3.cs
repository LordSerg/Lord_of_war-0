using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Map_Wizard_LOW0
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();             
        }
        int x, i, rase;
        int live, at, armor, s_earth, s_water, s_air;

        

        //int at_arrow, lookout, at_weapons, prot_weapons;
        //bool alchemist, architecture, fighter/*костолом*/;

        private void Form3_Load(object sender, EventArgs e)
        {
            i = 1;
            string[] s = this.Text.Split(' ');
            x = int.Parse(s[0]);
            rase = int.Parse(s[1]);
            //1)нарисовать объекты улучшения (жизнь, атака и т. д.)
            //2)придумать наконец то окончательную цепочку последовательности улучшений для КАЖДОЙ расы

            //создаем и заполняем граф уровней
            /*
            bool[][,] graf;
            graf = new bool[3][,];//3 расы
            int[] neighbors;
            for (int k = 0; k < 3; k++)
                graf[k] = new bool[163, 163];//163-количество вариантов улучшения
            */

            //button1.ImageList.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (i <= x)
            {
                this.Text = "Level " + i + ":";
                levels(i, false);
            }
            if (i == x)
            {
                this.Close();
            }
            i++;
            if (i % 10 == 1 && (i % 100) / 10 == 2)
                label1.Text = "What do you coose for " + (i % 100) / 10 + "1-st level?";
            else if (i % 10 == 2 && (i % 100) / 10 == 2)
                label1.Text = "What do you coose for " + (i % 100) / 10 + "2-nd level?";
            else if (i % 10 == 3 && (i % 100) / 10 == 2)
                label1.Text = "What do you coose for " + (i % 100) / 10 + "3-rd level?";
            else if(i % 10 == 1 && (i % 100) / 10 == 0)
                label1.Text = "What do you coose for 1-st level?";
            else if (i % 10 == 2 && (i % 100) / 10 == 0)
                label1.Text = "What do you coose for 2-nd level?";
            else if (i % 10 == 3 && (i % 100) / 10 == 0)
                label1.Text = "What do you coose for 3-rd level?";
            else
                label1.Text = "What do you coose for " + i + "-th level?";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (i <= x)
            {
                this.Text = "Level " + i + ":";
                levels(i, true);
            }
            if (i == x)
            {
                this.Close();
            }
            i++;
            if (i % 10 == 1 && (i % 100) / 10 == 2)
                label1.Text = "What do you coose for " + (i % 100) / 10 + "1-st level?";
            else if (i % 10 == 2 && (i % 100) / 10 == 2)
                label1.Text = "What do you coose for " + (i % 100) / 10 + "2-nd level?";
            else if (i % 10 == 3 && (i % 100) / 10 == 2)
                label1.Text = "What do you coose for " + (i % 100) / 10 + "3-rd level?";
            else if (i % 10 == 1 && (i % 100) / 10 == 0)
                label1.Text = "What do you coose for 1-st level?";
            else if (i % 10 == 2 && (i % 100) / 10 == 0)
                label1.Text = "What do you coose for 2-nd level?";
            else if (i % 10 == 3 && (i % 100) / 10 == 0)
                label1.Text = "What do you coose for 3-rd level?";
            else
                label1.Text = "What do you coose for " + i + "-th level?";
        }

        bool lvl5, lvl10, lvl15;

        void levels(int i,bool b)
        {
            //true - правая веть
            //false - левая
            if(rase==1)
            {
                //human
                if(b==true)
                {
                    if(i == 1)
                    {
                        //1защита
                    }
                    if (i == 2)
                    {
                        //1скорость земля
                    }
                    if (i == 3)
                    {
                        //1жизнь
                    }
                    if (i == 4)
                    {
                        //1скорость земля
                    }
                    if (i == 5)
                    {
                        //2атака
                        lvl5 = true;
                    }
                    if(lvl5==true)
                    {
                        if (i == 6)
                        {
                            //1атака
                        }
                        if (i == 7)
                        {
                            //1скорость земля
                        }
                        if (i == 8)
                        {
                            //1скорость вода
                        }
                        if (i == 9)
                        {
                            //1скорость земля
                        }
                        if (i == 10)
                        {
                            //2защита
                            lvl10 = true;
                        }
                        if(lvl10==true)
                        {
                            if (i == 11)
                            {
                                //1защита
                            }
                            if (i == 12)
                            {
                                //1скорость земля
                            }
                            if (i == 13)
                            {
                                //2атака
                            }
                            if (i == 14)
                            {
                                //2скорость вода
                            }
                            if (i == 15)
                            {
                                //(волшебник)
                                lvl15 = true;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {
                                    //1/1/1/1/1/1
                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                        if(lvl10==false)
                        {
                            if (i == 11)
                            {
                                //1скорость вода
                            }
                            if (i == 12)
                            {
                                //2скорость земля
                            }
                            if (i == 13)
                            {
                                //1скорость вода
                            }
                            if (i == 14)
                            {
                                //2жизнь
                            }
                            if (i == 15)
                            {
                                //(арбалетчик)
                                lvl15 = true;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                    }
                    if(lvl5==false)
                    {
                        if (i == 6)
                        {
                            //1скорость вода
                        }
                        if (i == 7)
                        {
                            //1атака
                        }
                        if (i == 8)
                        {
                            //1скорость земля
                        }
                        if (i == 9)
                        {
                            //1жизнь
                        }
                        if (i == 10)
                        {
                            //2скорость вода
                            lvl10 = true;
                        }
                        if (lvl10 == true)
                        {
                            if (i == 11)
                            {
                                //2защита
                            }
                            if (i == 12)
                            {
                                //1жизнь
                            }
                            if (i == 13)
                            {
                                //2атака
                            }
                            if (i == 14)
                            {
                                //1скорость земля
                            }
                            if (i == 15)
                            {
                                //(лучник)
                                lvl15 = true;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                        if (lvl10 == false)
                        {
                            if (i == 11)
                            {
                                //2жизнь
                            }
                            if (i == 12)
                            {
                                //1скорость земля
                            }
                            if (i == 13)
                            {
                                //1защита
                            }
                            if (i == 14)
                            {
                                //2атака
                            }
                            if (i == 15)
                            {
                                //(рыцарь)
                                lvl15 = true;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                    }
                }
                if(b==false)
                {
                    if (i == 1)
                    {
                        //1атака
                    }
                    if (i == 2)
                    {
                        //1жизнь
                    }
                    if (i == 3)
                    {
                        //1защита
                    }
                    if (i == 4)
                    {
                        //1атака
                    }
                    if (i == 5)
                    {
                        //2жизнь
                        lvl5 = false;
                    }
                    if (lvl5 == true)
                    {
                        if (i == 6)
                        {
                            //1скорость вода
                        }
                        if (i == 7)
                        {
                            //1защита
                        }
                        if (i == 8)
                        {
                            //1атака
                        }
                        if (i == 9)
                        {
                            //1жизнь
                        }
                        if (i == 10)
                        {
                            //1жизнь
                            lvl10 = false;
                        }
                        if (lvl10 == true)
                        {
                            if (i == 11)
                            {
                                //1атака
                            }
                            if (i == 12)
                            {
                                //1скорость вода
                            }
                            if (i == 13)
                            {
                                //2жизнь
                            }
                            if (i == 14)
                            {
                                //1защита
                            }
                            if (i == 15)
                            {
                                //(берсерк)
                                lvl15 = false;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                        if (lvl10 == false)
                        {
                            if (i == 11)
                            {
                                //2защита
                            }
                            if (i == 12)
                            {
                                //2атака
                            }
                            if (i == 13)
                            {
                                //1жизнь
                            }
                            if (i == 14)
                            {
                                //1скорость вода
                            }
                            if (i == 15)
                            {
                                //(капитан)
                                lvl15 = false;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                    }
                    if (lvl5 == false)
                    {
                        if (i == 6)
                        {
                            //1жизнь
                        }
                        if (i == 7)
                        {
                            //1защита
                        }
                        if (i == 8)
                        {
                            //1скорость вода
                        }
                        if (i == 9)
                        {
                            //1атака
                        }
                        if (i == 10)
                        {
                            //1скорость земля
                            lvl10 = false;
                        }
                        if (lvl10 == true)
                        {
                            if (i == 11)
                            {
                                //2скорость земля
                            }
                            if (i == 12)
                            {
                                //1скорость вода
                            }
                            if (i == 13)
                            {
                                //1защита
                            }
                            if (i == 14)
                            {
                                //1жизнь
                            }
                            if (i == 15)
                            {
                                //(алхимик)
                                lvl15 = false;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                        if (lvl10 == false)
                        {
                            if (i == 11)
                            {
                                //1атака
                            }
                            if (i == 12)
                            {
                                //2защита
                            }
                            if (i == 13)
                            {
                                //1жизнь
                            }
                            if (i == 14)
                            {
                                //2скорость земля
                            }
                            if (i == 15)
                            {
                                //(ангел)
                                //скорсть_вода=0
                                lvl15 = false;
                            }
                            if (lvl15 == true)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                            if (lvl15 == false)
                            {
                                if (i == 16)
                                {

                                }
                                if (i == 17)
                                {

                                }
                                if (i == 18)
                                {

                                }
                                if (i == 19)
                                {

                                }
                                if (i == 20)
                                {

                                }
                                if (i == 21)
                                {

                                }
                                if (i == 22)
                                {

                                }
                                if (i == 23)
                                {

                                }
                                if (i == 24)
                                {

                                }
                                if (i == 25)
                                {

                                }
                                if (i == 26)
                                {

                                }
                                if (i == 27)
                                {

                                }
                                if (i == 28)
                                {

                                }
                                if (i == 29)
                                {

                                }
                                if (i == 30)
                                {

                                }
                            }
                        }
                    }
                }
            }
            if(rase==2)
            {
                //reptile

            }
        }
    }
}
