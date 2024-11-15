﻿using Microsoft.AspNetCore.Identity;

namespace CouponSystem.Models
{
    public class User : IdentityUser
    {
        public bool IsEnabled { get; set; } = true;
        public ICollection<Coupon>? Coupons { get; set; }
    }
}
