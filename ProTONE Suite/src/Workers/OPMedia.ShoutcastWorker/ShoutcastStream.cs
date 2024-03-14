using OPMedia.Core.Configuration;
using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using TagLib.Mpeg;
using static TagLib.File;

namespace OPMedia.ShoutcastWorker
{
    public delegate void StreamPropertyChangedDG(Dictionary<string, string> props);

    /// <summary>
    /// Provides the functionality to receive a shoutcast media stream
    /// </summary>
    public class ShoutcastStream : Stream
    {
        private int bitrate = 128;
        private int metaInt = 8192;
        private string contentType = "";

        private int receivedBytes;
        private Stream netStream;
        private bool connected = false;

        private string _streamTitle;

        public bool Connected { get { return connected; } }

        public int Bitrate { get { return bitrate; } set { bitrate = value; } }

        private int sampleRate = 44100;
        public int SampleRate { get { return sampleRate; } set { sampleRate = value; } }

        public string ContentType { get; private set; }

        List<byte> _reservoir = new List<byte>();

        public event StreamPropertyChangedDG StreamPropertyChanged = null;

        /// <summary>
        /// Creates a new ShoutcastStream and connects to the specified Url
        /// </summary>
        /// <param name="url">Url of the Shoutcast stream</param>
        public ShoutcastStream(string url, int timeout)
        {
            this.ContentType = string.Empty;
            InitShoutcastStream(url, timeout, false);

            if (connected)
                InitShoutcastStream(url, timeout, true);
        }

