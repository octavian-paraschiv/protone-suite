using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

// Ported from original header file deezer-connect.h

namespace OPMedia.DeezerInterop.PlayerApi
{
    #region Enums
    /// <summary>
    /// Streaming modes.
    /// </summary>
    public enum dz_streaming_mode_t
    {
        DZ_STREAMING_MODE_UNKNOWN,  /* Mode is not known or audio ad is playing. */
        DZ_STREAMING_MODE_ONDEMAND, /* On demand streaming mode. */
        DZ_STREAMING_MODE_RADIO,    /* Radio streaming mode. */
    }

    /// <summary>
    /// Connect events
    /// </summary>
    public enum dz_connect_event_t 
    {
        DZ_CONNECT_EVENT_UNKNOWN,                           /* Connect event has not been set yet, not a valid value. */
        DZ_CONNECT_EVENT_USER_OFFLINE_AVAILABLE,            /* User logged in, and credentials from offline store are loaded. */
    
        DZ_CONNECT_EVENT_USER_ACCESS_TOKEN_OK,              /* (Not available) dz_connect_login_with_email() ok, and access_token is available */
        DZ_CONNECT_EVENT_USER_ACCESS_TOKEN_FAILED,          /* (Not available) dz_connect_login_with_email() failed */

        DZ_CONNECT_EVENT_USER_LOGIN_OK,                     /* Login with access_token ok, infos from user available. */
        DZ_CONNECT_EVENT_USER_LOGIN_FAIL_NETWORK_ERROR,     /* Login with access_token failed because of network condition. */
        DZ_CONNECT_EVENT_USER_LOGIN_FAIL_BAD_CREDENTIALS,   /* Login with access_token failed because of bad credentials. */
        DZ_CONNECT_EVENT_USER_LOGIN_FAIL_USER_INFO,         /* Login with access_token failed because of other problem. */
        DZ_CONNECT_EVENT_USER_LOGIN_FAIL_OFFLINE_MODE,      /* Login with access_token failed because we are in forced offline mode. */
    
        DZ_CONNECT_EVENT_USER_NEW_OPTIONS,                  /* User options have just changed. */
    
        DZ_CONNECT_EVENT_ADVERTISEMENT_START,               /* A new advertisement needs to be displayed. */
        DZ_CONNECT_EVENT_ADVERTISEMENT_STOP,                /* An advertisement needs to be stopped. */
    }

    /// <summary>
    /// Connect smartcache events
    /// </summary>
    public enum dz_connect_cache_event_t
    {
        DZ_CONNECT_CACHE_EVENT_UNKNOWN,                     /* Cache event has not been set yet, not a valid value. */
        DZ_CONNECT_CACHE_EVENT_SMART_CACHE_SIZE_CHANGED,    /* Smart cache size has changed */
        DZ_CONNECT_CACHE_EVENT_OFFLINE_CACHE_SIZE_CHANGED,  /* Offline cache size has changed */
        DZ_CONNECT_CACHE_EVENT_PARTITION_NEARLY_FULL,       /* Partition where the tracks are cached is almost full, only 200MB left */
        DZ_CONNECT_CACHE_EVENT_PARTITION_FULL,              /* Partition where the tracks are cached is full */
    }
    #endregion

    #region Delegates
    /// <summary>
    /// Prototype for on connect event callback
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void dz_connect_onevent_cb(IntPtr handle, IntPtr evtHandle, IntPtr userData);

    /// <summary>
    /// Prototype for crash report notification delegate
    /// </summary>
    /// <returns>If the application has previously crashed (in the previous session)</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool dz_connect_crash_reporting_delegate();

    /// <summary>
    /// Prototype for smartcache usage callback
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void dz_connect_on_cache_event_cb(IntPtr handle, IntPtr evtHandle, IntPtr userData);
    #endregion

