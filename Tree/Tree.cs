using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tree
{
    public class MenuItemsTree
    {
        public List<MenuItemsTree> nextLevelNodes = new List<MenuItemsTree>();
        public int itemStatus { get; private set; }
        public string itemName { get; private set; }
        public string itemMethod { get; private set; }
        public MenuItemsTree(string value, int status, string method)
        {
            itemStatus = status;
            itemName = value;
            itemMethod = method;
        }
        public MenuItemsTree(string value, int status)
        {
            itemStatus = status;
            itemName = value;
        }
        public MenuItemsTree findNode(string nodeData_)
        {
            if (itemName == nodeData_)
            {
                return this;
            }
            foreach (MenuItemsTree item in nextLevelNodes)
            {
                if (item.itemName == nodeData_) return item;
                MenuItemsTree ret = item.findNode(nodeData_);
                if (ret != null) return ret;
            }
            return null;
        }
        public bool joinToNode(string val, string toJoin, int status, string method)
        {
            MenuItemsTree joined = findNode(val);
            if (joined == null)
            {
                return false;
            }
            else
            {
                MenuItemsTree newTree = new MenuItemsTree(toJoin, status, method);
                joined.nextLevelNodes.Add(newTree);
                Console.WriteLine($"Добавление {newTree.itemName} к {joined.itemName}");
                Console.WriteLine($"Колво детей у {joined.itemName} : {joined.nextLevelNodes.Count()}\n");
                return true;
            }
        }
        public bool joinToNode(string val, string toJoin, int status)
        {
            MenuItemsTree joined = findNode(val);
            if (joined == null)
            {
                return false;
            }
            else
            {
                MenuItemsTree newTree = new MenuItemsTree(toJoin, status);
                joined.nextLevelNodes.Add(newTree);
                Console.WriteLine($"Добавление {newTree.itemName} к {joined.itemName}");
                Console.WriteLine($"Колво детей у {joined.itemName} : {joined.nextLevelNodes.Count()}\n");
                return true;
            }
        }
        public void Print(MenuItemsTree polyTree, int level = 0)
        {
            Console.WriteLine($"Level: {level}, {polyTree.itemName}, {polyTree.itemStatus}, {polyTree.itemMethod}");
            foreach(MenuItemsTree tree in polyTree.nextLevelNodes)
            {
                Print(tree, level+1);
            }
        }
    }
}