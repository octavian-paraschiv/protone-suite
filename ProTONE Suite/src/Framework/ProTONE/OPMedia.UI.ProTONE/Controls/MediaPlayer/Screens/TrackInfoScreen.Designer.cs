﻿namespace OPMedia.UI.ProTONE.Controls.MediaPlayer.Screens
{
    partial class TrackInfoScreen
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
            this.pgProperties = new OPMedia.UI.Controls.OPMPropertyGrid();
            this.pnlLayout = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblItem = new OPMedia.UI.Controls.OPMLabel();
            this.layoutPanelInner = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.lblNoInfo = new OPMedia.UI.Controls.OPMLabel();
            this.playlistScreen = new OPMedia.UI.ProTONE.Controls.MediaPlayer.PlaylistScreen();
            this.pnlLayout.SuspendLayout();
            this.layoutPanelInner.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgProperties
            // 
            this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgProperties.HelpVisible = false;
            this.pgProperties.Location = new System.Drawing.Point(5, 18);
            this.pgProperties.Margin = new System.Windows.Forms.Padding(5);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Size = new System.Drawing.Size(254, 301);
            this.pgProperties.TabIndex = 1;
            this.pgProperties.ToolbarVisible = false;
            // 
            // pnlLayout
            // 
            this.pnlLayout.ColumnCount = 2;
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlLayout.Controls.Add(this.lblItem, 0, 0);
            this.pnlLayout.Controls.Add(this.playlistScreen, 0, 1);
            this.pnlLayout.Controls.Add(this.layoutPanelInner, 1, 1);
            this.pnlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlLayout.Name = "pnlLayout";
            this.pnlLayout.OverrideBackColor = System.Drawing.Color.Empty;
            this.pnlLayout.RowCount = 2;
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlLayout.Size = new System.Drawing.Size(527, 343);
            this.pnlLayout.TabIndex = 2;
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.pnlLayout.SetColumnSpan(this.lblItem, 2);
            this.lblItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblItem.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.lblItem.Location = new System.Drawing.Point(0, 3);
            this.lblItem.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.lblItem.Name = "lblItem";
            this.lblItem.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblItem.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblItem.Size = new System.Drawing.Size(527, 13);
            this.lblItem.TabIndex = 1;
            this.lblItem.Text = "[ item ]";
            this.lblItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layoutPanelInner
            // 
            this.layoutPanelInner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            this.layoutPanelInner.Controls.Add(this.lblNoInfo, 0, 0);
            this.layoutPanelInner.Controls.Add(this.pgProperties, 0, 1);
            this.layoutPanelInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanelInner.Location = new System.Drawing.Point(263, 19);
            this.layoutPanelInner.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPanelInner.Name = "layoutPanelInner";
            this.layoutPanelInner.OverrideBackColor = System.Drawing.Color.Empty;
            this.layoutPanelInner.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPanelInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanelInner.Size = new System.Drawing.Size(264, 324);
            this.layoutPanelInner.TabIndex = 6;
            // 
            // lblNoInfo
            // 
            this.lblNoInfo.AutoSize = true;
            this.lblNoInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblNoInfo.Location = new System.Drawing.Point(3, 0);
            this.lblNoInfo.Name = "lblNoInfo";
            this.lblNoInfo.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblNoInfo.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblNoInfo.Size = new System.Drawing.Size(258, 13);
            this.lblNoInfo.TabIndex = 0;
            this.lblNoInfo.Text = "TXT_THEREARENOITEMS";
            // 
            // playlistScreen
            // 
            this.playlistScreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playlistScreen.CompactMode = true;
            this.playlistScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistScreen.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.playlistScreen.Location = new System.Drawing.Point(0, 19);
            this.playlistScreen.Margin = new System.Windows.Forms.Padding(0);
            this.playlistScreen.Name = "playlistScreen";
            this.playlistScreen.OverrideBackColor = System.Drawing.Color.Empty;
            this.playlistScreen.Size = new System.Drawing.Size(263, 324);
            this.playlistScreen.TabIndex = 5;
            // 
            // TrackInfoScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlLayout);
            this.Name = "TrackInfoScreen";
            this.Size = new System.Drawing.Size(527, 343);
            this.pnlLayout.ResumeLayout(false);
            this.pnlLayout.PerformLayout();
            this.layoutPanelInner.ResumeLayout(false);
            this.layoutPanelInner.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMPropertyGrid pgProperties;
        private UI.Controls.OPMTableLayoutPanel pnlLayout;
        private UI.Controls.OPMLabel lblItem;
        private PlaylistScreen playlistScreen;
        private UI.Controls.OPMTableLayoutPanel layoutPanelInner;
        private UI.Controls.OPMLabel lblNoInfo;
    }
}
