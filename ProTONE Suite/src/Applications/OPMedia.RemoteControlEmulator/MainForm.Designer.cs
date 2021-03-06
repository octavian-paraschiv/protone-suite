﻿namespace OPMedia.RemoteControlEmulator
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabEmulator = new OPMedia.UI.Controls.OPMTabControl();
            this.tpApi = new System.Windows.Forms.TabPage();
            this.opmTableLayoutPanel2 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmGroupBox1 = new OPMedia.UI.Controls.OPMGroupBox();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.lblPlaybackCmd = new OPMedia.UI.Controls.OPMLabel();
            this.cmbCommandType = new OPMedia.UI.Controls.OPMComboBox();
            this.cmbPlaybackCmd = new OPMedia.UI.Controls.OPMComboBox();
            this.opmGroupBox2 = new OPMedia.UI.Controls.OPMGroupBox();
            this.opmTableLayoutPanel3 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.cmbDestination = new OPMedia.UI.Controls.OPMComboBox();
            this.opmLabel3 = new OPMedia.UI.Controls.OPMLabel();
            this.txtDestinationName = new OPMedia.UI.Controls.OPMTextBox();
            this.btnExecute = new OPMedia.UI.Controls.OPMButton();
            this.txtResult = new OPMedia.UI.Controls.OPMTextBox();
            this.tpRemoteControl = new System.Windows.Forms.TabPage();
            this.pnlSimulator = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.opmButton6 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton5 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton4 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton11 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton10 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton9 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton16 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton15 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton14 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton7 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton8 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton12 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton2 = new OPMedia.UI.Controls.OPMButton();
            this.ilX = new System.Windows.Forms.ImageList(this.components);
            this.opmButton3 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton26 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton27 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton24 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton25 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton23 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton20 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton21 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton13 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton17 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton18 = new OPMedia.UI.Controls.OPMButton();
            this.opmButton19 = new OPMedia.UI.Controls.OPMButton();
            this.tpMockup = new System.Windows.Forms.TabPage();
            this.opmTableLayoutPanel4 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.btnPlayer = new OPMedia.UI.Controls.OPMButton();
            this.btnRemote = new OPMedia.UI.Controls.OPMButton();
            this.btnMediaLib = new OPMedia.UI.Controls.OPMButton();
            this.lblDesc = new OPMedia.UI.Controls.OPMLabel();
            this.tpWCF = new System.Windows.Forms.TabPage();
            this.tbWCFDetails = new System.Windows.Forms.TextBox();
            this.opmNumericUpDown1 = new OPMedia.UI.Controls.OPMNumericUpDown();
            this.opmNumericTextBox1 = new OPMedia.UI.Controls.OPMNumericTextBox();
            this.pnlContent.SuspendLayout();
            this.tabEmulator.SuspendLayout();
            this.tpApi.SuspendLayout();
            this.opmTableLayoutPanel2.SuspendLayout();
            this.opmGroupBox1.SuspendLayout();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.opmGroupBox2.SuspendLayout();
            this.opmTableLayoutPanel3.SuspendLayout();
            this.tpRemoteControl.SuspendLayout();
            this.pnlSimulator.SuspendLayout();
            this.tpMockup.SuspendLayout();
            this.opmTableLayoutPanel4.SuspendLayout();
            this.tpWCF.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.tabEmulator);
            // 
            // tabEmulator
            // 
            this.tabEmulator.Controls.Add(this.tpApi);
            this.tabEmulator.Controls.Add(this.tpRemoteControl);
            this.tabEmulator.Controls.Add(this.tpMockup);
            this.tabEmulator.Controls.Add(this.tpWCF);
            this.tabEmulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEmulator.InnerPadding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tabEmulator.Location = new System.Drawing.Point(0, 0);
            this.tabEmulator.Multiline = true;
            this.tabEmulator.Name = "tabEmulator";
            this.tabEmulator.SelectedIndex = 0;
            this.tabEmulator.Size = new System.Drawing.Size(441, 430);
            this.tabEmulator.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabEmulator.TabIndex = 0;
            // 
            // tpApi
            // 
            this.tpApi.BackColor = System.Drawing.Color.White;
            this.tpApi.Controls.Add(this.opmTableLayoutPanel2);
            this.tpApi.Location = new System.Drawing.Point(4, 23);
            this.tpApi.Name = "tpApi";
            this.tpApi.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tpApi.Size = new System.Drawing.Size(433, 403);
            this.tpApi.TabIndex = 0;
            this.tpApi.Text = "API";
            // 
            // opmTableLayoutPanel2
            // 
            this.opmTableLayoutPanel2.ColumnCount = 1;
            this.opmTableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel2.Controls.Add(this.opmGroupBox1, 0, 0);
            this.opmTableLayoutPanel2.Controls.Add(this.opmGroupBox2, 0, 1);
            this.opmTableLayoutPanel2.Controls.Add(this.btnExecute, 0, 2);
            this.opmTableLayoutPanel2.Controls.Add(this.txtResult, 0, 3);
            this.opmTableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel2.Location = new System.Drawing.Point(5, 10);
            this.opmTableLayoutPanel2.Name = "opmTableLayoutPanel2";
            this.opmTableLayoutPanel2.RowCount = 4;
            this.opmTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel2.Size = new System.Drawing.Size(423, 388);
            this.opmTableLayoutPanel2.TabIndex = 1;
            // 
            // opmGroupBox1
            // 
            this.opmGroupBox1.AutoSize = true;
            this.opmGroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.opmGroupBox1.Controls.Add(this.opmTableLayoutPanel1);
            this.opmGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmGroupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opmGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.opmGroupBox1.Name = "opmGroupBox1";
            this.opmGroupBox1.Size = new System.Drawing.Size(417, 73);
            this.opmGroupBox1.TabIndex = 0;
            this.opmGroupBox1.TabStop = false;
            this.opmGroupBox1.Text = "Command Parameters";
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.AutoSize = true;
            this.opmTableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.opmTableLayoutPanel1.ColumnCount = 2;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.lblPlaybackCmd, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbCommandType, 1, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.cmbPlaybackCmd, 1, 1);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 3;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(411, 52);
            this.opmTableLayoutPanel1.TabIndex = 0;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.Location = new System.Drawing.Point(13, 0);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(35, 26);
            this.opmLabel1.TabIndex = 0;
            this.opmLabel1.Text = "Type:";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPlaybackCmd
            // 
            this.lblPlaybackCmd.AutoSize = true;
            this.lblPlaybackCmd.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPlaybackCmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPlaybackCmd.Location = new System.Drawing.Point(3, 26);
            this.lblPlaybackCmd.Name = "lblPlaybackCmd";
            this.lblPlaybackCmd.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblPlaybackCmd.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblPlaybackCmd.Size = new System.Drawing.Size(45, 26);
            this.lblPlaybackCmd.TabIndex = 1;
            this.lblPlaybackCmd.Text = "Action:";
            this.lblPlaybackCmd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCommandType
            // 
            this.cmbCommandType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCommandType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbCommandType.FormattingEnabled = true;
            this.cmbCommandType.Location = new System.Drawing.Point(54, 3);
            this.cmbCommandType.Name = "cmbCommandType";
            this.cmbCommandType.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbCommandType.Size = new System.Drawing.Size(354, 20);
            this.cmbCommandType.TabIndex = 2;
            this.cmbCommandType.SelectedIndexChanged += new System.EventHandler(this.cmbCommandType_SelectedIndexChanged);
            // 
            // cmbPlaybackCmd
            // 
            this.cmbPlaybackCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPlaybackCmd.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPlaybackCmd.FormattingEnabled = true;
            this.cmbPlaybackCmd.Location = new System.Drawing.Point(54, 29);
            this.cmbPlaybackCmd.Name = "cmbPlaybackCmd";
            this.cmbPlaybackCmd.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbPlaybackCmd.Size = new System.Drawing.Size(354, 20);
            this.cmbPlaybackCmd.TabIndex = 4;
            // 
            // opmGroupBox2
            // 
            this.opmGroupBox2.AutoSize = true;
            this.opmGroupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.opmGroupBox2.Controls.Add(this.opmTableLayoutPanel3);
            this.opmGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmGroupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opmGroupBox2.Location = new System.Drawing.Point(3, 82);
            this.opmGroupBox2.Name = "opmGroupBox2";
            this.opmGroupBox2.Size = new System.Drawing.Size(417, 77);
            this.opmGroupBox2.TabIndex = 1;
            this.opmGroupBox2.TabStop = false;
            this.opmGroupBox2.Text = "Command Destination";
            // 
            // opmTableLayoutPanel3
            // 
            this.opmTableLayoutPanel3.AutoSize = true;
            this.opmTableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.opmTableLayoutPanel3.ColumnCount = 2;
            this.opmTableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel3.Controls.Add(this.opmLabel2, 0, 0);
            this.opmTableLayoutPanel3.Controls.Add(this.cmbDestination, 1, 0);
            this.opmTableLayoutPanel3.Controls.Add(this.opmLabel3, 0, 1);
            this.opmTableLayoutPanel3.Controls.Add(this.txtDestinationName, 1, 1);
            this.opmTableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel3.Location = new System.Drawing.Point(3, 18);
            this.opmTableLayoutPanel3.Name = "opmTableLayoutPanel3";
            this.opmTableLayoutPanel3.RowCount = 2;
            this.opmTableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel3.Size = new System.Drawing.Size(411, 56);
            this.opmTableLayoutPanel3.TabIndex = 1;
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.Location = new System.Drawing.Point(28, 0);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(50, 26);
            this.opmLabel2.TabIndex = 0;
            this.opmLabel2.Text = "Send to:";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDestination
            // 
            this.cmbDestination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDestination.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbDestination.FormattingEnabled = true;
            this.cmbDestination.Items.AddRange(new object[] {
            "Player",
            "RCC Service"});
            this.cmbDestination.Location = new System.Drawing.Point(84, 3);
            this.cmbDestination.Name = "cmbDestination";
            this.cmbDestination.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbDestination.Size = new System.Drawing.Size(324, 20);
            this.cmbDestination.TabIndex = 2;
            // 
            // opmLabel3
            // 
            this.opmLabel3.AutoSize = true;
            this.opmLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel3.Location = new System.Drawing.Point(3, 26);
            this.opmLabel3.Name = "opmLabel3";
            this.opmLabel3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel3.Size = new System.Drawing.Size(75, 30);
            this.opmLabel3.TabIndex = 3;
            this.opmLabel3.Text = "Destination\r\n(name or IP):";
            this.opmLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDestinationName
            // 
            this.txtDestinationName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDestinationName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDestinationName.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtDestinationName.Lines = new string[0];
            this.txtDestinationName.Location = new System.Drawing.Point(81, 26);
            this.txtDestinationName.Margin = new System.Windows.Forms.Padding(0);
            this.txtDestinationName.MaximumSize = new System.Drawing.Size(3000, 22);
            this.txtDestinationName.MaxLength = 32767;
            this.txtDestinationName.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtDestinationName.Name = "txtDestinationName";
            this.txtDestinationName.OverrideBackColor = System.Drawing.Color.Transparent;
            this.txtDestinationName.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtDestinationName.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtDestinationName.PasswordChar = '\0';
            this.txtDestinationName.ReadOnly = false;
            this.txtDestinationName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtDestinationName.ShortcutsEnabled = true;
            this.txtDestinationName.Size = new System.Drawing.Size(330, 22);
            this.txtDestinationName.TabIndex = 4;
            this.txtDestinationName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtDestinationName.UseSystemPasswordChar = false;
            this.txtDestinationName.WordWrap = true;
            // 
            // btnExecute
            // 
            this.btnExecute.AutoSize = true;
            this.btnExecute.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.Location = new System.Drawing.Point(3, 165);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnExecute.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnExecute.ShowDropDown = false;
            this.btnExecute.Size = new System.Drawing.Size(117, 27);
            this.btnExecute.TabIndex = 2;
            this.btnExecute.Text = "Execute command";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // txtResult
            // 
            this.txtResult.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.txtResult.Lines = new string[0];
            this.txtResult.Location = new System.Drawing.Point(0, 195);
            this.txtResult.Margin = new System.Windows.Forms.Padding(0);
            this.txtResult.MaximumSize = new System.Drawing.Size(3000, 22);
            this.txtResult.MaxLength = 32767;
            this.txtResult.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtResult.Name = "txtResult";
            this.txtResult.OverrideBackColor = System.Drawing.Color.Transparent;
            this.txtResult.OverrideForeColor = System.Drawing.Color.Empty;
            this.txtResult.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtResult.PasswordChar = '\0';
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtResult.ShortcutsEnabled = true;
            this.txtResult.Size = new System.Drawing.Size(423, 22);
            this.txtResult.TabIndex = 3;
            this.txtResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtResult.UseSystemPasswordChar = false;
            this.txtResult.WordWrap = true;
            // 
            // tpRemoteControl
            // 
            this.tpRemoteControl.BackColor = System.Drawing.Color.White;
            this.tpRemoteControl.Controls.Add(this.pnlSimulator);
            this.tpRemoteControl.Location = new System.Drawing.Point(4, 23);
            this.tpRemoteControl.Name = "tpRemoteControl";
            this.tpRemoteControl.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tpRemoteControl.Size = new System.Drawing.Size(433, 403);
            this.tpRemoteControl.TabIndex = 1;
            this.tpRemoteControl.Text = "Simulator";
            // 
            // pnlSimulator
            // 
            this.pnlSimulator.ColumnCount = 7;
            this.pnlSimulator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlSimulator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.pnlSimulator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.pnlSimulator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.pnlSimulator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.pnlSimulator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.pnlSimulator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pnlSimulator.Controls.Add(this.opmButton6, 4, 1);
            this.pnlSimulator.Controls.Add(this.opmButton5, 3, 1);
            this.pnlSimulator.Controls.Add(this.opmButton4, 2, 1);
            this.pnlSimulator.Controls.Add(this.opmButton11, 4, 2);
            this.pnlSimulator.Controls.Add(this.opmButton10, 3, 2);
            this.pnlSimulator.Controls.Add(this.opmButton9, 2, 2);
            this.pnlSimulator.Controls.Add(this.opmButton16, 4, 3);
            this.pnlSimulator.Controls.Add(this.opmButton15, 3, 3);
            this.pnlSimulator.Controls.Add(this.opmButton14, 2, 3);
            this.pnlSimulator.Controls.Add(this.opmButton7, 2, 4);
            this.pnlSimulator.Controls.Add(this.opmButton8, 3, 4);
            this.pnlSimulator.Controls.Add(this.opmButton12, 4, 4);
            this.pnlSimulator.Controls.Add(this.opmButton2, 2, 0);
            this.pnlSimulator.Controls.Add(this.opmButton3, 4, 0);
            this.pnlSimulator.Controls.Add(this.opmButton26, 3, 10);
            this.pnlSimulator.Controls.Add(this.opmButton27, 3, 9);
            this.pnlSimulator.Controls.Add(this.opmButton24, 2, 9);
            this.pnlSimulator.Controls.Add(this.opmButton25, 4, 9);
            this.pnlSimulator.Controls.Add(this.opmButton23, 3, 8);
            this.pnlSimulator.Controls.Add(this.opmButton20, 2, 6);
            this.pnlSimulator.Controls.Add(this.opmButton21, 4, 6);
            this.pnlSimulator.Controls.Add(this.opmButton13, 2, 12);
            this.pnlSimulator.Controls.Add(this.opmButton17, 3, 12);
            this.pnlSimulator.Controls.Add(this.opmButton18, 4, 12);
            this.pnlSimulator.Controls.Add(this.opmButton19, 3, 6);
            this.pnlSimulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSimulator.Location = new System.Drawing.Point(5, 10);
            this.pnlSimulator.Name = "pnlSimulator";
            this.pnlSimulator.RowCount = 14;
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlSimulator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlSimulator.Size = new System.Drawing.Size(423, 388);
            this.pnlSimulator.TabIndex = 0;
            // 
            // opmButton6
            // 
            this.opmButton6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton6.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton6.Location = new System.Drawing.Point(242, 38);
            this.opmButton6.Name = "opmButton6";
            this.opmButton6.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton6.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton6.ShowDropDown = false;
            this.opmButton6.Size = new System.Drawing.Size(49, 29);
            this.opmButton6.TabIndex = 4;
            this.opmButton6.Text = "3";
            this.opmButton6.UseVisualStyleBackColor = true;
            // 
            // opmButton5
            // 
            this.opmButton5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton5.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton5.Location = new System.Drawing.Point(187, 38);
            this.opmButton5.Name = "opmButton5";
            this.opmButton5.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton5.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton5.ShowDropDown = false;
            this.opmButton5.Size = new System.Drawing.Size(49, 29);
            this.opmButton5.TabIndex = 3;
            this.opmButton5.Text = "2";
            this.opmButton5.UseVisualStyleBackColor = true;
            // 
            // opmButton4
            // 
            this.opmButton4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton4.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton4.Location = new System.Drawing.Point(132, 38);
            this.opmButton4.Name = "opmButton4";
            this.opmButton4.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton4.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton4.ShowDropDown = false;
            this.opmButton4.Size = new System.Drawing.Size(49, 29);
            this.opmButton4.TabIndex = 2;
            this.opmButton4.Text = "1";
            this.opmButton4.UseVisualStyleBackColor = true;
            // 
            // opmButton11
            // 
            this.opmButton11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton11.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton11.Location = new System.Drawing.Point(242, 73);
            this.opmButton11.Name = "opmButton11";
            this.opmButton11.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton11.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton11.ShowDropDown = false;
            this.opmButton11.Size = new System.Drawing.Size(49, 29);
            this.opmButton11.TabIndex = 9;
            this.opmButton11.Text = "6";
            this.opmButton11.UseVisualStyleBackColor = true;
            // 
            // opmButton10
            // 
            this.opmButton10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton10.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton10.Location = new System.Drawing.Point(187, 73);
            this.opmButton10.Name = "opmButton10";
            this.opmButton10.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton10.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton10.ShowDropDown = false;
            this.opmButton10.Size = new System.Drawing.Size(49, 29);
            this.opmButton10.TabIndex = 8;
            this.opmButton10.Text = "5";
            this.opmButton10.UseVisualStyleBackColor = true;
            // 
            // opmButton9
            // 
            this.opmButton9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton9.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton9.Location = new System.Drawing.Point(132, 73);
            this.opmButton9.Name = "opmButton9";
            this.opmButton9.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton9.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton9.ShowDropDown = false;
            this.opmButton9.Size = new System.Drawing.Size(49, 29);
            this.opmButton9.TabIndex = 7;
            this.opmButton9.Text = "4";
            this.opmButton9.UseVisualStyleBackColor = true;
            // 
            // opmButton16
            // 
            this.opmButton16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton16.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton16.Location = new System.Drawing.Point(242, 108);
            this.opmButton16.Name = "opmButton16";
            this.opmButton16.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton16.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton16.ShowDropDown = false;
            this.opmButton16.Size = new System.Drawing.Size(49, 29);
            this.opmButton16.TabIndex = 14;
            this.opmButton16.Text = "9";
            this.opmButton16.UseVisualStyleBackColor = true;
            // 
            // opmButton15
            // 
            this.opmButton15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton15.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton15.Location = new System.Drawing.Point(187, 108);
            this.opmButton15.Name = "opmButton15";
            this.opmButton15.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton15.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton15.ShowDropDown = false;
            this.opmButton15.Size = new System.Drawing.Size(49, 29);
            this.opmButton15.TabIndex = 13;
            this.opmButton15.Text = "8";
            this.opmButton15.UseVisualStyleBackColor = true;
            // 
            // opmButton14
            // 
            this.opmButton14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton14.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton14.Location = new System.Drawing.Point(132, 108);
            this.opmButton14.Name = "opmButton14";
            this.opmButton14.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton14.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton14.ShowDropDown = false;
            this.opmButton14.Size = new System.Drawing.Size(49, 29);
            this.opmButton14.TabIndex = 12;
            this.opmButton14.Text = "7";
            this.opmButton14.UseVisualStyleBackColor = true;
            // 
            // opmButton7
            // 
            this.opmButton7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton7.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton7.Location = new System.Drawing.Point(132, 143);
            this.opmButton7.Name = "opmButton7";
            this.opmButton7.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton7.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton7.ShowDropDown = false;
            this.opmButton7.Size = new System.Drawing.Size(49, 29);
            this.opmButton7.TabIndex = 28;
            this.opmButton7.Text = "*";
            this.opmButton7.UseVisualStyleBackColor = true;
            // 
            // opmButton8
            // 
            this.opmButton8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton8.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton8.Location = new System.Drawing.Point(187, 143);
            this.opmButton8.Name = "opmButton8";
            this.opmButton8.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton8.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton8.ShowDropDown = false;
            this.opmButton8.Size = new System.Drawing.Size(49, 29);
            this.opmButton8.TabIndex = 29;
            this.opmButton8.Text = "0";
            this.opmButton8.UseVisualStyleBackColor = true;
            // 
            // opmButton12
            // 
            this.opmButton12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton12.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton12.Location = new System.Drawing.Point(242, 143);
            this.opmButton12.Name = "opmButton12";
            this.opmButton12.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton12.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton12.ShowDropDown = false;
            this.opmButton12.Size = new System.Drawing.Size(49, 29);
            this.opmButton12.TabIndex = 30;
            this.opmButton12.Text = "#";
            this.opmButton12.UseVisualStyleBackColor = true;
            // 
            // opmButton2
            // 
            this.opmButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.opmButton2.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton2.ImageIndex = 4;
            this.opmButton2.ImageList = this.ilX;
            this.opmButton2.Location = new System.Drawing.Point(132, 3);
            this.opmButton2.Name = "opmButton2";
            this.opmButton2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton2.ShowDropDown = false;
            this.opmButton2.Size = new System.Drawing.Size(49, 29);
            this.opmButton2.TabIndex = 0;
            this.opmButton2.UseVisualStyleBackColor = true;
            // 
            // ilX
            // 
            this.ilX.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilX.ImageStream")));
            this.ilX.TransparentColor = System.Drawing.Color.White;
            this.ilX.Images.SetKeyName(0, "Down.png");
            this.ilX.Images.SetKeyName(1, "Left.png");
            this.ilX.Images.SetKeyName(2, "Right.png");
            this.ilX.Images.SetKeyName(3, "Up.png");
            this.ilX.Images.SetKeyName(4, "Info.png");
            this.ilX.Images.SetKeyName(5, "OnOff.png");
            // 
            // opmButton3
            // 
            this.opmButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.opmButton3.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton3.ImageIndex = 5;
            this.opmButton3.ImageList = this.ilX;
            this.opmButton3.Location = new System.Drawing.Point(242, 3);
            this.opmButton3.Name = "opmButton3";
            this.opmButton3.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton3.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton3.ShowDropDown = false;
            this.opmButton3.Size = new System.Drawing.Size(49, 29);
            this.opmButton3.TabIndex = 1;
            this.opmButton3.UseVisualStyleBackColor = true;
            // 
            // opmButton26
            // 
            this.opmButton26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton26.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton26.ImageIndex = 0;
            this.opmButton26.ImageList = this.ilX;
            this.opmButton26.Location = new System.Drawing.Point(187, 303);
            this.opmButton26.Name = "opmButton26";
            this.opmButton26.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton26.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton26.ShowDropDown = false;
            this.opmButton26.Size = new System.Drawing.Size(49, 29);
            this.opmButton26.TabIndex = 24;
            this.opmButton26.UseVisualStyleBackColor = true;
            // 
            // opmButton27
            // 
            this.opmButton27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton27.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton27.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton27.Location = new System.Drawing.Point(187, 268);
            this.opmButton27.Name = "opmButton27";
            this.opmButton27.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton27.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton27.ShowDropDown = false;
            this.opmButton27.Size = new System.Drawing.Size(49, 29);
            this.opmButton27.TabIndex = 25;
            this.opmButton27.Text = "OK";
            this.opmButton27.UseVisualStyleBackColor = true;
            // 
            // opmButton24
            // 
            this.opmButton24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton24.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton24.ImageIndex = 1;
            this.opmButton24.ImageList = this.ilX;
            this.opmButton24.Location = new System.Drawing.Point(132, 268);
            this.opmButton24.Name = "opmButton24";
            this.opmButton24.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton24.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton24.ShowDropDown = false;
            this.opmButton24.Size = new System.Drawing.Size(49, 29);
            this.opmButton24.TabIndex = 22;
            this.opmButton24.UseVisualStyleBackColor = true;
            // 
            // opmButton25
            // 
            this.opmButton25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton25.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton25.ImageIndex = 2;
            this.opmButton25.ImageList = this.ilX;
            this.opmButton25.Location = new System.Drawing.Point(242, 268);
            this.opmButton25.Name = "opmButton25";
            this.opmButton25.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton25.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton25.ShowDropDown = false;
            this.opmButton25.Size = new System.Drawing.Size(49, 29);
            this.opmButton25.TabIndex = 23;
            this.opmButton25.UseVisualStyleBackColor = true;
            // 
            // opmButton23
            // 
            this.opmButton23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton23.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton23.ImageIndex = 3;
            this.opmButton23.ImageList = this.ilX;
            this.opmButton23.Location = new System.Drawing.Point(187, 233);
            this.opmButton23.Name = "opmButton23";
            this.opmButton23.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton23.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton23.ShowDropDown = false;
            this.opmButton23.Size = new System.Drawing.Size(49, 29);
            this.opmButton23.TabIndex = 21;
            this.opmButton23.UseVisualStyleBackColor = true;
            // 
            // opmButton20
            // 
            this.opmButton20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton20.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton20.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmButton20.Location = new System.Drawing.Point(132, 188);
            this.opmButton20.Name = "opmButton20";
            this.opmButton20.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton20.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton20.ShowDropDown = false;
            this.opmButton20.Size = new System.Drawing.Size(49, 29);
            this.opmButton20.TabIndex = 18;
            this.opmButton20.Text = "CD";
            this.opmButton20.UseVisualStyleBackColor = true;
            // 
            // opmButton21
            // 
            this.opmButton21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton21.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmButton21.Location = new System.Drawing.Point(242, 188);
            this.opmButton21.Name = "opmButton21";
            this.opmButton21.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton21.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton21.ShowDropDown = false;
            this.opmButton21.Size = new System.Drawing.Size(49, 29);
            this.opmButton21.TabIndex = 19;
            this.opmButton21.Text = "RADIO";
            this.opmButton21.UseVisualStyleBackColor = true;
            // 
            // opmButton13
            // 
            this.opmButton13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton13.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton13.Location = new System.Drawing.Point(132, 348);
            this.opmButton13.Name = "opmButton13";
            this.opmButton13.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton13.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton13.ShowDropDown = false;
            this.opmButton13.Size = new System.Drawing.Size(49, 29);
            this.opmButton13.TabIndex = 31;
            this.opmButton13.Text = "A";
            this.opmButton13.UseVisualStyleBackColor = true;
            // 
            // opmButton17
            // 
            this.opmButton17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton17.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton17.Location = new System.Drawing.Point(187, 348);
            this.opmButton17.Name = "opmButton17";
            this.opmButton17.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton17.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton17.ShowDropDown = false;
            this.opmButton17.Size = new System.Drawing.Size(49, 29);
            this.opmButton17.TabIndex = 32;
            this.opmButton17.Text = "B";
            this.opmButton17.UseVisualStyleBackColor = true;
            // 
            // opmButton18
            // 
            this.opmButton18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton18.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.opmButton18.Location = new System.Drawing.Point(242, 348);
            this.opmButton18.Name = "opmButton18";
            this.opmButton18.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton18.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton18.ShowDropDown = false;
            this.opmButton18.Size = new System.Drawing.Size(49, 29);
            this.opmButton18.TabIndex = 33;
            this.opmButton18.Text = "C";
            this.opmButton18.UseVisualStyleBackColor = true;
            // 
            // opmButton19
            // 
            this.opmButton19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmButton19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmButton19.FontSize = OPMedia.UI.Themes.FontSizes.Smallest;
            this.opmButton19.Location = new System.Drawing.Point(187, 188);
            this.opmButton19.Name = "opmButton19";
            this.opmButton19.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmButton19.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmButton19.ShowDropDown = false;
            this.opmButton19.Size = new System.Drawing.Size(49, 29);
            this.opmButton19.TabIndex = 34;
            this.opmButton19.Text = "TAPE";
            this.opmButton19.UseVisualStyleBackColor = true;
            // 
            // tpMockup
            // 
            this.tpMockup.BackColor = System.Drawing.Color.White;
            this.tpMockup.Controls.Add(this.opmTableLayoutPanel4);
            this.tpMockup.Location = new System.Drawing.Point(4, 23);
            this.tpMockup.Name = "tpMockup";
            this.tpMockup.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tpMockup.Size = new System.Drawing.Size(433, 403);
            this.tpMockup.TabIndex = 2;
            this.tpMockup.Text = "Mock-up";
            // 
            // opmTableLayoutPanel4
            // 
            this.opmTableLayoutPanel4.ColumnCount = 4;
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel4.Controls.Add(this.btnPlayer, 2, 1);
            this.opmTableLayoutPanel4.Controls.Add(this.btnRemote, 2, 3);
            this.opmTableLayoutPanel4.Controls.Add(this.btnMediaLib, 2, 2);
            this.opmTableLayoutPanel4.Controls.Add(this.lblDesc, 1, 0);
            this.opmTableLayoutPanel4.Controls.Add(this.opmNumericUpDown1, 3, 0);
            this.opmTableLayoutPanel4.Controls.Add(this.opmNumericTextBox1, 2, 0);
            this.opmTableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel4.Location = new System.Drawing.Point(5, 10);
            this.opmTableLayoutPanel4.Name = "opmTableLayoutPanel4";
            this.opmTableLayoutPanel4.RowCount = 5;
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opmTableLayoutPanel4.Size = new System.Drawing.Size(423, 388);
            this.opmTableLayoutPanel4.TabIndex = 16;
            // 
            // btnPlayer
            // 
            this.btnPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayer.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.btnPlayer.Location = new System.Drawing.Point(89, 38);
            this.btnPlayer.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.btnPlayer.Name = "btnPlayer";
            this.btnPlayer.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnPlayer.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnPlayer.ShowDropDown = false;
            this.btnPlayer.Size = new System.Drawing.Size(249, 100);
            this.btnPlayer.TabIndex = 13;
            this.btnPlayer.UseVisualStyleBackColor = true;
            // 
            // btnRemote
            // 
            this.btnRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemote.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.btnRemote.Location = new System.Drawing.Point(89, 250);
            this.btnRemote.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.btnRemote.Name = "btnRemote";
            this.btnRemote.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnRemote.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnRemote.ShowDropDown = false;
            this.btnRemote.Size = new System.Drawing.Size(249, 100);
            this.btnRemote.TabIndex = 15;
            this.btnRemote.UseVisualStyleBackColor = true;
            // 
            // btnMediaLib
            // 
            this.btnMediaLib.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMediaLib.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMediaLib.FontSize = OPMedia.UI.Themes.FontSizes.VeryLarge;
            this.btnMediaLib.Location = new System.Drawing.Point(89, 144);
            this.btnMediaLib.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.btnMediaLib.Name = "btnMediaLib";
            this.btnMediaLib.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnMediaLib.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnMediaLib.ShowDropDown = false;
            this.btnMediaLib.Size = new System.Drawing.Size(249, 100);
            this.btnMediaLib.TabIndex = 14;
            this.btnMediaLib.UseVisualStyleBackColor = true;
            // 
            // lblDesc
            // 
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDesc.FontSize = OPMedia.UI.Themes.FontSizes.ExtremeLarge;
            this.lblDesc.Location = new System.Drawing.Point(44, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblDesc.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel4.SetRowSpan(this.lblDesc, 5);
            this.lblDesc.Size = new System.Drawing.Size(34, 388);
            this.lblDesc.TabIndex = 16;
            this.lblDesc.Text = "ProTONE Suite 2.0";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDesc.VerticalText = true;
            // 
            // tpWCF
            // 
            this.tpWCF.BackColor = System.Drawing.Color.White;
            this.tpWCF.Controls.Add(this.tbWCFDetails);
            this.tpWCF.Location = new System.Drawing.Point(4, 23);
            this.tpWCF.Name = "tpWCF";
            this.tpWCF.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.tpWCF.Size = new System.Drawing.Size(433, 403);
            this.tpWCF.TabIndex = 3;
            this.tpWCF.Text = "Signal Analisys WCF";
            // 
            // tbWCFDetails
            // 
            this.tbWCFDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWCFDetails.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbWCFDetails.Location = new System.Drawing.Point(5, 10);
            this.tbWCFDetails.Multiline = true;
            this.tbWCFDetails.Name = "tbWCFDetails";
            this.tbWCFDetails.Size = new System.Drawing.Size(423, 388);
            this.tbWCFDetails.TabIndex = 0;
            // 
            // opmNumericUpDown1
            // 
            this.opmNumericUpDown1.DecimalPlaces = 0;
            this.opmNumericUpDown1.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.opmNumericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.opmNumericUpDown1.Location = new System.Drawing.Point(341, 0);
            this.opmNumericUpDown1.Margin = new System.Windows.Forms.Padding(0);
            this.opmNumericUpDown1.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.opmNumericUpDown1.MaximumSize = new System.Drawing.Size(3000, 25);
            this.opmNumericUpDown1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.opmNumericUpDown1.MinimumSize = new System.Drawing.Size(22, 25);
            this.opmNumericUpDown1.Name = "opmNumericUpDown1";
            this.opmNumericUpDown1.OverrideBackColor = System.Drawing.Color.Transparent;
            this.opmNumericUpDown1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmNumericUpDown1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.opmNumericUpDown1.ReadOnly = false;
            this.opmNumericUpDown1.Size = new System.Drawing.Size(82, 25);
            this.opmNumericUpDown1.TabIndex = 17;
            this.opmNumericUpDown1.Text = "0";
            this.opmNumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.opmNumericUpDown1.Unit = null;
            this.opmNumericUpDown1.UnitFirst = false;
            this.opmNumericUpDown1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // opmNumericTextBox1
            // 
            this.opmNumericTextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.opmNumericTextBox1.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.opmNumericTextBox1.Lines = new string[] {
        "OPMNUMERICTEXTBOX1"};
            this.opmNumericTextBox1.Location = new System.Drawing.Point(81, 0);
            this.opmNumericTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.opmNumericTextBox1.MaximumSize = new System.Drawing.Size(3000, 22);
            this.opmNumericTextBox1.MaxLength = 5;
            this.opmNumericTextBox1.MinimumSize = new System.Drawing.Size(22, 22);
            this.opmNumericTextBox1.Name = "opmNumericTextBox1";
            this.opmNumericTextBox1.NumBase = OPMedia.UI.Controls.NumberingBase.Base10;
            this.opmNumericTextBox1.OverrideBackColor = System.Drawing.Color.Transparent;
            this.opmNumericTextBox1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmNumericTextBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.opmNumericTextBox1.PasswordChar = '\0';
            this.opmNumericTextBox1.ReadOnly = false;
            this.opmNumericTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.opmNumericTextBox1.ShortcutsEnabled = true;
            this.opmNumericTextBox1.Size = new System.Drawing.Size(260, 22);
            this.opmNumericTextBox1.TabIndex = 18;
            this.opmNumericTextBox1.Text = "OPMNUMERICTEXTBOX1";
            this.opmNumericTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.opmNumericTextBox1.UseSystemPasswordChar = false;
            this.opmNumericTextBox1.WordWrap = true;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(445, 456);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "MainForm";
            this.pnlContent.ResumeLayout(false);
            this.tabEmulator.ResumeLayout(false);
            this.tpApi.ResumeLayout(false);
            this.opmTableLayoutPanel2.ResumeLayout(false);
            this.opmTableLayoutPanel2.PerformLayout();
            this.opmGroupBox1.ResumeLayout(false);
            this.opmGroupBox1.PerformLayout();
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.opmGroupBox2.ResumeLayout(false);
            this.opmGroupBox2.PerformLayout();
            this.opmTableLayoutPanel3.ResumeLayout(false);
            this.opmTableLayoutPanel3.PerformLayout();
            this.tpRemoteControl.ResumeLayout(false);
            this.pnlSimulator.ResumeLayout(false);
            this.tpMockup.ResumeLayout(false);
            this.opmTableLayoutPanel4.ResumeLayout(false);
            this.tpWCF.ResumeLayout(false);
            this.tpWCF.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.OPMTabControl tabEmulator;
        private System.Windows.Forms.TabPage tpApi;
        private System.Windows.Forms.TabPage tpRemoteControl;
        private UI.Controls.OPMGroupBox opmGroupBox1;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel1;
        private UI.Controls.OPMLabel opmLabel1;
        private UI.Controls.OPMLabel lblPlaybackCmd;
        private UI.Controls.OPMComboBox cmbCommandType;
        private UI.Controls.OPMComboBox cmbPlaybackCmd;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel2;
        private UI.Controls.OPMGroupBox opmGroupBox2;
        private UI.Controls.OPMButton btnExecute;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel3;
        private UI.Controls.OPMLabel opmLabel2;
        private UI.Controls.OPMComboBox cmbDestination;
        private UI.Controls.OPMLabel opmLabel3;
        private UI.Controls.OPMTextBox txtDestinationName;
        private UI.Controls.OPMTextBox txtResult;
        private UI.Controls.OPMTableLayoutPanel pnlSimulator;
        private UI.Controls.OPMButton opmButton2;
        private UI.Controls.OPMButton opmButton3;
        private UI.Controls.OPMButton opmButton4;
        private UI.Controls.OPMButton opmButton5;
        private UI.Controls.OPMButton opmButton6;
        private UI.Controls.OPMButton opmButton9;
        private UI.Controls.OPMButton opmButton10;
        private UI.Controls.OPMButton opmButton11;
        private UI.Controls.OPMButton opmButton14;
        private UI.Controls.OPMButton opmButton15;
        private UI.Controls.OPMButton opmButton16;
        private UI.Controls.OPMButton opmButton20;
        private UI.Controls.OPMButton opmButton21;
        private UI.Controls.OPMButton opmButton23;
        private UI.Controls.OPMButton opmButton24;
        private UI.Controls.OPMButton opmButton25;
        private UI.Controls.OPMButton opmButton26;
        private UI.Controls.OPMButton opmButton27;
        private UI.Controls.OPMButton opmButton7;
        private UI.Controls.OPMButton opmButton8;
        private UI.Controls.OPMButton opmButton12;
        private System.Windows.Forms.ImageList ilX;
        private UI.Controls.OPMButton opmButton13;
        private UI.Controls.OPMButton opmButton17;
        private UI.Controls.OPMButton opmButton18;
        private UI.Controls.OPMButton opmButton19;
        private System.Windows.Forms.TabPage tpMockup;
        private UI.Controls.OPMButton btnPlayer;
        private UI.Controls.OPMButton btnMediaLib;
        private UI.Controls.OPMButton btnRemote;
        private UI.Controls.OPMTableLayoutPanel opmTableLayoutPanel4;
        private UI.Controls.OPMLabel lblDesc;
        private System.Windows.Forms.TabPage tpWCF;
        private System.Windows.Forms.TextBox tbWCFDetails;
        private UI.Controls.OPMNumericUpDown opmNumericUpDown1;
        private UI.Controls.OPMNumericTextBox opmNumericTextBox1;
    }
}

