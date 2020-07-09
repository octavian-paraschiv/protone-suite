using OPMedia.UI.Controls;
using System.Windows.Forms;

namespace OPMedia.UI.Configuration
{
    partial class ProxySettingsDefinitionCtl
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
            this.tableLayoutPanel1 = new OPMedia.UI.Controls.OPMTableLayoutPanel();
            this.label9 = new OPMedia.UI.Controls.OPMLabel();
            this.label7 = new OPMedia.UI.Controls.OPMLabel();
            this.label6 = new OPMedia.UI.Controls.OPMLabel();
            this.label3 = new OPMedia.UI.Controls.OPMLabel();
            this.label5 = new OPMedia.UI.Controls.OPMLabel();
            this.txtProxyPassword = new OPMedia.UI.Controls.OPMTextBox();
            this.txtProxyUser = new OPMedia.UI.Controls.OPMTextBox();
            this.txtProxyPort = new OPMedia.UI.Controls.OPMNumericUpDown();
            this.txtProxyServer = new OPMedia.UI.Controls.OPMTextBox();
            this.cmbProxyType = new OPMedia.UI.Controls.OPMComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtProxyPassword, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtProxyUser, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtProxyPort, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtProxyServer, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbProxyType, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(330, 154);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.FontSize = MetroFramework.MetroLabelSize.Small;
            this.label9.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label9.Location = new System.Drawing.Point(3, 111);
            this.label9.MaximumSize = new System.Drawing.Size(10000, 22);
            this.label9.MinimumSize = new System.Drawing.Size(22, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 22);
            this.label9.TabIndex = 8;
            this.label9.Text = "TXT_PROXYPASSWORD";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.FontSize = MetroFramework.MetroLabelSize.Small;
            this.label7.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label7.Location = new System.Drawing.Point(3, 84);
            this.label7.MaximumSize = new System.Drawing.Size(10000, 22);
            this.label7.MinimumSize = new System.Drawing.Size(22, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 22);
            this.label7.TabIndex = 6;
            this.label7.Text = "TXT_PROXYUSERNAME";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.FontSize = MetroFramework.MetroLabelSize.Small;
            this.label6.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label6.Location = new System.Drawing.Point(3, 57);
            this.label6.MaximumSize = new System.Drawing.Size(10000, 22);
            this.label6.MinimumSize = new System.Drawing.Size(22, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 22);
            this.label6.TabIndex = 4;
            this.label6.Text = "TXT_PROXYPORT";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.FontSize = MetroFramework.MetroLabelSize.Small;
            this.label3.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label3.Location = new System.Drawing.Point(3, 30);
            this.label3.MaximumSize = new System.Drawing.Size(10000, 22);
            this.label3.MinimumSize = new System.Drawing.Size(22, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "TXT_PROXYADDRESS";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.FontSize = MetroFramework.MetroLabelSize.Small;
            this.label5.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.MaximumSize = new System.Drawing.Size(10000, 22);
            this.label5.MinimumSize = new System.Drawing.Size(22, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 22);
            this.label5.TabIndex = 0;
            this.label5.Text = "TXT_PROXYTYPE";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProxyPassword
            // 
            // 
            // 
            // 
            this.txtProxyPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyPassword.Lines = new string[0];
            this.txtProxyPassword.Location = new System.Drawing.Point(142, 111);
            this.txtProxyPassword.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtProxyPassword.MaximumSize = new System.Drawing.Size(10000, 22);
            this.txtProxyPassword.MaxLength = 512;
            this.txtProxyPassword.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.PasswordChar = '●';
            this.txtProxyPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtProxyPassword.SelectedText = "";
            this.txtProxyPassword.Size = new System.Drawing.Size(188, 22);
            this.txtProxyPassword.TabIndex = 9;
            this.txtProxyPassword.UseSelectable = true;
            this.txtProxyPassword.UseSystemPasswordChar = true;
            // 
            // txtProxyUser
            // 
            // 
            // 
            // 
            this.txtProxyUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyUser.Lines = new string[0];
            this.txtProxyUser.Location = new System.Drawing.Point(142, 84);
            this.txtProxyUser.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtProxyUser.MaximumSize = new System.Drawing.Size(10000, 22);
            this.txtProxyUser.MaxLength = 512;
            this.txtProxyUser.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.PasswordChar = '\0';
            this.txtProxyUser.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtProxyUser.SelectedText = "";
            this.txtProxyUser.Size = new System.Drawing.Size(188, 22);
            this.txtProxyUser.TabIndex = 7;
            this.txtProxyUser.UseSelectable = true;
            // 
            // txtProxyPort
            // 
            // 
            // 
            // 
            this.txtProxyPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyPort.Location = new System.Drawing.Point(142, 57);
            this.txtProxyPort.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtProxyPort.MaximumSize = new System.Drawing.Size(10000, 22);
            this.txtProxyPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtProxyPort.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(188, 22);
            this.txtProxyPort.TabIndex = 5;
            this.txtProxyPort.Text = "1";
            this.txtProxyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtProxyPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtProxyServer
            // 
            // 
            // 
            this.txtProxyServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyServer.Lines = new string[0];
            this.txtProxyServer.Location = new System.Drawing.Point(142, 30);
            this.txtProxyServer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.txtProxyServer.MaximumSize = new System.Drawing.Size(10000, 22);
            this.txtProxyServer.MaxLength = 512;
            this.txtProxyServer.MinimumSize = new System.Drawing.Size(22, 22);
            this.txtProxyServer.Name = "txtProxyServer";
            this.txtProxyServer.PasswordChar = '\0';
            this.txtProxyServer.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtProxyServer.SelectedText = "";
            this.txtProxyServer.Size = new System.Drawing.Size(188, 22);
            this.txtProxyServer.TabIndex = 3;
            this.txtProxyServer.UseSelectable = true;
            // 
            // cmbProxyType
            // 
            this.cmbProxyType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbProxyType.FormattingEnabled = true;
            this.cmbProxyType.ItemHeight = 19;
            this.cmbProxyType.Location = new System.Drawing.Point(142, 0);
            this.cmbProxyType.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.cmbProxyType.MaximumSize = new System.Drawing.Size(10000, 0);
            this.cmbProxyType.MinimumSize = new System.Drawing.Size(22, 0);
            this.cmbProxyType.Name = "cmbProxyType";
            this.cmbProxyType.Size = new System.Drawing.Size(188, 25);
            this.cmbProxyType.TabIndex = 1;
            // 
            // ProxySettingsDefinitionCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ProxySettingsDefinitionCtl";
            this.Size = new System.Drawing.Size(330, 154);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OPMTableLayoutPanel tableLayoutPanel1;
        private OPMLabel label9;
        private OPMLabel label7;
        private OPMLabel label6;
        private OPMLabel label3;
        private OPMLabel label5;
        private OPMTextBox txtProxyPassword;
        private OPMTextBox txtProxyUser;
        private OPMNumericUpDown txtProxyPort;
        private OPMTextBox txtProxyServer;
        private OPMComboBox cmbProxyType;

    }
}