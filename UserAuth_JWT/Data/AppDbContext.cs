using Microsoft.EntityFrameworkCore;
using UserAuth_JWT.Models;

namespace UserAuth_JWT.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
