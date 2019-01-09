using System;

namespace Olymp.Exceptions
{
    public class UnknownUserException : Exception
    {
        public UnknownUserException(string user) : base($"Unknown user: {user}!"){}
    }
}