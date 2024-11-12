using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CouponSystem.SeedConfig
{
    public class SeedUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        // Seeding 'Admin' and 'User' roles to AspNetRoles table
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "bf3b9da8-7434-4409-aac3-502d18baf972",
                    RoleId = "45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f"
                }
            );
        }
    }
}
