namespace Olymp.Communication.Messages
{
    public class DistributeMessage : IMessage
    {
        public string File { get; set; }
        public string Target { get; set; }
    }
}