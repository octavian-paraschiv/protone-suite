using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace OPMedia.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args?.Length > 0)
            {
                if (args[0].ToUpperInvariant() == "-KILLALL")
                {
                    var res = KillAllApps(0);

                    if (args?.Length > 1 && args[1].ToUpperInvariant() == "-REDIRECTTOFILE")
                        File.WriteAllText(".\\OPMedia.Utility.res", res);
                    else
                        Console.WriteLine(res);
                }
            }
        }

        private static string KillAllApps(int level)
        {
            List<Process> opmediaProcs = GetOpmediaProcesses();

            foreach (var proc in opmediaProcs)
            {
                if (HasExited(proc))
                    continue;

                try { proc.Kill(); }
                catch { }
            }

            Thread.Sleep(700);

            opmediaProcs = GetOpmediaProcesses();

            if (opmediaProcs.Count > 0)
            {
                if (level >= 5)
                {
                    string s = string.Empty;
                    foreach (var proc in opmediaProcs)
                    {
                        if (HasExited(proc))
                            continue;

                        s += $"{proc.ProcessName}; ";
                    }

                    return s.Trim("; ".ToCharArray());
                }

                return KillAllApps(level + 1);
            }

            return string.Empty;
        }

        private static List<Process> GetOpmediaProcesses()
        {
            List<Process> opmediaProcs = new List<Process>();
            var procs = Process.GetProcesses();
            if (procs?.Length > 0)
            {
                foreach (var proc in procs)
                {
                    if (proc?.ProcessName?.Length > 0)
                    {
                        var procName = proc.ProcessName.ToUpperInvariant();
                        if (procName.Contains("OPMEDIA") && !procName.Contains("OPMEDIA.UTILITY") && !HasExited(proc))
                            opmediaProcs.Add(proc);
                    }
                }
            }

            return opmediaProcs;
        }

        private static bool HasExited(Process proc)
        {
            try
            {
                return proc?.HasExited ?? false;
            }
            catch
            {
                return false;
            }
        }
    }
}
