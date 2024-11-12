using CouponSystem.Data;
using CouponSystem.DTOs.Responses;
using CouponSystem.DTOs.Users;
using CouponSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CouponSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;

        public UsersController(UserManager<User> userManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        // ----------------------------------------------------------------- //
                           // Add New User With "User" Role
                              // (requires "Admin" role)
        // ----------------------------------------------------------------- //

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userDTO)
        {
            // If userDTO is empty --> Return 400 Bad Request
            if (userDTO == null)
            {
                return BadRequest(new { Error = "Invalid user data." });
            }

            // Copy data from DTO to User
            var user = new User
            {
                UserName = userDTO.Email,
                Email = userDTO.Email,
            };

            // Register user
            var result = await _userManager.CreateAsync(user, userDTO.Password!);

            // Check registration results
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                // 400 Bad Request if registration faild
                return BadRequest(new RegistrationResponseDTO { Errors = errors });
            }

            // Assign role to the new user
            await _userManager.AddToRoleAsync(user, "User");

            // 201 Created with the user in the response body
            return StatusCode(201, user);
        }

        // ----------------------------------------------------------------- //
                                    // User Login
        // ----------------------------------------------------------------- //

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userDTO)
        {
            // Find user by email address
            var user = await _userManager.FindByNameAsync(userDTO.Email!);

            // Check if user not exist or password not match or user is disabled
            if (user == null || !await _userManager.CheckPasswordAsync(user, userDTO.Password!) || !user.IsEnabled)
            {
                // 401 Unauthorized if login faild
                return Unauthorized(new LoginResponseDTO { Error = "The username or passoword are incorrect." });
            }

            // Create token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHandler.CreateToken(user, roles);

            // 200 Ok with the token in the response body
            return Ok(new LoginResponseDTO { IsSuccess = true, Token = token });
        }
    }
}
