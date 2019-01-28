using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class SetUserLevelMessage : IMessage
    {
        public string User;
        public int NewLevel;
    }
}