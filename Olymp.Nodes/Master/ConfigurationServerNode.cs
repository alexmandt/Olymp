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
            switch (message.command)
            {
                case Command.CONF_ADD_USER:
                    var addUserMsg = JsonConvert.DeserializeObject<AddUserMessage>(unencryptedMessage);
                    try
                    {
                        UserRepository.Instance.AddUser(addUserMsg);
                        Success($"Added user {addUserMsg.Username}!", Name);
                    }
                    catch (Exception e)
                    {
                        Error($"Couldn't add user {addUserMsg.Username}!", Name);
                    }                    
                    break;
            }

            return (Command.OK, "");
        }
    }
}