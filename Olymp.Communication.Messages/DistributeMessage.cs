using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class DistributeMessage : IMessage
    {
        public string File { get; set; }
        public string Target { get; set; }
    }
}