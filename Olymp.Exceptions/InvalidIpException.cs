using System;

namespace Olymp.Exceptions
{
    public class InvalidIpException : Exception
    {
        public InvalidIpException(string ip) : base($"IP address format invalid: {ip}!")
        {
        }
    }
}