namespace CouponSystem.DTOs.Coupons
{
    public class UseCouponDTO
    {
        public bool IsSuccess { get; set; }
        public List<string>? Errors { get; set; }
        public decimal? Price { get; set; } // price after discount
        public bool? CombinedDiscounts { get; set; } // is the coupon used allow combined discounts or not
        public int? CouponsCount { get; set; } // how many coupons used by the current user this far
    }
}
