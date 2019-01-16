using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class EmptyMessage : IMessage
    {
        
    }
}