﻿using CouponSystem.Data;
using CouponSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CouponSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // requires "Admin" role
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ReportsController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // ----------------------------------------------------------------- //
                          // Display Coupons Created By User
        // ----------------------------------------------------------------- //

        [HttpGet("{email}")]
        public IActionResult GetCouponsByUser(string email)
        {
            // Find the user by its email address
            var user = _userManager.Users.Where(u => u.Email == email).FirstOrDefault();

            // If user doesn't exist --> Return 404 Not Found
            if (user == null)
            {
                return NotFound(new { Error = "The user does not exist." });
            }

            // Store the coupons created by the user in list
            var coupons = _dbContext.Coupons.Where(c => c.UserId == user.Id).ToList();

            // 200 OK with the coupons in the response body
            return Ok(coupons);
        }
    }
}
