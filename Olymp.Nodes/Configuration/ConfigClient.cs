using System;
using Olymp.Communication;

namespace Olymp.Nodes.Configuration
{
    public class ConfigClient
    {
        private Util.Configuration _configuration;
        
        public ConfigClient(Util.Configuration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            var name = NodeCommunicationClient.Send(_configuration.ConfigurationAddress, _configuration.User, _configuration.Password, "CONFIG", Command.REQ, "CONFIG");
            
            while (true)
            {
                Console.Write($"{name.content}>");
                var command = Console.ReadLine();
            }
        }
    }
}