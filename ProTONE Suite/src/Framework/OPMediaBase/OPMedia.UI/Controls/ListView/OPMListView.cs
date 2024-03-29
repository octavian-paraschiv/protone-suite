
#region Using directives
using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.UI.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
#endregion

namespace OPMedia.UI.Controls
{
    #region Event Args
    /// <summary>
    /// Implements event arguments wrapper for the SubItemEditing/Edited events.
    /// </summary>
    public class ListViewSubItemEventArgs : HandledEventArgs
    {
        #region Members
        /// <summary>
        /// Reference to the editable control for the subitem.
        /// </summary>
        private Control editableControl = null;
        /// <summary>
        /// The edited subitem.
        /// </summary>
        private ListViewItem.ListViewSubItem editedSubItem = null;
        /// <summary>
        /// The edited item (whose subitem is edited).
        /// </summary>
        private ListViewItem editedItem = null;
        /// <summary>
        /// The edited subitem index (column index).
        /// </summary>
        private int editedSubItemIndex = -1;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the editable control that is used for editing this item.
        /// </summary>
        public Control EditableControl
        {
            get
            {
                return editableControl;
            }
        }

        /// <summary>
        /// Gets the edited subitem.
        /// </summary>
        public ListViewItem.ListViewSubItem SubItem
        {
            get
            {
                return editedSubItem;
            }
        }

        /// <summary>
        /// Gets the edited item.
        /// </summary>
        public ListViewItem Item
        {
            get
            {
                return editedItem;
            }
        }

        /// <summary>
        /// Gets the index of the subitem in the item list 
        /// (the edited column index).
        /// </summary>
        public int SubItemIndex
        {
            get
            {
                return editedSubItemIndex;
            }
        }
        #endregion

        #region Construction
        /// <summary>
        /// Creates an empty new event argument instance.
        /// </summary>
        public ListViewSubItemEventArgs()
            : base()
        {
        }

        /// <summary>
        /// Creates a new event argument instance.
        /// </summary>
        /// <param name="editableControl">The editable control.</param>
        /// <param name="editedItem">The edited item.</param>
        /// <param name="editedSubItem">The edited subitem.</param>
        /// <param name="editedSubItemIndex">The edited subitem index (the edited
        /// column).</param>
        public ListViewSubItemEventArgs(Control editableControl, ListViewItem editedItem,
            ListViewItem.ListViewSubItem editedSubItem, int editedSubItemIndex)
            : base()
        {
            this.editableControl = editableControl;
            this.editedItem = editedItem;
            this.editedSubItem = editedSubItem;
            this.editedSubItemIndex = editedSubItemIndex;
        }
        #endregion
    }
    #endregion

    /// <summary>
    /// Implements a custom editable list view control.
    /// </summary>
    public class OPMListView : ListView
    {
        int _sortColumn = -1;
        SortOrder _sortorder = SortOrder.Ascending;

        int _hotItemRow = 0, _hotItemColumn = 0;

        ImageList _stateImageList = null;

        #region Delegates
        /// <summary>
        /// Type of delegate used to raise EditableListViewXXX events.
        /// Usually, this will be used to display and handle in-place
        /// edit controls, other than the supported ones.
        /// Use carefully since it is currently under development.
        public delegate void EditableListViewEventHandler(object sender,
            ListViewSubItemEventArgs args);
        #endregion

        #region Members
        /// <summary>
        /// A reference to the active edit control (that is,
        /// the control that is used to edit the list contents 
        /// at a given moment.
        /// </summary>
        private Control activeEditControl = null;
        /// <summary>
        /// The collection of edit controls, represented by column/control pairs.
        /// </summary>
        //private Dictionary<int, Control> editControls = new Dictionary<int, Control>();
        /// <summary>
        /// The 0-based index of the edited subitem row.
        /// </summary>
        private int row = -1;
        /// <summary>
        /// The 0-based index of the edited subitem column.
        /// </summary>
        private int column = -1;
        /// <summary>
        /// The method that will be called when raising StartEditing events.
        /// Usually, this will be used to display and handle in-place
        /// edit controls, other than the supported ones.
        /// Use carefully since it is currently under development.
        /// </summary>
        public event EditableListViewEventHandler SubItemEditing = null;
        /// <summary>
        /// The method that will be called when raising EndEditing events.
        /// Usually, this will be used to display and handle in-place
        /// edit controls, other than the supported ones.
        /// Use carefully since it is currently under development.
        /// </summary>
        public event EditableListViewEventHandler SubItemEdited = null;

        #region Win32 Constants
        // Windows Messages (Win32 Legacy programming)
        private const int WM_PAINT = 0x000F;
        #endregion

        #endregion

        #region Properties
        bool _allowEdit = true;
        public bool AllowEditing
        {
            get
            {
                return _allowEdit;
            }

            set
            {
                _allowEdit = value;
            }
        }

