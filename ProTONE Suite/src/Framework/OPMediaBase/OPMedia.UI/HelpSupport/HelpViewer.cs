using OPMedia.Core.TranslationSupport;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.HelpSupport
{
    public class HelpViewer : ToolForm
    {
        private Controls.OPMToolStrip tsMain;
        private OPMTriStateToolStripButton tsbPrev;
        private OPMTriStateToolStripButton tsbNext;
        private System.Windows.Forms.WebBrowser wbHelpDisplay;

        private Stack<string> _bckUrls = new Stack<string>();
        private Stack<string> _fwdUrls = new Stack<string>();

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.wbHelpDisplay = new System.Windows.Forms.WebBrowser();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tsMain = new OPMedia.UI.Controls.OPMToolStrip();
            this.tsbPrev = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tsbNext = new OPMedia.UI.Controls.OPMTriStateToolStripButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // wbHelpDisplay
            // 
            this.wbHelpDisplay.AllowWebBrowserDrop = false;
            this.wbHelpDisplay.CausesValidation = false;
            this.wbHelpDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbHelpDisplay.IsWebBrowserContextMenuEnabled = false;
            this.wbHelpDisplay.Location = new System.Drawing.Point(3, 40);
            this.wbHelpDisplay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
            this.wbHelpDisplay.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbHelpDisplay.Name = "wbHelpDisplay";
            this.wbHelpDisplay.ScriptErrorsSuppressed = true;
            this.wbHelpDisplay.Size = new System.Drawing.Size(791, 557);
            this.wbHelpDisplay.TabIndex = 0;
            this.wbHelpDisplay.TabStop = false;
            this.wbHelpDisplay.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.wbHelpDisplay_PreviewKeyDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.Controls.Add(this.wbHelpDisplay, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tsMain, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(797, 600);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tsMain
            // 
            this.tsMain.AutoSize = false;
            this.tsMain.BackColor = System.Drawing.Color.White;
            this.tsMain.ForeColor = System.Drawing.Color.Black;
            this.tsMain.GripMargin = new System.Windows.Forms.Padding(0);
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPrev,
            this.tsbNext});
            this.tsMain.Location = new System.Drawing.Point(1, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.ShowBorder = true;
            this.tsMain.Size = new System.Drawing.Size(795, 40);
            this.tsMain.TabIndex = 1;
            this.tsMain.Text = "opmToolStrip1";
            this.tsMain.VerticalGradient = false;
            // 
            // tsbPrev
            // 
            this.tsbPrev.ActiveImage = null;
            this.tsbPrev.AutoSize = false;
            this.tsbPrev.AutoToolTip = false;
            this.tsbPrev.CheckedImage = null;
            this.tsbPrev.DisabledImage = null;
            this.tsbPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrev.Image = global::OPMedia.UI.Properties.Resources.Back;
            this.tsbPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrev.InactiveImage = global::OPMedia.UI.Properties.Resources.Back;
            this.tsbPrev.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tsbPrev.Name = "tsbPrev";
            this.tsbPrev.Size = new System.Drawing.Size(30, 30);
            this.tsbPrev.Text = "toolStripButton1";
            this.tsbPrev.Click += new System.EventHandler(this.tsbPrev_Click);
            // 
            // tsbNext
            // 
            this.tsbNext.ActiveImage = null;
            this.tsbNext.AutoSize = false;
            this.tsbNext.AutoToolTip = false;
            this.tsbNext.CheckedImage = null;
            this.tsbNext.DisabledImage = null;
            this.tsbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNext.Image = global::OPMedia.UI.Properties.Resources.Forward;
            this.tsbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNext.InactiveImage = global::OPMedia.UI.Properties.Resources.Forward;
            this.tsbNext.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.tsbNext.Name = "tsbNext";
            this.tsbNext.Size = new System.Drawing.Size(30, 30);
            this.tsbNext.Text = "toolStripButton2";
            this.tsbNext.Click += new System.EventHandler(this.tsbNext_Click);
            // 
            // HelpViewer
            // 
            this.ClientSize = new System.Drawing.Size(797, 600);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "HelpViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Shown += new System.EventHandler(this.HelpViewer_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        public HelpViewer()
            : base()
        {
            InitializeComponent();

            UpdatePrevnextDocuments();

            SetTitle("TXT_APP_NAME");
            this.InheritAppIcon = false;
            this.Icon = SystemIcons.Question;


            wbHelpDisplay.DocumentCompleted += wbHelpDisplay_DocumentCompleted;
            wbHelpDisplay.IsWebBrowserContextMenuEnabled = true;
        }


        void HelpViewer_Shown(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(10, 10);
            this.Width = Screen.GetWorkingArea(this).Width / 2;
            this.Height = Screen.GetWorkingArea(this).Height - 20;
        }

        private TableLayoutPanel tableLayoutPanel1;

        string _displayedUrl = string.Empty;
        bool _fromHistory = false;

        public void OpenURL(string helpUri, bool fromHistory = false)
        {
            _fromHistory = fromHistory;
            wbHelpDisplay.DocumentText = "Document is loading, please wait ...";
            wbHelpDisplay.Url = new Uri(helpUri);
        }

        void wbHelpDisplay_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var newUrl = GetUrlWithoutFragment(e.Url);

            if (_displayedUrl?.Length > 0 &&
                string.Compare(newUrl ?? "", _displayedUrl, true) != 0 &&
                !_fromHistory &&
                PushUrlToStack(_bckUrls, _displayedUrl))
                UpdatePrevnextDocuments();

            _fromHistory = false;
            _displayedUrl = newUrl;

            string newTitle = string.Format("{0} {1}: {2}",
                Translator.Translate("TXT_APP_NAME"),
                Translator.Translate("TXT_HELP"),
                wbHelpDisplay.DocumentTitle
                );

            SetTitle(newTitle);
        }

        private void wbHelpDisplay_PreviewKeyDown(object sender, System.Windows.Forms.PreviewKeyDownEventArgs e)
        {
            // TODO identify which keys should be allowed and which not

            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (e.Modifiers == Keys.Control)
                        tsbNext_Click(sender, e);
                    return;

                case Keys.Left:
                    if (e.Modifiers == Keys.Control)
                        tsbPrev_Click(sender, e);
                    return;

                case Keys.Back:
                    tsbPrev_Click(sender, e);
                    return;

                case Keys.F1:
                    return;
            }

            base.ProcessKeyDown(sender as Control, e.KeyCode, e.Modifiers);
        }


        private void tsbPrev_Click(object sender, EventArgs e)
        {
            var url = PopUrlFromStack(_bckUrls);

            if (url?.Length > 0 && PushUrlToStack(_fwdUrls, _displayedUrl))
                OpenURL(url, true);

            UpdatePrevnextDocuments();
        }

        private void tsbNext_Click(object sender, EventArgs e)
        {
            var url = PopUrlFromStack(_fwdUrls);

            if (url?.Length > 0 && PushUrlToStack(_bckUrls, _displayedUrl))
                OpenURL(url, true);

            UpdatePrevnextDocuments();
        }

        private void UpdatePrevnextDocuments()
        {
            if (_bckUrls.Count > 0)
            {
                tsbPrev.Enabled = true;
                tsbPrev.ToolTipText = string.Format("{0}: {1}",
                  Translator.Translate("TXT_BACK"), _bckUrls.Peek());
            }
            else
            {
                tsbPrev.Enabled = false;
                tsbPrev.ToolTipText = string.Empty;
            }

            if (_fwdUrls.Count > 0)
            {
                tsbNext.Enabled = true;
                tsbNext.ToolTipText = string.Format("{0}: {1}",
                   Translator.Translate("TXT_FORWARD"), _fwdUrls.Peek());
            }
            else
            {
                tsbNext.Enabled = false;
                tsbNext.ToolTipText = string.Empty;
            }
        }

        private bool PushUrlToStack(Stack<string> stack, string url)
        {
            try
            {
                if (stack != null)
                {
                    var uri = new Uri(url);
                    if (string.Compare(uri.Scheme, "about", true) != 0 && string.IsNullOrEmpty(uri.Fragment))
                    {
                        string topUriInStack = (stack.Count > 0) ? stack.Peek() : null;
                        if (string.Compare(topUriInStack, url, true) != 0)
                        {
                            stack.Push(url);
                            return true;
                        }
                    }
                }
            }
            catch { }

            return false;
        }

        private string PopUrlFromStack(Stack<string> stack)
        {
            return (stack?.Count > 0) ? stack.Pop() : null;
        }

        private string GetUrlWithoutFragment(Uri uri)
        {
            try
            {
                if (uri?.Fragment?.Length > 0)
                    return uri.ToString().Replace(uri.Fragment, "");
            }
            catch { }

            return uri?.ToString();
        }
    }
}
