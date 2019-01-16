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
        REQ = 0x0,
        OK = 0x1,
        FAIL = 0x2,
        UNKNOWN = 0x03,

        //CONF
        CONF_ADD_USER = 0x10,
        CONF_PUT_PIPELINE = 0x11,
        CONF_PUT_PROGRAM = 0x12,
        CONF_DISTRIBUTE = 0x13,
        CONF_GET_STATUS = 0x14,
    }
}