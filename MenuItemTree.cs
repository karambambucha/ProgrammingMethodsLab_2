using System.Collections.Generic;
using System.Windows.Forms;

namespace MenuCreator
{
    public class MenuItemsTree
    {
        public List<MenuItemsTree> nextLevelNodes = new List<MenuItemsTree>();
        public int ItemStatus { get; private set; }
        public string ItemName { get; private set; }
        public string ItemMethod { get; private set; }
        public ToolStripMenuItem MenuItem { get; private set; }
        public MenuItemsTree(string value, int status, string method)
        {
            ItemStatus = status;
            ItemName = value;
            ItemMethod = method;
            MenuItem = new ToolStripMenuItem(ItemName);
        }
        public MenuItemsTree(string value, int status)
        {
            ItemStatus = status;
            ItemName = value;
            MenuItem = new ToolStripMenuItem(ItemName);
        }
        public MenuItemsTree FindNode(string nodeData_)
        {
            if (ItemName == nodeData_)
            {
                return this;
            }
            foreach (MenuItemsTree item in nextLevelNodes)
            {
                if (item.ItemName == nodeData_) return item;
                MenuItemsTree ret = item.FindNode(nodeData_);
                if (ret != null) return ret;
            }
            return null;
        }
        public bool JoinToNode(string val, string toJoin, int status, string method)
        {
            MenuItemsTree joined = FindNode(val);
            if (joined == null)
            {
                return false;
            }
            else
            {
                MenuItemsTree newTree = new MenuItemsTree(toJoin, status, method);
                if (status == 2)
                {
                    newTree.MenuItem.Visible = false;
                }

                joined.nextLevelNodes.Add(newTree);
                return true;
            }
        }
        public bool JoinToNode(string val, string toJoin, int status)
        {
            MenuItemsTree joined = FindNode(val);
            if (joined == null)
            {
                return false;
            }
            else
            {
                MenuItemsTree newTree = new MenuItemsTree(toJoin, status);
                if (status == 2)
                {
                    newTree.MenuItem.Visible = false;
                }
                joined.nextLevelNodes.Add(newTree);
                return true;
            }
        }
    }
}
