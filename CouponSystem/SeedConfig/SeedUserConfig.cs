using CouponSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponSystem.SeedConfig
{
    public class SeedUserConfig : IEntityTypeConfiguration<User>
    {
        // Seeding the Admin User to AspNetUsers table
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Store the admin user in variable
            var adminUser = new User
            {
                Id = "bf3b9da8-7434-4409-aac3-502d18baf972", // primary key
                UserName = "admin@company.com",
                NormalizedUserName = "ADMIN@COMPANY.COM",
                Email = "admin@company.com",
                NormalizedEmail = "ADMIN@COMPANY.COM",
                LockoutEnabled = false
            };

            // Set the admin's user password
            var hasher = new PasswordHasher<User>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "abcd1234");

            // Seeding the user data
            builder.HasData(adminUser);
        }
    }
}
