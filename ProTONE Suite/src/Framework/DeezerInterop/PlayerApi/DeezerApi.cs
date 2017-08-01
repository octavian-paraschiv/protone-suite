using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OPMedia.Core.Logging;

// Ported from original header file deezer-api.h

namespace OPMedia.DeezerInterop.PlayerApi
{
    /// <summary>
    /// Deezer REST command type
    /// </summary>
    public enum dz_api_cmd_t
    {
        /// <summary>
        /// API Cmd has not been set yet, not a valid value.
        /// </summary>
        DZ_API_CMD_UNKNOWN,
        /// <summary>
        /// REST GET method
        /// </summary>
        DZ_API_CMD_GET,
        /// <summary>
        /// REST POST method
        /// </summary>
        DZ_API_CMD_POST,
        /// <summary>
        /// REST DELETE method
        /// </summary>
        DZ_API_CMD_DELETE
    }

    /// <summary>
    /// Deezer REST processing status.
    /// </summary>
    public enum dz_api_result_t
    {
        /// <summary>
        /// API result has not been set yet, not a valid value
        /// </summary>
        DZ_API_RESULT_UNKNOWN,
        /// <summary>
        /// The request was completed, result in responsedata
        /// </summary>
        DZ_API_RESULT_COMPLETED,
        /// <summary>
        /// dz_api_request_processing_cancel() was called
        /// </summary>
        DZ_API_RESULT_CANCELED,
        /// <summary>
        /// An IO (socket, ie ip or tcp) exception occured
        /// </summary>
        DZ_API_RESULT_IOEXCEPTION,
        /// <summary>
        /// A SSL (certificate, ...) exception occured
        /// </summary>
        DZ_API_RESULT_SSLEXCEPTION,
        /// <summary>
        /// A HTTP (parsing,...) exception occured
        /// </summary>
        DZ_API_RESULT_HTTPEXCEPTION,
        /// <summary>
        /// A JSON (parsing, ...) exception occured
        /// </summary>
        DZ_API_RESULT_JSONEXCEPTION,
        /// <summary>
        /// The request could not complete before timeout
        /// </summary>
        DZ_API_RESULT_TIMEOUT,
    }

    /// <summary>
    /// Prototype of callback called by an request #dz_api_request_processing_handle.
    /// <param name="responsedata">
    /// A stream object which is the JSON object processed by the #dz_stream_parser_class
    /// provided to the dz_api_request_processing_async() or the JSON as a raw C-string</param>
    /// if the no stream parser has been provided.
    /// <param name="userdata"></param>
    public delegate void dz_api_request_done_cb(IntPtr handle, dz_api_result_t  ret, IntPtr responsedata, IntPtr userdata);

    /// <summary>
    /// Deezer stream parser interface handle.
    /// An optional parser can be passed to the Native SDK to retrieve big JSON responses
    /// by chunks instead of having one huge string.
    /// The role of the class implementing this interface is to convert the JSON string
    /// into the expected structure.
    /// For instance, you could use it to convert JSON strings to dictionaries.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class dz_stream_parser_class 
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr tokener_new();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void tokener_free(IntPtr tok);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr tokener_parse(IntPtr tok, string data, uint len);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void tokener_object_release(IntPtr obj);
    };

    public class DeezerPlayerException : Exception
    {
        public dz_error_t DzErrorCode { get; private set; }

        public static DeezerPlayerException FromDzErrorCode(dz_error_t err)
        {
            DeezerPlayerException ex = new DeezerPlayerException();
            ex.DzErrorCode = err;

            throw ex;
        }

        public override string ToString()
        {
            return string.Format("Error raised by the Deezer Player Runtime: {0}", DzErrorCode);
        }

        private DeezerPlayerException() 
            : base()
        {
        }
    }

    public partial class DeezerApi
    {
        public static void ThrowExceptionForDzErrorCode(dz_error_t err)
        {
            switch (err)
            {
                case dz_error_t.DZ_ERROR_NO_ERROR:
                case dz_error_t.DZ_ERROR_NO_ERROR_ASYNC:
                    return;

                default:
                    Logger.LogToConsole(" ... last Deezer operation returned error, code={0}", err);
                    throw DeezerPlayerException.FromDzErrorCode(err);
            }
        }
    }
}
