using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Map_Wizard_LOW0
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        bool b = false;
        int x, y;
        string path = @"Maps";
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out x))
            {
                if (x > 49 && x < 1001)
                {
                    if (int.TryParse(textBox2.Text, out y))
                    {
                        if (y > 49 && y < 1001)
                        {
                            if (WordCount(textBox3.Text) > 0)
                            {
                                if (WordCount(textBox4.Text) > 0)
                                {
                                    b = true;
                                }
                                else
                                {
                                    b = false;
                                    MessageBox.Show("Please give path to your map", "Oooops");
                                }
                            }
                            else
                            {
                                b = false;
                                MessageBox.Show("You have forgotten to name your map", "Oooops");
                            }
                        }
                        else
                        {
                            b = false;
                            MessageBox.Show("Height shouldn't be more than 1000 and less than 50", "Remark");
                        }
                    }
                    else
                    {
                        b = false;
                        MessageBox.Show("You might be wrong in the height of the map", "Mistake");
                    }
                }
                else
                {
                    b = false;
                    MessageBox.Show("Width shouldn't be more than 1000 and less than 50", "Remark");
                }
            }
            else
            {
                b = false;
                MessageBox.Show("You might be wrong in the width of the map", "Mistake");
            }

            if (b == true)
            {
                if (File.Exists(textBox4.Text))
                {
                    MessageBox.Show("You alredy have this file in this folder!", "Try another name");
                    b = false;
                }
                else
                {
                    FileStream f = File.Create(textBox4.Text);
                    f.Close();
                    this.Close();
                }
            }            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (WordCount(textBox3.Text) > 0)
            {
                SaveFileDialog op = new SaveFileDialog();
                if (With_txt(textBox3.Text) == true)
                    op.FileName = textBox3.Text;
                else
                    op.FileName = textBox3.Text + ".txt";
                op.InitialDirectory = Path.GetFullPath(path);

                if (op.ShowDialog() == DialogResult.OK)
                {
                    textBox4.Text = op.FileName;
                    char ch = '\\';
                    string[] s123=op.FileName.Split(ch);
                    string s1 = s123[s123.Length - 1];
                    s123 = s1.Split('.');
                    s1 = s123[0];
                    for(int i=1;i<s123.Length-1;i++)
                    {
                        if (s123.Length > i + 1)
                            s1 += "." + s123[i];
                    }
                    textBox3.Text = s1;
                }
            }
            else
            {
                MessageBox.Show("Please give name to your map", "Oooops");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            b = false;
            this.Close();
        }


        static int WordCount(string str)
        {
            int nCount = 0;
            bool alpha = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ' || str[i] == '\t')
                {
                    alpha = false;
                }
                else
                {
                    if (alpha == false)
                    {
                        alpha = true;
                        nCount++;
                    }
                }
            }
            return nCount;
        }
        static bool With_txt(string s)
        {
            bool b1 = false;
            string[] s1;
            s1 = s.Split('.');
            if (s1.Length > 1)
            {
                if (s1[s1.Length-1] == "txt")
                {
                    b1 = true;
                }
            }
            else if (s1.Length == 1)
                b1 = false;
            return b1;
        }

        public string Map_Path()
        {
            return textBox4.Text;
        }
        public string Map_Name()
        {
            return textBox3.Text + ".txt";
        }
        public int Map_Width()
        {
            return x;
        }
        public int Map_Height()
        {
            return y;
        }
        public string Name()
        {
            string text = textBox3.Text;
            return text;
        }
        public bool answer()
        {
            return b;
        }
    }
}
