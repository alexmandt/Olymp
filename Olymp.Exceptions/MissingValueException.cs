using System;

namespace Olymp.Exceptions
{
    public class MissingValueException : Exception
    {
        public MissingValueException(string value) : base($"Expected value for {value}!"){}
    }
}