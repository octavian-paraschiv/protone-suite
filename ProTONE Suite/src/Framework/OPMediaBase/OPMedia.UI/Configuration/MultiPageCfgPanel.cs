using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using OPMedia.UI.Themes;
using OPMedia.Core.Configuration;
using OPMedia.UI.Properties;
using OPMedia.UI.Controls;
using OPMedia.Core;
using OPMedia.Core.TranslationSupport;

namespace OPMedia.UI.Configuration
{
    public partial class MultiPageCfgPanel : BaseCfgPanel, IMultiPageCfgPanel
    {
        public MultiPageCfgPanel()
            : base()
        {
            InitializeComponent();

            tabSubPages.SizeMode = TabSizeMode.Fixed;
            tabSubPages.ItemSize = new System.Drawing.Size(110, 28);

            tabSubPages.ImageList = new ImageList();
            tabSubPages.ImageList.ImageSize = new System.Drawing.Size(24, 24);
        }

        public void AddSubPage(BaseCfgPanel page)
        {
            string title = Translator.Translate(page.Title);

            page.Dock = DockStyle.Fill;

            OPMTabPage tp = new OPMTabPage(title, page);
            tp.Dock = DockStyle.Fill;
            tp.ImageIndex = tabSubPages.ImageList.Images.Count;
            tp.Tag = page.Title;
            tp.Name = page.Name;
            
            tabSubPages.ImageList.Images.Add(page.Image);
            tabSubPages.TabPages.Add(tp);

            if (page.RequestedItemSize != null)
                tabSubPages.ItemSize = page.RequestedItemSize.Value;

            page.ModifiedActive -= new EventHandler(OnModifiedActive);
            page.ModifiedActive += new EventHandler(OnModifiedActive);
        }

        void OnModifiedActive(object sender, EventArgs e)
        {
            Modified = true;
        }

        protected override void SaveInternal()
        {
            foreach (OPMTabPage tp in tabSubPages.TabPages)
            {
                BaseCfgPanel page = tp.Control as BaseCfgPanel;
                if (page != null)
                {
                    page.Save();
                }
            }

            Modified = false;
        }

        protected override void DiscardInternal()
        {
            foreach (OPMTabPage tp in tabSubPages.TabPages)
            {
                BaseCfgPanel page = tp.Control as BaseCfgPanel;
                if (page != null)
                {
                    page.Discard();
                }
            }

            Modified = false;
        }


        void IMultiPageCfgPanel.SelectSubPage(string itemName)
        {
            foreach (OPMTabPage tp in tabSubPages.TabPages)
            {
                if (tp.Text == itemName)
                {
                    tabSubPages.SelectedTab = tp;
                    break;
                }
            }
        }

        public override string GetHelpTopic()
        {
            if (tabSubPages.SelectedTab != null)
                return string.Format("{0}/{1}", this.Name, tabSubPages.SelectedTab.Name);

            return base.GetHelpTopic();
        }
    }

    public interface IMultiPageCfgPanel
    {
        void SelectSubPage(string itemName);
    }
}