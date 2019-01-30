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
        // GENERAL COMMANDS
        REQ = 0x00,
        OK = 0x01,
        FAIL = 0x02,
        UNKNOWN = 0x03,

        // CONFIGURATION RELATED COMMANDS
        CONF_ADD_USER = 0x10,
        CONF_SET_USER_LEVEL = 0x11,
        CONF_REMOVE_USER = 0x12,
        CONF_PUT_PIPELINE = 0x13,
        CONF_PUT_PROGRAM = 0x14,
        CONF_DISTRIBUTE = 0x15,
        CONF_GET_STATUS = 0x16,

        // MASTER TO CHILD COMMANDS
        MC_PUT_PROGRAM = 0x20,
        MC_GET_STATUS = 0x21,

        // CHILD TO MASTER COMMANDS
        CM_REPORT_STATUS = 0x30,
        CM_REPORT_RESULT = 0x31
    }
}