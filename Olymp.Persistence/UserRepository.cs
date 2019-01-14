using System;
using System.Linq;
using Olymp.Communication.Messages;
using Olymp.Exceptions;
using Olymp.Util;

namespace Olymp.Persistence
{
    public class UserRepository
    {
        private static readonly Lazy<UserRepository> Lazy = new Lazy<UserRepository>(() => new UserRepository());

        private readonly StoreContext _db;

        private UserRepository()
        {
            _db = new StoreContext();

            // Insert an admin user on startup
            try
            {
                GetUser("admin");
            }
            catch (Exception)
            {
                AddUser(new AddUserMessage {IsAdmin = true, Username = "admin", Password = "admin"});
            }
        }

        public static UserRepository Instance => Lazy.Value;

        public void AddUser(AddUserMessage newUser)
        {
            var user = ConvertToUser(newUser);
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public StoreContext.User GetUser(string username)
        {
            return _db.Users
                       .FirstOrDefault(a => a.UserName == username) ??
                   throw new UnknownUserException(username);
        }

        private StoreContext.User ConvertToUser(AddUserMessage newUserMessage)
        {
            return new StoreContext.User
            {
                Id = Guid.NewGuid().ToString(),
                IsAdmin = newUserMessage.IsAdmin,
                Password = MD5Helper.CalculateMD5Hash(newUserMessage.Password),
                UserName = newUserMessage.Username
            };
        }
    }
}