        /// <summary>
        /// Gets the 0-based index of the edited subitem row.
        /// </summary>
        public int EditedRow
        {
            get
            {
                return row;
            }
        }

        /// <summary>
        /// Gets the 0-based index of the edited subitem column.
        /// </summary>
        public int EditedColumn
        {
            get
            {
                return column;
            }
        }



        /// <summary>
        /// Gets a value indicating whether clicking an item
        /// selects all its subitems.  
        /// For an EditableListView control, it must be always
        /// true, so the property is made read only.
        /// </summary>
        [ReadOnly(true)]
        public new bool FullRowSelect
        {
            get
            {
                return base.FullRowSelect;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the selected item
        /// in the control remains highlighted when the control
        /// loses focus. 
        /// For an EditableListView control, it must be always
        /// false, so the property is made read only.
        /// </summary>
        [ReadOnly(true)]
        public new bool HideSelection
        {
            get
            {
                return base.HideSelection;
            }
        }

        public new bool GridLines { get; set; }

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

        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool OwnerDraw { get { return base.OwnerDraw; } }

        [ReadOnly(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new ColumnHeaderStyle HeaderStyle { get { return base.HeaderStyle; } }

        public bool AlternateRowColors
        {
            get;
            set;
        }

        Color _overrideBackColor = Color.Empty;
        public Color OverrideBackColor
        {
            get { return _overrideBackColor; }
            set
            {
                _overrideBackColor = value;
                base.BackColor = GetBackColor();
                Invalidate(true);
            }
        }

        private Color GetBackColor()
        {
            if (_overrideBackColor != Color.Empty)
                return _overrideBackColor;

            return ThemeManager.BackColor;
        }

        #endregion

        #region Construction

        public OPMListView(ColumnHeaderStyle headerStyle)
            : this()
        {
            base.HeaderStyle = headerStyle;
        }

        /// <summary>
        /// Default contructor. Calls also the base
        /// class contructor to ensure complete
        /// initialization.
        /// </summary>
        public OPMListView() : base()
        {
            // Set properties to their default values
            // for an EditableListView.

            base.BorderStyle = BorderStyle.FixedSingle;

            base.View = View.Details;
            base.GridLines = false;
            base.MultiSelect = false;
            base.FullRowSelect = true;

            // Set grid row height equal with default combo box height.
            // With this all "regular" comboboxes should fit exactly
            // in a grid cell.
            int h = new ComboBox().Height;
            _stateImageList = new ImageList();
            _stateImageList.ImageSize = new System.Drawing.Size(h, h);
            this.StateImageList = _stateImageList;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.ResizeRedraw = true;
            this.DoubleBuffered = true;

            base.OwnerDraw = true;
            base.DrawItem += OnDrawItem;
            base.DrawColumnHeader += OnDrawColumnHeader;

            base.BackColor = ThemeManager.BackColor;
            base.ForeColor = ThemeManager.ForeColor;

            this.AlternateRowColors = true;

            this.HandleCreated += new EventHandler(OPMListView_HandleCreated);

            this.RegisterAsEventSink();
            OnThemeUpdated();

            this.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);
            this.KeyDown += new KeyEventHandler(OnListViewKeyDown);
            this.GotFocus += new EventHandler(OnEntered);
            this.Enter += new EventHandler(OnEntered);

            OnSelectedIndexChanged(this, EventArgs.Empty);
        }

        void OnEntered(object sender, EventArgs e)
        {
            if (Items != null && Items.Count > 0 && _allowEdit)
            {
                if (SelectedItems != null && SelectedItems.Count == 0)
                {
                    Items[0].Selected = true;
                }
            }
        }

        void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_allowEdit && SelectedItems != null && SelectedItems.Count > 0)
            {
                _hotItemRow = this.SelectedItems[0].Index;
                if (MoveHotItem(0, -1))
                {
                    MoveHotItem(0, 1);
                }
            }

            Invalidate();
        }

        [EventSink(EventNames.ThemeUpdated)]
        public void OnThemeUpdated()
        {
            OnThemeUpdatedInternal();
        }

        protected virtual void OnThemeUpdatedInternal()
        {
            base.BackColor = ThemeManager.BackColor;
            base.ForeColor = ThemeManager.ForeColor;
            Invalidate(true);
        }

        OPMListViewHeaderWrapper _headerWrap = null;
        void OPMListView_HandleCreated(object sender, EventArgs e)
        {
            if (base.HeaderStyle != ColumnHeaderStyle.None)
            {
                _headerWrap = new OPMListViewHeaderWrapper(this);
            }
        }

        void OnDrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            //_sgListHeader.CustomRenderer(e.Graphics, e.Bounds, e);
            OnRenderHeader(e.Graphics, e.Bounds, e);
        }

