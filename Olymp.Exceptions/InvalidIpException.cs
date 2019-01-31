using System;

namespace Olymp.Exceptions
{
    public class InvalidIpOrHostnameException : Exception
    {
        public InvalidIpOrHostnameException(string ip) : base($"IP address format invalid: {ip}!")
        {
        }
    }
}