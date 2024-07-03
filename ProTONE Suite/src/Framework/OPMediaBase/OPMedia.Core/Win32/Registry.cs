using Microsoft.Win32;

namespace OPMedia.Core.Win32
{
    public static class Regedit
    {
        public static string InstallLanguageID
        {
            get
            {
                string retVal = null;

                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\OPMedia Research\ProTONE Suite");
                    if (key != null)
                    {
                        retVal = key.GetValue("InstallLanguageID", "en") as string;
                    }
                }
                catch
                {
                    retVal = null;
                }

                return retVal;
            }
        }

        public static bool IsDevelopmentMachine
        {
            get
            {
                bool retVal = false;

                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\OPMedia Research\ProTONE Suite");
                    if (key != null)
                    {
                        retVal = ((int)key.GetValue("IsDevelopmentMachine", 0) == 1);
                    }
                }
                catch
                {
                    retVal = false;
                }

                return retVal;
            }
        }
    }
}