    #region Structures
    /// <summary>
    /// Configuration for a Deezer connect handle
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class dz_connect_configuration 
    {
        // The Deezer application ID being used
        [MarshalAs(UnmanagedType.LPStr)]
        public string app_id;
    
        // Product ID of the application being used.\n
        // In a compact form, only ascii characters and '.' (point character) are allowed.
        [MarshalAs(UnmanagedType.LPStr)]
        public string product_id;

        // Build product ID which will be used by the Native SDK. 
        [MarshalAs(UnmanagedType.LPStr)]
        public string product_build_id;
    
        // User profile path of application being used. 
        [MarshalAs(UnmanagedType.LPStr)]
        public string user_profile_path;
    
        // (Optional) #connect_event_cb() connect event callback of application being used.
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public dz_connect_onevent_cb connect_event_cb;
    
        // (Optional) Use to allow discovery.
        [MarshalAs(UnmanagedType.LPStr)]
        public string anonymous_blob;
    
        // (Optional) Delegate used to let the lib know if the application has previously crashed.\n
        // If this delegate is not set, an internal crash reporter will be used by the Native SDK.
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public dz_connect_crash_reporting_delegate app_has_crashed_delegate;
    }
    #endregion

    #region deezer-connect API

    public partial class DeezerApi
    {
        /**
         * @brief Get the type of the event.
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_connect_event_t dz_connect_event_get_type(IntPtr self);

        /**
         * @brief Get the access token of the current session for a #DZ_CONNECT_EVENT_USER_ACCESS_TOKEN_OK event.
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_connect_event_get_access_token(IntPtr self);

        /**
         * @brief Get a JSON describing events.
         *
         * Available for events:
         * - #DZ_CONNECT_EVENT_ADVERTISEMENT_START
         * - #DZ_CONNECT_EVENT_ADVERTISEMENT_STOP
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_connect_event_get_advertisement_infos_json(IntPtr self);

        /**
         * @brief Get api /user/me JSON.
         *
         * Available for events:
         * - #DZ_CONNECT_EVENT_USER_OFFLINE_AVAILABLE
         * - #DZ_CONNECT_EVENT_USER_LOGIN_OK
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_connect_event_get_user_me_json(IntPtr self);
    
        /**
         * @brief Get api /options JSON.
         *
         * Available for events:
         * - #DZ_CONNECT_EVENT_USER_OFFLINE_AVAILABLE
         * - #DZ_CONNECT_EVENT_USER_LOGIN_OK
         * - #DZ_CONNECT_EVENT_USER_NEW_OPTIONS
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_connect_event_get_options_json(IntPtr self);

        /**
         * @brief Get the type of the event.
         *
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_connect_cache_event_t dz_connect_cache_event_get_type(IntPtr self);

        /**
         * @brief Get the event's size info.
         *
         * Available for events:
         * - #DZ_CONNECT_CACHE_EVENT_SMART_CACHE_SIZE_CHANGED.
         * - #DZ_CONNECT_CACHE_EVENT_OFFLINE_CACHE_SIZE_CHANGED.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 dz_connect_cache_event_get_new_size(IntPtr self);

                /**
         * @brief Constructor for IntPtr.
         *
         * @param config see #dz_connect_configuration.
         * @returns      A Deezer connect handle.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr dz_connect_new([MarshalAs(UnmanagedType.LPStruct)]dz_connect_configuration config);


        /**
         * @brief Disable log generated by the lib.
         *
         * @param self A Deezer connect handle.
         * @returns    DZ_ERROR_NO_ERROR.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_debug_log_disable(IntPtr self);
    
        /**
         * @brief Get the device_id associated with the lib.
         *
         * @param self A Deezer connect handle.
         * @returns    A string to a device_id.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_connect_get_device_id(IntPtr self);
    
        /**
         * @brief Activate the connect session.
         *
         *
         * @param self     The Deezer connect handle you want to activate.
         * @param userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_activate(IntPtr self, IntPtr userdata);


        /**
         * @brief Deactivate for IntPtr.
         *
         * @param self               The Deezer connect handle you want to deactivate.
         * @param cb                 Function called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_deactivate(IntPtr self,
            dz_activity_operation_callback cb,
            IntPtr operation_userdata);

        /**
         * @brief Set current OAuth access token.
         *
         * @param self               A Deezer connect handle.
         * @param cb                 Function called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param access_token       OAuth access token.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_set_access_token(IntPtr self,
            dz_activity_operation_callback cb,
            IntPtr       operation_userdata,
                                                                
            [MarshalAs(UnmanagedType.LPStr)]
            string access_token);

        /**
         * @brief Logout the user.
         *
         * User data is kept, and removed only if another user logs in after.
         *
         * @param self               A Deezer connect handle.
         * @param cb                 Function called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_logout(IntPtr self,
                                                   dz_activity_operation_callback cb,
                                                   IntPtr operation_userdata);

        /**
         * @brief Set the path of the Native SDK cache.
         *
         * @note The Native SDK has a local cache mechanism which can be erased at any time.\n
         * In such a case, the cache will be updated over again whenever it has internet access.
         *
         * @param self               A Deezer connect handle.
         * @param cb                 Function called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param local_path         Path to the cache.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_cache_path_set(IntPtr self,

            [MarshalAs(UnmanagedType.FunctionPtr)]
            dz_activity_operation_callback cb,
            
            IntPtr operation_userdata,
            
            [MarshalAs(UnmanagedType.LPStr)]
            string local_path);

        /**
         * @brief Set the audio smartcache quota.
         *
         * The smartcache allows the player to store tracks while playing.
         *
         * A smartcache should not be shared between application or several instances of your
         * application.
         *
         * @param self               A Deezer connect handle.
         * @param cb                 Function called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param quota_kB           Quota in kB.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_smartcache_quota_set(IntPtr self,
                dz_activity_operation_callback cb,
                IntPtr operation_userdata,
                UInt32 quota_kB);

        /**
         * @brief Set the smartcache event callback.
         *
         * @note Set to NULL to disable.
         *
         * @param self               A Deezer connect handle.
         * @param cb                 Function called when the result is available.
         * @param operation_userdata A reference to the user’s data.
         * @param cacheevent_cb      Callback call on event.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_cache_eventcb_set(IntPtr self,
                                                              dz_activity_operation_callback cb,
                                                              IntPtr operation_userdata,
                                                              dz_connect_on_cache_event_cb cacheevent_cb);
    
        /**
          * @brief Get the cache size (smartcache + offline tracks).
          *
          * @param self Deezer connect handle.
          * @return     The current size in bytes.
          */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 dz_connect_cache_current_size(IntPtr self);
    
