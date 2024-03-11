using OPMedia.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public class WorkerEvent : WorkerCommand
    {
        private WorkerEventType _evtType = WorkerEventType.Invalid;

        public new WorkerEventType Type
        {
            get
            {
                return _evtType;
            }
        }

        public new bool IsValid
        {
            get
            {
                return (base.IsValid && _evtType != WorkerEventType.Invalid);
            }
        }


        public WorkerEvent(WorkerEventType evtType) : base(WorkerCommandType.WorkerEvt)
        {
            _evtType = evtType;
            _args.Add(_evtType.ToString());
        }

        public WorkerEvent(WorkerCommand wc) : base(WorkerCommandType.WorkerEvt)
        {
            if (wc?.Type != WorkerCommandType.WorkerEvt)
                throw new Exception($"{nameof(wc)} must be of type {WorkerCommandType.WorkerEvt}");

            var evtType = wc.Arg<WorkerEventType>(0);

            _evtType = evtType;
            _args.Add(_evtType.ToString());
            _args = wc.Args.Skip(1).ToList();
        }
    }

    public class WorkerCommand
    {
        public const char FieldDelimiter = '|';

        private WorkerCommandType _type = WorkerCommandType.Invalid;

        protected List<string> _args = new List<string>();

        public List<string> Args => _args.AsReadOnly().ToList();

        public WorkerCommandType Type
        {
            get
            {
                return _type;
            }
        }

        public void AddParameter(string s)
        {
            _args.Add(s);
        }

        public T Arg<T>(int idx)
        {
            if (_args.Count > idx)
                return StringUtils.CastAs<T>(_args[idx], default(T));

            return default(T);
        }


        public WorkerCommand()
            : this(WorkerCommandType.Invalid)
        {
        }

        public WorkerCommand(WorkerCommandType type)
        {
            _type = type;
        }

        public bool IsValid
        {
            get
            {
                return (_type != WorkerCommandType.Invalid);
            }
        }

        public static WorkerCommand FromString(string s)
        {
            string[] fields = StringUtils.ToStringArray(s, FieldDelimiter, StringSplitOptions.None);
            if (fields == null || fields.Length < 1)
                return null;

            int i = 0;

            WorkerCommand wc = new WorkerCommand();
            wc._type = WorkerCommandTypeMapper.FromString(fields[i++]);

            while (i < fields.Length)
                wc.AddParameter(fields[i++]);

            return wc;
        }

        public override string ToString()
        {
            List<string> ls = new List<string>();
            ls.Add(_type.ToString());
            if (_args.Count > 0)
                ls.AddRange(_args);

            return StringUtils.FromStringList(ls, FieldDelimiter);
        }
    }
}
