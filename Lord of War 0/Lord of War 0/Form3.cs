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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        //1)нарисовать объекты улучшения (жизнь, атака и т. д.)
        //2)придумать наконец то окончательную цепочку последовательности улучшений для КАЖДОЙ расы


        private void Form3_MouseClick(object sender, MouseEventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(trackBar1.Value) == 6)
                label1.Text = (0.2).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 5)
                label1.Text = (0.1).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 4)
                label1.Text = (0.08).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 3)
                label1.Text = (0.05).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 2)
                label1.Text = (0.04).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 1)
                label1.Text = (0.02).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 0)
                label1.Text = (0.01).ToString();
            label3.Text = (2 / Convert.ToDouble(label1.Text)).ToString()+" x "+ (2 / Convert.ToDouble(label1.Text)).ToString();
        }
        public string Data
        {
            get
            {
                return label1.Text;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(trackBar1.Value) == 6)
                label1.Text = (0.2).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 5)
                label1.Text = (0.1).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 4)
                label1.Text = (0.08).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 3)
                label1.Text = (0.05).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 2)
                label1.Text = (0.04).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 1)
                label1.Text = (0.02).ToString();
            if (Convert.ToInt32(trackBar1.Value) == 0)
                label1.Text = (0.01).ToString();
            label3.Text = (2 / Convert.ToDouble(label1.Text)).ToString() + " x " + (2 / Convert.ToDouble(label1.Text)).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(textBox1.Text);
            label4.Text = ((a-(Math.Truncate(a))%10)*10).ToString();
        }
    }
}
