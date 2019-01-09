namespace Olymp.Communication
{
    public struct Message
    {
        public string User;
        public Command Command;
        public byte[] Content;
    }
    
    public enum Command
    {
        REQ,
        OK,
        FAIL,
        CONF_ADD_USER,
        CONF_PUT_PIPELINE,
        CONF_PUT_PROGRAM,
    }
}