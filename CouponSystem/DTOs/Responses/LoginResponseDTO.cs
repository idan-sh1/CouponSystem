namespace CouponSystem.DTOs.Responses
{
    public class LoginResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
    }
}
