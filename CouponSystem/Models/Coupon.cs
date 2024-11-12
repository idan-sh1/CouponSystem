using FoolProof.Core;
using System.Text.Json.Serialization;

namespace CouponSystem.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }

        [RequiredIfEmpty("PercentOff")]
        public decimal? AmountOff { get; set; }

        [RequiredIfEmpty("AmountOff")]
        public decimal? PercentOff { get; set; }

        public required string UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool CombinedDiscounts { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int UsesCount { get; set; }
        public int? MaxUses { get; set; }
    }
}
