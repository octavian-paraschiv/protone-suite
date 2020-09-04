using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public static class WorkerServerStream
    {
        static NamedPipeServerStream _srv;
        static ManualResetEvent _evt = new ManualResetEvent(false);

        public static StreamReader Input()
        {
            if (_evt.WaitOne())
                return new StreamReader(_srv);

            return null;
        }

        public static StreamWriter Output()
        {
            if (_evt.WaitOne())
                return new StreamWriter(_srv);

            return null;
        }


        static WorkerServerStream()
        {
            Task.Run(() =>
            {
                int pid = Process.GetCurrentProcess().Id;

                PipeSecurity pipeSecurity = CreateSystemIOPipeSecurity();

                _srv = new NamedPipeServerStream($"OPMEDIA_{pid}", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.None,
                    0x4000, 0x400, pipeSecurity, HandleInheritability.Inheritable);

                Debug.WriteLine($"Server named pipe: OPMEDIA_{pid}");

                _srv.WaitForConnection();

                _evt.Set();
            }
            );
        }

        // Creates a PipeSecurity that allows users read/write access
        static PipeSecurity CreateSystemIOPipeSecurity()
        {
            PipeSecurity pipeSecurity = new PipeSecurity();

            var id = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            // Allow Everyone read and write access to the pipe. 
            pipeSecurity.SetAccessRule(new PipeAccessRule(id, PipeAccessRights.ReadWrite, AccessControlType.Allow));

            return pipeSecurity;
        }
    }

    public class WorkerClientStream : IDisposable
    {
        NamedPipeClientStream _cl;
        StreamReader _sr;
        StreamWriter _sw;

        ManualResetEvent _clearToSend = new ManualResetEvent(true);
        object _uniqueSender = new object();

        public WorkerCommand ReadCommand()
        {
            try
            {
                string s = _sr.ReadLine();
                return WorkerCommand.FromString(s);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
            finally
            {
                _clearToSend.Set();
            }
        }

        public bool WriteCommand(WorkerCommand cmd)
        {
            try
            {
                lock (_uniqueSender)
                {
                    _clearToSend.WaitOne();

                    string s = cmd.ToString();
                    _sw.WriteLine(s);
                    _sw.Flush();

                    _clearToSend.Reset();
                }

                return true;
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }

            return false;
        }

        public WorkerClientStream(int pid)
        {
            _cl = new NamedPipeClientStream(".", $"OPMEDIA_{pid}",
                PipeDirection.InOut, PipeOptions.None,
                TokenImpersonationLevel.Anonymous);

            _cl.Connect(5000);

            _sr = new StreamReader(_cl);
            _sw = new StreamWriter(_cl);
        }

        public void Dispose()
        {
            try { _sw?.Close(); } catch { }
            try { _sr?.Close(); } catch { }
            try { _cl.Close(); } catch { }
        }
    }
}
