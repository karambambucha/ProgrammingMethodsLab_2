using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MenuCreator
{
    public class MethodArgs : EventArgs
    {
        public string value { get; private set; }
        public MethodArgs(string value)
        {
            this.value = value;
        }
    }
    class MenuCreator
    {
        private List<MenuItemsTree> menuItems;
        public MenuCreator(string filename, MenuStrip menuStrip)
        {
            menuStrip.Items.Clear();
            List<string[]> elements = new List<string[]>();
            StreamReader f = new StreamReader(filename);
            while (!f.EndOfStream)
            {
                string s = f.ReadLine();
                string[] words = s.Split(' ');
                if (words.Count() != 3 && words.Count() != 4)
                {
                    f.Close();
                    throw new Exception("Неверное количество слов в строке!");
                }
                if (!Int32.TryParse(words[0], out _) || !Int32.TryParse(words[2], out _))
                {
                    f.Close();
                    throw new Exception("В первом или третьем слове строк(и) находятся не числа!");
                }
                elements.Add(words);
            }
            f.Close();
            menuItems = new List<MenuItemsTree>();
            int i = 0;
            Stack<int> parentsID = new Stack<int>();
            Stack<string> parentsName = new Stack<string>();

            int CurrentLevel = 0;
            while (i < elements.Count())
            {
                if (elements[i][0] != "0" && parentsID.Count == 0)
                {
                    throw new Exception("Нерректная иерархия!");
                }
                if (Int32.Parse(elements[i][0]) < 0)
                {
                    throw new Exception("Номер иерархии предмета(ов) меньше нуля!");
                }
                if (elements[i][0] == "0" && elements[i].Length == 4)
                {
                    menuItems.Add(new MenuItemsTree(elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]));
                }
                if (elements[i][0] == "0" && (elements[i].Length == 3))
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
                        menuItems[parentsID.Peek()].JoinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]));
                        parentsID.Push(menuItems.Count - 1);
                        parentsName.Push(elements[i][1]);
                    }
                    else
                    {
                        menuItems[parentsID.Peek()].JoinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]);
                    }
                }
                else if (Int32.Parse(elements[i][0]) > CurrentLevel)
                {
                    CurrentLevel++;
                    if (Int32.Parse(elements[i][0]) != CurrentLevel || parentsName.Count == 0)
                    {
                        throw new Exception("Неверная иерархия меню!");
                    }
                    if (elements[i].Length == 3)
                    {
                        menuItems[parentsID.Peek()].JoinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]));
                        parentsID.Push(menuItems.Count - 1); parentsName.Push(elements[i][1]);
                    }
                    else
                    {
                        menuItems[parentsID.Peek()].JoinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]);
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
                            menuItems[parentsID.Peek()].JoinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]));
                        }
                        else
                        {
                            menuItems[parentsID.Peek()].JoinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]);
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
            foreach (MenuItemsTree treeParent in menuItems)
            {
                SetProperties(treeParent);
                AddChildren(treeParent);
                menuStrip.Items.Add(treeParent.MenuItem);
            }
        }
        private void SetProperties(MenuItemsTree item)
        {
            if (item.nextLevelNodes.Count == 0 && item.ItemStatus == 0)
            {
                MethodArgs ee = new MethodArgs(item.ItemMethod);
                item.MenuItem.Click += new EventHandler((sender, e) => AddedItemClickEvent(sender, ee));
            }
            else if (item.ItemStatus == 1)
                item.MenuItem.Enabled = false;
            else if (item.ItemStatus == 2)
                item.MenuItem.Visible = false;
        }
        private void AddChildren(MenuItemsTree nonBinTree)
        {
            foreach (MenuItemsTree treeChild in nonBinTree.nextLevelNodes)
            {
                SetProperties(treeChild);
                AddChildren(treeChild);
                nonBinTree.MenuItem.DropDownItems.Add(treeChild.MenuItem);
            }
        }
        private void AddedItemClickEvent(object sender, MethodArgs e)
        {
            MessageBox.Show($"Вы нажали на {e.value} ");
        }
    }
}
