using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MToolkit.Helpers
{
    class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> _responderMethod;

        public WebServer(IReadOnlyCollection<string> prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
            }

            // URI prefixes are required eg: "http://localhost:8080/test/"
            if (prefixes == null || prefixes.Count == 0)
            {
                throw new ArgumentException("URI prefixes are required");
            }

            if (method == null)
            {
                throw new ArgumentException("responder method required");
            }

            foreach (var prefix in prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }

            _responderMethod = method;
            _listener.Start();
        }

        public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes)
           : this(prefixes, method)
        {
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine("Webserver running...");
                try
                {
                    while (_listener.IsListening)
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
                                var rstr = ctx.Request.HttpMethod == "GET" ? _responderMethod(ctx.Request) : string.Empty;
                                var buf = Encoding.UTF8.GetBytes(rstr);
                                ctx.Response.AddHeader("Access-Control-Allow-Origin", "*");
                                ctx.Response.AddHeader("Access-Control-Allow-Headers", "*");
                                ctx.Response.AddHeader("Access-Control-Allow-Methods", "GET");
                                ctx.Response.ContentEncoding = Encoding.UTF8;
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
                catch (Exception)
                {
                    // ignored
                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
