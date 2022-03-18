using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProgrammingMethodsLab_2
{
    public class MenuItemsTree
    {
        public List<MenuItemsTree> nextLevelNodes = new List<MenuItemsTree>();
        public int itemStatus { get; private set; }
        public string itemName { get; private set; }
        public string itemMethod { get; private set; }
        public ToolStripMenuItem menuItem { get; private set; }
        public MenuItemsTree(string value, int status, string method)
        {
            itemStatus = status;
            itemName = value;
            itemMethod = method;
            menuItem = new ToolStripMenuItem(itemName);
        }
        public MenuItemsTree(string value, int status)
        {
            itemStatus = status;
            itemName = value;
            menuItem = new ToolStripMenuItem(itemName);
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
                if (status == 2)
                {
                    newTree.menuItem.Visible = false;
                }

                joined.nextLevelNodes.Add(newTree);
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
                if (status == 2)
                {
                    newTree.menuItem.Visible = false;
                }
                joined.nextLevelNodes.Add(newTree);
                return true;
            }
        }
    }
}
