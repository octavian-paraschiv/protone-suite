using System;
using System.Collections.Generic;
using System.Text;
using OPMedia.UI.Configuration;
using OPMedia.Core;
using System.Windows.Forms;
using OPMedia.UI;
using OPMedia.Runtime.Addons.AddonsBase;
using OPMedia.Core.TranslationSupport;

namespace OPMedia.Runtime.Addons.Configuration
{
    public class AddonAppSettingsForm : SettingsForm
    {
        private UI.Controls.OPMTabPage opmTabPage1;

        public new static DialogResult Show()
        {
            AddonAppSettingsForm _instance = new AddonAppSettingsForm();
            return _instance.ShowDialog();
        }

        public static DialogResult Show(string titleToOpen, string subTitleToOpen = "")
        {
            AddonAppSettingsForm _instance = new AddonAppSettingsForm(titleToOpen, subTitleToOpen);
            return _instance.ShowDialog();
        }

        protected AddonAppSettingsForm(string titleToOpen, string subTitleToOpen) 
            : base(titleToOpen, subTitleToOpen)
        {
        }

        public AddonAppSettingsForm() : base()
        {
        }

        protected virtual bool DissalowAddonConfigPages()
        {
            return false;
        }

        public override void AddAditionalPanels()
        {
            Translator.RegisterTranslationAssembly(GetType().Assembly);

            bool dissalowAddonsConfig = DissalowAddonConfigPages();
            AddPanel(typeof(AddonCfgPanel), !dissalowAddonsConfig);
            AddPanel(typeof(AddonSettingsPanel), !AddonsConfig.IsInitialConfig);
        }

        public override void RemoveUnneededPanels()
        {
            if (AddonsConfig.IsInitialConfig)
            {
                Type typeConfigurator = typeof(AddonCfgPanel);
                KeepPanels(new List<Type>(new Type[] { typeConfigurator }));

                btnCancel.Visible = false;
                btnOk.Location = btnCancel.Location; 
            }
        }

        private void InitializeComponent()
        {
            this.opmTabPage1 = new OPMedia.UI.Controls.OPMTabPage();
            this.SuspendLayout();
            // 
            // opmTabPage1
            // 
            this.opmTabPage1.BackColor = System.Drawing.Color.White;
            this.opmTabPage1.ImageIndex = 0;
            this.opmTabPage1.Location = new System.Drawing.Point(4, 59);
            this.opmTabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.opmTabPage1.Name = "opmTabPage1";
            this.opmTabPage1.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.opmTabPage1.Size = new System.Drawing.Size(588, 373);
            this.opmTabPage1.TabIndex = 0;
            this.opmTabPage1.Text = "Support";
            this.opmTabPage1.Visible = false;
            // 
            // AddonAppSettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(599, 470);
            this.Name = "AddonAppSettingsForm";
            this.Text = "";
            this.ResumeLayout(false);

        }
    }
}
