using CouponSystem.Data;
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
    }
}
