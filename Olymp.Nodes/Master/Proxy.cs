using System;
using System.Net;
using Olymp.Nodes.Abstractions;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Master
{
    public class Proxy : IService
    {
        private int _port { get; set; }
        private string _name { get; set; }

        private Proxy()
        {
            if (HttpListener.IsSupported) return;
            
            Error("HttpListener isn't supported on this machine. Aborting!", _name);
            throw new NotSupportedException(typeof(HttpListener).ToString());
        }

        public static Proxy Configure(int port)
        {
            return new Proxy {_port = port, _name = ""};
        }
        
        public static Proxy Configure(int port, string name)
        {
            return new Proxy {_port = port, _name = name};
        }
        
        public void Start()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{_port}/");
            listener.Start();

            while (true)
            {
                try
                {
                    var ctx = listener.GetContext();
                    var req = ctx.Request;

                    var path = req.Url.AbsolutePath;
                    //TODO: Match path to registered methods
                }
                catch (Exception)
                {
                    Warning("Error while processing request!", _name);
                }
            }
            
        }
    }
}