        /**
          * @brief Force offline mode in lib.
          *
          * @param self               A Deezer connect handle.
          * @param cb                 Function called when the result is available.
          * @param operation_userdata A reference to the user’s data.
          */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_cache_flush(IntPtr self,
                                                        dz_activity_operation_callback cb,
                                                        IntPtr operation_userdata);

        /**
         * @brief Force offline mode in lib.
         *
         * @param self                A Deezer connect handle.
         * @param cb                  Function called when the result is available.
         * @param operation_userdata  A reference to the user’s data.
         * @param offline_mode_forced is offline mode enforced.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_offline_mode(IntPtr self,
                                                         dz_activity_operation_callback cb,
                                                         IntPtr operation_userdata,
                                                         bool  offline_mode_forced);

        /**
         * @brief Send an app event.
         *
         * There is no return code.
         *
         * Example of event_name:
         * - menu_click > type (settings, feed, explore), session_duration.
         *
         * @param self          A Deezer connect handle.
         * @param sz_event_name Event name string.
         * @param sz_properties Serialized JSON of properties.
         * @param sz_path       Path to associate event with.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern dz_error_t dz_connect_push_app_event(IntPtr self,
            [MarshalAs(UnmanagedType.LPStr)]
            string sz_event_name,
            
            [MarshalAs(UnmanagedType.LPStr)]
            string sz_properties,
        
            [MarshalAs(UnmanagedType.LPStr)]
            string sz_path);

        /**
         * @brief Get the build id of the Native SDK.
         *
         * @return A string representing the build id of the Native SDK.
         */
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(NativeStringMarshaller))]
        public static extern string dz_connect_get_build_id();

        //public static string dz_connect_get_build_id_2()
        //{
        //    IntPtr pStr = dz_connect_get_build_id();

        //    return Marshal.PtrToStringAnsi(pStr);
        //}
    }

    public class NativeStringMarshaller : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new NativeStringMarshaller();
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            //Marshal.FreeHGlobal(pNativeData);
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            return IntPtr.Zero;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return Marshal.PtrToStringAnsi(pNativeData);
        }
    }

    #endregion
}
