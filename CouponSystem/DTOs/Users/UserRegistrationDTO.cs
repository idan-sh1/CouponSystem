using System.ComponentModel.DataAnnotations;

namespace CouponSystem.DTOs.Users
{
    public class UserRegistrationDTO
    {
        // Email Address
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        public string? Email { get; set; }

        // Password
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        // Confirm Password
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
