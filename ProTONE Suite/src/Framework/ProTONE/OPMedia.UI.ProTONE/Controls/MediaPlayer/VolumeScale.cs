#region Copyright � 2008 OPMedia Research
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	VolumeScale.cs
#endregion

#region Using directives
using OPMedia.Core.TranslationSupport;
using OPMedia.Runtime.ProTONE.Configuration;
using OPMedia.Runtime.ProTONE.Rendering;
using OPMedia.Runtime.ProTONE.Rendering.Base;
using OPMedia.UI.Controls;
using OPMedia.UI.Themes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
#endregion

namespace OPMedia.UI.ProTONE.Controls.MediaPlayer
{
    public delegate void OnToolTipUpdated(Control ctl, string tooltip);

    public partial class VolumeScale : OPMBaseControl
    {
        #region Controls
        OPMToolTip _tip = new UI.Controls.OPMToolTip();
        #endregion

        #region Members
        public event ValueChangedEventHandler PositionChanged = null;
        #endregion

        #region Properties

        public bool IsOnMenuBar { get; set; }

        public Control ControlArea
        {
            get
            {
                return volumeProgress;
            }
        }

        public int Position
        {
            get
            {
                return (int)volumeProgress.Value;
            }
            set
            {
                volumeProgress.Value = value;
                UpdateValue();
            }

        }

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }

            set
            {
                base.Enabled = value;
                volumeProgress.Enabled = value;
            }
        }

        #endregion

        #region Construction
        public VolumeScale()
        {
            InitializeComponent();
            volumeProgress.Maximum = 10000;
            volumeProgress.Value = 5000;
            volumeProgress.PositionChanged +=
                new ValueChangedEventHandler(volumeProgress_PositionChanged);

            volumeProgress.HoveredPositionChanged +=
                new ValueChangedEventHandler(volumeProgress_HoveredPositionChanged);

            this.HandleCreated += VolumeScale_HandleCreated;
        }

        void VolumeScale_HandleCreated(object sender, EventArgs e)
        {
            lblMin.Visible = lblMax.Visible = lblCurrent.Visible = this.IsOnMenuBar;
        }

        void volumeProgress_HoveredPositionChanged(double val)
        {
            int hoverVol = (int)(val / 100);
            string tip = Translator.Translate("TXT_VOLSCALE", hoverVol);

            _tip.SetSimpleToolTip(volumeProgress, tip);
        }

        void volumeProgress_PositionChanged(double newVal)
        {
            UpdateValue();
            if (PositionChanged != null)
            {
                PositionChanged(newVal);
            }
        }

        public void ApplyMenuBarColors()
        {
            lblCurrent.OverrideBackColor = Color.FromKnownColor(KnownColor.Window);
            lblMin.OverrideBackColor = Color.FromKnownColor(KnownColor.Window);
            lblMax.OverrideBackColor = Color.FromKnownColor(KnownColor.Window);

            lblCurrent.OverrideForeColor = ThemeManager.MenuTextColor;
            lblMin.OverrideForeColor = ThemeManager.MenuTextColor;
            lblMax.OverrideForeColor = ThemeManager.MenuTextColor;
        }
        #endregion

        #region Event Handlers
        #endregion

        #region Implementation
        private void UpdateValue()
        {
            lblCurrent.Text = string.Format("VOL: {0}%", (int)(volumeProgress.Value / 100));
        }
        #endregion
    }

    [DesignerCategory("code")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class ToolStripVolumeScale : ToolStripControlHost
    {
        public event ValueChangedEventHandler PositionChanged = null;

        public VolumeScale VolumeScale
        {
            get
            {
                return this.Control as VolumeScale;
            }
        }

        public ToolStripVolumeScale()
            : base(new VolumeScale())
        {
            VolumeScale.PositionChanged += new ValueChangedEventHandler(VolumeScale_PositionChanged);
            VolumeScale.Visible = true;
            VolumeScale.IsOnMenuBar = true;

            //this.BackColor = Color.FromKnownColor(KnownColor.Window);
            //VolumeScale.OverrideBackColor = Color.FromKnownColor(KnownColor.Window);

            VolumeScale.ApplyMenuBarColors();


            VolumeScale.HandleCreated += new EventHandler(HandleCreated);
            VolumeScale.HandleDestroyed += new EventHandler(HandleDestroyed);
        }

        void HandleDestroyed(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                RenderingEngine.DefaultInstance.MediaRendererClock -= new MediaRendererEventHandler(OnMediaRendererClock);
            }
        }

        void HandleCreated(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                RenderingEngine.DefaultInstance.MediaRendererClock += new MediaRendererEventHandler(OnMediaRendererClock);
            }

        }

        public void OnMediaRendererClock()
        {
            if (!DesignMode)
            {
                VolumeScale.Enabled = (RenderingEngine.DefaultInstance.RenderedMediaType != MediaTypes.Video);
                VolumeScale.Position = ProTONEConfig.LastVolume;
            }
        }

        void VolumeScale_PositionChanged(double newVal)
        {
            if (PositionChanged != null)
            {
                PositionChanged(newVal);
            }
        }
    }
}

#region ChangeLog
#region Date: 27.02.2008			Author: Octavian Paraschiv
// File created.
#endregion
#endregion