using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

// Ported from original header file deezer-object.h

namespace OPMedia.DeezerInterop.PlayerApi
{
    /// <summary>
    /// Errors that can be returned by Deezer SDK API.
    /// </summary>
    public enum dz_error_t
    {
        DZ_ERROR_NO_ERROR = 0x00000000,
        DZ_ERROR_NO_ERROR_= 0x00000001,
        DZ_ERROR_ERROR_ARG = 0x00000002,
        DZ_ERROR_ERROR_STATE = 0x00000003,
        DZ_ERROR_NOT_IMPLEMENTED = 0x00000004,
        DZ_ERROR_ASYNC_CANCELED = 0x00000005,

        DZ_ERROR_NOT_ENOUGH_MEMORY,
        DZ_ERROR_OS_ERROR,
        DZ_ERROR_UNSUPPORTED,
        DZ_ERROR_CLASS_NOT_FOUND,
        DZ_ERROR_JSON_PARSING,
        DZ_ERROR_XML_PARSING,
        DZ_ERROR_PARSING,
        DZ_ERROR_CLASS_INSTANTIATION,
        DZ_ERROR_RUNNABLE_ALREADY_STARTED,
        DZ_ERROR_RUNNABLE_NOT_STARTED,
        DZ_ERROR_CACHE_RESOURCE_OPEN_FAILED,
        DZ_ERROR_FS_FULL,
        DZ_ERROR_FILE_EXISTS,
        DZ_ERROR_IO_ERROR,

        DZ_ERROR_CATEGORY_CONNECT = 0x00010000,
        DZ_ERROR_CONNECT_SESSION_LOGIN_FAILED,
        DZ_ERROR_USER_PROFILE_PERM_DENIED,
        DZ_ERROR_CACHE_DIRECTORY_PERM_DENIED,
        DZ_ERROR_CONNECT_SESSION_NOT_ONLINE,
        DZ_ERROR_CONNECT_SESSION_OFFLINE_MODE,
        DZ_ERROR_CONNECT_NO_OFFLINE_CACHE,

        DZ_ERROR_CATEGORY_PLAYER = 0x00020000,
        DZ_ERROR_PLAYER_QUEUELIST_NONE_SET,
        DZ_ERROR_PLAYER_QUEUELIST_BAD_INDEX,
        DZ_ERROR_PLAYER_QUEUELIST_NO_MEDIA,         /**< when trying to access non existant track/radio */
        DZ_ERROR_PLAYER_QUEUELIST_NO_RIGHTS,        /**< when trying to access track/radio with no rights */
        DZ_ERROR_PLAYER_QUEUELIST_RIGHT_TIMEOUT,    /**< when a timeout has occured when trying to get rights */
        DZ_ERROR_PLAYER_QUEUELIST_RADIO_TOO_MANY_SKIP,
        DZ_ERROR_PLAYER_QUEUELIST_NO_MORE_TRACK,
        DZ_ERROR_PLAYER_PAUSE_NOT_STARTED,
        DZ_ERROR_PLAYER_PAUSE_ALREADY_PAUSED,
        DZ_ERROR_PLAYER_UNPAUSE_NOT_STARTED,
        DZ_ERROR_PLAYER_UNPAUSE_NOT_PAUSED,
        DZ_ERROR_PLAYER_SEEK_NOT_SEEKABLE_NOT_STARTED,
        DZ_ERROR_PLAYER_SEEK_NOT_SEEKABLE_NO_DURATION,
        DZ_ERROR_PLAYER_SEEK_NOT_SEEKABLE_NOT_INDEXED,
        DZ_ERROR_PLAYER_SEEK_NOT_SEEKABLE,

        DZ_ERROR_CATEGORY_MEDIASTREAMER = 0x00030000,
        DZ_ERROR_MEDIASTREAMER_BAD_URL_SCHEME,
        DZ_ERROR_MEDIASTREAMER_BAD_URL_HOST,
        DZ_ERROR_MEDIASTREAMER_BAD_URL_TRACK,
        DZ_ERROR_MEDIASTREAMER_NOT_AVAILABLE_OFFLINE,
        DZ_ERROR_MEDIASTREAMER_NOT_READABLE,
        DZ_ERROR_MEDIASTREAMER_NO_DURATION,
        DZ_ERROR_MEDIASTREAMER_NOT_INDEXED,
        DZ_ERROR_MEDIASTREAMER_SEEK_NOT_SEEKABLE,
        DZ_ERROR_MEDIASTREAMER_NO_DATA,
        DZ_ERROR_MEDIASTREAMER_END_OF_STREAM,
        DZ_ERROR_MEDIASTREAMER_ALREADY_MAPPED,
        DZ_ERROR_MEDIASTREAMER_NOT_MAPPED,

        DZ_ERROR_CATEGORY_OFFLINE = 0x00040000,
        DZ_ERROR_OFFLINE_FS_FULL,

        DZ_ERROR_PLAYER_BAD_URL,
    }

    /// <summary>
    /// Prototype for any asynchronous operations of Deezer API.
    /// </summary>
    /// <param name="aDelegate">Application context data pointer that provides back when the result will be available.</param>
    /// <param name="operation_userdata">A reference to the user’s data.</param>
    /// <param name="status">error code returned</param>
    /// <param name="result">result the result of the operation</param>
    public delegate void dz_activity_operation_callback(IntPtr userData, IntPtr operation_userdata, dz_error_t status, IntPtr result);


    public partial class DeezerApi
    {
        const string DLL_NAME = "libdeezer.x86.dll";

        /// <summary>
        /// Keep object reference and avoid autorelease
        /// </summary>
        /// <param name="obj">The object handle that need to be retained</param>
        /// <returns>the object handle that is retained</returns>
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr dz_object_retain(IntPtr obj);

        /// <summary>
        /// Release object reference
        /// </summary>
        /// <param name="obj">The object handle that need to be released</param>
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dz_object_release(IntPtr obj);

        /// <summary>
        /// Deactivate the application activity
        /// This function is the last one to be called after the player and connect have been deactivated
        /// </summary>
        /// <param name="self">Application context</param>
        /// <param name="cb">Callback that will be called when the application activity has been deactivated</param>
        /// <param name="operation_userdata">A reference to the user’s data</param>
        /// <returns></returns>
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_activity_app_deactivate(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata);

        /// <summary>
        /// Dump some information about bad state of lib
        /// </summary>
        /// <param name="self">deezer connect handle</param>
        /// <remarks>
        /// Due to the way the Native SDK was designed, it means we didn't get the
        /// notifications all tasks are finished. 
        /// This is an internal bug, and there is a leak, it should be fixed.
        /// </remarks>
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dz_activity_debug_do_post_deactivation_analysis(IntPtr self);
    }
}
