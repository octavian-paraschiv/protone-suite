using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OPMedia.Core
{
    public class LiteAppConfig
    {
        public static string InstallationPath
        {
            get
            {
                string retVal = string.Empty;
                try
                {
                    if (string.IsNullOrEmpty(retVal))
                    {
                        Assembly asm = Assembly.GetAssembly(typeof(LiteAppConfig));
                        if (asm != null)
                        {
                            retVal = Path.GetDirectoryName(asm.Location);
                        }
                    }
                }
                catch
                {
                    retVal = string.Empty;
                }

                return retVal;
            }
        }
    }
}
