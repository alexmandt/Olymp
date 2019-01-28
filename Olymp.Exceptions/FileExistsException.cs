using System;

namespace Olymp.Exceptions
{
    public class FileExistsException : Exception
    {
        public FileExistsException(string file) : base($"File {file} already exists in the database!")
        {
        }
    }
}