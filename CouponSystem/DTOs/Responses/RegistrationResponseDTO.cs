namespace CouponSystem.DTOs.Responses
{
    public class RegistrationResponseDTO
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
