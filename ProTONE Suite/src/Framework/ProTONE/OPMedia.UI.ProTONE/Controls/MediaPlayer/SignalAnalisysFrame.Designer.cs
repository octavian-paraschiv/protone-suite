namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    partial class SignalAnalisysFrame
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
            this.signalAnalysisScreen1 = new OPMedia.UI.ProTONE.Controls.MediaPlayer.Screens.SignalAnalysisScreen();
            this.SuspendLayout();
            // 
            // signalAnalysisScreen1
            // 
            this.signalAnalysisScreen1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signalAnalysisScreen1.FontSize = OPMedia.UI.Themes.FontSizes.Normal;
            this.signalAnalysisScreen1.Location = new System.Drawing.Point(4, 25);
            this.signalAnalysisScreen1.Margin = new System.Windows.Forms.Padding(0);
            this.signalAnalysisScreen1.Name = "signalAnalysisScreen1";
            this.signalAnalysisScreen1.OverrideBackColor = System.Drawing.Color.Empty;
            this.signalAnalysisScreen1.Size = new System.Drawing.Size(436, 336);
            this.signalAnalysisScreen1.TabIndex = 0;
            // 
            // SignalAnalisysFrame
            // 
            this.AllowDrop = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(444, 365);
            this.Controls.Add(this.signalAnalysisScreen1);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimumSize = new System.Drawing.Size(200, 85);
            this.Name = "SignalAnalisysFrame";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "";
            this.ResumeLayout(false);

        }



        #endregion

        private Screens.SignalAnalysisScreen signalAnalysisScreen1;
    }
}