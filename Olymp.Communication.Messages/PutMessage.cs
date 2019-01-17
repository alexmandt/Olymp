using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class PutMessage : IMessage
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string TargetName { get; set; }
        public string TargetType { get; set; }
        public byte[] Content { get; set; }
        public string Runtime { get; set; }
    }

    public static class TargetTypes
    {
        public const string PIPELINE = "pipeline";
        public const string PROGRAM = "program";
    }
}