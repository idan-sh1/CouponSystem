using Microsoft.AspNetCore.Identity;

namespace CouponSystem.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
