using System;
using System.IO;
using MessagePack;
using Olymp.Communication;
using Olymp.Communication.Messages;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Child
{
    public class ChildNode : Node
    {
        public ChildNode(Util.Configuration configuration) : base(configuration, 17930)
        {
            Success("Started ChildNode!", _name);
        }

        protected override (Command cmd, IMessage unencryptedMessage) Handle(BaseMessage message)
        {
            switch (message.Command)
            {
                case Command.MC_GET_STATUS:
                    return (Command.CM_REPORT_STATUS, CheckStatus());
                case Command.MC_PUT_PROGRAM:
                    return SaveProgram(message);
                default:
                    return (Command.UNKNOWN, new SingleValueMessage {Value = "Child node didn't recognize command."});
            }
        }

        private (Command, IMessage) SaveProgram(BaseMessage message)
        {
            var putProgramMessage = MessagePackSerializer.Deserialize<PutMessage>(message.Content);
            if (putProgramMessage.TargetType != TargetTypes.PROGRAM)
                return (Command.FAIL, new SingleValueMessage {Value = "Invalid use. Pipelines must be sent to master!"});

            // Terrible MVP below:
            using (var fs = new FileStream($"./{Guid.NewGuid()}", FileMode.CreateNew))
            {
                foreach (var @byte in message.Content) fs.WriteByte(@byte);
            }

            return (Command.CM_REPORT_RESULT, new SingleValueMessage {Value = "Program saved to memory. Ready for execution."});
        }

        private IMessage CheckStatus()
        {
            return new SingleValueMessage {Value = $"{_name} is healthy!"};
        }
    }
}