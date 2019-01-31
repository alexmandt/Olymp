using System;

namespace Olymp.Exceptions
{
    public class InvalidIpOrHostnameException : Exception
    {
        public InvalidIpOrHostnameException(string address) : base($"IP address or Hostname format invalid: {address}!")
        {
        }
    }
}