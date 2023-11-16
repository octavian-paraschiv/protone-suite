using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;

namespace OPMedia.DeezerInterop.OAuth
{
    public class RedirectHandler : IDisposable
    {
        ManualResetEvent _evtCompleted = new ManualResetEvent(false);

        NameValueCollection _queryParameters = null;

        HttpListener _listener = null;

        public string Response { get; private set; }

        public RedirectHandler()
        {
            this.Response = null;
        }

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:11000/");
            _listener.Start();
            HandleRequests();
        }

        private void HandleRequests()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                try
                {
                    while (_listener.IsListening && !_evtCompleted.WaitOne(0))
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if (ctx == null)
                                {
                                    return;
                                }

                                var req = ctx.Request;
                                var rsp = HandleRequest(req);
                                var rstr = $"<HTML><BODY>{rsp}</BODY></HTML>";
                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch
                            {
                                // ignored
                            }
                            finally
                            {
                                // always close the stream
                                if (ctx != null)
                                {
                                    ctx.Response.OutputStream.Close();
                                }
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            });
        }

        private string HandleRequest(HttpListenerRequest req)
        {
            string rsp = "Hello World!";

            _queryParameters = req.QueryString;

            string code = GetQueryParameter("code");

            if (string.IsNullOrEmpty(code))
                rsp = "Your user could not be succesfully authenticated.<br>Please close this browser window and retry.";
            else
                rsp = "Your user was succesfully authenticated.<br>You can close this browser window now.";

            _evtCompleted.Set();

            return rsp;
        }

        public void WaitCompletion(int delay)
        {
            _evtCompleted.WaitOne(delay);
        }

        public string GetQueryParameter(string key)
        {
            if (_queryParameters != null && _queryParameters.Count > 0)
                return _queryParameters[key];

            return null;
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                _listener.Stop();
                _listener = null;
            }
        }
    }
}
