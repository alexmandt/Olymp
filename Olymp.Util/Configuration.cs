namespace Olymp.Util
{
    public enum Role
    {
        Master,
        Child,
        ConfigurationTool
    }

    public class Configuration
    {
        public string MasterIP
        {
            get => MasterIP;
            internal set
            {
                if (Validator.ValidateAddress(value))
                    MasterIP = value;
            }
        }

        public bool WebUI { get; internal set; }
        public Role? Role { get; internal set; } = null;
        public string Name { get; internal set; }
        public string ConfigurationToolAddress { get; internal set; }
        public string Address { get; internal set; } = null;
        public string User { get; internal set; }
        public string Password { get; internal set; }
    }
}