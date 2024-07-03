using OPMedia.UI.Controls;
using System.Windows.Forms;

namespace OPMedia.UI.Menus
{
    public class MenuWrapper<T>
    {
        T _targetMenu = default(T);

        public ToolStripItemCollection MenuItems
        {
            get
            {
                if (_targetMenu is OPMMenuStrip)
                {
                    return (_targetMenu as OPMMenuStrip).Items;
                }

                else if (_targetMenu is ContextMenuStrip)
                {
                    return (_targetMenu as ContextMenuStrip).Items;
                }

                else if (_targetMenu is ToolStripMenuItem)
                {
                    return (_targetMenu as ToolStripMenuItem).DropDownItems;
                }

                return null;
            }
        }

        public int MenuItemsCount
        {
            get
            {
                return (MenuItems != null) ? MenuItems.Count : 0;
            }
        }

        public MenuWrapper(T targetMenu)
        {
            _targetMenu = targetMenu;
        }

        public void AddMultipleEntries(ToolStripItem[] itemsToAdd)
        {
            foreach (ToolStripItem tsi in itemsToAdd)
            {
                AddSingleEntry(tsi);
            }
        }

        public int AddSingleEntry(ToolStripItem tsi)
        {
            return MenuItems.Add(tsi);
        }

        public void InsertSingleEntry(int index, ToolStripItem tsi)
        {
            MenuItems.Insert(index, tsi);
        }
    }


}
