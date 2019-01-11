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
        //GENERAL COMMANDS
        REQ,
        OK,
        FAIL,

        //CONF
        CONF_ADD_USER,
        CONF_PUT_PIPELINE,
        CONF_PUT_PROGRAM,
        CONF_DISTRIBUTE,
        CONF_GET_STATUS
    }
}