using OPMedia.Core.Logging;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace OPMedia.Runtime.ProTONE.Rendering.Base
{
    public class RenderingException : Exception
    {
        public string RenderedFile { get; set; }

        public static RenderingException FromException(Exception ex)
        {
            if (ex is RenderingException)
            {
                return ex as RenderingException;
            }
            else if (ex is COMException)
            {
                return new RenderingException(
                    ErrorDispatcher.GetErrorMessageForException(ex, false),
                    ex);
            }
            else if (ex is WorkerException)
            {
                return new RenderingException(
                    ex.ToString(), ex);
            }

            return new RenderingException(ex.Message, ex);
        }

        public override string ToString()
        {
            return string.Format("Cannot play: {0}\r\nReason: {1}",
                this.RenderedFile, this.Message);
        }

        public RenderingException()
            : base()
        {
        }

        public RenderingException(String message)
            : base(message)
        {
        }

        public RenderingException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RenderingException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }

    }

    public class RenderingExceptionEventArgs : HandledEventArgs
    {
        public RenderingException RenderingException { get; protected set; }

        public RenderingExceptionEventArgs(RenderingException ex)
        {
            RenderingException = ex;
        }
    }
}
