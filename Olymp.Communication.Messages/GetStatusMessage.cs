using System.Collections.Generic;
using MessagePack;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class GetStatusMessage : IMessage
    {
        public StatusTarget Target { get; set; }
        public List<Status> StatusInfo { get; set; }
    }

    [MessagePackObject(true)]
    public class Status
    {
        // TODO: Add more info (workers, resources, etc...)
        public bool Up { get; set; }
        public string Name { get; set; }
    }

    public enum StatusTarget
    {
        Self,
        All,
        Nodes
    }
}