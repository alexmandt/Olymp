namespace Olymp.Communication
{
    public struct Message
    {
        public string user;
        public Command command;
        public byte[] content;
    }
    
    public enum Command
    {
        REQ,
        OK,
        ADD
    }
}