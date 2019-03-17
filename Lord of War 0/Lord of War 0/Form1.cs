using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lord_of_War_0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
            button1.Text = "Продолжить игру";   //Продолжить игру //Продовжити гру //Continue the game
            button2.Text = "Начать новую";      //Начать новую    //Розпочати нову //New game
            button3.Text = "Настройки";         //Настройки       //Налаштування   //Settings
            button4.Text = "Выход";             //Выход           //Вихід          //Exit
        }
        int a=0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (a == 0)
            {
                comboBox1.Visible = true;
                button3.Visible = false;
                button4.Visible = false;
                //
                a = 1;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (a == 0)
            {

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (a == 0)
            {

            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (a == 0)
            {
                Application.Exit();
            }
        }
    }
}
