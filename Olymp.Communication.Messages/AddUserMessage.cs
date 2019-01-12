using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class AddUserMessage : IMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}