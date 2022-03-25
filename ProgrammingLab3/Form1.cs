using System;
using System.Windows.Forms;
using System.Reflection;
using System.Linq;
using MenuStripCreator; //неявное

namespace ProgrammingLab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            openFileDialog1.Title = "Открыть файл с структурой меню";
            openFileDialog1.FileName = "";
        }

        private void Button1Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            try
            {
                Assembly asm = Assembly.LoadFrom("MenuCreatorLibrary.dll"); //явное
                Type type = asm.GetTypes().FirstOrDefault(x => x.Name == "MenuCreator");
                object obj = Activator.CreateInstance(type, new object[] { filename, menuStrip1 });
                //MenuCreator menu = new MenuCreator(filename, menuStrip1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
