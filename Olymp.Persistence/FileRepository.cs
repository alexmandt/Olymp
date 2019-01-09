using System;
using System.IO;
using System.Linq;
using Olymp.Communication.Messages;
using Olymp.Exceptions;

namespace Olymp.Persistence
{
    public class FileRepository
    {
        private static readonly Lazy<FileRepository> Lazy = new Lazy<FileRepository>(()=> new FileRepository());
        public static FileRepository Instance => Lazy.Value;

        private readonly StoreContext _db;

        private FileRepository()
        {
            _db = new StoreContext();
        }
        
        public void AddFile(PutMessage file)
        {
            if (_db.Files.Any(a => a.TargetName == file.TargetName))
            {
                throw new FileExistsException(file.TargetName);
            }

            _db.Files.Add(file);
            _db.SaveChanges();
        }
    }
}