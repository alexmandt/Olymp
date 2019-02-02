using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class SetUserLevelMessage : IMessage
    {
        public int NewLevel;
        public string User;
    }
}