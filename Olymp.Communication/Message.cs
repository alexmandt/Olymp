using MessagePack;

namespace Olymp.Communication
{
    // Will be obsolete once we introduce MessagePack
    [MessagePackObject(true)]
    public struct Message
    {
        public string User { get; set; }
        public Command Command { get; set; }
        public byte[] Content { get; set; }
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