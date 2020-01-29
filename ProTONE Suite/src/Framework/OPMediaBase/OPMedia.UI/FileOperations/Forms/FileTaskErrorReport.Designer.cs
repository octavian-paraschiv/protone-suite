using OPMedia.UI.Controls;

namespace OPMedia.UI.FileTasks
{
    partial class FileTaskErrorReport
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
            this.btnOK = new OPMedia.UI.Controls.OPMButton();
            this.tvReports = new OPMedia.UI.Controls.OPMTreeView();
            this.lblDesc = new OPMedia.UI.Controls.OPMLabel();
            this.pbWarn = new System.Windows.Forms.PictureBox();
            this.opmTableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarn)).BeginInit();
            this.opmTableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(1036, 1025);
            this.btnOK.Name = "btnOK";
            this.btnOK.OverrideBackColor = System.Drawing.Color.Empty;
            this.btnOK.OverrideForeColor = System.Drawing.Color.Empty;
            this.btnOK.ShowDropDown = false;
            this.btnOK.Size = new System.Drawing.Size(72, 24);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "TXT_OK";
            // 
            // tvReports
            // 
            this.tvReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.opmTableLayoutPanel1.SetColumnSpan(this.tvReports, 2);
            this.tvReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvReports.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tvReports.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tvReports.Location = new System.Drawing.Point(5, 43);
            this.tvReports.Margin = new System.Windows.Forms.Padding(5);
            this.tvReports.Name = "tvReports";
            this.tvReports.Size = new System.Drawing.Size(686, 552);
            this.tvReports.TabIndex = 1;
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDesc.Location = new System.Drawing.Point(44, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.OverrideBackColor = System.Drawing.Color.Empty;
            this.lblDesc.OverrideForeColor = System.Drawing.Color.Empty;
            this.lblDesc.Size = new System.Drawing.Size(649, 38);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "TXT_ERRORSFOUND";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbWarn
            // 
            this.pbWarn.BackColor = System.Drawing.Color.Transparent;
            this.pbWarn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pbWarn.Location = new System.Drawing.Point(6, 3);
            this.pbWarn.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.pbWarn.Name = "pbWarn";
            this.pbWarn.Size = new System.Drawing.Size(32, 32);
            this.pbWarn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbWarn.TabIndex = 8;
            this.pbWarn.TabStop = false;
            // 
            // opmTableLayoutPanel1
            // 
            this.opmTableLayoutPanel1.ColumnCount = 2;
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.opmTableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Controls.Add(this.pbWarn, 0, 0);
            this.opmTableLayoutPanel1.Controls.Add(this.tvReports, 0, 1);
            this.opmTableLayoutPanel1.Controls.Add(this.lblDesc, 1, 0);
            this.opmTableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opmTableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.opmTableLayoutPanel1.Name = "opmTableLayoutPanel1";
            this.opmTableLayoutPanel1.RowCount = 2;
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.opmTableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.opmTableLayoutPanel1.Size = new System.Drawing.Size(696, 600);
            this.opmTableLayoutPanel1.TabIndex = 9;
            // 
            // FileTaskErrorReport
            // 
            this.ClientSize = new System.Drawing.Size(696, 600);
            this.Controls.Add(this.opmTableLayoutPanel1);
            this.Controls.Add(this.btnOK);
            this.MaximumSize = new System.Drawing.Size(1024, 800);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "FileTaskErrorReport";
            ((System.ComponentModel.ISupportInitialize)(this.pbWarn)).EndInit();
            this.opmTableLayoutPanel1.ResumeLayout(false);
            this.opmTableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OPMButton btnOK;
        private OPMTreeView tvReports;
        private OPMLabel lblDesc;
        private System.Windows.Forms.PictureBox pbWarn;
        private OPMTableLayoutPanel opmTableLayoutPanel1;
    }
}