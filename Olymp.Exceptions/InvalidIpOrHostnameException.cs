using System;

namespace Olymp.Exceptions
{
    public class InvalidIpException : Exception
    {
        public InvalidIpException(string address) : base($"IP address or Hostname format invalid: {address}!")
        {
        }
    }
}