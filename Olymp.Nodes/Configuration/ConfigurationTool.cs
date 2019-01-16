using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Colorful;
using MessagePack;
using Olymp.Communication;
using Olymp.Communication.Messages;
using Olymp.Nodes.Abstractions;
using Olymp.Util;

namespace Olymp.Nodes.Configuration
{
    public class ConfigurationTool : IService
    {
        private const string CONFIG = "CONFIG";
        private readonly Util.Configuration _configuration;

        private readonly Regex addUser = new Regex(" *ad?d? *us?e?r? *\"(.+)\" *\"(.+)\" *(tr?u?e?|fa?l?s?e?) *",
            RegexOptions.Compiled);

        private readonly Regex distribute =
            new Regex(" *di?s?t?r?i?b?u?t?e? *\"(.+)\" *to? *\"(.+)\" *", RegexOptions.Compiled);

        private readonly Regex getStatus =
            new Regex(" *ge?t? *st?a?t?u?s? *(se?l?f?|al?l?|no?d?e?s?) *", RegexOptions.Compiled);

        private readonly Regex putPipeline =
            new Regex(" *pu?t? *pip?e?l?i?n?e? *\"(.+)\" *as? *\"(.+)\" *", RegexOptions.Compiled);

        private readonly Regex putProgram =
            new Regex(" *pu?t? *pro?g?r?a?m? *\"(.+)\" *as? *\"(.+)\" *", RegexOptions.Compiled);

        public ConfigurationTool(Util.Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            var returnedData = NodeCommunicationClient.Send(
                _configuration.ConfigurationAddress,
                _configuration.User,
                _configuration.Password,
                new SingleValueMessage {Value = CONFIG},
                Command.REQ,
                nameof(ConfigurationTool));

            var name = MessagePackSerializer.Deserialize<SingleValueMessage>(returnedData.Content).Value;
            name = name.Substring(0, name.Length - 4);

            while (true)
            {
                Console.Write($"{name}> ");
                var command = Console.ReadLine();

                var msgCommand = Command.FAIL;
                IMessage content = null;

                //Add user
                if (addUser.IsMatch(command))
                {
                    var groups = addUser.Match(command).Groups.Select(a => a.Value).ToList();
                    var addUserMsg = new AddUserMessage
                    {
                        IsAdmin = groups[3].First() == 't',
                        Username = groups[1],
                        Password = groups[2]
                    };
                    content = addUserMsg;
                }
                //Upload file
                else if (putProgram.IsMatch(command) || putPipeline.IsMatch(command))
                {
                    var isProgram = putProgram.IsMatch(command);

                    var groups = isProgram
                        ? putProgram.Match(command).Groups.Select(a => a.Value).ToList()
                        : putPipeline.Match(command).Groups.Select(a => a.Value).ToList();

                    msgCommand = isProgram ? Command.CONF_PUT_PROGRAM : Command.CONF_PUT_PIPELINE;
                    var putMsg = new PutMessage
                    {
                        TargetName = groups[2],
                        TargetType = isProgram ? TargetTypes.PROGRAM : TargetTypes.PIPELINE,
                        Content = File.ReadAllBytes(groups[1])
                    };
                    content = putMsg;
                }
                //Distribute program/pipeline to child
                else if (distribute.IsMatch(command))
                {
                    var groups = distribute.Match(command).Groups.Select(a => a.Value).ToList();

                    msgCommand = Command.CONF_DISTRIBUTE;
                    var distributeMsg = new DistributeMessage
                    {
                        File = groups[1],
                        Target = groups[2]
                    };
                    content = distributeMsg;
                }
                //Get status
                else if (getStatus.IsMatch(command))
                {
                    var groups = getStatus.Match(command).Groups.Select(a => a.Value).ToList();

                    msgCommand = Command.CONF_GET_STATUS;
                    var statusTarget = StatusTarget.All;
                    switch (groups[1].First().ToString().ToUpper())
                    {
                        case "A":
                            statusTarget = StatusTarget.All;
                            break;
                        case "N":
                            statusTarget = StatusTarget.Nodes;
                            break;
                        case "S":
                            statusTarget = StatusTarget.Self;
                            break;
                    }

                    var getStatusMsg = new GetStatusMessage
                    {
                        Target = statusTarget,
                        StatusInfo = new List<Status>()
                    };
                    content = getStatusMsg;
                }

                if (msgCommand != Command.FAIL)
                {
                    var response = NodeCommunicationClient.Send(
                        _configuration.ConfigurationAddress,
                        _configuration.User,
                        _configuration.Password,
                        content,
                        msgCommand,
                        CONFIG);

                    if (response.Message.Command == Command.OK)
                        switch (msgCommand)
                        {
                            case Command.CONF_GET_STATUS:
                                var statusMessage =
                                    MessagePackSerializer.Deserialize<GetStatusMessage>(response.Content);
                                Console.WriteLine();
                                statusMessage.StatusInfo.ForEach(stat =>
                                {
                                    Console.WriteLine($"{stat.Name}");
                                    Console.WriteLine($"{stat.Name} - " + (stat.Up ? "UP" : "DOWN") + "\n",
                                        stat.Up ? Log.Green : Log.Red);
                                });
                                break;
                        }
                    else
                        Console.WriteLine(
                            $"Execution at node failed!:\n{MessagePackSerializer.Deserialize<SingleValueMessage>(response.Content).Value}",
                            Log.Red);
                }
                else
                {
                    Console.WriteLine($"Unknown command: {command}!", Log.Red);
                }
            }
        }
    }
}