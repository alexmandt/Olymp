using System;
using System.Threading.Tasks;
using Olymp.Communication;
using Olymp.Communication.Messages;
using Olymp.Nodes.Abstractions;
using Olymp.Util;

namespace Olymp.Nodes
{
    public abstract class Node : IService
    {
        protected string _name;
        private readonly int _port;
        private readonly string _localHost = "127.0.0.1";

        protected Node(Util.Configuration configuration, int port)
        {
            this._port = port;
            this._name = configuration.Name;
        }

        protected abstract (Command cmd, IMessage unencryptedMessage) Handle(Message message, byte[] unencryptedMessage);

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