        void OnRenderHeader(Graphics g, Rectangle clipRectangle, object data)
        {
            DrawListViewColumnHeaderEventArgs e = data as DrawListViewColumnHeaderEventArgs;

            if (base.HeaderStyle == ColumnHeaderStyle.None)
            {
                e.DrawDefault = true;
                return;
            }

            ThemeManager.PrepareGraphics(e.Graphics);

            Rectangle rcFull = Rectangle.Empty;
            if (e.ColumnIndex == 0)
            {
                rcFull = new Rectangle(e.Bounds.Left, e.Bounds.Top, this.Width, 15);
            }

            Rectangle rc = new Rectangle(e.Bounds.Left - 2, e.Bounds.Top - 1,
                e.Bounds.Width, e.Bounds.Height + 2);

            Rectangle rcHeader =
                (rcFull == Rectangle.Empty) ? e.Bounds : rcFull;

            if (e.Bounds.Width < 1 || e.Bounds.Height < 1)
                return;

            using (Brush b = new LinearGradientBrush(e.Bounds, ThemeManager.GradientNormalColor1, ThemeManager.GradientNormalColor2, 90f))
            using (Brush bText = new SolidBrush(ThemeManager.ForeColor))
            using (Pen p = new Pen(ThemeManager.BorderColor, 1))
            {
                e.Graphics.FillRectangle(b, rcHeader);
                e.Graphics.DrawRectangle(p, rc);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Trimming = StringTrimming.EllipsisWord;

                rc.Offset(2, 0);

                string append = (e.ColumnIndex == _sortColumn) ?
                    (_sortorder == SortOrder.Ascending ? " A" : "D") : "";

                e.Graphics.DrawString(e.Header.Text + append, this.Font, bText, rc, sf);
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Window procedure. Legacy Win32 programming.
        /// Neither ListView nor its base classes do expose have an
        /// OnScroll event handler. If we want to catch these events,
        /// we must do it in Win32 style. However, the best way to
        /// scroll in-place controls together with the list view is 
        /// to do via WM_PAINT.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                    {
                        // Set distinctive background for editable fields and also reposition the
                        // subitem being edited (if any)
                        ListViewItem.ListViewSubItem editedSubItem = null;

                        if (this.Items != null)
                        {
                            foreach (ListViewItem item in this.Items)
                            {
                                int subItemIndex = 0;

                                if (item != null && item.SubItems != null)
                                {
                                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                                    {
                                        Control editControl = null;
                                        OPMListViewSubItem editableSubItem = subItem as OPMListViewSubItem;
                                        if (editableSubItem != null)
                                        {
                                            if (!editableSubItem.ReadOnly)
                                            {
                                                editControl = editableSubItem.EditControl;
                                            }
                                        }

                                        if (editControl != null)
                                        {
                                            if (item.Index == EditedRow && subItemIndex == EditedColumn)
                                            {
                                                editedSubItem = subItem;
                                            }
                                        }

                                        subItemIndex++;
                                    }
                                }
                            }
                        }

                        // Reposition edit control.
                        if (editedSubItem != null && activeEditControl != null)
                        {
                            DisplayEditControl(true, activeEditControl, editedSubItem);
                        }
                    }
                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Overrides the default behaviour of the list view when 
        /// an item is clicked with the mouse. This is done in order
        /// to be able to display the edit controls. If no edit control
        /// can be displayed then the default event is raised.
        /// </summary>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            ListViewItem item = this.GetItemAt(e.X, e.Y);
            if (item == null)
                return;

            OPMListViewSubItem subItem = item.GetSubItemAt(e.X, e.Y) as OPMListViewSubItem;
            if (subItem == null)
                // Possibly not editable
                return;

            _hotItemRow = item.Index;
            for (int i = 0; i < item.SubItems.Count; i++)
            {
                if (item.SubItems[i] == subItem)
                {
                    _hotItemColumn = i;
                    break;
                }
            }

            StartEditing(item, subItem);
            base.OnMouseClick(e);
        }

        /// <summary>
        /// Event handler for the Leave event of the in-place edit control.
        /// This must result in ending the editing action. By design in this
        /// case it is assumed that the user accepts the changes.
        /// </summary>
        private void OnEditorLeave(object sender, EventArgs e)
        {
            if (row < 0 || column < 0)
                return;

            EndEditing(true);
        }

        void OnListViewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = false;

            bool isHandled = false;

            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        isHandled = MoveHotItem(-1, 0);
                        break;

                    case Keys.Down:
                        isHandled = MoveHotItem(1, 0);
                        break;

                    case Keys.Right:
                        isHandled = MoveHotItem(0, 1);
                        break;

                    case Keys.Left:
                        isHandled = MoveHotItem(0, -1);
                        break;

