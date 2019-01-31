using MessagePack;
using Olymp.Util;

namespace Olymp.Communication.Messages
{
    [MessagePackObject(true)]
    public class SingleValueMessage : IMessage
    {
        public string Value
        {
            get => Value;
            set
            {
                if (Validator.ValidateStringValues(value))
                    Value = value;
            }
        }
    }
}