using CouponSystem.Data;
using CouponSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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

        // ----------------------------------------------------------------- //
                      // Display Coupons Created Between 2 Dates
        // ----------------------------------------------------------------- //

        [HttpGet]
        public IActionResult GetCouponsByDate(DateTime? startDate, DateTime? endDate)
        {
            // If startDate is null, sets it to minimum value
            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            }

            // If endDate is null, sets it to maximum value
            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }

            // Store the coupons created within specified date range
            var coupons = _dbContext.Coupons.Where(c => c.CreatedAt >= startDate).Where(c => c.CreatedAt <= endDate).ToList();

            // 200 OK with the coupons in the response body
            return Ok(coupons);
        }

        // ----------------------------------------------------------------- //
                        // Export All Coupons To Excel File 
        // ----------------------------------------------------------------- //

        [HttpGet("export")]
        public FileResult ExportAllToExcel()
        {
            // Create excel file from the coupons list
            var file = ExcelHelper.CreateFile(_dbContext.Coupons.ToList());

            // Return the file
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "all-coupons.xlsx");
        }

        // ----------------------------------------------------------------- //
                    // Export Filtered Coupons Data To Excel File 
        // ----------------------------------------------------------------- //

        [HttpGet("data:{couponsData}")]
        public ActionResult ExportFilteredCoToExcel(string couponsData)
        {
            // Convert the string into coupons list
            List<Coupon> coupons = JsonConvert.DeserializeObject<List<Coupon>>(couponsData)!;

            // If coupons data is null or empty --> Return 404 Not Found
            if (coupons.IsNullOrEmpty())
            {
                return NotFound(new { Error = "Coupon Data was not found." });
            }

            // Create excel file from the coupons list
            var file = ExcelHelper.CreateFile(coupons);

            // Return the file
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "filtered-coupons.xlsx");
        }
    }
}
