using OPMedia.Runtime.Addons.AddonsBase.Prop;
using OPMedia.Runtime.FileInformation;
using OPMedia.UI.Controls;
using System.Collections.Generic;

namespace OPMedia.Addons.Builtin.__DefaultProp
{
    public partial class AddonPanel : PropBaseCtl
    {
        public static bool IsRequired { get { return true; } }

        public override string GetHelpTopic()
        {
            return "DefaultPropertyPanel";
        }

        public AddonPanel()
            : base()
        {
            InitializeComponent();
        }

        public override bool CanHandleFolders
        {
            get
            {
                return true;
            }
        }

        public override List<string> HandledFileTypes
        {
            get
            {
                return null;
            }
        }

        public override int MaximumHandledItems
        {
            get
            {
                return -1;
            }
        }

        public override void ShowProperties(List<string> strItems, object additionalData)
        {
            List<object> lofi = new List<object>();

            foreach (string item in strItems)
            {
                NativeFileInfo ofi = new NativeFileInfo(item, false);
                if (ofi.IsValid)
                {
                    lofi.Add(ofi);
                }
            }

            FileAttributesBrowser.ProcessObjectAttributes(lofi);

            pgProperties.SelectedObjects = lofi.ToArray();
        }
    }
}
