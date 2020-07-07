using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using OPMedia.UI.Themes;
using System.ComponentModel;
using OPMedia.Core;
using System.Drawing.Drawing2D;
using OPMedia.UI.Generic;
using System.Windows.Forms.VisualStyles;
using System.Reflection;
using OPMedia.UI.Controls;
using OPMedia.Core.TranslationSupport;
using System.Drawing.Text;

namespace OPMedia.UI.Controls
{
    public class OPMComboBox : MetroFramework.Controls.MetroComboBox
    {
        public OPMComboBox()
            : base()
        {
        }

        public void AddUniqueItem(object item)
        {
            if (!this.Items.Contains(item))
            {
                this.Items.Add(item);
            }
        }
    }

    public class YesNoComboBox : OPMComboBox
    {
        public new bool SelectedValue
        {
            get
            {
                return (SelectedIndex != 0);
            }

            set
            {
                SelectedIndex = value ? 1 : 0;
            }
        }

        public YesNoComboBox()
            : base()
        {
            // Keep this order !!
            Items.Add(Translator.Translate("TXT_NO"));
            Items.Add(Translator.Translate("TXT_YES"));
        }
    }

    public class ComboBoxItem
    {
        public Image Image { get; protected set; }

        public ComboBoxItem(Image img)
        {
            this.Image = img;
        }
    }


    public class FontComboBox : OPMComboBox
    {
        public FontComboBox()
            : base()
        {
            PopulateInstalledFonts();
        }
        
        private void PopulateInstalledFonts()
        {
            FontFamily[] fontFamilies = new InstalledFontCollection().Families;
            foreach (FontFamily ff in fontFamilies)
            {
                this.AddUniqueItem(ff);
            }
        }

        
    }


    public class ColorComboBox : OPMComboBox
    {
        public ColorComboBox() : base()
        {
            PopulateKnownColors();
        }
        
        private void PopulateKnownColors()
        {
            KnownColor[] knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            foreach (KnownColor kc in knownColors)
            {
                Color c = Color.FromName(kc.ToString());
                this.AddUniqueItem(c);
            }
        }
    }
}
