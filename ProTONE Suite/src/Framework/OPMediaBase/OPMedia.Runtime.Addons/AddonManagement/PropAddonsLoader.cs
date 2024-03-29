#region Copyright � 2006 OPMedia Research
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written permission of the copyright owner.

// File: 	PropAddonLoader.cs
#endregion

#region Using directives
using OPMedia.Core;
using OPMedia.Core.Logging;
using OPMedia.Runtime.Addons.AddonsBase;
using OPMedia.Runtime.Addons.AddonsBase.Prop;
using System;
using System.Collections.Generic;
using System.IO;
#endregion

namespace OPMedia.Runtime.Addons.AddonManagement
{
    public class PropAddonsLoader : AddonsLoader
    {
        /// <summary>
        /// Loads the property add-ons
        /// </summary>
        protected override void Load()
        {
            // Initialize each of property addons. Careful not to break the loop
            // on eventual exceptions. It's important to try loading as much as
            // possible registered addons.
            if (AddonsConfig.PropertyAddons != null)
            {
                foreach (string addonName in AddonsConfig.PropertyAddons)
                {
                    try
                    {
                        Logger.LogTrace("Loading property addon: {0} ...", addonName);

                        PropertyAddon navAddon = new PropertyAddon(addonName);
                        Addons.Add(addonName, navAddon);
                    }
                    catch (Exception ex)
                    {
                        ErrorDispatcher.DispatchError(string.Format("Could not load addon: {0}.\nError: {1}",
                            addonName, ex.Message), false);
                    }
                }
            }
        }

        /// <summary>
        /// Selects the proper property addon to handle the given items.
        /// </summary>
        internal override Addon SelectAddon(List<string> items)
        {
            Addon retVal = null;

            try
            {
                bool mustHandleFolders = false;
                List<string> extensions = new List<string>();

                foreach (string item in items)
                {
                    if (string.IsNullOrEmpty(item))
                        return null;

                    string ext = PathUtils.GetExtension(item);
                    if (!extensions.Contains(ext))
                    {
                        extensions.Add(ext);
                    }

                    mustHandleFolders = Directory.Exists(item);
                }

                retVal = InternalSelectAddon(extensions, mustHandleFolders, items.Count);
            }
            catch
            {
                retVal = null;
            }

            return retVal;
        }

        /// <summary>
        /// Effectively handles the addon selection.
        /// </summary>
        private Addon InternalSelectAddon(List<string> extensions, bool mustHandleFolders,
            int itemCount)
        {
            Addon genericAddon = null;
            Addon specializedAddon = null;

            foreach (KeyValuePair<string, Addon> kv in Addons)
            {
                PropertyAddon addon = kv.Value as PropertyAddon;

                if (addon != null)
                {
                    if (mustHandleFolders && !addon.AddonPanel.CanHandleFolders)
                    {
                        // Can't handle folders, and it's required, so move to next one.
                        continue;
                    }

                    if (addon.AddonPanel.MaximumHandledItems > 1 &&
                        addon.AddonPanel.MaximumHandledItems < itemCount)
                    {
                        // Can't handle the number of items requested, move to next one.
                        continue;
                    }

                    bool canHandleAllRequiredExtensions = true;
                    if (addon.AddonPanel.HandledFileTypes != null)
                    {
                        foreach (string extension in extensions)
                        {
                            if (!addon.AddonPanel.HandledFileTypes.Contains(extension))
                            {
                                // Cannot handle the requested extension.
                                canHandleAllRequiredExtensions = false;
                                break;
                            }
                        }
                    }

                    if (!canHandleAllRequiredExtensions)
                    {
                        // It cannot handle all required extensions.
                        continue;
                    }

                    // Seems it fulfills all conditions.

                    // (addons that do not apply to all file types).
                    if (addon.AddonPanel.HandledFileTypes != null)
                    {
                        // It's specialized (it does not apply to all extensions).
                        specializedAddon = addon;
                        break;
                    }
                    else
                    {
                        // It's not specialized, but maybe there can be found a specialized one.
                        genericAddon = addon;
                        continue;
                    }
                }
            }

            // As much as possbile, try to select specialized addons because they will
            // give more details than the generic ones.
            if (specializedAddon != null)
            {
                return specializedAddon;
            }
            else
            {
                return genericAddon;
            }
        }

        protected override void UnloadInternal()
        {
        }
    }
}

#region ChangeLog
#region Date: 24.06.2006			Author: octavian
// File created.
#endregion
#endregion