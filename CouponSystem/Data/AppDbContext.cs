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

            // ----------------------------------------------------------------- //
                             // Set relationships between entities
            // ----------------------------------------------------------------- //

            // User -> Coupons relationship (one to many)
            builder.Entity<User>()
                .HasMany(u => u.Coupons)             // User has many Coupons
                .WithOne(c => c.User)                 // Coupon has one User
                .HasForeignKey(c => c.UserId)         // Foreign key is UserId in Coupon
                .IsRequired();                     // Can't have Coupon without User
        }

        // ----------------------------------------------------------------- //
                                    // Add DbSets
        // ----------------------------------------------------------------- //

        public DbSet<Coupon> Coupons { get; set; }
    }
}
