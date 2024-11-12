using FoolProof.Core;

namespace CouponSystem.DTOs.Coupons
{
    public class AddCouponDTO
    {
        public required string Code { get; set; }
        public required string Description { get; set; }

        [RequiredIfEmpty("PercentOff")]
        public decimal? AmountOff { get; set; }

        [RequiredIfEmpty("AmountOff")]
        public decimal? PercentOff { get; set; }

        public required string UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool CombinedDiscounts { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? MaxUses { get; set; }
    }
}
