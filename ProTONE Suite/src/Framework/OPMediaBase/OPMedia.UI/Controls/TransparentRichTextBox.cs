using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using OPMedia.Core;
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime;
using System.Drawing;
using OPMedia.UI.Themes;

using OPMedia.Core.Utilities;
using OPMedia.Core.Configuration;

namespace OPMedia.UI.Controls
{
    public class TransparentRichTextBox : RichTextBox
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TransparentRichTextBox()
            : base()
        {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            base.ScrollBars = RichTextBoxScrollBars.None;
            this.ReadOnly = true;
            //this.StateCommon.Border.DrawBorders = PaletteDrawBorders.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        /// <summary>
        /// Override the default OnPaintBackground behavior
        /// (there is no need to paint the background but only
        /// the foreground).
        /// </summary>
        /// <param name="e">The paint event data.</param>
        override protected void OnPaintBackground(PaintEventArgs e)
        {
        }
    }

    public class InfoTextBox : TransparentRichTextBox
    {
        public FontSizes _fs = FontSizes.Normal;
        private string _desc = null;
        private Dictionary<string, string> _info = null;


        public FontSizes FontSize
        {
            get
            {
                return _fs;
            }

            set
            {
                if (_fs != value)
                {
                    _fs = value;
                    ThemeManager.SetFont(this, value);
                    Rebuild();
                }
            }
        }

        public InfoTextBox()
            : base()
        {
            Rebuild();
        }

        public void SetInfo(string desc, Dictionary<string, string> info)
        {
            _desc = desc;
            _info = info;
            Rebuild();
        }

        public void Rebuild()
        {
            string text = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033";

            string fontFamily = this.Font.FontFamily.Name;
            int fontSize = (int)(2 * this.Font.SizeInPoints);

            //switch (AppConfig.LanguageID)
            //{
            //    case "ro":
                    {
                        string format = @"{\fonttbl{\f0\fswiss\fprq2\fcharset238 #FF#;}}";
                        text += format.Replace("#FF#", fontFamily);
                        text += @"\viewkind4\uc1\pard\lang1048\f0\fs" + fontSize;
                    }
            //        break;
            //}

            if (!string.IsNullOrEmpty(_desc))
            {
                text += GenerateRtfLabel(_desc.Replace("\r\n", @" \par ").Replace("\n", @" \par "));
                text += @" \par ";
                text += @" \par ";
            }

            if (_info != null && _info.Count > 0)
            {
                int i = _info.Count;
                foreach (KeyValuePair<string, string> kvp in _info)
                {
                    i--;

                    if (!string.IsNullOrEmpty(kvp.Key) ||
                            !string.IsNullOrEmpty(kvp.Value))
                    {
                        text += GenerateRtfLabel(Translator.Translate(kvp.Key).Replace("\r\n", @" \par ").Replace("\n", @" \par "));
                        text += " ";
                        text += GenerateRtfValue(kvp.Value.Replace("\r\n", @"\par").Replace("\n", @" \par "));
                    }

                    if (i > 0)
                    {
                        text += @" \par ";
                    }

                }
            }

            text += "}";
                
            this.Rtf = text;
            
            this.ForeColor = ThemeManager.ForeColor;
        }

        private string GenerateRtfLabel(string p)
        {
            return StringUtils.ConvertDiacriticalsToRtfTags(string.Format(@"\b {0} \b0", p));
        }

        private string GenerateRtfValue(string p)
        {
            return StringUtils.ConvertDiacriticalsToRtfTags(string.Format(@"{0}", p));
        }
    }
}
