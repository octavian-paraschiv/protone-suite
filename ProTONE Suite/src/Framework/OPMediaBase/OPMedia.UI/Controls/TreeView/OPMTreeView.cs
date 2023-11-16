using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.UI.Properties;
using OPMedia.UI.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OPMedia.UI.Controls
{

    public class OPMTreeView : TreeView
    {
        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor { get { return base.BackColor; } }

        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ForeColor { get { return base.ForeColor; } }

        ImageList _sil = new ImageList();

        public OPMTreeView()
            : base()
        {
            base.BorderStyle = BorderStyle.FixedSingle;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;

            _sil.TransparentColor = Color.Magenta;
            _sil.Images.Add(Resources.TVState0);
            _sil.Images.Add(Resources.TVState1);
            this.StateImageList = _sil;

            this.RegisterAsEventSink();
            OnThemeUpdated();

            this.DrawNode += new DrawTreeNodeEventHandler(OPMTreeView_DrawNode);
        }

        void OPMTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            bool isFocused = (e.State & TreeNodeStates.Focused) == TreeNodeStates.Focused;
            bool isSelected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;

            if (isFocused == false && isSelected == false && e.Node.IsSelected == false)
            {
                e.DrawDefault = true;
                return;
            }

            ThemeManager.PrepareGraphics(e.Graphics);

            Font f = e.Node.NodeFont;
            if (f == null)
            {
                f = this.Font;
            }
            if (f == null)
            {
                f = ThemeManager.NormalFont;
            }

            SizeF sz = e.Graphics.MeasureString(e.Node.Text, f);
            Rectangle rc = new Rectangle(e.Bounds.Left, e.Bounds.Top, (int)sz.Width + 4, e.Bounds.Height - 1);

            Rectangle rcx = e.Bounds;
            rcx.Inflate(2, 2);

            using (Brush b = new SolidBrush(ThemeManager.BackColor))
            using (Brush bBack = new SolidBrush(ThemeManager.SelectedColor))
            using (Brush bFore = new SolidBrush(ThemeManager.ForeColor))
            {
                e.Graphics.FillRectangle(b, rcx);
                e.Graphics.FillRectangle(bBack, rc);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Trimming = StringTrimming.EllipsisWord;

                e.Graphics.DrawString(e.Node.Text, f, bFore, rc, sf);
            }
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            base.BackColor = ThemeManager.BackColor;
            base.ForeColor = ThemeManager.ForeColor;
        }

        public TreeNode FindNode(string nodeText, bool compareNoCase)
        {
            foreach (TreeNode node in Nodes)
            {
                TreeNode childNode = FindNode(node, nodeText, compareNoCase);
                if (childNode != null)
                {
                    return childNode;
                }
            }

            return null;
        }

        TreeNode FindNode(TreeNode startNode, string nodeText, bool compareNoCase)
        {
            if (string.Compare(startNode.Text, nodeText, compareNoCase) == 0)
            {
                return startNode;
            }

            foreach (TreeNode node in startNode.Nodes)
            {
                TreeNode childNode = FindNode(node, nodeText, compareNoCase);
                if (childNode != null)
                {
                    return childNode;
                }
            }

            return null;
        }

        protected override void WndProc(ref Message m)
        {
            // Suppress WM_LBUTTONDBLCLK
            if (base.CheckBoxes)
            {
                if (m.Msg == (int)Messages.WM_LBUTTONDBLCLK)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
            }

            base.WndProc(ref m);
        }
    }


}
