using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace ProgrammingMethodsLab_2
{
    public class MethodArgs : EventArgs
    {
        public string value { get; private set; }
        public MethodArgs(string value)
        {
            this.value = value;
        }
    }
    public partial class Form1 : Form
    {
        public void AddChildren(MenuItemsTree polyTree)
        {
            foreach (MenuItemsTree tree in polyTree.nextLevelNodes)
            {
                if (tree.nextLevelNodes.Count == 0 && tree.itemStatus == 0)
                {
                    MethodArgs ee = new MethodArgs(tree.itemMethod);
                    tree.menuItem.Click += new EventHandler((sender, e) => addedItemClickEvent(sender, ee));
                }
                polyTree.menuItem.DropDownItems.Add(tree.menuItem);
                AddChildren(tree);
            }
        }
        public List<MenuItemsTree> StructureMenu(string filename)
        {
            List<string[]> elements = new List<string[]>();
            StreamReader f = new StreamReader(filename);
            while (!f.EndOfStream)
            {
                string s = f.ReadLine();
                string[] words = s.Split(' ');
                elements.Add(words);
            }
            List<MenuItemsTree> menuItems = new List<MenuItemsTree>();
            int i = 0;
            Stack<int> parentsID = new Stack<int>();
            Stack<string> parentsName = new Stack<string>();

            int CurrentLevel = 0;
            while (i < elements.Count())
            {
                if (elements[i][0] == "0" && elements[i].Length == 4)
                {
                    menuItems.Add(new MenuItemsTree(elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]));
                }
                if (elements[i][0] == "0" && elements[i].Length == 3)
                {
                    menuItems.Add(new MenuItemsTree(elements[i][1], Int32.Parse(elements[i][2])));
                    parentsID.Push(menuItems.Count - 1);
                    parentsName.Push(elements[i][1]);
                    CurrentLevel++;
                }

                if (Int32.Parse(elements[i][0]) == CurrentLevel && CurrentLevel != 0)
                {
                    if (elements[i].Length == 3)
                    {
                        menuItems[parentsID.Peek()].joinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]));
                        parentsID.Push(menuItems.Count - 1); 
                        parentsName.Push(elements[i][1]);
                    }
                    else
                    {
                        menuItems[parentsID.Peek()].joinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]);
                    }
                }
                else if (Int32.Parse(elements[i][0]) > CurrentLevel)
                {
                    CurrentLevel++;
                    if (elements[i].Length == 3)
                    {
                        menuItems[parentsID.Peek()].joinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]));
                        parentsID.Push(menuItems.Count - 1); parentsName.Push(elements[i][1]);
                    }
                    else
                    {
                        menuItems[parentsID.Peek()].joinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]);
                    }
                }
                else if (Int32.Parse(elements[i][0]) < CurrentLevel)
                {
                    int difference = CurrentLevel - Int32.Parse(elements[i][0]);
                    CurrentLevel = Int32.Parse(elements[i][0]);
                    
                    if (parentsID.Count() != 0 && parentsName.Count() != 0)
                    {
                        for (int j = 0; j < difference; j++)
                        {
                            parentsID.Pop();    
                            parentsName.Pop();
                        }
                    }

                    if (parentsID.Count() >= 1 && parentsName.Count() >= 1)
                    {
                        if (elements[i].Length == 3)
                        {
                            menuItems[parentsID.Peek()].joinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]));
                        }
                        else
                        {
                            menuItems[parentsID.Peek()].joinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]);
                        }
                    }
                    if (elements[i].Length == 3)
                    {
                        parentsID.Push(menuItems.Count - 1); 
                        parentsName.Push(elements[i][1]);
                    }
                }
                i++;
            }
            return menuItems;
        }
        public Form1(string filename)
        {
            InitializeComponent();
            List<MenuItemsTree> menuItems = StructureMenu(filename);
            foreach (MenuItemsTree item in menuItems)
            {
                if(item.nextLevelNodes.Count == 0 && item.itemStatus == 0)
                {
                    MethodArgs ee = new MethodArgs(item.itemMethod);
                    item.menuItem.Click += new EventHandler((sender, e) => addedItemClickEvent(sender, ee));
                }
                AddChildren(item);
                if (item.itemStatus == 2)
                {
                    item.menuItem.Visible = false;
                }
                menuStrip1.Items.Add(item.menuItem);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void addedItemClickEvent(object sender, MethodArgs e)
        {
            MessageBox.Show(e.value);
        }
    }

}
