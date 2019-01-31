using System;
using MessagePack;
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
            if (_name.Substring(_name.Length - 4) != ".cfg")
                _name = _name + ".cfg";

            Success("Started ConfigurationServer", _name);
        }

        protected override (Command cmd, IMessage unencryptedMessage) Handle(BaseMessage message)
        {
            switch (message.Command)
            {
                case Command.CONF_ADD_USER:
                    var addUserMsg = MessagePackSerializer.Deserialize<AddUserMessage>(message.Content);
                    try
                    {
                        UserRepository.Instance.AddUser(addUserMsg);
                        Success($"Added user {addUserMsg.Username}!", _name);
                        return (Command.OK, new SingleValueMessage {Value = addUserMsg.Username});
                    }
                    catch (Exception)
                    {
                        Error($"Couldn't add user {addUserMsg.Username}!", _name);
                        return (Command.FAIL, new SingleValueMessage {Value = addUserMsg.Username});
                    }
                case Command.CONF_PUT_PROGRAM:
                // Not implemented => default to pipeline
                case Command.CONF_PUT_PIPELINE:
                    var file = MessagePackSerializer.Deserialize<PutMessage>(message.Content);
                    try
                    {
                        FileRepository.Instance.AddFile(file);
                        return (Command.OK, new SingleValueMessage {Value = file.TargetName});
                    }
                    catch (Exception)
                    {
                        return (Command.FAIL, new SingleValueMessage {Value = file.TargetName});
                    }
                case Command.CONF_SET_USER_LEVEL:
                    var setUserLevelMessage = MessagePackSerializer.Deserialize<SetUserLevelMessage>(message.Content);

                    try
                    {
                        var user = UserRepository.Instance.GetUser(setUserLevelMessage.User);
                        user.Level = setUserLevelMessage.NewLevel;
                        UserRepository.Instance.UpdateUser(user.UserName, user);
                        return (Command.OK, setUserLevelMessage);
                    }
                    catch (Exception)
                    {
                        return (Command.FAIL, setUserLevelMessage);
                    }
                case Command.CONF_REMOVE_USER:
                    var removeUser = MessagePackSerializer.Deserialize<SingleValueMessage>(message.Content);
                    try
                    {
                        UserRepository.Instance.RemoveUser(removeUser.Value);
                        return (Command.OK, removeUser);
                    }
                    catch (Exception)
                    {
                        return (Command.FAIL, removeUser);
                    }
//                case Command.CONF_DISTRIBUTE:
//                    // TODO: Implement a distribute command
//                    return (Command.FAIL, "NOT IMPLEMENTED YET");
//                    break;
//                case Command.CONF_GET_STATUS:
//                    var status = MessagePackSerializer.Deserialize<GetStatusMessage>(unencryptedMessage);
//                    return (Command.OK, new GetStatusMessage
//                    {
//                        Target = status.Target,
//                        StatusInfo = new List<Status>
//                        {
//                            new Status { Up = true, Name = "child1.local" },
//                            new Status { Up = false, Name = "child2.local" }
//                        }
//                    });
                default:
                    return (Command.FAIL, new SingleValueMessage {Value = "Command not recognized. Check versions compatibility. :("});
            }
        }
    }
}