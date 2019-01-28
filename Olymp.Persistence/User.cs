using Microsoft.EntityFrameworkCore;

namespace Olymp.Persistence
{
    public partial class StoreContext : DbContext
    {
        public class User
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int Level { get; set; }
        }
    }
}