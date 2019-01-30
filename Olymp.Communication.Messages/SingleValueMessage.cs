using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class SingleValueMessage : IMessage
    {
        public SingleValueMessage()
        {
        }

        public SingleValueMessage(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}