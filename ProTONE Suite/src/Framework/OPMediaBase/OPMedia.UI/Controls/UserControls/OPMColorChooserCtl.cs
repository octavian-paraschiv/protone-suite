﻿using System;
using System.Drawing;

namespace OPMedia.UI.Controls
{
    public partial class OPMColorChooserCtl : OPMBaseControl
    {
        public event EventHandler ColorChanged = null;

        public string Description
        {
            get { return lblColorName.Text; }
            set
            {
                lblColorName.Text = value;
                lblColorName.Visible = !string.IsNullOrEmpty(lblColorName.Text);
            }
        }

        public Color Color
        {
            get { return lblResultingColor.OverrideBackColor; }
            set
            {
                lblResultingColor.OverrideBackColor = value;
                ApplyColor(false, false);
            }
        }

        private void ApplyColor(bool raiseEvent, bool skipApplyingText)
        {
            try
            {
                UnsubscribeEvents();

                Color c = lblResultingColor.OverrideBackColor;
                cgR.Value = c.R;
                cgG.Value = c.G;
                cgB.Value = c.B;
                nudR.Value = c.R;
                nudG.Value = c.G;
                nudB.Value = c.B;

                if (skipApplyingText == false)
                {
                    string cStr = string.Format("{0:000} {1:000} {2:000}", c.R, c.G, c.B);
                    txtColor.Text = cStr;
                }

                int i = 0;
                for (; i < cmbKnownColors.Items.Count; i++)
                {
                    Color colorItem = (Color)cmbKnownColors.Items[i];
                    if (c.ToArgb() == colorItem.ToArgb())
                        break;
                }

                if (i < cmbKnownColors.Items.Count)
                {
                    cmbKnownColors.SelectedIndex = i;
                }
                else
                {
                    cmbKnownColors.SelectedIndex = -1;
                }

                if (ColorChanged != null && raiseEvent)
                {
                    ColorChanged(this, EventArgs.Empty);
                }
            }
            finally
            {
                SubscribeEvents();
            }
        }

        public OPMColorChooserCtl()
        {
            InitializeComponent();
            SubscribeEvents();
        }

        public void SubscribeEvents()
        {
            this.nudR.ValueChanged += new System.EventHandler(this.ColorChangedByNumericFields);
            this.nudG.ValueChanged += new System.EventHandler(this.ColorChangedByNumericFields);
            this.nudB.ValueChanged += new System.EventHandler(this.ColorChangedByNumericFields);
            this.cgR.PositionChanged += new OPMedia.UI.Controls.ValueChangedEventHandler(this.ColorChangedByGauges);
            this.cgG.PositionChanged += new OPMedia.UI.Controls.ValueChangedEventHandler(this.ColorChangedByGauges);
            this.cgB.PositionChanged += new OPMedia.UI.Controls.ValueChangedEventHandler(this.ColorChangedByGauges);
            this.txtColor.TextChanged += new System.EventHandler(this.ColorChangedByText);
            this.cmbKnownColors.SelectedIndexChanged += new System.EventHandler(this.ColorChangedByComboBox);

        }
        public void UnsubscribeEvents()
        {
            this.nudR.ValueChanged -= new System.EventHandler(this.ColorChangedByNumericFields);
            this.nudG.ValueChanged -= new System.EventHandler(this.ColorChangedByNumericFields);
            this.nudB.ValueChanged -= new System.EventHandler(this.ColorChangedByNumericFields);
            this.cgR.PositionChanged -= new OPMedia.UI.Controls.ValueChangedEventHandler(this.ColorChangedByGauges);
            this.cgG.PositionChanged -= new OPMedia.UI.Controls.ValueChangedEventHandler(this.ColorChangedByGauges);
            this.cgB.PositionChanged -= new OPMedia.UI.Controls.ValueChangedEventHandler(this.ColorChangedByGauges);
            this.txtColor.TextChanged -= new System.EventHandler(this.ColorChangedByText);
            this.cmbKnownColors.SelectedIndexChanged -= new System.EventHandler(this.ColorChangedByComboBox);
        }

        private void ColorChangedByNumericFields(object sender, EventArgs e)
        {
            Color c = Color.FromArgb((int)nudR.Value, (int)nudG.Value, (int)nudB.Value);
            lblResultingColor.OverrideBackColor = c;
            ApplyColor(true, false);
        }

        private void ColorChangedByGauges(double val)
        {
            Color c = Color.FromArgb((int)cgR.Value, (int)cgG.Value, (int)cgB.Value);
            lblResultingColor.OverrideBackColor = c;
            ApplyColor(true, false);
        }

        private void ColorChangedByText(object sender, EventArgs e)
        {
            byte r = 255, g = 255, b = 255;

            string cStr = txtColor.Text;
            string[] fields = cStr.Split(' ');
            if (fields != null && fields.Length == 3)
            {
                if (byte.TryParse(fields[0], out r) == false)
                    r = 255;
                if (byte.TryParse(fields[1], out g) == false)
                    g = 255;
                if (byte.TryParse(fields[2], out b) == false)
                    b = 255;
            }

            Color c = Color.FromArgb(r, g, b);
            lblResultingColor.OverrideBackColor = c;
            ApplyColor(true, true);
        }

        private void ColorChangedByComboBox(object sender, EventArgs e)
        {
            Color c = (Color)cmbKnownColors.Items[cmbKnownColors.SelectedIndex];
            lblResultingColor.OverrideBackColor = c;
            ApplyColor(true, false);
        }
    }
}
