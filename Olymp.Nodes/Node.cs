using System;
using System.Threading.Tasks;
using Olymp.Communication;
using Olymp.Util;

namespace Olymp.Nodes
{
    public abstract class Node
    {
        protected readonly string Name;
        private readonly int _port;

        protected Node(Util.Configuration configuration, int port)
        {
            _port = port;
            Name = configuration.Name;
        }

        public abstract (Command cmd, string unencryptedMessage) Handle(Message message, string unecryptedMessage);
        
        public void Start()
        {
            var ncs = new NodeCommunicationServer("127.0.0.1", _port, Name);
            Task.Run(() =>
            {
                ncs.Start(Handle);
            });
        }
    }
}