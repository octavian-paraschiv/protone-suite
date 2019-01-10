using OPMedia.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public class WorkerCommandHelper
    {
        public static WorkerCommand ReadCommand(StreamReader sr)
        {
            string s = sr.ReadLine();
            return WorkerCommand.FromString(s);
        }

        public static bool WriteCommand(StreamWriter sw, WorkerCommand cmd)
        {
            try
            {
                string s = cmd.ToString();
                sw.WriteLine(s);
                sw.Flush();

                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}