                    case Keys.F2:
                        {
                            ListViewItem item = null;
                            OPMListViewSubItem subItem = GetEditableSubItem(_hotItemRow, _hotItemColumn, out item);
                            if (item != null && subItem != null && subItem.ReadOnly == false)
                            {
                                StartEditing(item, subItem);
                                isHandled = true;
                            }
                        }
                        break;
                }
            }

            e.Handled = isHandled;
            e.SuppressKeyPress = isHandled;
        }

        private bool MoveHotItem(int dRow, int dColumn)
        {
            int initialRow = _hotItemRow;
            int initialColumn = _hotItemColumn;

            if (_allowEdit &&
                (Items.Count > 0 && Columns.Count > 0))
            {
                do
                {
                    _hotItemRow += dRow;
                    _hotItemColumn += dColumn;

                    if (_hotItemRow < 0)
                        _hotItemRow = Items.Count - 1;
                    if (_hotItemColumn < 0)
                        _hotItemColumn = Columns.Count - 1;

                    if (_hotItemRow >= Items.Count)
                        _hotItemRow = 0;
                    if (_hotItemColumn >= Columns.Count)
                        _hotItemColumn = 0;

                    ListViewItem item = null;
                    OPMListViewSubItem subItem = GetEditableSubItem(_hotItemRow, _hotItemColumn, out item);
                    if (item != null && subItem != null && subItem.ReadOnly == false)
                    {
                        Invalidate();
                        return true;
                    }
                }
                while ((dColumn == 0 || _hotItemColumn != initialColumn) && (dRow == 0 || _hotItemRow != initialRow));
            }

            return false;
        }

        private OPMListViewSubItem GetEditableSubItem(int row, int column, out ListViewItem item)
        {
            OPMListViewSubItem subItem = null;

            try
            {
                item = this.Items[row];
                if (item != null)
                {
                    subItem = item.SubItems[column] as OPMListViewSubItem;
                }
            }
            catch
            {
                item = null;
                subItem = null;
            }

            return subItem;
        }

        /// <summary>
        /// Occurs when a key is pressed in the area of the in-place edit control.
        /// </summary>
        /// <param name="sender">The object that has sent the event.</param>
        /// <param name="e">The event argumets.</param>
        private void OnEditorKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = false;
            if (e.Modifiers == Keys.None)
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        EndEditing(false);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        return;

                    case Keys.Enter:
                        EndEditing(true);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        return;

                }
            }

            e.SuppressKeyPress = false;
        }

        private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            //_sgListItems.CustomRenderer(e.Graphics, e.Bounds, e);
            OnRenderItems(e.Graphics, e.Bounds, e);
        }

        private void OnRenderItems(Graphics g, Rectangle clipRectangle, object data)
        {
            DrawListViewItemEventArgs e = data as DrawListViewItemEventArgs;

            for (int col = 0; col < e.Item.SubItems.Count; col++)
            {
                ListViewItem.ListViewSubItem subitem = e.Item.SubItems[col];
                if (subitem != null)
                {
                    if (this is OPMShellListView)
                        subitem.Font = e.Item.Font;

                    try
                    {
                        DrawListViewSubItemEventArgs e2 = new DrawListViewSubItemEventArgs(
                            e.Graphics,
                            subitem.Bounds,
                            e.Item,
                            subitem,
                            e.ItemIndex,
                            col,
                            this.Columns[col],
                            e.State);

                        OnDrawSubItem(null, e2);
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message;
                    }
                }
            }
        }

        void OnDrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            try
            {
                if (this.Columns[e.ColumnIndex].Width < 10)
                    return;
            }
            catch
            {
                return;
            }

            try
            {
                bool isSelected = e.Item.Selected;
                bool drawBorder = false;
                bool isValid = true;
                bool isHotItem = false;

                Font drawFont = e.SubItem.Font;

                bool alt = this.AlternateRowColors && (e.ItemIndex % 2 == 1);

                if (e.SubItem is OPMListViewSubItem)
                {
                    isValid = (e.SubItem as OPMListViewSubItem).IsValid;
                }

                Color altRowColor = ThemeManager.AltRowColor;
                if (altRowColor == Color.Empty)
                    altRowColor = GetBackColor();

                Color bColor = isSelected ? ThemeManager.SelectedColor :
                    (alt ? altRowColor : GetBackColor());

                Color fColor = e.Item.ForeColor;
                if (e.Item.UseItemStyleForSubItems)
                    fColor = ThemeManager.ForeColor;

                ExtendedSubItemDetail esid = e.SubItem.Tag as ExtendedSubItemDetail;

                OPMListViewSubItem osi = e.SubItem as OPMListViewSubItem;
                if (osi != null && !osi.ReadOnly)
                {
                    isHotItem = this.Focused &&
                        endEditingPending == 0 &&
                        (e.ColumnIndex == _hotItemColumn && e.ItemIndex == _hotItemRow);

                    if (osi.EditControl is LinkLabel)
                    {
                        fColor = ThemeManager.LinkColor;
                        drawFont = new Font(drawFont, drawFont.Style | FontStyle.Underline);
                    }
                    else
                    {
                        // here's an editable subitem
                        drawBorder = true;

                        if (isValid)
                        {
                            bColor = ThemeManager.WndValidColor;
                            fColor = ThemeManager.WndTextColor;

                            if (isSelected)
                            {
                                bColor = ThemeManager.GradientNormalColor2;
                                fColor = ThemeManager.WndValidColor;
                            }
                        }
                        else
                        {
                            bColor = ThemeManager.ColorValidationFailed;
                            fColor = ThemeManager.WndTextColor;
                        }
                    }
                }

                int d = 0;
                bool drawImage = false;

                if (e.ColumnIndex == 0)
                {
                    if (this.SmallImageList != null)
                    {
                        d += this.SmallImageList.ImageSize.Width + 2;
                        drawImage = true;
                    }
                }

                Rectangle rc1 = new Rectangle(e.Bounds.Left + d, e.Bounds.Top, e.Bounds.Width - d, e.Bounds.Height);
                Rectangle rc2 = new Rectangle(e.Bounds.Left + d - 1, e.Bounds.Top, e.Bounds.Width - d, e.Bounds.Height);
                rc2.Inflate(1, 0);

                Rectangle rcHot = new Rectangle(e.Bounds.Left + d - 1, e.Bounds.Top + 1, e.Bounds.Width - d - 2, e.Bounds.Height - 2);

                using (Brush b1 = new SolidBrush(bColor))
                using (Brush b2 = new SolidBrush(GetBackColor()))
                using (Brush bt = new SolidBrush(fColor))
                using (Pen pEditableItemsBorder = new Pen(ThemeManager.BorderColor))
                using (Pen pHotItemsBorder = new Pen(ThemeManager.ListHotItemColor, 2))
                using (Pen pGridLines = new Pen(ThemeManager.ForeColor))
                {
                    ThemeManager.PrepareGraphics(e.Graphics);

                    //e.Graphics.FillRectangle(b2, rc2);
                    e.Graphics.FillRectangle(b1, rc2);

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    sf.Trimming = StringTrimming.EllipsisWord;

                    if (GridLines)
                    {
                        e.Graphics.DrawRectangle(pGridLines, rc1);
                    }

                    if (drawBorder)
                    {
                        e.Graphics.DrawRectangle(pEditableItemsBorder, rc2);
                    }

                    if (isHotItem)
                    {
                        e.Graphics.DrawRectangle(pHotItemsBorder, rcHot);
                    }


                    rc1.X += 3;

                    string text = e.SubItem.Text;
                    if (esid != null)
                    {
                        text = esid.Text;
                    }

                    if (text == null)
                        text = string.Empty;

                    bool drawText = true;

                    if (drawImage)
                    {
                        Image imgToDraw = null;

                        if (string.IsNullOrEmpty(e.Item.ImageKey) == false)
                            imgToDraw = SmallImageList.Images[e.Item.ImageKey];

                        if (imgToDraw == null && 0 <= e.Item.ImageIndex && e.Item.ImageIndex < SmallImageList.Images.Count)
                            imgToDraw = SmallImageList.Images[e.Item.ImageIndex];

                        Rectangle rc = new Rectangle(e.Bounds.Location, imgToDraw.Size);
                        rc.Offset(0, (e.Bounds.Height - imgToDraw.Height) / 2);

                        e.Graphics.DrawImageUnscaled(imgToDraw, rc);
                    }
                    else if (esid != null && esid.Image != null)
                    {
                        Size imgSize = new Size(16, 16);
                        if (this.StateImageList != null)
                        {
                            imgSize = this.StateImageList.ImageSize;
                        }

                        var imgToDraw = ImageProvider.ScaleImage(esid.Image, imgSize, false);

                        Rectangle rc = new Rectangle(e.Bounds.Location, imgToDraw.Size);
                        rc.Offset(0, (e.Bounds.Height - imgToDraw.Height) / 2);

                        e.Graphics.DrawImageUnscaled(imgToDraw, rc);

                        drawText = false;
                    }

                    if (drawText)
                        e.Graphics.DrawString(text, drawFont, bt, rc1, sf);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        #endregion

        #region Methods
        public void ShowSortGlyph(int column, SortOrder order)
        {
            _sortColumn = column;
            _sortorder = order;
            Invalidate();
        }

        /// <summary>
        /// Overloaded. Assigns the in-place edit control for a given column.
        /// Also ensures that the size of the edit controls collection is 
        /// grown automatically if needed. 
        /// CAUTIONS: 
        /// 1. The in-place edit control and the list view must be siblings, 
        /// children of the same UI component, otherwise the SetEditControl
        /// action will be ignored.
        /// 2. The in-place edit control must be a suppeorted edit control 
        /// otherwise the SetEditControl action will be ignored. Use 
        /// IsSupportedEditControl to verify is a specified control is a
        /// supported in-place edit control.
        /// </summary>
        /// <param name="column">The column index.</param>
        /// <param name="editControl">The edit control that must be assigned
        /// for the specified column.
        /// </param>
        public void RegisterEditControl(Control editControl)
        {

            // Check the in-place edit control is a supported in-place
            // edit control.
            if (!(editControl is TextBox ||
               editControl is ComboBox ||
               editControl is NumericUpDown ||
               editControl is LinkLabel ||
               editControl is DateTimePicker))
                return;

            if (editControl is ComboBox)
            {
                (editControl as ComboBox).Font = ThemeManager.SmallestFont;
                (editControl as ComboBox).ItemHeight = ThemeManager.SmallestFont.Height + 2;
                (editControl as ComboBox).DropDownHeight = 6 * this.Font.Height + 4;
            }
            else if (editControl is MultilineEditTextBox)
            {
                (editControl as MultilineEditTextBox).Height = 6 * this.Font.Height + 4;
                (editControl as MultilineEditTextBox).Multiline = true;
                (editControl as MultilineEditTextBox).ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                editControl.Height = this.Font.Height + 4;
            }

            // Set parent for the in-place edit control.
            if (this != editControl.Parent)
            {
                this.SuspendLayout();
                editControl.Parent = this;
                editControl.Visible = false;
                this.ResumeLayout();
            }
        }

        #endregion

        #region Implementation
        bool _restoreKeyPreview = false;

        /// <summary>
        /// Displays the in-place edit control for a given list view subitem.
        /// It is assumed that the in-place edit control was previously created
        /// and assigned to the proper column, by means of the SetEditControl
        /// method.
        /// </summary>
        /// <param name="editedItem">The item to be edited.</param>
        /// <param name="editedSubItem">The subitem to be edited.</param>
        public void StartEditing(ListViewItem editedItem, OPMListViewSubItem editedSubItem)
        {
            if (_allowEdit == false)
                return;

            row = editedItem.Index;
            column = editedItem.SubItems.IndexOf(editedSubItem);

            if (row < 0 || column < 0 || editedSubItem == null)
                return;

            // Check if event handler available and if positive, raise the event

            Control editControl = null;

            // Override the editable control in the custom subitem, if such an item is used.
            OPMListViewSubItem customSubItem = editedSubItem as OPMListViewSubItem;
            if (customSubItem != null)
            {
                if (customSubItem.ReadOnly)
                {
                    // non-editable item
                    return;
                }
                else
                {
                    // override the edit control
                    editControl = customSubItem.EditControl;
                }
            }

            if (editControl == null)
                return;

            ListViewSubItemEventArgs args =
                new ListViewSubItemEventArgs(editControl, editedItem,
                editedSubItem, column);
            if (SubItemEditing != null && editControl != null)
            {
                SubItemEditing(this, args);
            }

            if (!(editControl is LinkLabel))
            {
                // For link labels, we only raise the SubItemEditing event

                // Check if the event was handled - thus the in-place edit controls
                // displayed.
                if (!args.Handled)
                {
                    // Display edit control and also set text.
                    DisplayEditControl(false, editControl, editedSubItem);
                }

                ThemeForm frm = FindForm() as ThemeForm;
                if (frm != null)
                {
                    frm.SuppressKeyPress = true;
                    _restoreKeyPreview = true;
                }
            }
        }

        int endEditingPending = 0;

        /// <summary>
        /// End the in-place editing action. It results in "hiding" the
        /// in-place edit control.
        /// </summary>
        /// <param name="acceptChanges">Set to true to accept the new value,
        /// that is resulting after the editing action.</param>
        public void EndEditing(bool acceptChanges)
        {
            if (1 == Interlocked.CompareExchange(ref endEditingPending, 1, 0))
                return;

            try
            {
                if (activeEditControl == null || !activeEditControl.Visible)
                    return;

                ThemeForm frm = FindForm() as ThemeForm;
                if (frm != null && _restoreKeyPreview)
                {
                    frm.SuppressKeyPress = false;
                    _restoreKeyPreview = false;
                }

                // Check the row bounds.
                if (row < 0 || row >= this.Items.Count)
                    return;
                // Check the column bounds.
                if (column < 0 || column >= this.Columns.Count)
                    return;

                ListViewItem editedItem = this.Items[row];
                ListViewItem.ListViewSubItem editedSubItem = editedItem.SubItems[column];
                if (acceptChanges)
                {
                    // Editing results should be handled differently
                    // for each type of supported in-place edit control.
                    OPMListViewSubItem customSubItem = editedSubItem as OPMListViewSubItem;


                    string text = "";

                    if (activeEditControl is MultilineEditTextBox)
                        text = (activeEditControl as MultilineEditTextBox).MultiLineText;
                    else
                        text = activeEditControl.Text;

                    if (customSubItem != null)
                        customSubItem.Text = text;
                    else
                        editedSubItem.Text = text;

                    // Check if event handler available and if positive, raise the event
                    if (SubItemEdited != null)
                    {

                        ListViewSubItemEventArgs args =
                            new ListViewSubItemEventArgs(activeEditControl, editedItem,
                            editedSubItem, column);
                        SubItemEdited(this, args);

                        // Check if the event was handled - thus the in-place edit controls
                        // displayed.
                        if (args.Handled)
                        {
                            return;
                        }
                    }
                }
            }
            finally
            {
                // Nothing is edited right now.
                row = column = -1;

                // Disable the control and make it invisible ("hide" it).
                //>>>> FIXUP for nasty bug
                // for some strange reason (which we don't have time to investigate)
                // the SubItemEdited event fucks up the activeEditControl of the list.
                // So we put an extra protection here to prevent exceptions popping up
                if (activeEditControl != null)
                {
                    activeEditControl.Hide();
                    activeEditControl.Enabled = false;
                    activeEditControl.BringToFront();
                    // Unsubscribe for the event handlers.
                    activeEditControl.Leave -= new EventHandler(OnEditorLeave);
                    activeEditControl.KeyDown -= new KeyEventHandler(OnEditorKeyDown);

                    // Set activeEditControl as null
                    activeEditControl = null;
                }

                // Focus back to the list.
                Focus();

                Interlocked.Exchange(ref endEditingPending, 0);
                Invalidate();
            }
        }

        /// <summary>
        /// Displays the proper in-place edit control.
        /// </summary>
        /// <param name="positioningOnly">Indicates that we only require repositioning the 
        /// given edit control in the current row/column.</param>
        /// <param name="editControl">The edit control to display/reposition.</param>
        /// <param name="editedSubItem">The subitem being edited (to display
        /// the edit control for).</param>
        private void DisplayEditControl(bool positioningOnly, Control editControl,
            ListViewItem.ListViewSubItem editedSubItem)
        {
            // see if the subitem is a customized one
            activeEditControl = editControl;
            if (activeEditControl == null)
            {
                // non-editable column
                return;
            }

            #region Commented code
            //// Set the bounding rectangle for the in-place edit control
            //// Check if edit control is overlayed on column header.
            //if (editedSubItem.Bounds.Location.Y + ClientSize.Height >= Height - 3)
            //{
            //    int origHeight = activeEditControl.Height;
            //    int dh = (editedSubItem.Bounds.Height - origHeight) / 2;


            //    // Not overlayed, so display the edit control.
            //    activeEditControl.Bounds = new Rectangle(editedSubItem.Bounds.Left, editedSubItem.Bounds.Top + dh,
            //        editedSubItem.Bounds.Width, editedSubItem.Bounds.Height - dh);

            //    activeEditControl.Height = origHeight;

            //}
            //else
            //{
            //    // Overlayed, so control must be hidden.
            //    // Caution, Setting visible to false will trigger OnLeave event.
            //    // So the "hiding" control must be done by means of resizing.
            //    activeEditControl.Bounds = new Rectangle(0, 0, 0, 0);
            //}
            #endregion

            int origHeight = activeEditControl.Height;
            //int dh = (editedSubItem.Bounds.Height - origHeight) / 2;

            // Not overlayed, so display the edit control.
            activeEditControl.Bounds = new Rectangle(editedSubItem.Bounds.Left, editedSubItem.Bounds.Top,
                editedSubItem.Bounds.Width, editedSubItem.Bounds.Height);

            if (editControl is OPMComboBox)
            {
                activeEditControl.Bounds = new Rectangle(editedSubItem.Bounds.Left - 1, editedSubItem.Bounds.Top,
                    editedSubItem.Bounds.Width + 1, editedSubItem.Bounds.Height);
            }
            else
            {
                activeEditControl.Bounds = new Rectangle(editedSubItem.Bounds.Left, editedSubItem.Bounds.Top,
                    editedSubItem.Bounds.Width, editedSubItem.Bounds.Height);
            }

            activeEditControl.Height = origHeight;

            // Enable the control
            activeEditControl.Visible = true;
            activeEditControl.Enabled = true;

            // If the control had only to be repositined, return.
            if (positioningOnly)
                return;

            // (re)Subscribe for the event handlers.
            activeEditControl.Leave -= new EventHandler(OnEditorLeave);
            activeEditControl.KeyDown -= new KeyEventHandler(OnEditorKeyDown);
            activeEditControl.Leave += new EventHandler(OnEditorLeave);
            activeEditControl.KeyDown += new KeyEventHandler(OnEditorKeyDown);

            // Set the focus on the control.
            activeEditControl.Focus();

            try
            {
                DateTimePicker activeDTP = (activeEditControl as DateTimePicker);
                if (activeDTP != null && activeDTP.ShowUpDown &&
                    activeDTP.Format == DateTimePickerFormat.Custom &&
                    activeDTP.CustomFormat == "HH:mm:ss")
                {
                    // This will reset field selection to Hour field
                    activeDTP.CustomFormat = "HHmmss";
                    activeDTP.CustomFormat = "HH:mm:ss";

                    // And then we move the field selection to Seconds
                    SendKeys.Send("{RIGHT}");
                    SendKeys.Send("{RIGHT}");
                }
            }
            catch { }

            // Finally also display the text
            DisplayEditControlText(editedSubItem.Text);
        }

        /// <summary>
        /// Sets the text to be displayed in the edit control.
        /// Depending on the type of the edit control, this sometimes can be
        /// done in various ways.
        /// </summary>
        /// <param name="text">The text to display.</param>
        private void DisplayEditControlText(string text)
        {
            try
            {
                MultilineEditTextBox tbmInPlaceEditor = activeEditControl as MultilineEditTextBox;
                if (tbmInPlaceEditor != null)
                {
                    // In-place edit control is a MultilineEditTextBox
                    tbmInPlaceEditor.MultiLineText = text;
                    //tbmInPlaceEditor.SelectAll();
                    return;
                }

                TextBox tbInPlaceEditor = activeEditControl as TextBox;
                if (tbInPlaceEditor != null)
                {
                    // In-place edit control is a TextBox
                    tbInPlaceEditor.Text = text;
                    tbInPlaceEditor.SelectAll();
                    return;
                }

                ComboBox comboInPlaceEditor = activeEditControl as ComboBox;
                if (comboInPlaceEditor != null)
                {
                    // In-place edit control is a ComboBox
                    comboInPlaceEditor.Text = text;
                    return;
                }

                DateTimePicker dtp = activeEditControl as DateTimePicker;
                if (dtp != null)
                {
                    DateTimeConverter dtc = new DateTimeConverter();
                    dtp.Value = (DateTime)dtc.ConvertFromInvariantString(text);
                    return;
                }

                NumericUpDown nud = activeEditControl as NumericUpDown;
                if (nud != null)
                {
                    DecimalConverter dc = new DecimalConverter();
                    nud.Value = (decimal)dc.ConvertFromInvariantString(text);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        public int EffectiveWidth
        {
            get
            {
                return this.Width - SystemInformation.VerticalScrollBarWidth - 5;
            }
        }
    }

    public class HeaderlessListView : OPMListView
    {
        public HeaderlessListView()
            : base(ColumnHeaderStyle.None)
        {
        }
    }

    public class OPMListViewHeaderWrapper : NativeWindow
    {
        OPMListView _parent;

        public OPMListViewHeaderWrapper(OPMListView parent) : base()
        {
            _parent = parent;

            int hHeader = User32.SendMessage(parent.Handle, (int)HeaderItemFlags.LVM_GETHEADER, 0, 0); //Get the handle of the ListView header

            this.AssignHandle(new IntPtr(hHeader));
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)HeaderItemFlags.HDM_LAYOUT:
                    {
                        HDLAYOUT layout = (HDLAYOUT)Marshal.PtrToStructure(m.LParam, typeof(HDLAYOUT));
                        RECT rc = (RECT)Marshal.PtrToStructure(layout.prc, typeof(RECT));
                        WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(layout.pwpos, typeof(WINDOWPOS));


                        //using (Graphics g = _parent.CreateGraphics())
                        {
                            int height = 15;
                            pos.hwnd = this.Handle;
                            pos.hwndInsertAfter = IntPtr.Zero;
                            pos.flags = 32; // SWP_FRAMECHANGED;
                            pos.x = rc.Left;
                            pos.y = rc.Top;
                            pos.cx = rc.Right - rc.Left;
                            pos.cy = height;

                            rc.Top = height;

                            Marshal.StructureToPtr(rc, layout.prc, false);
                            Marshal.StructureToPtr(pos, layout.pwpos, false);

                            _parent.BeginInvoke((MethodInvoker)delegate
                            {
                                User32.InvalidateRect(this.Handle, 0, true);
                                _parent.Invalidate(true);
                            });
                        }
                    }

                    m.Result = new IntPtr(1);

                    break;

                default:
                    base.WndProc(ref m);
                    break;

            }
        }
    }

    public class ExtendedSubItemDetail
    {
        public Image Image { get; private set; }
        public string Text { get; private set; }

        public ExtendedSubItemDetail(Image image, string text)
        {
            Image = image;
            Text = text;
        }
    }
}


