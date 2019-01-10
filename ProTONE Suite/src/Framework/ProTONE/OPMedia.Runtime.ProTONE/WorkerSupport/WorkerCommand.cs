using OPMedia.Core.Logging;
using OPMedia.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMedia.Runtime.ProTONE.WorkerSupport
{
    public class WorkerCommand
    {
        public const char FieldDelimiter = '|';

        private WorkerCommandType _type = WorkerCommandType.Invalid;

        private List<string> _args = new List<string>();

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

        public T Args<T>(int idx)
        {
            if (_args.Count > idx)
                return StringUtils.Coalesce<T>(_args[idx]);

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
            string[] fields = StringUtils.ToStringArray(s, FieldDelimiter);
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
