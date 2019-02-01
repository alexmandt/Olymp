using System;
using Olymp.Communication;
using Olymp.Communication.Messages;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Master
{
    public class MasterNode : Node
    {
        public MasterNode(Util.Configuration configuration) : base(configuration, 17930)
        {
            // TODO: Register as service with DI
            new ConfigurationServerNode(configuration).Start();
            Success("Started MasterNode!", _name);
        }

        protected override (Command cmd, IMessage unencryptedMessage) Handle(BaseMessage message)
        {
            throw new NotImplementedException();
        }
    }
}