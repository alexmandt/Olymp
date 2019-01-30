using Olymp.Communication;
using Olymp.Communication.Messages;
using Olymp.Util;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Child
{
    public class ChildNode : Node
    {
        public ChildNode(Util.Configuration configuration) : base(configuration, 17930)
        {
            Success("Started ChildNode!", _name);
        }

        protected override (Command cmd, IMessage unencryptedMessage) Handle(Message message, byte[] unencryptedMessage)
        {
            switch (message.Command)
            {
                case Command.MC_GET_STATUS:
                    return (Command.CM_REPORT_STATUS, this.CheckStatus());
                default:
                    return (Command.UNKNOWN, new SingleValueMessage() { Value = "Child node didn't recognize command." });
            }
        }

        private IMessage CheckStatus() => new SingleValueMessage() { Value = $"{_name} is healthy!" };
    }
}