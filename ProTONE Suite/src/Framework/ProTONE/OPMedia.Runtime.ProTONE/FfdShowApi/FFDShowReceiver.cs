using OPMedia.Core;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace OPMedia.Runtime.ProTONE.FfdShowApi
{
    /// <summary>
    /// This class is instantiated and used by FFDShowAPI to receive the answers from FFDShow
    /// It must derivate from Form because FFDShow communication API is based on
    /// windows messages. This class is not used directly by user.
    /// </summary>
    public class FFDShowReceiver : Form
    {
        /// <summary>
        /// Thread instance of FFDShowAPI waiting for the response
        /// </summary>
        private Thread parentThread;

        /// <summary>
        /// Gets or sets the string received from FFDShow
        /// </summary>
        public String ReceivedString { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the value to retrieve
        /// </summary>
        public Int32 ReceivedType { get; set; }


        #region Constructors
        /// <summary>
        /// FFDShowReceiver constructor
        /// </summary>
        /// <param name="parentThread">FFDShowAPI thread to be interrupted once the response is received</param>
        public FFDShowReceiver(Thread parentThread)
        {
            this.parentThread = parentThread;
        }
        #endregion Constructors

        /// <summary>
        /// Main method that receives window messages
        /// We handle only for WM_COPYDATA messages
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Messages.WM_COPYDATA)
            {
                try
                {
                    COPYDATASTRUCT cd = new COPYDATASTRUCT();
                    cd = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));

                    ReceivedString = Marshal.PtrToStringUni(cd.lpData);
                    ReceivedType = (int)cd.dwData.ToUInt32();

                    if (parentThread != null && parentThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                        parentThread.Interrupt();
                }
                catch (Exception)
                {
                }
            }

            base.WndProc(ref m);
        }
    }
}
