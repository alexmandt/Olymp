using Microsoft.EntityFrameworkCore;

namespace Olymp.Persistence
{
    public class StoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=LocalUserStore.db");
        }
        
        public class User
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
        }
    }
}