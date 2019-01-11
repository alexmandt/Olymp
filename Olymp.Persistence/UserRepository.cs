using System;
using System.Collections.Generic;
using System.Linq;
using Olymp.Communication.Messages;
using Olymp.Exceptions;
using Olymp.Util;

namespace Olymp.Persistence
{
    public class UserRepository
    {

        private static readonly Lazy<UserRepository> Lazy = new Lazy<UserRepository>(()=> new UserRepository());
        public static UserRepository Instance => Lazy.Value;

        private readonly StoreContext _db;

        private UserRepository()
        {
            _db = new StoreContext();
            
            try
            {
                GetUser("admin");
            }
            catch (Exception)
            {
                AddUser(new AddUserMessage{IsAdmin = true,Password = "admin",Username = "admin"});
            }
        }

        public void AddUser(AddUserMessage newUser)
        {   
            var user = new StoreContext.User
            {
                Id = Guid.NewGuid().ToString(),
                IsAdmin = newUser.IsAdmin,
                Password = MD5Helper.CalculateMD5Hash(newUser.Password),
                UserName = newUser.Username
            };
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public StoreContext.User GetUser(string username)
        {   
            if (_db.Users.Any(a => a.UserName == username))
            {
                return _db.Users.First(a => a.UserName == username);
            }

            throw new UnknownUserException(username);
        }
    }
}