        private void InitShoutcastStream(string url, int timeout, bool skipMetaInfo)
        {
            HttpWebResponse response = null;

            if (netStream != null)
            {
                netStream.Close();
                netStream = null;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Clear();
            request.Headers.Add("Icy-MetaData", "1");

            request.Proxy = AppConfig.GetWebProxy();
            request.KeepAlive = false;
            request.UserAgent = ProTONEConstants.PlayerUserAgent;
            request.ServicePoint.Expect100Continue = false;

            request.Timeout = timeout;

            try
            {
                ToggleAllowUnsafeHeaderParsing(true);
                response = (HttpWebResponse)request.GetResponse();

                Dictionary<string, string> nvc = new Dictionary<string, string>();
                foreach (string key in response.Headers.AllKeys)
                {
                    if (response.Headers[key] != null)
                        nvc.Add(key.ToLowerInvariant(), response.Headers[key].ToLowerInvariant());
                }

                try
                {
                    metaInt = int.Parse(nvc["icy-metaint"]);
                }
                catch { }

                try
                {
                    if (skipMetaInfo == false)
                        bitrate = int.Parse(nvc["icy-br"]);
                }
                catch { }

                try
                {
                    contentType = nvc["content-type"];
                }
                catch { }

                switch (contentType)
                {
                    case "audio/mpg":
                    case "audio/mpeg":
                        //case "audio/aac":
                        //case "audio/aacp":
                        this.ContentType = contentType;
                        break;

                    default:
                        WorkerException.Throw(WorkerError.UnsupportedMediaType, -1);
                        break;
                }

                receivedBytes = 0;

                netStream = response.GetResponseStream();

                if (skipMetaInfo == false)
                {
                    int passes = 0;
                    while (passes < 100 * 1024)
                    {
                        passes++;
                        byte[] buff = new byte[1024];
                        int result = netStream.Read(buff, 0, buff.Length);
                        if (result > 0)
                        {
                            try
                            {
                                using (BufferFileAbstraction bfa = new BufferFileAbstraction(buff))
                                {
                                    AudioFile f = new AudioFile(bfa);
                                    AudioHeader hdr = (AudioHeader)f.Properties.Codecs.FirstOrDefault();

                                    if (hdr.Version == TagLib.Mpeg.Version.Version1 && hdr.AudioLayer == 3)
                                    {
                                        sampleRate = hdr.AudioSampleRate;
                                        bitrate = hdr.AudioBitrate;
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                connected = true;
            }
            catch
            {
                connected = false;
                WorkerException.Throw(WorkerError.CannotConnectToMedia, -1);
            }
            finally
            {
                ToggleAllowUnsafeHeaderParsing(false);
            }
        }

        static bool ToggleAllowUnsafeHeaderParsing(bool enable)
        {
            //Get the assembly that contains the internal class
            Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (settingsSectionType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created already invoking the property will create it for us.
                    object anInstance = settingsSectionType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework if unsafe header parsing is allowed
                        FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, enable);
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Parses the received Meta Info
        /// </summary>
        /// <param name="metaInfo"></param>
        private void ParseMetaInfo(byte[] metaInfo)
        {
            string metaString = Encoding.ASCII.GetString(metaInfo);
            if (metaString?.Length > 0)
            {
                Logger.LogTrace("MetaInfo: " + metaString);

                var grps = metaString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (grps?.Length > 0)
                {
                    foreach (var grp in grps)
                    {
                        var data = grp.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (data?.Length == 2)
                        {
                            var key = (data[0] ?? "").ToUpperInvariant();
                            if (key == "STREAMTITLE")
                            {
                                string newStreamTitle = data[1].Trim('\'').Trim();
                                if (!newStreamTitle.Equals(_streamTitle))
                                {
                                    _streamTitle = newStreamTitle;

                                    Dictionary<string, string> props = new Dictionary<string, string>();
                                    props.Add("TXT_TITLE", _streamTitle);
                                    StreamPropertyChanged?.Invoke(props);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the ShoutcastStream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return connected; }
        }

        /// <summary>
        /// Gets a value that indicates whether the ShoutcastStream supports seeking.
        /// This property will always be false.
        /// </summary>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value that indicates whether the ShoutcastStream supports writing.
        /// This property will always be false.
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the title of the stream
        /// </summary>
        public string StreamTitle
        {
            get { return _streamTitle; }
        }

        /// <summary>
        /// Flushes data from the stream.
        /// This method is currently not supported
        /// </summary>
        public override void Flush()
        {
            return;
        }

        /// <summary>
        /// Gets the length of the data available on the Stream.
        /// This property is not currently supported and always thows a <see cref="NotSupportedException"/>.
        /// </summary>
        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets the current position in the stream.
        /// This property is not currently supported and always thows a <see cref="NotSupportedException"/>.
        /// </summary>
        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Reads data from the ShoutcastStream.
        /// </summary>
        /// <param name="buffer">An array of bytes to store the received data from the ShoutcastStream.</param>
        /// <param name="offset">The location in the buffer to begin storing the data to.</param>
        /// <param name="count">The number of bytes to read from the ShoutcastStream.</param>
        /// <returns>The number of bytes read from the ShoutcastStream.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                if (receivedBytes == metaInt)
                {
                    int metaLen = netStream.ReadByte();
                    if (metaLen > 0)
                    {
                        byte[] metaInfo = new byte[metaLen * 16];
                        int len = 0;
                        while ((len += netStream.Read(metaInfo, len, metaInfo.Length - len)) < metaInfo.Length) ;
                        ParseMetaInfo(metaInfo);
                    }
                    receivedBytes = 0;
                }

                int bytesLeft = ((metaInt - receivedBytes) > count) ? count : (metaInt - receivedBytes);
                int result = netStream.Read(buffer, offset, bytesLeft);
                receivedBytes += result;

                return result;
            }
            catch (Exception e)
            {
                connected = false;
                Console.WriteLine(e.Message);
                return -1;
            }
        }


        /// <summary>
        /// Closes the ShoutcastStream.
        /// </summary>
        public override void Close()
        {
            connected = false;
            netStream.Close();
        }

        /// <summary>
        /// Sets the current position of the stream to the given value.
        /// This Method is not currently supported and always throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the length of the stream.
        /// This Method always throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes data to the ShoutcastStream.
        /// This method is not currently supported and always throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }

    public class BufferFileAbstraction : IFileAbstraction, IDisposable
    {
        public string Name => "";

        public Stream ReadStream => _ms;

        public Stream WriteStream => throw new NotImplementedException();

        private MemoryStream _ms = null;

        public BufferFileAbstraction(byte[] buffer)
        {
            _ms = new MemoryStream(buffer);
        }

        public void CloseStream(Stream stream)
        {
        }

        public void Dispose()
        {
            _ms.Close();
            _ms = null;
        }
    }

}
