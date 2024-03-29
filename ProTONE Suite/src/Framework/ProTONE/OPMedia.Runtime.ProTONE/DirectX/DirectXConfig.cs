﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OPMedia.Runtime.ProTONE.DirectX
{
    public static class DirectXConfig
    {
        public const string DxModuleName = "dxdiagn.dll";
        public const string DxRegPath = @"SOFTWARE\Microsoft\DirectX";
        public static readonly Version Dx1Version = new Version(4, 2, 95); // DirectX 1.0 - there is no DirectX older than this :)
        public static readonly Version Dx9cVersion = new Version("4.9.0.904");

        static Dictionary<Version, string> _dxVersionMap = new Dictionary<Version, string>();

        static DirectXConfig()
        {
            _dxVersionMap.Add(Dx1Version, "1.0");

            _dxVersionMap.Add(new Version("4.03.00.1096"), "2.0");
            _dxVersionMap.Add(new Version("4.04.00.0068"), "3.0");
            _dxVersionMap.Add(new Version("4.04.00.0069"), "3.0a");
            _dxVersionMap.Add(new Version("4.04.00.0070"), "3.0b");
            _dxVersionMap.Add(new Version("4.05.00.0155"), "5.0");
            _dxVersionMap.Add(new Version("4.05.01.1600"), "5.2");
            _dxVersionMap.Add(new Version("4.05.01.1998"), "5.2");
            _dxVersionMap.Add(new Version("4.06.00.0318"), "6.0");
            _dxVersionMap.Add(new Version("4.06.02.0436"), "6.1");
            _dxVersionMap.Add(new Version("4.06.03.0518"), "6.1a");
            _dxVersionMap.Add(new Version("4.07.00.0700"), "7.0");
            _dxVersionMap.Add(new Version("4.07.00.0716"), "7.0a");
            _dxVersionMap.Add(new Version("4.07.01.3000"), "7.1");
            _dxVersionMap.Add(new Version("4.08.00.0400"), "8.0");
            _dxVersionMap.Add(new Version("4.08.01.0810"), "8.1");
            _dxVersionMap.Add(new Version("4.08.01.0881"), "8.1");
            _dxVersionMap.Add(new Version("4.08.01.0901"), "8.1a");
            _dxVersionMap.Add(new Version("4.08.02.0134"), "8.2");
            _dxVersionMap.Add(new Version("4.09.00.0900"), "9.0");
            _dxVersionMap.Add(new Version("4.09.00.0901"), "9.0a");
            _dxVersionMap.Add(new Version("4.09.00.0902"), "9.0b");
            _dxVersionMap.Add(new Version("4.09.00.0903"), "9.0c");

            _dxVersionMap.Add(Dx9cVersion, "DirectX 9.0c");

            _dxVersionMap.Add(new Version("6.00.6000.16386"), "10");
            _dxVersionMap.Add(new Version("6.00.6001.18000"), "10.1");
            _dxVersionMap.Add(new Version("6.00.6002.18005"), "10.1");
            _dxVersionMap.Add(new Version("6.01.7600.16385"), "11");
            _dxVersionMap.Add(new Version("6.00.6002.18107"), "11");
            _dxVersionMap.Add(new Version("6.01.7601.17514"), "11");
            _dxVersionMap.Add(new Version("6.02.9200.16384"), "11.1");
            _dxVersionMap.Add(new Version("6.03.9600.16384"), "11.2");

            _dxVersionMap.Add(new Version("10.00.10240.16384"), "12.0");
            _dxVersionMap.Add(new Version("10.00.16299.15"), "12.0.1");

            _dxVersionMap.Add(new Version(int.MaxValue, int.MaxValue), "????");
        }

        public static Version GetDirectXVersion(out string friendlyDirectXName)
        {
            friendlyDirectXName = "DirectX 1.0";
            Version dxVersion = Dx1Version;

            // Try getting the version of DXDIAGN.DLL (i.e. detect DirectX newer than 9.0c)
            try
            {
                string dxModulePath = Path.Combine(Environment.SystemDirectory, DxModuleName);
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(dxModulePath);

                dxVersion = new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);
            }
            catch
            {
                dxVersion = Dx1Version;
            }

            if (dxVersion.Major < 6 /* Starting with DirectX 10, major version of dxdiagn.dll is 6 */ )
            {
                // Read from Registry
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(DxRegPath))
                    {
                        if (key != null)
                        {
                            string dxVersionStr = key.GetValue("Version") as string;
                            key.Close();

                            dxVersion = new Version(dxVersionStr);
                        }
                    }
                }
                catch
                {
                    dxVersion = Dx1Version;
                }
            }

            friendlyDirectXName = GetDirectXSymbolicVersion(dxVersion);

            return dxVersion;
        }

        public static string GetDirectXSymbolicVersion(Version dxVersion)
        {
            if (_dxVersionMap.ContainsKey(dxVersion))
                return _dxVersionMap[dxVersion];

            List<Version> dxVersions = _dxVersionMap.Keys.ToList();

            Version v1 = dxVersions[0];
            Version v2 = dxVersions[1];

            for (int i = 0; i < dxVersions.Count - 1; i++)
            {
                v1 = dxVersions[i];
                v2 = dxVersions[i + 1];

                if (v1 <= dxVersion && dxVersion <= v2)
                    break;
            }

            return string.Format("{0}+", _dxVersionMap[v1]);
        }
    }
}
