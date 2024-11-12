using CouponSystem.Data;
using CouponSystem.DTOs.Coupons;
using CouponSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CouponSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public CouponsController(AppDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        // ----------------------------------------------------------------- //
                                 // Add New Coupon
                           // (requires being logged in)
        // ----------------------------------------------------------------- //

        [HttpPost]
        [Authorize]
        public IActionResult AddCoupon([FromBody] AddCouponDTO couponDTO)
        {
            // If couponDTO is empty --> Return 400 Bad Request
            if (couponDTO == null)
            {
                return BadRequest(new { Error = "Invalid coupon data." });
            }

            // Add errors list
            var errors = new List<string>();

            // Create regext to check the coupon code contains both letters and numbers and at leeast 6 characters
            Regex r = new Regex(@"^(?=.*?\d)(?=.*?[a-zA-Z])[a-zA-Z\d]{6,}$");

            if (!r.IsMatch(couponDTO.Code))
            {
                // Add error
                errors.Add("Coupon code must have at least 6 characters, contain both letters and numbers.");
            }

            // Check if coupon have neither amount or precent off values
            if ((!couponDTO.AmountOff.HasValue || couponDTO.AmountOff.Value == 0) &&
                (!couponDTO.PercentOff.HasValue || couponDTO.PercentOff.Value == 0))
            {
                // Add error
                errors.Add("Coupon code must have either amount or percent value greater than 0.");
            }

            // If there are errors --> Return 400 Bad Request
            if (errors.Count > 0)
            {
                return BadRequest(new { Errors = errors });
            }

            // Copy data from DTO to Coupon
            var coupon = new Coupon
            {
                Code = couponDTO.Code,
                Description = couponDTO.Description,
                AmountOff = couponDTO.AmountOff,
                PercentOff = couponDTO.PercentOff,
                UserId = couponDTO.UserId,
                CreatedAt = couponDTO.CreatedAt,
                CombinedDiscounts = couponDTO.CombinedDiscounts,
                ExpiresAt = couponDTO.ExpiresAt,
                MaxUses = couponDTO.MaxUses
            };

            // Save changes to db
            _dbContext.Coupons.Add(coupon);
            _dbContext.SaveChanges();

            // 201 Created with the coupon in the response body
            return StatusCode(201, coupon);
        }

        // ----------------------------------------------------------------- //
                                   // Edit Coupon
                             // (requires "Admin" role)
        // ----------------------------------------------------------------- //

        [HttpPut("id:{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditCoupon([FromBody] EditCouponDTO couponDTO, int id)
        {
            // If couponDTO is empty --> Return 400 Bad Request
            if (couponDTO == null)
            {
                return BadRequest(new { Error = "Invalid coupon data." });
            }

            // Find the coupon by Id
            var coupon = _dbContext.Coupons.Where(c => c.Id == id).FirstOrDefault();

            // If coupon doesn't exist --> Return 404 Not Found
            if (coupon == null)
            {
                return NotFound(new { Error = "The coupon does not exist." });
            }

            // Add errors list
            var errors = new List<string>();

            // Create regext to check the coupon code contains both letters and numbers and at leeast 6 characters
            Regex r = new Regex(@"^(?=.*?\d)(?=.*?[a-zA-Z])[a-zA-Z\d]{6,}$");

            if (!r.IsMatch(couponDTO.Code))
            {
                // Add error
                errors.Add("Coupon code must have at least 6 characters, contain both letters and numbers.");
            }

            // Check if coupon have neither amount or precent off values
            if ((!couponDTO.AmountOff.HasValue || couponDTO.AmountOff.Value == 0) &&
                (!couponDTO.PercentOff.HasValue || couponDTO.PercentOff.Value == 0))
            {
                // Add error
                errors.Add("Coupon code must have either amount or percent value greater than 0.");
            }

            // If there are errors --> Return 400 Bad Request
            if (errors.Count > 0)
            {
                return BadRequest(new { Errors = errors });
            }

            // Copy data from DTO to Coupon
            coupon.Code = couponDTO.Code;
            coupon.Description = couponDTO.Description;
            coupon.AmountOff = couponDTO.AmountOff;
            coupon.PercentOff = couponDTO.PercentOff;
            coupon.CombinedDiscounts = couponDTO.CombinedDiscounts;
            coupon.ExpiresAt = couponDTO.ExpiresAt;
            coupon.MaxUses = couponDTO.MaxUses;

            // Update coupon and save changes to db
            _dbContext.Coupons.Update(coupon);
            _dbContext.SaveChanges();

            // 200 OK with the coupon in the response body
            return Ok(coupon);
        }

        // ----------------------------------------------------------------- //
                                  // Delete Coupon
                             // (requires "Admin" role)
        // ----------------------------------------------------------------- //

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCoupon(int id)
        {
            // Find coupon by id
            var coupon = _dbContext.Coupons.Where(c => c.Id == id).FirstOrDefault();

            // If coupon doesn't exist --> Return 404 Not Found
            if (coupon == null)
            {
                return NotFound(new { Error = "The coupon does not exist." });
            }

            // Remove coupon and save changes to db
            _dbContext.Coupons.Remove(coupon);
            _dbContext.SaveChanges();

            // 204 No Content
            return NoContent();
        }

        // ----------------------------------------------------------------- //
                              // Display Coupon By Id
                             // (requires "Admin" role)
        // ----------------------------------------------------------------- //

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetCouponById(int id)
        {
            // Find coupon by id
            var coupon = _dbContext.Coupons.Where(c => c.Id == id).FirstOrDefault();

            // If coupon doesn't exist --> Return 404 Not Found
            if (coupon == null)
            {
                return NotFound(new { Error = "The coupon does not exist." });
            }

            // 200 OK with the coupon in the response body
            return Ok(coupon);
        }
    }
}
