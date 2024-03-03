using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;//responsible for db table
using System.Data.SqlClient;//responsible for sqldatareader data type
using realtimefraudsystem.utility;
using Microsoft.AspNetCore.Authorization;
using realtimefraudsystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace realtimefraudsystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly DBContext _dbContext;

        public AdminController(IConfiguration configuration, DBContext dBContext)
        {
            _configuration = configuration;
            _dbContext = dBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _dbContext.Admins.ToListAsync();
        }

        [HttpPost("registerAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult<Admin>> Register(Admin user)
        {
            var admin = new Admin { Email = user.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash) };
            _dbContext.Admins.Add(admin);
            await _dbContext.SaveChangesAsync();

            return Ok("admin created successfully");
        }

    }
}
