﻿using FoolProof.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace CouponSystem.DTOs.Coupons
{
    public class EditCouponDTO
    {
        public required string Code { get; set; }
        public required string Description { get; set; }

        [RequiredIfEmpty("PercentOff")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? AmountOff { get; set; }

        [RequiredIfEmpty("AmountOff")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? PercentOff { get; set; }

        public bool CombinedDiscounts { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int? MaxUses { get; set; }
    }
}