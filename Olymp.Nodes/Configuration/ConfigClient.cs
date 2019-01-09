using System;
using System.Linq;
using Newtonsoft.Json;
using Olymp.Communication;
using Olymp.Communication.Messages;

namespace Olymp.Nodes.Configuration
{
    /// <summary>
    /// add user username password isAdmin
    /// </summary>
    public class ConfigClient
    {
        private readonly Util.Configuration _configuration;
        private readonly string CONFIG = "CONFIG";
        
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

                var groups = command.Split(" ").Select(a => a.Trim()).ToList();       
                switch (groups[0])
                {
                    case "add":
                        switch (groups[1])
                        {
                            case "user":
                                var addUser = new AddUserMessage
                                {
                                    IsAdmin = bool.Parse(groups[4]),
                                    Username = groups[2],
                                    Password = groups[3]
                                };
                                var result = NodeCommunicationClient.Send(_configuration.ConfigurationAddress,
                                    _configuration.User,
                                    _configuration.Password,
                                    JsonConvert.SerializeObject(addUser),
                                    Command.CONF_ADD_USER,
                                    CONFIG);
                                if (result.message.command == Command.OK)
                                {
                                    Console.WriteLine("Succes!");
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }
}