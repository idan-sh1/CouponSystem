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
                                // Use Coupons By Code
        // ----------------------------------------------------------------- //

        [HttpPut("{code}")]
        public IActionResult UseCouponByCode(string code, decimal price = 100, int couponsCount = 0)
        {
            // Find the coupon by its code
            var coupon = _dbContext.Coupons.Where(c => c.Code == code).FirstOrDefault();

            // If coupon doesn't exist --> Return 404 Not Found
            if (coupon == null)
            {
                return NotFound(new { Error = "The coupon does not exist." });
            }

            // Add errors list
            var errors = new List<string>();

            // Check if the coupon have usage limit (and if it has been reached)
            if (coupon.MaxUses.HasValue &&
                coupon.MaxUses.Value <= coupon.UsesCount)
            {
                // Add error
                errors.Add("Coupon usage limit has been reached.");
            }

            // Check if the coupon have expiration date (and if it has been expired)
            if (coupon.ExpiresAt.HasValue &&
                coupon.ExpiresAt.Value < DateTime.UtcNow)
            {
                // Add error
                errors.Add("Coupon has been expired.");
            }

            // Check if there are coupons already used by the current user
            // (and if the current coupon not allow combined discounts)
            if (couponsCount > 0 && !coupon.CombinedDiscounts)
            { 
                // Add error
                errors.Add("This coupon not allow combined discounts.");
            }

            // Check if the coupon have either amount / perecent off value
            // (if yes calculate price, otherwise add error)
            if (coupon.AmountOff.HasValue && coupon.AmountOff.Value > 0)
            {
                // Calculate new price
                price -= coupon.AmountOff.Value;
            }
            else if (coupon.PercentOff.HasValue && coupon.PercentOff.Value > 0)
            {
                // Calculate new price
                price -= price * coupon.PercentOff.Value / 100;
            }
            else
            {
                // Add error
                errors.Add("Coupon is invaild.");
            }

            // If there are errors --> Return 400 Bad Request
            if (errors.Count > 0)
            {
                return BadRequest(new { Errors = errors });
            }

            // Incerase the coupon's uses count by 1
            coupon.UsesCount++;

            // Increase the coupon's count of the current user by 1
            couponsCount++;

            // Update coupon and save changes to db
            _dbContext.Coupons.Update(coupon);
            _dbContext.SaveChanges();

            // 200 OK with all relevant data in the response body
            return Ok(new UseCouponDTO { IsSuccess = true, Price = price,
                CombinedDiscounts = coupon.CombinedDiscounts, CouponsCount = couponsCount});
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
