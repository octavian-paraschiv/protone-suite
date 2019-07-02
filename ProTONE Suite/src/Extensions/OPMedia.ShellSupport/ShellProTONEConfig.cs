using OPMedia.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.ShellSupport
{
    public static class ShellProTONEConfig
    {
        public static string PlayerInstallationPath
        {
            get
            {
                return Path.Combine(LiteAppConfig.InstallationPath, ShellConstants.PlayerBinary);
            }
        }

        public static string LibraryInstallationPath
        {
            get
            {
                return Path.Combine(LiteAppConfig.InstallationPath, ShellConstants.LibraryBinary);
            }
        }
    }
}
