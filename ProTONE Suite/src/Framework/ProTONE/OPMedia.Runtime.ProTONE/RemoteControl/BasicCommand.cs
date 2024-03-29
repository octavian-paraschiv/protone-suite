using OPMedia.Core;
using OPMedia.Core.GlobalEvents;
using OPMedia.Core.Logging;
using OPMedia.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPMedia.Runtime.ProTONE.RemoteControl
{
    public interface ICommandTarget
    {
        void EnqueueCommand(BasicCommand cmd);
    }

    public enum CommandType
    {
        Activate = 0,
        Terminate,
        PlayFiles,
        EnqueueFiles,
        ClearPlaylist,
        Playback,

        BrowseRemoteFiles,
        GetDriveList,
        QueryMediaRenderer,

        KeyPress,
    }


    public class BasicCommandTarget : SelfRegisteredEventSinkObject
    {
        ICommandTarget _target = null;

        ~BasicCommandTarget()
        {
            EventDispatch.UnregisterHandler(this);
        }

        public BasicCommandTarget(ICommandTarget target) : base()
        {
            _target = target;
        }

        [EventSink(BasicCommand.EventName)]
        public void RunCommand(string data)
        {
            BasicCommand cmd = BasicCommand.Create(data);
            _RunCommand(cmd);
        }

        private void _RunCommand(BasicCommand cmd)
        {
            Logger.LogTrace("BasicCommand.RunCommand: {0}", cmd);

            if (cmd == null)
                return;

            if (cmd.RequiresAnswer)
            {
                string response = RunExtendedCommand(cmd);
                PersistenceProxy.SendIpcEvent(BasicCommand.ResponseEventName, response);
                return;
            }

            if (_target != null)
            {
                _target.EnqueueCommand(cmd);
            }
        }

        private static string RunExtendedCommand(BasicCommand cmd)
        {
            try
            {
                switch (cmd.CommandType)
                {
                    case CommandType.BrowseRemoteFiles:
                        return (cmd as BrowseRemoteFilesCommand).Execute();

                    case CommandType.GetDriveList:
                        return (cmd as GetDriveListCommand).Execute();

                    case CommandType.QueryMediaRenderer:
                        return (cmd as QueryMediaRendererCommand).Execute();
                }

                throw new ArgumentException("Uncomprehensible command", "cmd");
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }

            return null;
        }
    }

    [Serializable]
    public abstract class BasicCommand
    {
        public const string EventName = "BasicCommand";
        public const string ResponseEventName = "BasicCommandResponse";

        public const char FieldSeparator = '?';

        protected CommandType type;
        protected string[] args;
        protected bool requiresAnswer;

        public CommandType CommandType
        {
            get
            {
                return type;
            }
        }

        public bool RequiresAnswer
        {
            get
            {
                return requiresAnswer;
            }
        }

        public string[] Args
        {
            get
            {
                return args;
            }
        }

        public override string ToString()
        {
            List<string> fields = new List<string>();
            fields.Add(type.ToString());

            if (args != null && args.Length > 0)
            {
                fields.AddRange(args);
            }

            return StringUtils.FromStringArray(fields.ToArray(), FieldSeparator);
        }

        internal BasicCommand(CommandType type, string[] args)
        {
            this.type = type;
            this.args = args;
        }

        public static BasicCommand Create(byte[] data)
        {
            return Create(Encoding.Unicode.GetString(data));
        }

        public static BasicCommand Create(string data)
        {
            try
            {
                string[] args = null;
                string[] rawArgs = data.Split(new char[] { BasicCommand.FieldSeparator });
                if (rawArgs.Length >= 1)
                {
                    CommandType type = (CommandType)Enum.Parse(typeof(CommandType), rawArgs[0]);

                    args = new string[rawArgs.Length - 1];
                    Array.Copy(rawArgs, 1, args, 0, rawArgs.Length - 1);

                    return Create(type, args);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        public static BasicCommand Create(CommandType type)
        {
            return Create(type, null);
        }

        public static bool RequiresArguments(CommandType type)
        {
            switch (type)
            {
                case CommandType.PlayFiles:
                case CommandType.EnqueueFiles:
                case CommandType.Playback:
                case CommandType.BrowseRemoteFiles:
                case CommandType.KeyPress:
                    return true;

                case CommandType.Activate:
                case CommandType.Terminate:
                case CommandType.GetDriveList:
                case CommandType.QueryMediaRenderer:
                    return false;


            }

            return false;
        }

        public static BasicCommand Create(CommandType type, params string[] args)
        {
            try
            {
                switch (type)
                {
                    case CommandType.Activate:
                        return new ActivateCommand();
                    case CommandType.Terminate:
                        return new TerminateCommand();
                    case CommandType.PlayFiles:
                        return new PlayFilesCommand(args);
                    case CommandType.EnqueueFiles:
                        return new EnqueueFilesCommand(args);
                    case CommandType.Playback:
                        return new PlaybackCommand(args);

                    case CommandType.BrowseRemoteFiles:
                        return new BrowseRemoteFilesCommand(args);
                    case CommandType.GetDriveList:
                        return new GetDriveListCommand();

                    case CommandType.QueryMediaRenderer:
                        return new QueryMediaRendererCommand();

                    case CommandType.KeyPress:
                        return new KeyPressCommand(args);

                    case CommandType.ClearPlaylist:
                        return new ClearPlaylistCommand();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }
    }
}
