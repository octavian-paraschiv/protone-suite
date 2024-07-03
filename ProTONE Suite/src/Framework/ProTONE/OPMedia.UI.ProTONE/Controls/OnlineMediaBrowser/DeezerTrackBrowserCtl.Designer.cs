namespace OPMedia.UI.ProTONE.Controls.OnlineMediaBrowser
{
    partial class DeezerTrackBrowserCtl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lvTracks = new OPMedia.UI.Controls.OPMListView();
            this.colEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAlbum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel3 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbArtist = new System.Windows.Forms.ComboBox();
            this.cmbAlbum = new System.Windows.Forms.ComboBox();
            this.cmbTitle = new System.Windows.Forms.ComboBox();
            this.btnSearch = new OPMedia.UI.Controls.OPMButton();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTracks
            // 
            this.lvTracks.AllowEditing = false;
            this.lvTracks.AlternateRowColors = true;
            this.lvTracks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEmpty,
            this.colName,
            this.colArtist,
            this.colAlbum,
            this.colURL});
            this.opmTableLayoutPanel1.SetColumnSpan(this.lvTracks, 7);
            this.lvTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTracks.Location = new System.Drawing.Point(0, 0);
            this.lvTracks.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lvTracks.MultiSelect = false;
            this.lvTracks.Name = "lvTracks";
            this.lvTracks.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvTracks.Size = new System.Drawing.Size(663, 466);
            this.lvTracks.TabIndex = 2;
            this.lvTracks.UseCompatibleStateImageBehavior = false;
            this.lvTracks.View = System.Windows.Forms.View.Details;
            // 
            // colEmpty
            // 
            this.colEmpty.Text = "";
            this.colEmpty.Width = 0;
            // 
            // colName
            // 
            this.colName.Text = "TXT_NAME";
            this.colName.Width = 116;
            // 
            // colArtist
            // 
            this.colArtist.Text = "TXT_ARTIST";
            // 
            // colAlbum
            // 
            this.colAlbum.Text = "TXT_ALBUM";
            this.colAlbum.Width = 105;
            // 
            // colURL
            // 
            this.colURL.Text = "URL";
            this.colURL.Width = 113;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 7;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.Controls.Add(this.lvTracks, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel2, 2, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel3, 4, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbArtist, 1, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbAlbum, 3, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbTitle, 5, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.btnSearch, 6, 1);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 2;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(663, 494);
            this.opmTableLayoutPanel1.TabIndex = 3;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.opmLabel1.Location = new System.Drawing.Point(0, 471);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(78, 23);
            this.opmLabel1.TabIndex = 3;
            this.opmLabel1.Text = "TXT_ARTIST:";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.opmLabel2.Location = new System.Drawing.Point(199, 471);
            this.opmLabel2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(79, 23);
            this.opmLabel2.TabIndex = 4;
            this.opmLabel2.Text = "TXT_ALBUM:";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opmLabel3
            // 
            this.opmLabel3.AutoSize = true;
            this.opmLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel3.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.opmLabel3.Location = new System.Drawing.Point(399, 471);
            this.opmLabel3.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.opmLabel3.Name = "opmLabel3";
            this.opmLabel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel3.Size = new System.Drawing.Size(67, 23);
            this.opmLabel3.TabIndex = 5;
            this.opmLabel3.Text = "TXT_TITLE:";
            this.opmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbArtist
            // 
            this.cmbArtist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbArtist.Location = new System.Drawing.Point(81, 471);
            this.cmbArtist.Margin = new System.Windows.Forms.Padding(0);
            this.cmbArtist.MaximumSize = new System.Drawing.Size(3000, 0);
            this.cmbArtist.MaxLength = 32767;
            this.cmbArtist.MinimumSize = new System.Drawing.Size(22, 0);
            this.cmbArtist.Name = "cmbArtist";
            this.cmbArtist.Size = new System.Drawing.Size(118, 23);
            this.cmbArtist.TabIndex = 6;
            this.cmbArtist.TextChanged += new System.EventHandler(this.OnSearchTextChanged);
            this.cmbArtist.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbSearch_KeyUp);
            // 
            // cmbAlbum
            // 
            this.cmbAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAlbum.Location = new System.Drawing.Point(281, 471);
            this.cmbAlbum.Margin = new System.Windows.Forms.Padding(0);
            this.cmbAlbum.MaximumSize = new System.Drawing.Size(3000, 0);
            this.cmbAlbum.MaxLength = 32767;
            this.cmbAlbum.MinimumSize = new System.Drawing.Size(22, 0);
            this.cmbAlbum.Name = "cmbAlbum";
            this.cmbAlbum.Size = new System.Drawing.Size(118, 23);
            this.cmbAlbum.TabIndex = 7;
            this.cmbAlbum.TextChanged += new System.EventHandler(this.OnSearchTextChanged);
            this.cmbAlbum.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbSearch_KeyUp);
            // 
            // cmbTitle
            // 
            this.cmbTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTitle.Location = new System.Drawing.Point(469, 471);
            this.cmbTitle.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.cmbTitle.MaximumSize = new System.Drawing.Size(3000, 0);
            this.cmbTitle.MaxLength = 32767;
            this.cmbTitle.MinimumSize = new System.Drawing.Size(22, 0);
            this.cmbTitle.Name = "cmbTitle";
            this.cmbTitle.Size = new System.Drawing.Size(115, 23);
            this.cmbTitle.TabIndex = 8;
            this.cmbTitle.TextChanged += new System.EventHandler(this.OnSearchTextChanged);
            this.cmbTitle.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbSearch_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Location = new System.Drawing.Point(587, 471);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnSearch.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnSearch.ShowDropDown = false;
            this.btnSearch.Size = new System.Drawing.Size(76, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "TXT_SEARCH";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // DeezerTrackBrowserCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Name = "DeezerTrackBrowserCtl";
            this.Size = new System.Drawing.Size(663, 494);
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMListView lvTracks;
        private System.Windows.Forms.ColumnHeader colEmpty;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colURL;
        private System.Windows.Forms.ColumnHeader colArtist;
        private System.Windows.Forms.ColumnHeader colAlbum;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMLabel opmLabel2;
        private UI.Controls.OPMLabel opmLabel3;
        private System.Windows.Forms.ComboBox cmbArtist;
        private System.Windows.Forms.ComboBox cmbAlbum;
        private System.Windows.Forms.ComboBox cmbTitle;
        private UI.Controls.OPMButton btnSearch;
    }
}
