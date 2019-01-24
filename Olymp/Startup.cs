using Olymp.Nodes.Abstractions;
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
            _configuration = ConfigurationManager.GetConfiguration(args);
        }

        public void Start()
        {
            // Initialize an emty service
            IService service;

            // Configure the service to an implementation
            switch (_configuration.Role)
            {
                case Role.Master:
                    service = new MasterNode(_configuration);
                    break;
                // Child node creation goes here
                case Role.ConfigurationTool:
                    service = new ConfigurationTool(_configuration);
                    break;
                default:
                    Log.Error("No role was specified for the node", _configuration.Name);
                    return;
            }

            service.Start();

            // TODO: Refactor to Task.Run();
            while (true)
            {
            }
        }
    }
}