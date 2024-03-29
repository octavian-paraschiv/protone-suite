using OPMedia.Core;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.UI.Controls;
using OPMedia.UI.Dialogs;
using OPMedia.UI.Properties;
using OPMedia.UI.Themes;
using System;

namespace OPMedia.Addons.Builtin.FileExplorer
{
    public class FavoriteFoldersManager : ThemeForm
    {
        private FavoriteFoldersControl favoriteFoldersControl;
        private OPMButton btnOK;

        public FavoriteFoldersManager(string favFoldersHiveName)
            : base("TXT_MANAGE_FAVORITES")
        {
            InitializeComponent();

            favoriteFoldersControl.FavoriteFoldersHiveName = favFoldersHiveName;
            favoriteFoldersControl.ShowOKButton = true;

            this.Load += new EventHandler(FavoriteFoldersManager_Load);
        }

        void FavoriteFoldersManager_Load(object sender, EventArgs e)
        {
            this.InheritAppIcon = false;
            this.Icon = Resources.Favorites16.ToIcon();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ProTONEConfig.SetFavoriteFolders(favoriteFoldersControl.FavoriteFolders,
                favoriteFoldersControl.FavoriteFoldersHiveName);
        }

        private void InitializeComponent()
        {
            this.favoriteFoldersControl = new OPMedia.UI.Dialogs.FavoriteFoldersControl();
            this.btnOK = new OPMedia.UI.Controls.OPMButton();
            this.SuspendLayout();
            // 
            // favoriteFoldersControl
            // 
            this.favoriteFoldersControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favoriteFoldersControl.FavoriteFoldersHiveName = null;
            this.favoriteFoldersControl.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.favoriteFoldersControl.Location = new System.Drawing.Point(0, 0);
            this.favoriteFoldersControl.Name = "favoriteFoldersControl";
            this.favoriteFoldersControl.ShowOKButton = true;
            this.favoriteFoldersControl.Size = new System.Drawing.Size(398, 200);
            this.favoriteFoldersControl.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(470, 219);
            this.btnOK.Name = "btnOK";
            this.btnOK.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOK.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOK.ShowDropDown = false;
            this.btnOK.Size = new System.Drawing.Size(61, 27);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "TXT_OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FavoriteFoldersManager
            // 
            this.ClientSize = new System.Drawing.Size(398, 200);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.favoriteFoldersControl);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "FavoriteFoldersManager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
