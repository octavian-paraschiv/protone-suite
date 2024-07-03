using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace OPMedia.Core
{
    public static class EventDispatch
    {
        private static Dictionary<string, Dictionary<object, EventSinkMethodInfo>> _invocationMap =
            new Dictionary<string, Dictionary<object, EventSinkMethodInfo>>();

        static EventDispatch()
        {
        }

        public static void RegisterAsEventSink(this Control ctl)
        {
            if (ctl != null)
            {
                try
                {
                    RegisterHandler(ctl);

                    ctl.HandleDestroyed -= UnregisterAsEventSink;
                    ctl.HandleDestroyed += UnregisterAsEventSink;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        private static void UnregisterAsEventSink(object s, EventArgs e)
        {
            try
            {
                Control ctl = s as Control;
                if (ctl != null)
                {
                    UnregisterHandler(ctl);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void RegisterHandler(object handler)
        {
            if (handler != null)
            {
                BindingFlags bindingAttr = BindingFlags.NonPublic
                    | BindingFlags.Public
                    | BindingFlags.Instance
                    | BindingFlags.OptionalParamBinding
                    ;

                MethodInfo[] miArray = handler.GetType().GetMethods(bindingAttr);

                if (miArray != null)
                {
                    foreach (MethodInfo info in miArray)
                    {
                        object[] attrList = info.GetCustomAttributes(typeof(EventSinkAttribute), false);
                        if (attrList != null && attrList.Length > 0)
                        {
                            foreach (EventSinkAttribute attr in attrList)
                            {
                                string eventName = attr.EventName;

                                EventSinkMethodInfo esmi = new EventSinkMethodInfo
                                {
                                    ExecutionType = attr.ExecutionType,
                                    MethodInfo = info,
                                };

                                SafeAddToInvocationMap(eventName, handler, esmi);
                            }
                        }
                    }
                }

                GC.Collect();
            }
        }

        private static void SafeAddToInvocationMap(string eventName, object handler, EventSinkMethodInfo info)
        {
            lock (_invocationMap)
            {
                if (!_invocationMap.ContainsKey(eventName))
                {
                    _invocationMap.Add(eventName, new Dictionary<object, EventSinkMethodInfo>());
                }

                Dictionary<object, EventSinkMethodInfo> table = _invocationMap[eventName];
                if (table != null)
                {
                    if (table.ContainsKey(handler))
                    {
                        table[handler] = info; // override existing information
                    }
                    else
                    {
                        table.Add(handler, info);
                    }
                }
            }
        }

        public static void UnregisterHandler(object handler)
        {
            if (handler != null)
            {
                lock (_invocationMap)
                {
                    foreach (Dictionary<object, EventSinkMethodInfo> table in _invocationMap.Values)
                    {
                        if (table.ContainsKey(handler))
                        {
                            table.Remove(handler);
                        }
                    }
                }
            }

            GC.Collect();
        }

        private static Dictionary<object, EventSinkMethodInfo> SafeCopy(Dictionary<object, EventSinkMethodInfo> original)
        {
            Dictionary<object, EventSinkMethodInfo> retVal = null;

            if (original != null)
            {
                retVal = new Dictionary<object, EventSinkMethodInfo>();

                foreach (KeyValuePair<object, EventSinkMethodInfo> kvp in original)
                {
                    if (retVal.ContainsKey(kvp.Key))
                    {
                        retVal[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        retVal.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return retVal;
        }

        public static void DispatchEvent(string eventName, params object[] eventData)
        {
            Dictionary<object, EventSinkMethodInfo> tableEvent = null;

            bool handlerNotFound = true; ;

            lock (_invocationMap)
            {
                if (_invocationMap.ContainsKey(eventName))
                {
                    tableEvent = SafeCopy(_invocationMap[eventName]);
                    handlerNotFound = false;
                }
            }

            if (tableEvent != null)
            {
                foreach (KeyValuePair<object, EventSinkMethodInfo> kvp in tableEvent)
                {
                    try
                    {
                        object handler = kvp.Key;
                        EventSinkMethodInfo esmi = kvp.Value;
                        MethodInfo func = esmi.MethodInfo;

                        switch (esmi.ExecutionType)
                        {
                            case ExecutionType.CallerThread:
                                func.Invoke(handler, eventData);
                                break;

                            case ExecutionType.PoolThread:
                                ThreadPool.QueueUserWorkItem((c) =>
                                {
                                    func.Invoke(handler, eventData);
                                });
                                break;

                            case ExecutionType.MainThreadSend:
                                MainThread.Send(delegate (object x)
                                {
                                    func.Invoke(kvp.Key, eventData);
                                });
                                break;

                            case ExecutionType.MainThreadPost:
                            default:
                                MainThread.Post(delegate (object x)
                                {
                                    func.Invoke(kvp.Key, eventData);
                                });
                                break;
                        }
                    }
                    catch (TargetInvocationException ex)
                    {
                        Logger.LogException(ex.InnerException);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
            }

            if (handlerNotFound)
            {
                if (eventName == EventNames.ShowMessageBox)
                {
                    ProcessMessageBoxEvent(eventData);
                }
            }
        }

        private static void ProcessMessageBoxEvent(params object[] eventData)
        {
            string message = string.Empty;
            string title = string.Empty;
            MessageBoxIcon icon = MessageBoxIcon.None;

            int i = 0;
            if (eventData.Length > i)
            {
                message = eventData[i++] as string;
            }
            if (eventData.Length > i)
            {
                title = eventData[i++] as string;
            }
            if (eventData.Length > i)
            {
                icon = (MessageBoxIcon)eventData[i++];
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        public static void DumpStatistics()
        {
            Debug.WriteLine("EventDispatch: # of registered events: {0}", _invocationMap.Count);

            int objectCount = 0;
            foreach (KeyValuePair<string, Dictionary<object, EventSinkMethodInfo>> kvp in _invocationMap)
            {
                Debug.WriteLine("    EventDispatch: Event: {0} has {1} registered objects: ",
                    kvp.Key, kvp.Value.Count);

                foreach (object obj in kvp.Value.Keys)
                {
                    Debug.WriteLine("        EventDispatch: Registered object:  {0}", obj);
                }

                objectCount += kvp.Value.Count;

            }

            Debug.WriteLine("EventDispatch: # of registered objects: {0}", objectCount);
        }

    }
}
