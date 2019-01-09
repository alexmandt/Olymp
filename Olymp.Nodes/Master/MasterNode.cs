using Olymp.Communication;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Master
{
    public class MasterNode : Node
    {
        public MasterNode(Util.Configuration configuration) : base(configuration, 17930)
        {
            new ConfigurationServerNode(configuration).Start();
            Success("Started MasterNode!",Name);
        }

        public override (Command cmd, string unencryptedMessage) Handle(Message message, string unecryptedMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}