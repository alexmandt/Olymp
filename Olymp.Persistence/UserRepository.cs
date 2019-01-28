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
                AddUser(new AddUserMessage {Level = 9, Username = "admin", Password = "admin"});
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
            return _db.Users.FirstOrDefault(a => a.UserName == username) ?? throw new UnknownUserException(username);
        }

        public void UpdateUser(string name, StoreContext.User user)
        {
            var dbUser = GetUser(name);
            dbUser.Level = user.Level;
            dbUser.UserName = user.UserName;
            dbUser.Password = user.Password;
            _db.SaveChanges();
        }

        public void RemoveUser(string name)
        {
            _db.Users.Remove(GetUser(name));
            _db.SaveChanges();
        }
        
        private StoreContext.User ConvertToUser(AddUserMessage newUserMessage)
        {
            return new StoreContext.User
            {
                Id = Guid.NewGuid().ToString(),
                Level = newUserMessage.Level,
                Password = MD5Helper.CalculateMD5Hash(newUserMessage.Password),
                UserName = newUserMessage.Username
            };
        }
    }
}