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

    public class TargetTypes
    {
        public static readonly string PIPELINE = "pi";
        public static readonly string PROGRAM = "pr";
    }
}