using CouponSystem.Data;
using CouponSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
