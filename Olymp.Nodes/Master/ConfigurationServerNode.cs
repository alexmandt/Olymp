using System;
using Newtonsoft.Json;
using Olymp.Communication;
using Olymp.Communication.Messages;
using Olymp.Persistence;
using static Olymp.Util.Log;

namespace Olymp.Nodes.Master
{
    public class ConfigurationServerNode : Node
    {
        public ConfigurationServerNode(Util.Configuration configuration) : base(configuration, 17929)
        {
            Success("Started ConfigurationServerNode!", Name);
        }

        public override (Command cmd, string unencryptedMessage) Handle(Message message, string unencryptedMessage)
        {
            switch (message.Command)
            {
                case Command.CONF_ADD_USER:
                    var addUserMsg = JsonConvert.DeserializeObject<AddUserMessage>(unencryptedMessage);
                    try
                    {
                        UserRepository.Instance.AddUser(addUserMsg);
                        Success($"Added user {addUserMsg.Username}!", Name);
                        return (Command.OK, addUserMsg.Username);
                    }
                    catch (Exception)
                    {
                        Error($"Couldn't add user {addUserMsg.Username}!", Name);
                        return (Command.FAIL, addUserMsg.Username);
                    }
                case Command.CONF_PUT_PROGRAM:
                case Command.CONF_PUT_PIPELINE:
                    var file = JsonConvert.DeserializeObject<PutMessage>(unencryptedMessage);
                    try
                    {
                        FileRepository.Instance.AddFile(file);
                        return (Command.OK, file.TargetName);
                    }
                    catch (Exception)
                    {
                        return (Command.FAIL, file.TargetName);
                    }
                default:
                    return (Command.FAIL, ":(");
            }
        }
    }
}