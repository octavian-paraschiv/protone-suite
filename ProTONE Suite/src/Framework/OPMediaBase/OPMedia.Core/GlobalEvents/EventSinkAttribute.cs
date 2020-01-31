using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OPMedia.Core.GlobalEvents
{
    public enum ExecutionType
    {
        /// <summary>
        /// Execute event sink code in the context of the calling thread.
        /// </summary>
        CallerThread,

        /// <summary>
        /// Execute the event sink code in the context of the main (UI) thread using the Post model.
        /// </summary>
        MainThreadPost,

        /// <summary>
        /// Execute the event sink code in the context of the main (UI) thread using the Send model.
        /// </summary>
        MainThreadSend,

        /// <summary>
        /// Execute the event sink code in the context of a new pool thread.
        /// </summary>
        PoolThread
    }

    public class EventSinkMethodInfo
    {
        public ExecutionType ExecutionType { get; set; }
        public MethodInfo MethodInfo { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited=false)]
    public class EventSinkAttribute : Attribute
    {
        public string EventName { get; private set; }

        public ExecutionType ExecutionType { get; private set; }

        /// <summary>
        /// Creates a new EventSink attribute.
        /// </summary>
        /// <param name="eventName">Name of the event to listen for.</param>
        /// <param name="execType">Thread contrext for executing the event sink code. Default is to execute on MainThread (UI thread) using the Post model.</param>
        public EventSinkAttribute(string eventName, ExecutionType execType = ExecutionType.MainThreadPost)
        {
            if (string.IsNullOrEmpty(eventName))
                throw new ArgumentNullException("eventName");

            this.EventName = eventName;
            this.ExecutionType = execType;
        }
    }

    public class SelfRegisteredEventSinkObject : IDisposable
    {
        public SelfRegisteredEventSinkObject()
        {
            EventDispatch.RegisterHandler(this);
        }

        ~SelfRegisteredEventSinkObject()
        {
            EventDispatch.UnregisterHandler(this);
        }

        public void Dispose()
        {
            EventDispatch.UnregisterHandler(this);
        }
    }
}
