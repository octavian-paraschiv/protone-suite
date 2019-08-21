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
using OPMedia.Core.GlobalEvents;

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

            base.ScrollBars = RichTextBoxScrollBars.Both;
            this.ReadOnly = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;

            this.RegisterAsEventSink();
            OnThemeUpdated();
        }

        [EventSink(EventNames.ThemeUpdated)]
        public virtual void OnThemeUpdated()
        {
            this.ForeColor = ThemeManager.ForeColor;
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
            this.RegisterAsEventSink();
            OnThemeUpdated();
        }


        [EventSink(EventNames.ThemeUpdated)]
        public override void OnThemeUpdated()
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
            if (string.IsNullOrEmpty(_desc) && (_info == null || _info.Count < 1))
                return;

            string text = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033";

            var font = ThemeManager.SmallFont;

            string fontFamily = font.FontFamily.Name;
            int fontSize = (int)(2 * font.Size);

            string format = @"{\fonttbl{\f0\fswiss\fprq2\fcharset238 #FF#;}}";
            text += format.Replace("#FF#", fontFamily);

            Color cf = ThemeManager.ForeColor;

            text += $"{{\\colortbl ;\\red{cf.R}\\green{cf.G}\\blue{cf.B};}}";

            text += @"\viewkind4\uc1\pard\lang1048\f0\fs" + fontSize;

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
            
            this.ForeColor = cf;
        }

        private string GenerateRtfLabel(string p)
        {
            return StringUtils.ConvertDiacriticalsToRtfTags(string.Format(@"\cf1\b {0} \b0\cf0", p));
        }

        private string GenerateRtfValue(string p)
        {
            return StringUtils.ConvertDiacriticalsToRtfTags(string.Format(@"\cf1 {0} \cf0", p));
        }
    }
}
