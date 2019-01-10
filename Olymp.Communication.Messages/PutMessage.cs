using System.ComponentModel.DataAnnotations;

namespace Olymp.Communication.Messages
{
    public class PutMessage
    {
        [Key]
        public string TargetName { get; set; }
        public string TargetType { get; set; }
        public string Content { get; set; }
    }

    public static class TargetTypes
    {
        public const string PIPELINE = "pipeline";
        public const string PROGRAM = "program";
    }
}