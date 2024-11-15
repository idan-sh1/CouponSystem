﻿using CouponSystem.Data;
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

        // ----------------------------------------------------------------- //
                       // Disable/Enable User By Email Address
                             // (requires "Admin" role)
        // ----------------------------------------------------------------- //

        [HttpPut("{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DisableOrEnableUser(string email)
        {
            // Store the user and its roles in variables
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user!);

            if (user == null)
            {
                // 404 Not Found if the user does not exist
                return NotFound(new UpdateResponseDTO { Message = "The user does not exist." });
            }
            else if (roles.Contains("Admin"))
            {
                // 400 Bad Request if trying to disable admin user
                return BadRequest(new UpdateResponseDTO { Message = "Can't disable admin user." });
            }

            // Disable the user if he was enabled, and vice versa
            user.IsEnabled = !user.IsEnabled;
            await _userManager.UpdateAsync(user);

            // Set message to the response
            var message = "User is now ";
            message += user.IsEnabled ? "Enabled." : "Disabled.";

            // 200 OK with the user and the message in the response body
            return Ok(new UpdateResponseDTO { IsSuccess = true, Message = message, User = user });
        }

        // ----------------------------------------------------------------- //
                                 // Display All Users
                              // (requires "Admin" role)
        // ----------------------------------------------------------------- //

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            // Get list of all users and store only the UserName, IsEnabled and Roles to a new list
            var users = _userManager.Users.ToList().Select(u => new { u.UserName, u.IsEnabled, Roles = _userManager.GetRolesAsync(u).Result.ToArray() });

            // 200 OK with the new list of users
            return Ok(users);
        }
    }
}
