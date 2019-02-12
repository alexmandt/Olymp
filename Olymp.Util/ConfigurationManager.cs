using System.Text.RegularExpressions;
using Olymp.Exceptions;

namespace Olymp.Util
{
    public class ConfigurationManager
    {
        private static string GetValue(string[] args, ref int i)
        {
            if (i + 1 < args.Length)
            {
                i++;
                return args[i].Trim();
            }

            throw new MissingValueException(args[i]);
        }

        public static Configuration GetConfiguration(string[] args)
        {
            var config = new Configuration();

            for (var i = 0; i < args.Length; i++)
                switch (args[i])
                {
                    case "--master":
                    case "-m":
                        config.Role = Role.Master;
                        break;
                    case "--child":
                    case "-c":
                        config.Role = Role.Child;
                        config.MasterIP = GetValue(args, ref i);
                        Validator.ValidateAddress(config.MasterIP);
                        break;
                    case "--configure":
                    case "--conf":
                        config.Role = Role.ConfigurationTool;
                        config.ConfigurationToolAddress = GetValue(args, ref i);
                        Validator.ValidateAddress(config.ConfigurationToolAddress);
                        break;
                    case "--address":
                    case "-a":
                        config.Address = GetValue(args, ref i);
                        Validator.ValidateAddress(config.Address);
                        break;
                    case "--webui":
                    case "--web":
                    case "-w":
                        config.WebUI = true;
                        break;
                    case "--name":
                    case "-n":
                        config.Name = GetValue(args, ref i);
                        break;
                    case "--user":
                    case "-u":
                        config.User = GetValue(args, ref i);
                        break;
                    case "--password":
                    case "-p":
                        config.Password = GetValue(args, ref i);
                        break;
                    default:
                        throw new UnknownArgumentException(args[i]);
                }

            return config;
        }
    }
}