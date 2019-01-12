using Olymp.Communication;
using Olymp.Communication.Messages;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Master
{
    public class MasterNode : Node
    {
        public MasterNode(Util.Configuration configuration) : base(configuration, 17930)
        {
            new ConfigurationServerNode(configuration).Start();
            Success("Started MasterNode!", base._name);
        }

        protected override (Command cmd, IMessage unencryptedMessage) Handle(Message message, byte[] unencryptedMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}