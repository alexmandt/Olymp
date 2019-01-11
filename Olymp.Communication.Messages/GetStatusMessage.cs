using System.Collections.Generic;

namespace Olymp.Communication.Messages
{
    public class GetStatusMessage
    {
        public StatusTarget Target { get; set; }
        public List<Status> StatusInfo { get; set; }
    }

    public class Status
    {
        // TODO: Add more info (workers, resources, etc...)
        public bool Up { get; set; }
        public string Name { get; set; }
    }

    public enum StatusTarget
    {
        // Just first letters needed for parsing partially spelled commands
        S, // Self
        A, // All
        N // Nodes
    }
}