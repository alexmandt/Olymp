using System.Threading.Tasks;
using Olymp.Communication;
using Olymp.Communication.Messages;
using Olymp.Nodes.Abstractions;

namespace Olymp.Nodes
{
    public abstract class Node : IService
    {
        private readonly string _localHost = "127.0.0.1";
        private readonly int _port;
        protected string _name;

        protected Node(Util.Configuration configuration, int port)
        {
            _port = port;
            _name = configuration.Name;
        }

        public void Start()
        {
            var server = new NodeCommunicationServer(_localHost, _port, _name);
            Task.Run(() => { server.Start(Handle); });
        }

        protected abstract (Command cmd, IMessage unencryptedMessage)
            Handle(Message message, byte[] unencryptedMessage);
    }
}