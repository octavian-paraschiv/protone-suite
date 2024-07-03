using OPMedia.UI.Configuration;
using System;

namespace OPMedia.UI.ProTONE.Configuration.MiscConfig
{
    public partial class FavoriteFoldersPage : BaseCfgPanel
    {
        public FavoriteFoldersPage()
        {
            InitializeComponent();
            favoriteFoldersControl.ShowOKButton = false;
            favoriteFoldersControl.FavoriteFoldersHiveName = "FavoriteFolders";
            favoriteFoldersControl.OnModified += new EventHandler(favoriteFoldersControl_OnModified);
        }

        void favoriteFoldersControl_OnModified(object sender, EventArgs e)
        {
            Modified = true;
        }

        protected override void SaveInternal()
        {
            favoriteFoldersControl.Save();
        }
    }
}
