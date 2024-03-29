﻿using OPMedia.Core;
using OPMedia.UI.Themes;
using System.Collections.Generic;
using System.Windows.Forms;


namespace OPMedia.UI.FileTasks
{
    public partial class FileTaskErrorReport : ThemeForm
    {
        public FileTaskErrorReport(Dictionary<string, string> errorMap, string title, string message)
        {
            InitializeComponent();

            SetTitle(title);
            lblDesc.Text = message;
            pbWarn.Image = ImageProvider.GetUser32Icon(User32Icon.Warning, true);

            tvReports.Nodes.Clear();

            foreach (KeyValuePair<string, string> kvp in errorMap)
            {
                TreeNode tnPath = new TreeNode(kvp.Key);
                tnPath.NodeFont = ThemeManager.NormalBoldFont;

                TreeNode tnError = new TreeNode(kvp.Value);
                tnError.NodeFont = ThemeManager.NormalFont;

                tnPath.Nodes.Add(tnError);

                tvReports.Nodes.Add(tnPath);
            }

            tvReports.ExpandAll();

            int treeHeight = 2 * errorMap.Count * tvReports.ItemHeight + SystemInformation.HorizontalScrollBarHeight;


            this.Height = tvReports.Top + treeHeight + 30;
        }
    }
}
