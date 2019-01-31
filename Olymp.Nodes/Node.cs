using System.Threading.Tasks;
using Olymp.Communication;
using Olymp.Communication.Messages;
using Olymp.Nodes.Abstractions;
using Olymp.Util;

namespace Olymp.Nodes
{
    public abstract class Node : IService
    {
        private readonly string _address;
        private readonly int _port;
        protected string _name;

        protected Node(Util.Configuration configuration, int port)
        {
            _address = configuration.Address ?? "127.0.0.1";
            _port = Validator.ValidatePort(port.ToString()) ? port : -1;
            _name = configuration.Name;
        }

        public void Start()
        {
            var server = new NodeCommunicationServer(_address, _port, _name);
            Task.Run(() => { server.Start(Handle); });
        }

        protected abstract (Command cmd, IMessage unencryptedMessage)
            Handle(Message message, byte[] unencryptedMessage);
    }
}