using System;
using System.Threading.Tasks;
using Olymp.Communication;
using Olymp.Nodes.Abstractions;
using Olymp.Util;

namespace Olymp.Nodes
{
    public abstract class Node : IService
    {
        protected readonly string _name;
        private readonly int _port;
        private readonly string _localHost = "127.0.0.1";

        protected Node(Util.Configuration configuration, int port)
        {
            this._port = port;
            this._name = configuration.Name;
        }

        public abstract (Command cmd, string unencryptedMessage) Handle(Message message, string unecryptedMessage);

        public void Start()
        {
            var server = new NodeCommunicationServer(this._localHost, this._port, this._name);
            Task.Run(() =>
            {
                server.Start(Handle);
            });
        }
    }
}