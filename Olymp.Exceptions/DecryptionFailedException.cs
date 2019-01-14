using System;

namespace Olymp.Exceptions
{
    public class DecryptionFailedException : Exception
    {
        public DecryptionFailedException() : base("Decryption of message failed")
        {
        }
    }
}