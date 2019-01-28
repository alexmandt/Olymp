using System;
using System.Linq;
using Olymp.Communication.Messages;
using Olymp.Exceptions;

namespace Olymp.Persistence
{
    public class FileRepository
    {
        private static readonly Lazy<FileRepository> Lazy = new Lazy<FileRepository>(() => new FileRepository());

        private readonly StoreContext _db;

        private FileRepository()
        {
            _db = new StoreContext();
        }

        public static FileRepository Instance => Lazy.Value;

        public void AddFile(PutMessage file)
        {
            if (_db.Files.Any(a => a.TargetName == file.TargetName)) throw new FileExistsException(file.TargetName);

            _db.Files.Add(file);
            _db.SaveChanges();
        }
    }
}