namespace Olymp.Communication.Messages
{
    public class AddUserMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}