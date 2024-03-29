﻿namespace OPMedia.UI.Controls.Dialogs
{
    partial class OPMFileDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel3 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbFilter = new OPMedia.UI.Controls.OPMComboBox();
            this.txtFileNames = new OPMedia.UI.Controls.OPMTextBox();
            this.btnOK = new OPMedia.UI.Controls.OPMButton();
            this.btnCancel = new OPMedia.UI.Controls.OPMButton();
            this.lvExplorer = new OPMedia.UI.Controls.OPMShellListView();
            this.lblCurrentPath = new OPMedia.UI.Controls.OPMLabel();
            this.pnlButtons = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.btnNewFolder = new OPMedia.UI.Controls.OPMButton();
            this.btnAddToFavorites = new OPMedia.UI.Controls.OPMButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsSpecialFolders = new OPMedia.UI.Controls.OPMToolStrip();
            this.pnlLayout.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLayout
            // 
            this.pnlLayout.AutoSize = true;
            this.pnlLayout.ColumnCount = 7;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.pnlLayout.Controls.Add(this.opmLabel2, 0, 3);
            this.pnlLayout.Controls.Add(this.opmLabel3, 0, 5);
            this.pnlLayout.Controls.Add(this.cmbFilter, 1, 5);
            this.pnlLayout.Controls.Add(this.txtFileNames, 1, 3);
            this.pnlLayout.Controls.Add(this.btnOK, 4, 3);
            this.pnlLayout.Controls.Add(this.btnCancel, 4, 5);
            this.pnlLayout.Controls.Add(this.lvExplorer, 1, 1);
            this.pnlLayout.Controls.Add(this.lblCurrentPath, 0, 0);
            this.pnlLayout.Controls.Add(this.pnlButtons, 5, 0);
            this.pnlLayout.Controls.Add(this.panel1, 0, 1);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.RowCount = 7;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.pnlLayout.Size = new System.Drawing.Size(649, 520);
            this.pnlLayout.TabIndex = 0;
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.Location = new System.Drawing.Point(3, 459);
            this.opmLabel2.Margin = new System.Windows.Forms.Padding(3, 0, 4, 0);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(93, 23);
            this.opmLabel2.TabIndex = 4;
            this.opmLabel2.Text = "TXT_FILENAME:";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opmLabel3
            // 
            this.opmLabel3.AutoSize = true;
            this.opmLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel3.Location = new System.Drawing.Point(3, 490);
            this.opmLabel3.Margin = new System.Windows.Forms.Padding(3, 0, 4, 0);
            this.opmLabel3.Name = "opmLabel3";
            this.opmLabel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel3.Size = new System.Drawing.Size(93, 25);
            this.opmLabel3.TabIndex = 6;
            this.opmLabel3.Text = "TXT_FILE_TYPE:";
            this.opmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbFilter
            // 
            this.pnlLayout.SetColumnSpan(this.cmbFilter, 2);
            this.cmbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(100, 491);
            this.cmbFilter.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbFilter.Size = new System.Drawing.Size(456, 24);
            this.cmbFilter.TabIndex = 7;
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // txtFileNames
            // 
            this.txtFileNames.AutoSize = true;
            this.txtFileNames.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.txtFileNames.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.pnlLayout.SetColumnSpan(this.txtFileNames, 2);
            this.txtFileNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFileNames.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtFileNames.Lines = new string[0];
            this.txtFileNames.Location = new System.Drawing.Point(100, 460);
            this.txtFileNames.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.txtFileNames.MaximumSize = new System.Drawing.Size(3000, 22);
            this.txtFileNames.MaxLength = 32767;
            this.txtFileNames.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtFileNames.Name = "txtFileNames";
            this.txtFileNames.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtFileNames.PasswordChar = '\0';
            this.txtFileNames.ReadOnly = false;
            this.txtFileNames.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFileNames.ShortcutsEnabled = true;
            this.txtFileNames.Size = new System.Drawing.Size(456, 22);
            this.txtFileNames.TabIndex = 5;
            this.txtFileNames.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFileNames.UseSystemPasswordChar = false;
            this.txtFileNames.WordWrap = true;
            this.txtFileNames.TextChanged += new System.EventHandler(this.txtFileNames_TextChanged);
            // 
            // btnOK
            // 
            this.pnlLayout.SetColumnSpan(this.btnOK, 2);
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(564, 459);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOK.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOK.ShowDropDown = false;
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "TXT_OK";
            this.btnOK.Click += new System.EventHandler(this.OnOK);
            // 
            // btnCancel
            // 
            this.pnlLayout.SetColumnSpan(this.btnCancel, 2);
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(564, 490);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnCancel.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnCancel.ShowDropDown = false;
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "TXT_CANCEL";
            this.btnCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // lvExplorer
            // 
            this.lvExplorer.AllowEditing = true;
            this.lvExplorer.AlternateRowColors = true;
            this.lvExplorer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLayout.SetColumnSpan(this.lvExplorer, 5);
            this.lvExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvExplorer.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lvExplorer.LabelEdit = true;
            this.lvExplorer.Location = new System.Drawing.Point(100, 26);
            this.lvExplorer.Margin = new System.Windows.Forms.Padding(0);
            this.lvExplorer.MultiSelect = false;
            this.lvExplorer.Name = "lvExplorer";
            this.lvExplorer.OverrideBackColor = System.Drawing.Color.Empty;
            this.lvExplorer.Size = new System.Drawing.Size(544, 423);
            this.lvExplorer.TabIndex = 3;
            this.lvExplorer.UseCompatibleStateImageBehavior = false;
            this.lvExplorer.View = System.Windows.Forms.View.Details;
            // 
            // lblCurrentPath
            // 
            this.lblCurrentPath.AutoSize = true;
            this.pnlLayout.SetColumnSpan(this.lblCurrentPath, 5);
            this.lblCurrentPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrentPath.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.lblCurrentPath.Location = new System.Drawing.Point(5, 3);
            this.lblCurrentPath.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
            this.lblCurrentPath.Name = "lblCurrentPath";
            this.lblCurrentPath.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblCurrentPath.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblCurrentPath.Size = new System.Drawing.Size(595, 20);
            this.lblCurrentPath.TabIndex = 9;
            this.lblCurrentPath.Text = "opmLabel4";
            this.lblCurrentPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlButtons
            // 
            this.pnlButtons.AutoSize = true;
            this.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlButtons.Controls.Add(this.btnNewFolder);
            this.pnlButtons.Controls.Add(this.btnAddToFavorites);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(603, 3);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(41, 20);
            this.pnlButtons.TabIndex = 10;
            this.pnlButtons.WrapContents = false;
            // 
            // btnNewFolder
            // 
            this.btnNewFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewFolder.Location = new System.Drawing.Point(0, 0);
            this.btnNewFolder.Margin = new System.Windows.Forms.Padding(0);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnNewFolder.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnNewFolder.ShowDropDown = false;
            this.btnNewFolder.Size = new System.Drawing.Size(20, 20);
            this.btnNewFolder.TabIndex = 11;
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new System.EventHandler(this.btnNewFolder_Click);
            // 
            // btnAddToFavorites
            // 
            this.btnAddToFavorites.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAddToFavorites.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToFavorites.Location = new System.Drawing.Point(21, 0);
            this.btnAddToFavorites.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.btnAddToFavorites.MaximumSize = new System.Drawing.Size(20, 20);
            this.btnAddToFavorites.MinimumSize = new System.Drawing.Size(20, 20);
            this.btnAddToFavorites.Name = "btnAddToFavorites";
            this.btnAddToFavorites.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnAddToFavorites.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnAddToFavorites.ShowDropDown = false;
            this.btnAddToFavorites.Size = new System.Drawing.Size(20, 20);
            this.btnAddToFavorites.TabIndex = 10;
            this.btnAddToFavorites.UseVisualStyleBackColor = true;
            this.btnAddToFavorites.Click += new System.EventHandler(this.btnAddToFavorites_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tsSpecialFolders);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 423);
            this.panel1.TabIndex = 11;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // tsSpecialFolders
            // 
            this.tsSpecialFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.tsSpecialFolders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tsSpecialFolders.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsSpecialFolders.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsSpecialFolders.Location = new System.Drawing.Point(0, 0);
            this.tsSpecialFolders.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.tsSpecialFolders.MaximumSize = new System.Drawing.Size(110, 3700);
            this.tsSpecialFolders.MinimumSize = new System.Drawing.Size(80, 300);
            this.tsSpecialFolders.Name = "tsSpecialFolders";
            this.tsSpecialFolders.ShowBorder = true;
            this.tsSpecialFolders.Size = new System.Drawing.Size(100, 300);
            this.tsSpecialFolders.TabIndex = 2;
            this.tsSpecialFolders.Text = "opmToolStrip1";
            this.tsSpecialFolders.VerticalGradient = false;
            // 
            // OPMFileDialog
            // 
            this.ClientSize = new System.Drawing.Size(649, 520);
            this.Controls.Add(this.pnlLayout);
            this.MinimumSize = new System.Drawing.Size(650, 520);
            this.Name = "OPMFileDialog";
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OPMedia.UI.Controls.OPMTableLayoutPanel pnlLayout;
        private OPMedia.UI.Controls.OPMLabel opmLabel2;
        private OPMedia.UI.Controls.OPMLabel opmLabel3;
        private OPMedia.UI.Controls.OPMComboBox cmbFilter;
        private OPMedia.UI.Controls.OPMTextBox txtFileNames;
        private OPMedia.UI.Controls.OPMButton btnOK;
        private OPMedia.UI.Controls.OPMButton btnCancel;
        private OPMedia.UI.Controls.OPMLabel lblCurrentPath;
        protected internal OPMedia.UI.Controls.OPMShellListView lvExplorer;
        private OPMToolStrip tsSpecialFolders;
        private OPMButton btnAddToFavorites;
        private OPMFlowLayoutPanel pnlButtons;
        private OPMButton btnNewFolder;
        private System.Windows.Forms.Panel panel1;
    }
}