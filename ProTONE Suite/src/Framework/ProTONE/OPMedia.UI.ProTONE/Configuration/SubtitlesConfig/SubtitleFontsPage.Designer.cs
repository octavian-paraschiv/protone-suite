﻿using OPMedia.UI.Controls;
namespace OPMedia.UI.ProTONE.Configuration
{
    partial class SubtitleOsdPage
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
            this.chkFilterStateNotificationsEnabled = new OPMedia.UI.Controls.OPMCheckBox();
            this.lblOsdFont = new OPMedia.UI.Controls.OPMLinkLabel();
            this.lblOsdColor = new OPMedia.UI.Controls.OPMLinkLabel();
            this.nudOsdTmr = new OPMedia.UI.Controls.OPMNumericUpDown();
            this.tableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.chkSubEnabled = new OPMedia.UI.Controls.OPMCheckBox();
            this.flowLayoutPanel2 = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.lblSubFont = new OPMedia.UI.Controls.OPMLinkLabel();
            this.lblSubColor = new OPMedia.UI.Controls.OPMLinkLabel();
            this.flowLayoutPanel3 = new OPMedia.UI.Controls.OPMFlowLayoutPanel();
            this.tableLayoutPanel4 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.chkOsdEnabled = new OPMedia.UI.Controls.OPMCheckBox();
            this.label2 = new OPMedia.UI.Controls.OPMLabel();
            this.lblOsdText = new OPMedia.UI.ProTONE.Controls.OSD.OSDLabel();
            this.lblOsdText2 = new OPMedia.UI.ProTONE.Controls.OSD.OSDLabel();
            this.lblSubText2 = new OPMedia.UI.ProTONE.Controls.OSD.OSDLabel();
            this.lblSubText1 = new OPMedia.UI.ProTONE.Controls.OSD.OSDLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkFilterStateNotificationsEnabled
            // 
            this.chkFilterStateNotificationsEnabled.AccessibleName = "chkFilterStateNotificationsEnabled";
            this.chkFilterStateNotificationsEnabled.AutoSize = true;
            this.chkFilterStateNotificationsEnabled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkFilterStateNotificationsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFilterStateNotificationsEnabled.Location = new System.Drawing.Point(0, 390);
            this.chkFilterStateNotificationsEnabled.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.chkFilterStateNotificationsEnabled.Name = "chkFilterStateNotificationsEnabled";
            this.chkFilterStateNotificationsEnabled.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkFilterStateNotificationsEnabled.Size = new System.Drawing.Size(548, 21);
            this.chkFilterStateNotificationsEnabled.TabIndex = 8;
            this.chkFilterStateNotificationsEnabled.Text = "TXT_NOTIFICATIONSENABLED";
            // 
            // lblOsdFont
            // 
            this.lblOsdFont.AccessibleName = "lblOsdFont";
            this.lblOsdFont.AutoSize = true;
            this.lblOsdFont.Location = new System.Drawing.Point(0, 0);
            this.lblOsdFont.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblOsdFont.Name = "lblOsdFont";
            this.lblOsdFont.Size = new System.Drawing.Size(87, 15);
            this.lblOsdFont.TabIndex = 0;
            this.lblOsdFont.TabStop = true;
            this.lblOsdFont.Text = "TXT_OSDFONT";
            this.lblOsdFont.Click += new System.EventHandler(this.btnOsdFont_Click);
            // 
            // lblOsdColor
            // 
            this.lblOsdColor.AccessibleName = "lblOsdColor";
            this.lblOsdColor.AutoSize = true;
            this.lblOsdColor.Location = new System.Drawing.Point(87, 0);
            this.lblOsdColor.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblOsdColor.Name = "lblOsdColor";
            this.lblOsdColor.Size = new System.Drawing.Size(95, 15);
            this.lblOsdColor.TabIndex = 1;
            this.lblOsdColor.TabStop = true;
            this.lblOsdColor.Text = "TXT_OSDCOLOR";
            this.lblOsdColor.Click += new System.EventHandler(this.btnOsdColor_Click);
            // 
            // nudOsdTmr
            // 
            this.nudOsdTmr.AccessibleName = "nudOsdTmr";
            this.nudOsdTmr.DecimalPlaces = 0;
            this.nudOsdTmr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudOsdTmr.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.nudOsdTmr.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudOsdTmr.Location = new System.Drawing.Point(492, 0);
            this.nudOsdTmr.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.nudOsdTmr.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudOsdTmr.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudOsdTmr.MinimumSize = new System.Drawing.Size(10, 25);
            this.nudOsdTmr.Name = "nudOsdTmr";
            this.nudOsdTmr.OverrideForeColor = System.Drawing.Color.Empty;
            this.nudOsdTmr.ReadOnly = true;
            this.nudOsdTmr.Size = new System.Drawing.Size(56, 29);
            this.nudOsdTmr.TabIndex = 2;
            this.nudOsdTmr.Text = "0";
            this.nudOsdTmr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudOsdTmr.Unit = null;
            this.nudOsdTmr.UnitFirst = false;
            this.nudOsdTmr.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AccessibleName = "tableLayoutPanel1";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.chkSubEnabled, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkFilterStateNotificationsEnabled, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblOsdText, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblOsdText2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblSubText2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSubText1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(548, 414);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // chkSubEnabled
            // 
            this.chkSubEnabled.AccessibleName = "";
            this.chkSubEnabled.AutoSize = true;
            this.chkSubEnabled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkSubEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSubEnabled.Location = new System.Drawing.Point(0, 0);
            this.chkSubEnabled.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.chkSubEnabled.Name = "chkSubEnabled";
            this.chkSubEnabled.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkSubEnabled.Size = new System.Drawing.Size(548, 19);
            this.chkSubEnabled.TabIndex = 0;
            this.chkSubEnabled.Text = "TXT_SUBENABLED";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.lblSubFont);
            this.flowLayoutPanel2.Controls.Add(this.lblSubColor);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 166);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(548, 18);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // lblSubFont
            // 
            this.lblSubFont.AccessibleName = "lblOsdFont";
            this.lblSubFont.AutoSize = true;
            this.lblSubFont.Location = new System.Drawing.Point(0, 0);
            this.lblSubFont.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblSubFont.Name = "lblSubFont";
            this.lblSubFont.Size = new System.Drawing.Size(85, 15);
            this.lblSubFont.TabIndex = 0;
            this.lblSubFont.TabStop = true;
            this.lblSubFont.Text = "TXT_SUBFONT";
            this.lblSubFont.Click += new System.EventHandler(this.btnSubFont_Click);
            // 
            // lblSubColor
            // 
            this.lblSubColor.AccessibleName = "lblOsdColor";
            this.lblSubColor.AutoSize = true;
            this.lblSubColor.Location = new System.Drawing.Point(85, 0);
            this.lblSubColor.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblSubColor.Name = "lblSubColor";
            this.lblSubColor.Size = new System.Drawing.Size(93, 15);
            this.lblSubColor.TabIndex = 1;
            this.lblSubColor.TabStop = true;
            this.lblSubColor.Text = "TXT_SUBCOLOR";
            this.lblSubColor.Click += new System.EventHandler(this.btnSubColor_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel3.Controls.Add(this.lblOsdFont);
            this.flowLayoutPanel3.Controls.Add(this.lblOsdColor);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 366);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(548, 18);
            this.flowLayoutPanel3.TabIndex = 7;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel4.Controls.Add(this.chkOsdEnabled, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.nudOsdTmr, 2, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 190);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(548, 32);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // chkOsdEnabled
            // 
            this.chkOsdEnabled.AccessibleName = "chkOsdEnabled";
            this.chkOsdEnabled.AutoSize = true;
            this.chkOsdEnabled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkOsdEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkOsdEnabled.Location = new System.Drawing.Point(0, 0);
            this.chkOsdEnabled.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.chkOsdEnabled.Name = "chkOsdEnabled";
            this.chkOsdEnabled.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkOsdEnabled.Size = new System.Drawing.Size(246, 29);
            this.chkOsdEnabled.TabIndex = 0;
            this.chkOsdEnabled.Text = "TXT_OSDENABLED";
            // 
            // label2
            // 
            this.label2.AccessibleName = "label2";
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Location = new System.Drawing.Point(246, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 5, 3);
            this.label2.Name = "label2";
            this.label2.OverrideBackColor = System.Drawing.Color.Empty;
            this.label2.OverrideForeColor = System.Drawing.Color.Empty;
            this.label2.Size = new System.Drawing.Size(241, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "TXT_OSDTIMER";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOsdText
            // 
            this.lblOsdText.AccessibleName = "";
            this.lblOsdText.BackColor = System.Drawing.Color.Black;
            this.lblOsdText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOsdText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOsdText.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblOsdText.Location = new System.Drawing.Point(0, 222);
            this.lblOsdText.Margin = new System.Windows.Forms.Padding(0);
            this.lblOsdText.Name = "lblOsdText";
            this.lblOsdText.Size = new System.Drawing.Size(548, 72);
            this.lblOsdText.TabIndex = 5;
            this.lblOsdText.Text = "TXT_OSDSAMPLETEXT";
            // 
            // lblOsdText2
            // 
            this.lblOsdText2.AccessibleName = "lblSampleText2";
            this.lblOsdText2.BackColor = System.Drawing.Color.White;
            this.lblOsdText2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOsdText2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOsdText2.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblOsdText2.Location = new System.Drawing.Point(0, 294);
            this.lblOsdText2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblOsdText2.Name = "lblOsdText2";
            this.lblOsdText2.Size = new System.Drawing.Size(548, 69);
            this.lblOsdText2.TabIndex = 6;
            this.lblOsdText2.Text = "TXT_OSDSAMPLETEXT";
            // 
            // lblSubText2
            // 
            this.lblSubText2.AccessibleName = "lblSampleText2";
            this.lblSubText2.BackColor = System.Drawing.Color.White;
            this.lblSubText2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSubText2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubText2.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblSubText2.Location = new System.Drawing.Point(0, 94);
            this.lblSubText2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblSubText2.Name = "lblSubText2";
            this.lblSubText2.Size = new System.Drawing.Size(548, 69);
            this.lblSubText2.TabIndex = 2;
            this.lblSubText2.Text = "TXT_SUBSAMPLETEXT";
            // 
            // lblSubText1
            // 
            this.lblSubText1.AccessibleName = "lblSampleText";
            this.lblSubText1.BackColor = System.Drawing.Color.Black;
            this.lblSubText1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSubText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubText1.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.lblSubText1.Location = new System.Drawing.Point(0, 22);
            this.lblSubText1.Margin = new System.Windows.Forms.Padding(0);
            this.lblSubText1.Name = "lblSubText1";
            this.lblSubText1.Size = new System.Drawing.Size(548, 72);
            this.lblSubText1.TabIndex = 1;
            this.lblSubText1.Text = "TXT_SUBSAMPLETEXT";
            // 
            // SubtitleOsdPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SubtitleOsdPage";
            this.Size = new System.Drawing.Size(548, 414);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OPMCheckBox chkFilterStateNotificationsEnabled;
        private OPMLinkLabel lblOsdFont;
        private OPMLinkLabel lblOsdColor;
        private OPMNumericUpDown nudOsdTmr;
        private OPMTableLayoutPanel tableLayoutPanel1;
        private Controls.OSD.OSDLabel lblOsdText2;
        private Controls.OSD.OSDLabel lblOsdText;
        private OPMCheckBox chkOsdEnabled;
        private Controls.OSD.OSDLabel lblSubText2;
        private Controls.OSD.OSDLabel lblSubText1;
        private OPMFlowLayoutPanel flowLayoutPanel2;
        private OPMLinkLabel lblSubFont;
        private OPMLinkLabel lblSubColor;
        private OPMFlowLayoutPanel flowLayoutPanel3;
        private OPMTableLayoutPanel tableLayoutPanel4;
        private OPMCheckBox chkSubEnabled;
        private OPMLabel label2;
    }
}
