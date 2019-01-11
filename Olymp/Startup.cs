using System;
using Olymp.Communication;
using Olymp.Nodes.Configuration;
using Olymp.Nodes.Master;
using Olymp.Util;

namespace Olymp
{
    public class Startup
    {
        private readonly Configuration _configuration;

        public Startup(string[] args)
        {
            this._configuration = ConfigurationManager.GetConfiguration(args);
        }

        public void Start()
        {
            switch (this._configuration.Role)
            {
                case Role.Master:
                    new MasterNode(this._configuration).Start();
                    break;
                case Role.ConfigClient:
                    new ConfigClient(this._configuration).Start();
                    break;
            }

            while (true){}
        }
    }
}