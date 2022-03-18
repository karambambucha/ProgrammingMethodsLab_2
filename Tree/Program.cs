using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Tree
{
    class Program
    {
        
        static void Main(string[] args)
        {
            List<string[]> elements = new List<string[]>();
            StreamReader f = new StreamReader("text.txt");
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
                if(elements[i][0] == "0" && elements[i].Length == 4)
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
                        parentsID.Push(menuItems.Count - 1); parentsName.Push(elements[i][1]);
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
                            parentsID.Push(menuItems.Count - 1); parentsName.Push(elements[i][1]);
                        }
                        else
                        {
                            menuItems[parentsID.Peek()].joinToNode(parentsName.Peek(), elements[i][1], Int32.Parse(elements[i][2]), elements[i][3]);
                        }
                    }
                    if (elements[i].Length == 3)
                    {
                        parentsID.Push(menuItems.Count - 1); parentsName.Push(elements[i][1]);
                    }
                }
                i++;
            }
            foreach (MenuItemsTree item in menuItems)
            {
                item.Print(item);
                Console.WriteLine();
            }
        }
    }
}
