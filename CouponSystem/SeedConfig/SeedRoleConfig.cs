using CouponSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponSystem.SeedConfig
{
    public class SeedRoleConfig : IEntityTypeConfiguration<Role>
    {
        // Seeding 'Admin' and 'User' roles to AspNetRoles table
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role // admin
                {
                    Id = "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "The owner. Have full permissions."
                },
                new Role // user
                {
                    Id = "639de03f-7876-4fff-96ec-37f8bd3bf180",
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "An employee. Allowed to add new coupons."
                }
            );
        }
    }
}
