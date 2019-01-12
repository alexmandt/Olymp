using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class SingleValueMessage : IMessage
    {
        public string Value { get; set; }
    }
}