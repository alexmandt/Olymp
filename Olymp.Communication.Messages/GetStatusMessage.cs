using System.Collections.Generic;

namespace Olymp.Communication.Messages
{
    public class GetStatusMessage
    {
        public StatusTarget Target;
        public List<Status> Status;
    }

    public class Status
    {
        //TODO more info (workers, resources etc)
        public bool Up;
        public string Name;
    }
    
    public enum StatusTarget
    {
        //Self
        S,
        //All
        A,
        //Nodes
        N
    }
}