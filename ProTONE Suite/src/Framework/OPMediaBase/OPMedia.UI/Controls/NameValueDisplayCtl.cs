using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OPMedia.UI.Themes;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using OPMedia.Core.TranslationSupport;

namespace OPMedia.UI.Controls
{
    public partial class NameValueDisplayCtl : OPMBaseControl
    {
        OPMToolTipData data = null;

        Font _fVal = ThemeManager.NormalFont;
        Font _fKey = ThemeManager.NormalBoldFont;
        Font _fTitle = ThemeManager.LargeFont;

        public Size RequiredSize { get; set; }

        public NameValueDisplayCtl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(OnPaint);
        }

        public void AssignData(string title, Dictionary<string, string> values, Image img)
        {
            data = new OPMToolTipData { Values = values, TitleImage = img, Title = title };
            this.RequiredSize = CalculateSize(data);
            this.Size = this.RequiredSize;
            Invalidate();
        }

        public void ClearData()
        {
            data = null;

            this.RequiredSize = CalculateSize(data);
            this.Size = this.RequiredSize;

            Invalidate();
        }

        void OnPaint(object sender, PaintEventArgs e)
        {
            ThemeManager.PrepareGraphics(e.Graphics);

            using (LinearGradientBrush b = new LinearGradientBrush(ClientRectangle,
                ControlPaint.Light(ThemeManager.GradientNormalColor1),
                    ControlPaint.Light(ThemeManager.GradientNormalColor2), 90))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }

            using (Pen p = new Pen(ThemeManager.BorderColor))
            {
                Rectangle rc = new Rectangle(ClientRectangle.Location,
                    new Size(ClientRectangle.Width - 1, ClientRectangle.Height - 1));

                e.Graphics.DrawRectangle(p, rc);
            }

            DrawContents(e.Graphics);
        }

        private void DrawContents(Graphics g)
        {
            if (data != null)
            {
                int titleOffsetX = 5;
                int titleOffsetY = 5;
                int textOffsetY = 5;

                SizeF sizeTitle = g.MeasureString(data.Title, _fTitle);

                if (data.TitleImage != null)
                {
                    g.DrawImage(data.TitleImage, 5, 5);
                    titleOffsetX += 5 + data.TitleImage.Width;
                    titleOffsetY = (int)(data.TitleImage.Height / 2 - sizeTitle.Height / 4);

                    textOffsetY += data.TitleImage.Height + 5;
                }
                else
                {
                    textOffsetY += (int)sizeTitle.Height + 5;
                }

                using (Brush b = new SolidBrush(ThemeManager.ForeColor))
                {
                    g.DrawString(data.Title, _fTitle, b, titleOffsetX, titleOffsetY);

                    int x = 5;
                    int y = textOffsetY;

                    if (data.Values != null)
                    {
                        foreach (KeyValuePair<string, string> val in data.Values)
                        {
                            string key = Translator.Translate(val.Key.Trim());
                            string keyMeasure = string.IsNullOrEmpty(key) ? " " : key;
                            string valMeasure = string.IsNullOrEmpty(val.Value) ? " " : val.Value;

                            SizeF szKey = g.MeasureString(keyMeasure, _fKey);
                            SizeF szVal = g.MeasureString(valMeasure, _fVal);

                            if (!string.IsNullOrEmpty(val.Key))
                            {
                                g.DrawString(key, _fKey, b, x, y);
                            }

                            if (!string.IsNullOrEmpty(val.Value))
                            {
                                g.DrawString(val.Value, _fVal, b, x + szKey.Width, y);
                            }

                            y += (int)Math.Max(szKey.Height, szVal.Height) + 2;
                        }
                    }
                }
            }
        }

        private Size CalculateSize(OPMToolTipData data)
        {
            int x = 0, y = 0;

            if (data != null)
            {
                int titleOffsetX = 5;
                int titleOffsetY = 5;

                using (Graphics g = CreateGraphics())
                {
                    SizeF sizeTitle = g.MeasureString(data.Title, _fTitle);

                    if (data.TitleImage != null)
                    {
                        x += 5 + data.TitleImage.Width;
                        y += 5 + data.TitleImage.Height;

                        titleOffsetX += 5 + data.TitleImage.Width;
                        titleOffsetY = (int)(data.TitleImage.Height / 2 - sizeTitle.Height / 2);

                        y += 5;
                    }
                    else
                    {
                        y += (int)sizeTitle.Height + 10;
                    }

                    if (data.Values != null)
                    {
                        foreach (KeyValuePair<string, string> val in data.Values)
                        {
                            string key = Translator.Translate(val.Key.Trim());
                            string keyMeasure = string.IsNullOrEmpty(key) ? " " : key;
                            string valMeasure = string.IsNullOrEmpty(val.Value) ? " " : val.Value;

                            SizeF szKey = g.MeasureString(keyMeasure, _fKey);
                            SizeF szVal = g.MeasureString(valMeasure, _fVal);

                            y += (int)szKey.Height + 2;
                            x = (int)Math.Max(x, szKey.Width + szVal.Width + 10);
                        }
                    }

                    x = (int)Math.Max(x, titleOffsetX + sizeTitle.Width);
                }

                x += 10;
                y += 10;
            }

            return new Size(x, y);
        }
    }
}
