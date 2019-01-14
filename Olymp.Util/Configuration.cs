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
        public string MasterIP;
        public bool WebUI;
        public Role? Role { get; set; } = null;
        public string Name { get; set; }
        public string ConfigurationAddress { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}