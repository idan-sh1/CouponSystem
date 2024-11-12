using CouponSystem.Models;

namespace CouponSystem.DTOs.Responses
{
    public class UpdateResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public User? User { get; set; }
    }
}
