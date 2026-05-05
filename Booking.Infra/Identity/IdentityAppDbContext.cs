using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Entities.Identity;

namespace Restaurant.Infra.Identity
{
    public class IdentityAppDbContext : IdentityDbContext
    {
        public IdentityAppDbContext(DbContextOptions options) :
            base(options)
        {

        }
        public DbSet<AppUser> Users { get; set; }
    }
}
