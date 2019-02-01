using System;

namespace Olymp.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string argument) : base($"Argument is either null or empty: {argument}!")
        {
        }
    }
}