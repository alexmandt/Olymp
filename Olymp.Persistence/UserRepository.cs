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
            this._db = new StoreContext();

            // Insert an admin user on startup
            try
            {
                this.GetUser("admin");
            }
            catch (Exception)
            {
                this.AddUser(new AddUserMessage { IsAdmin = true, Username = "admin", Password = "admin" });
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
            this._db.Users.Add(user);
            this._db.SaveChanges();
        }

        public StoreContext.User GetUser(string username) => this._db.Users
            .FirstOrDefault(a => a.UserName == username) ??
                throw new UnknownUserException(username);
    }
}