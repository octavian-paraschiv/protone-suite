using OPMedia.UI.Controls;
using System.Windows.Forms;

namespace OPMedia.UI.ProTONE.Configuration.MiscConfig
{
    partial class SchedulerSettingsPage
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
            this.wsScheduledEvtDays = new OPMedia.UI.Controls.WeekdaySelector();
            this.cmbPlaylistEvtHandler = new OPMedia.UI.Controls.OPMComboBox();
            this.lblCaution = new OPMedia.UI.Controls.OPMLabel();
            this.chkEnablePlaylistEvt = new OPMedia.UI.Controls.OPMCheckBox();
            this.cmbScheduledEvtHandler = new OPMedia.UI.Controls.OPMComboBox();
            this.label5 = new OPMedia.UI.Controls.OPMLabel();
            this.nudSchedulerWaitTimerProceed = new OPMedia.UI.Controls.OPMNumericUpDown();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.opmLabel1 = new OPMedia.UI.Controls.OPMLabel();
            this.opmLabel2 = new OPMedia.UI.Controls.OPMLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new OPMedia.UI.Controls.OPMLabel();
            this.chkEnableScheduledEvt = new OPMedia.UI.Controls.OPMCheckBox();
            this.dtpScheduledEvtTime = new OPMedia.UI.Controls.OPMDateTimePicker();
            this.lblSep1 = new System.Windows.Forms.Label();
            this.lblSep2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSchedulerWaitTimerProceed)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // wsScheduledEvtDays
            // 
            this.wsScheduledEvtDays.AutoSize = true;
            this.wsScheduledEvtDays.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.wsScheduledEvtDays, 4);
            this.wsScheduledEvtDays.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wsScheduledEvtDays.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.wsScheduledEvtDays.Location = new System.Drawing.Point(3, 119);
            this.wsScheduledEvtDays.MaximumSize = new System.Drawing.Size(4270, 50);
            this.wsScheduledEvtDays.MinimumSize = new System.Drawing.Size(400, 45);
            this.wsScheduledEvtDays.Name = "wsScheduledEvtDays";
            this.wsScheduledEvtDays.OverrideBackColor = System.Drawing.Color.Empty;
            this.wsScheduledEvtDays.Size = new System.Drawing.Size(493, 50);
            this.wsScheduledEvtDays.TabIndex = 3;
            // 
            // cmbPlaylistEvtHandler
            // 
            this.cmbPlaylistEvtHandler.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbPlaylistEvtHandler.FormattingEnabled = true;
            this.cmbPlaylistEvtHandler.Location = new System.Drawing.Point(76, 28);
            this.cmbPlaylistEvtHandler.Name = "cmbPlaylistEvtHandler";
            this.cmbPlaylistEvtHandler.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbPlaylistEvtHandler.Size = new System.Drawing.Size(155, 24);
            this.cmbPlaylistEvtHandler.TabIndex = 1;
            // 
            // lblCaution
            // 
            this.lblCaution.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCaution, 4);
            this.lblCaution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaution.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCaution.FontSize = OPMedia.UI.Themes.FontSizes.Small;
            this.lblCaution.Location = new System.Drawing.Point(3, 58);
            this.lblCaution.Margin = new System.Windows.Forms.Padding(3);
            this.lblCaution.Name = "lblCaution";
            this.lblCaution.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblCaution.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblCaution.Size = new System.Drawing.Size(493, 13);
            this.lblCaution.TabIndex = 3;
            this.lblCaution.Text = "TXT_PLAYLISTEVT_CAUTION";
            this.lblCaution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEnablePlaylistEvt
            // 
            this.chkEnablePlaylistEvt.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkEnablePlaylistEvt, 4);
            this.chkEnablePlaylistEvt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkEnablePlaylistEvt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkEnablePlaylistEvt.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.chkEnablePlaylistEvt.Location = new System.Drawing.Point(3, 3);
            this.chkEnablePlaylistEvt.Name = "chkEnablePlaylistEvt";
            this.chkEnablePlaylistEvt.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkEnablePlaylistEvt.Size = new System.Drawing.Size(493, 19);
            this.chkEnablePlaylistEvt.TabIndex = 0;
            this.chkEnablePlaylistEvt.Text = "TXT_ENABLE_PLAYLISTEVT";
            // 
            // cmbScheduledEvtHandler
            // 
            this.cmbScheduledEvtHandler.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbScheduledEvtHandler.FormattingEnabled = true;
            this.cmbScheduledEvtHandler.Location = new System.Drawing.Point(76, 175);
            this.cmbScheduledEvtHandler.Name = "cmbScheduledEvtHandler";
            this.cmbScheduledEvtHandler.OverrideForeColor = System.Drawing.Color.Empty;
            this.cmbScheduledEvtHandler.Size = new System.Drawing.Size(155, 24);
            this.cmbScheduledEvtHandler.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Location = new System.Drawing.Point(3, 220);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.OverrideBackColor = System.Drawing.Color.Empty;
            this.label5.OverrideForeColor = System.Drawing.Color.Empty;
            this.label5.Size = new System.Drawing.Size(228, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "TXT_SCHEDULERWAITTIMERPROCEED";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudSchedulerWaitTimerProceed
            // 
            this.nudSchedulerWaitTimerProceed.AutoSize = true;
            this.nudSchedulerWaitTimerProceed.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudSchedulerWaitTimerProceed.Location = new System.Drawing.Point(237, 220);
            this.nudSchedulerWaitTimerProceed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSchedulerWaitTimerProceed.Name = "nudSchedulerWaitTimerProceed";
            this.nudSchedulerWaitTimerProceed.ReadOnly = true;
            this.nudSchedulerWaitTimerProceed.Size = new System.Drawing.Size(35, 23);
            this.nudSchedulerWaitTimerProceed.TabIndex = 1;
            this.nudSchedulerWaitTimerProceed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudSchedulerWaitTimerProceed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.nudSchedulerWaitTimerProceed, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.cmbScheduledEvtHandler, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.chkEnablePlaylistEvt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.wsScheduledEvtDays, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblCaution, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbPlaylistEvtHandler, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.opmLabel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.opmLabel2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblSep1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblSep2, 0, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(499, 407);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // opmLabel1
            // 
            this.opmLabel1.AutoSize = true;
            this.opmLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel1.Location = new System.Drawing.Point(3, 28);
            this.opmLabel1.Margin = new System.Windows.Forms.Padding(3);
            this.opmLabel1.Name = "opmLabel1";
            this.opmLabel1.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel1.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel1.Size = new System.Drawing.Size(67, 24);
            this.opmLabel1.TabIndex = 6;
            this.opmLabel1.Text = "TXT_THEN:";
            this.opmLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // opmLabel2
            // 
            this.opmLabel2.AutoSize = true;
            this.opmLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.opmLabel2.Location = new System.Drawing.Point(3, 175);
            this.opmLabel2.Margin = new System.Windows.Forms.Padding(3);
            this.opmLabel2.Name = "opmLabel2";
            this.opmLabel2.OverrideBackColor = System.Drawing.Color.Empty;
            this.opmLabel2.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmLabel2.Size = new System.Drawing.Size(67, 24);
            this.opmLabel2.TabIndex = 7;
            this.opmLabel2.Text = "TXT_THEN:";
            this.opmLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 4);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkEnableScheduledEvt, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dtpScheduledEvtTime, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 89);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(499, 27);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.label4.Location = new System.Drawing.Point(292, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.OverrideBackColor = System.Drawing.Color.Empty;
            this.label4.OverrideForeColor = System.Drawing.Color.Empty;
            this.label4.Size = new System.Drawing.Size(204, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "TXT_ONDAYS";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEnableScheduledEvt
            // 
            this.chkEnableScheduledEvt.AutoSize = true;
            this.chkEnableScheduledEvt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkEnableScheduledEvt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkEnableScheduledEvt.FontSize = OPMedia.UI.Themes.FontSizes.NormalBold;
            this.chkEnableScheduledEvt.Location = new System.Drawing.Point(3, 3);
            this.chkEnableScheduledEvt.Name = "chkEnableScheduledEvt";
            this.chkEnableScheduledEvt.OverrideForeColor = System.Drawing.Color.Empty;
            this.chkEnableScheduledEvt.Size = new System.Drawing.Size(203, 21);
            this.chkEnableScheduledEvt.TabIndex = 0;
            this.chkEnableScheduledEvt.Text = "TXT_ENABLE_SCHEDULEDEVT";
            // 
            // dtpScheduledEvtTime
            // 
            this.dtpScheduledEvtTime.CustomFormat = "HH:mm tt";
            this.dtpScheduledEvtTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpScheduledEvtTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpScheduledEvtTime.Location = new System.Drawing.Point(212, 1);
            this.dtpScheduledEvtTime.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.dtpScheduledEvtTime.Name = "dtpScheduledEvtTime";
            this.dtpScheduledEvtTime.ShowUpDown = true;
            this.dtpScheduledEvtTime.Size = new System.Drawing.Size(74, 23);
            this.dtpScheduledEvtTime.TabIndex = 1;
            // 
            // lblSep1
            // 
            this.lblSep1.BackColor = System.Drawing.Color.Silver;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSep1, 4);
            this.lblSep1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSep1.Location = new System.Drawing.Point(3, 81);
            this.lblSep1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.lblSep1.Name = "lblSep1";
            this.lblSep1.Size = new System.Drawing.Size(493, 3);
            this.lblSep1.TabIndex = 9;
            // 
            // lblSep2
            // 
            this.lblSep2.BackColor = System.Drawing.Color.Silver;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSep2, 4);
            this.lblSep2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSep2.Location = new System.Drawing.Point(3, 209);
            this.lblSep2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
            this.lblSep2.Name = "lblSep2";
            this.lblSep2.Size = new System.Drawing.Size(493, 3);
            this.lblSep2.TabIndex = 10;
            // 
            // SchedulerSettingsPage
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SchedulerSettingsPage";
            this.Size = new System.Drawing.Size(499, 407);
            ((System.ComponentModel.ISupportInitialize)(this.nudSchedulerWaitTimerProceed)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private WeekdaySelector wsScheduledEvtDays;
        private OPMCheckBox chkEnablePlaylistEvt;
        private OPMComboBox cmbPlaylistEvtHandler;
        private OPMComboBox cmbScheduledEvtHandler;
        private OPMLabel label5;
        private OPMNumericUpDown nudSchedulerWaitTimerProceed;
        private OPMLabel lblCaution;
        private TableLayoutPanel tableLayoutPanel1;
        private OPMLabel opmLabel1;
        private OPMLabel opmLabel2;
        private OPMCheckBox chkEnableScheduledEvt;
        private OPMDateTimePicker dtpScheduledEvtTime;
        private OPMLabel label4;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblSep1;
        private Label lblSep2;
    }
}
