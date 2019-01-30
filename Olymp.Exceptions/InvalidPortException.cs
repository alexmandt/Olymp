using System;

namespace Olymp.Exceptions
{
    public class InvalidPortException : Exception
    {
        public InvalidPortException(string port) : base($"Port number range invalid: {port}!")
        {
        }
    }
}