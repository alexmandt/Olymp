using System;
using Olymp.Communication;
using Olymp.Nodes.Configuration;
using Olymp.Nodes.Master;
using Olymp.Util;

namespace Olymp
{
    public class Startup
    {
        private readonly string[] _args;
        private Configuration _configuration;
        
        public Startup(string[] args)
        {
            _args = args;
        }

        public void Configure()
        {
            _configuration = ConfigurationManager.GetConfiguration(_args);
        }

        public void Start()
        {
            switch (_configuration.Role)
            {
                case Role.Master:
                    new MasterNode(_configuration).Start();
                    break;
                case Role.ConfigClient:
                    new ConfigClient(_configuration).Start();
                    break;
            }

            while (true){}
        }
    }
}