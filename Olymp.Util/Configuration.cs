namespace Olymp.Util
{
    public enum Role
    {
        Master,
        Child,
        ConfigClient
    }

    public class Configuration
    {
        public bool WebUI;
        public string MasterIP;
        public Role? Role { get; set; } = null;
        public string Name { get; set; }
        public string ConfigurationAddress { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}