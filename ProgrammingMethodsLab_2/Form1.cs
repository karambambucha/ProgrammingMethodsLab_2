using System;
using System.Windows.Forms;
using MenuStripCreator;

namespace ProgrammingMethodsLab_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            openFileDialog1.Title = "Открыть файл с структурой меню";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            try
            {
                MenuCreator menu = new MenuCreator(filename, menuStrip1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
