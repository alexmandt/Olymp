using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Olymp.Communication;
using Olymp.Communication.Messages;

namespace Olymp.Nodes.Configuration
{
    public class ConfigClient
    {
        private readonly Util.Configuration _configuration;
        private readonly string CONFIG = "CONFIG";
        
        private readonly Regex addUser = new Regex(" *ad?d? +us?e?r? +(.+) +(.+) +(tr?u?e?|fa?l?s?e?)",RegexOptions.Compiled);
        private readonly Regex putProgram = new Regex(" *pu?t? +pro?g?r?a?m? +\"(.+)\" +as? +\"(.+)\"", RegexOptions.Compiled);
        private readonly Regex putPipeline = new Regex(" *pu?t? +pip?e?l?i?n?e? +\"(.+)\" +as? +\"(.+)\"", RegexOptions.Compiled);
        
        public ConfigClient(Util.Configuration configuration)
        {
            _configuration = configuration;
        }
        
        public void Start()
        {
            var name = NodeCommunicationClient.Send(_configuration.ConfigurationAddress, 
                _configuration.User,
                _configuration.Password, 
                CONFIG, 
                Command.REQ, 
                CONFIG);
            
            while (true)
            {
                Console.Write($"{name.content}>");
                var command = Console.ReadLine();

                var msgCommand = Command.FAIL;
                object content = null;
                
                //Add user
                if (addUser.IsMatch(command))
                {
                    var groups = addUser.Match(command).Groups.Select(a => a.Value).ToList();
                    var addUserMsg = new AddUserMessage
                    {
                        IsAdmin = bool.Parse(groups[3]),
                        Username = groups[1],
                        Password = groups[2]
                    };
                    content = addUserMsg;
                }
                //Upload file
                else if(putProgram.IsMatch(command) || putPipeline.IsMatch(command))
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
                        Content = Util.FileHelper.ReadHexString(groups[1])
                    };
                    content = putMsg;
                }
                
                var result = NodeCommunicationClient.Send(_configuration.ConfigurationAddress,
                    _configuration.User,
                    _configuration.Password,
                    JsonConvert.SerializeObject(content),
                    msgCommand,
                    CONFIG);
                if (result.message.Command == Command.OK)
                {
                    Console.WriteLine("Success!");
                }
            }
        }
    }
}