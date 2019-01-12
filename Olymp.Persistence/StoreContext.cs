using Microsoft.EntityFrameworkCore;
using Olymp.Communication.Messages;

namespace Olymp.Persistence
{
    public partial class StoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PutMessage> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=LocalUserStore.db");
        }
    }
}