using OPMedia.UI.Controls;

namespace OPMedia.UI.Dialogs
{
    partial class GenericWaitDialog
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
            this.lblNotifyText = new OPMedia.UI.Controls.OPMLabel();
            this.pictureBox1 = new OPMedia.UI.Controls.WaitingPictureBox();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.pbProgress = new OPMedia.UI.Controls.OPMProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNotifyText
            // 
            this.lblNotifyText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNotifyText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblNotifyText.Location = new System.Drawing.Point(41, 0);
            this.lblNotifyText.Name = "lblNotifyText";
            this.lblNotifyText.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblNotifyText.OverrideForeColor = System.Drawing.Color.Empty;
            this.opmTableLayoutPanel1.SetRowSpan(this.lblNotifyText, 3);
            this.lblNotifyText.Size = new System.Drawing.Size(211, 74);
            this.lblNotifyText.TabIndex = 8;
            this.lblNotifyText.Text = "fdsfdsf";
            this.lblNotifyText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 2;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.lblNotifyText, 1, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.pbProgress, 1, 3);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 4;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(255, 93);
            this.opmTableLayoutPanel1.TabIndex = 9;
            // 
            // pbProgress
            // 
            this.pbProgress.AllowDragging = false;
            this.pbProgress.AutoSize = true;
            this.pbProgress.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pbProgress.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbProgress.EffectiveMaximum = 0D;
            this.pbProgress.Enabled = false;
            this.pbProgress.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.pbProgress.Location = new System.Drawing.Point(41, 77);
            this.pbProgress.Maximum = 100D;
            this.pbProgress.MinimumSize = new System.Drawing.Size(10, 12);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.NrTicks = 10;
            this.pbProgress.OverrideBackColor = System.Drawing.Color.Empty;
            this.pbProgress.OverrideElapsedBackColor = System.Drawing.Color.Empty;
            this.pbProgress.ShowTicks = false;
            this.pbProgress.Size = new System.Drawing.Size(211, 13);
            this.pbProgress.TabIndex = 9;
            this.pbProgress.Value = 0D;
            this.pbProgress.Vertical = false;
            // 
            // GenericWaitDialog
            // 
            this.ClientSize = new System.Drawing.Size(255, 93);
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(200, 70);
            this.Name = "GenericWaitDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OPMLabel lblNotifyText;
        private WaitingPictureBox pictureBox1;
        private OPMTableLayoutPanel opmTableLayoutPanel1;
        private OPMProgressBar pbProgress;
    }
}