using OPMedia.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public static class WorkerCommandHelper
    {
        static ManualResetEvent _clearToSend = new ManualResetEvent(true);
        static object _uniqueSender = new object();

        public static WorkerCommand ReadCommand(StreamReader sr)
        {
            try
            {
                string s = sr.ReadLine();
                return WorkerCommand.FromString(s);
            }
            finally
            {
                _clearToSend.Set();
            }
        }

        public static bool WriteEvent(StreamWriter sw, WorkerCommand cmd)
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

        public static bool WriteCommand(StreamWriter sw, WorkerCommand cmd)
        {
            try
            {
                lock (_uniqueSender)
                {
                    _clearToSend.WaitOne();

                    string s = cmd.ToString();
                    sw.WriteLine(s);
                    sw.Flush();

                    _clearToSend.Reset();
                }

                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}
