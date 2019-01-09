using System;

namespace Olymp.Exceptions
{
    public class UnknownArgumentException : Exception
    {
        public UnknownArgumentException(string argument) : base($"Unknown argument: {argument}!"){}
    }
}