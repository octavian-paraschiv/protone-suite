using System;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace OPMedia.DeezerInterop.PlayerApi
{
    public class DeezerApi
    {
        internal static T _callDzMethod<T>(params object[] parameters)
        {
            StackTrace st = new StackTrace();
            MethodBase mb = st.GetFrame(1).GetMethod();

            Type t = null;
            if (Environment.Is64BitProcess)
                t = typeof(DeezerApi64);
            else
                t = typeof(DeezerApi32);

            System.Reflection.MethodInfo mi = t.GetMethod(mb.Name, BindingFlags.Static | BindingFlags.Public);
            if (mi != null)
                return (T)mi.Invoke(null, parameters);

            throw new Exception("Runtime exception while calling " + "a");
        }

            public static dz_connect_event_t dz_connect_event_get_type(IntPtr self)
            {
                return _callDzMethod<dz_connect_event_t>(self);
            }

            public static IntPtr dz_connect_new(dz_connect_configuration config)
            {
                return _callDzMethod<IntPtr>(config);
            }

            public static dz_error_t dz_connect_debug_log_disable(IntPtr self)
            {
                return _callDzMethod<dz_error_t>(self);
            }

            public static dz_error_t dz_connect_activate(IntPtr self, IntPtr userdata)
            {
                return _callDzMethod<dz_error_t>(self, userdata);
            }

            public static dz_error_t dz_connect_deactivate(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata);
            }

            public static dz_error_t dz_connect_set_access_token(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, string access_token)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, access_token);
            }

            public static dz_error_t dz_connect_logout(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata);
            }

            public static dz_error_t dz_connect_cache_path_set(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, string local_path)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, local_path);
            }

            public static dz_error_t dz_connect_offline_mode(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, bool offline_mode_forced)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, offline_mode_forced);
            }

            public static dz_player_event_t dz_player_event_get_type(IntPtr self)
            {
                return _callDzMethod<dz_player_event_t>(self);
            }

            public static string dz_player_event_track_selected_dzapiinfo(IntPtr self)
            {
                return _callDzMethod<string>(self);
            }

            public static IntPtr dz_player_new(IntPtr connect)
            {
                return _callDzMethod<IntPtr>(connect);
            }

            public static dz_error_t dz_player_set_event_cb(IntPtr self, dz_player_onevent_cb cb)
            {
                return _callDzMethod<dz_error_t>(self, cb);
            }

            public static dz_error_t dz_player_set_render_progress_cb(IntPtr self, dz_player_onrenderprogress_cb cb, ulong refresh_us)
            {
                return _callDzMethod<dz_error_t>(self, cb, refresh_us);
            }

            public static dz_error_t dz_player_activate(IntPtr self, IntPtr supervisor)
            {
                return _callDzMethod<dz_error_t>(self, supervisor);
            }

            public static dz_error_t dz_player_deactivate(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata);
            }

            public static dz_error_t dz_player_load(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, string tracklist_data)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, tracklist_data);
            }

            public static dz_error_t dz_player_play(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, dz_player_play_command_t command, int idx)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, command, idx);
            }

            public static dz_error_t dz_player_stop(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata);
            }

            public static dz_error_t dz_player_pause(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata);
            }

            public static dz_error_t dz_player_resume(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata);
            }

            public static dz_error_t dz_player_seek(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, ulong pos_usecond)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, pos_usecond);
            }

            public static dz_error_t dz_player_set_output_volume(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, int volume)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, volume);
            }

            public static string dz_renderer_event_get_infos(IntPtr self)
            {
                return _callDzMethod<string>(self);
            }

            public static dz_error_t dz_player_set_renderer_event_cb(IntPtr self, dz_player_onrendererevent_cb cb)
            {
                return _callDzMethod<dz_error_t>(self, cb);
            }

            public static dz_error_t dz_player_renderer_set_volume(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, string renderer_id, int volume)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, renderer_id);
            }

            public static dz_error_t dz_player_set_crossfading_duration(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, uint fade_duration_ms)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, fade_duration_ms);
            }

            public static dz_error_t dz_player_set_track_quality(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, dz_track_quality_t quality)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, quality);
            }

            public static dz_error_t dz_player_set_output_mute(IntPtr self, dz_activity_operation_callback cb, IntPtr operation_userdata, bool muted)
            {
                return _callDzMethod<dz_error_t>(self, cb, operation_userdata, muted);
            }
        }
    }
