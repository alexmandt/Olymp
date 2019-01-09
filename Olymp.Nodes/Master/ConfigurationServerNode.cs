using Olymp.Communication;
using Olymp.Util;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Master
{
    public class ConfigurationServerNode : Node
    {
        public ConfigurationServerNode(Util.Configuration configuration) : base(configuration, 17929)
        {
            Success("Started ConfigurationServerNode!", nodeId);
        }

        public override (Command cmd, string unencryptedMessage) Handle(Message message, string unecryptedMessage)
        {
            switch (message.command)
            {
                case Command.ADD:
                    break;
            }

            return (Command.OK, "");
        }
    }
}