using CouponSystem.Models;
using CouponSystem.SeedConfig;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CouponSystem.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ----------------------------------------------------------------- //
                                    // Seed Configuration
            // ----------------------------------------------------------------- //

            // Seeding 'Admin' and 'User' roles to AspNetRoles table
            builder.ApplyConfiguration(new SeedRoleConfig());

            // Seeding the Admin User to AspNetUsers table
            builder.ApplyConfiguration(new SeedUserConfig());

            // Seeding the User-Role relation to AspNetUserRoles table
            builder.ApplyConfiguration(new SeedUserRoleConfig());
        }
    }
}
