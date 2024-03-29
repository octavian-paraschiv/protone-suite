#region Copyright � 2006 OPMedia Research
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	PropertyAddon.cs
#endregion

#region Using directives
using System;
#endregion

namespace OPMedia.Runtime.Addons.AddonsBase.Prop
{
    /// <summary>
    /// Class that provides all functionality needed to load a registered property add-on.
    /// </summary>
    public class PropertyAddon : Addon
    {
        /// <summary>
        /// Each property add-on exposes a Panel. This is what the user sees in the UI.
        /// All these Panels should be derived from PropBaseCtl.
        /// </summary>
        private PropBaseCtl addonPanel = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PropertyAddon(string name) : base(name)
        {
        }

        /// <summary>
        /// Retrieves the addon panel.
        /// </summary>
        public PropBaseCtl AddonPanel
        {
            get
            {
                return addonPanel;
            }
        }

        /// <summary>
        /// Called from the base class constructor.
        /// Various types of addons can be initialized in various ways
        /// so Initialize must be overidden in all the Addon derived classes.
        /// </summary>
        protected override void LoadAddonType()
        {
            // Determine which of the subtypes must be loaded.
            // By convention, all navigation addons must expose a object
            // that is derived from NavBaseCtl, and this member is the addon panel.
            // This object must be initialized.
            foreach (System.Type type in this.AddonAssembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(PropBaseCtl)) &&
                    type.FullName == this.AddonTypeName)
                {
                    // Call the panel contructor, via .NET Reflection since we don't
                    // now exactly its type...
                    addonPanel = Activator.CreateInstance(type) as PropBaseCtl;

                    // Job done. Addon initialized, so exit.
                    return;
                }
            }

            // If we're here, the specified addon is not a valid navigation addon.
            string exception =
                string.Format("An error occured while loading the addon: \"{0}\".\n\n" +
                    "The associated assembly: \"{1}\" does not represent a valid property addon,\n" +
                    "because it does not contain any subclasses of \"{2}\".\n\n" +
                    "The addon might not be properly registered as an OPMedia Property Addon.",
                    this.Name, this.AssemblyFileName, typeof(PropBaseCtl).ToString());

            throw new ApplicationException(exception);
        }
    }
}

#region ChangeLog
#region Date: 24.06.2006			Author: octavian
// File created.
#endregion
#endregion