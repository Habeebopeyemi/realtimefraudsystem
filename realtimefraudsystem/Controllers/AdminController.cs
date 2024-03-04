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
        private readonly DBContext _dbContext;

        public AdminController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login(Admin user)
        {
            var admin = await _dbContext.Admins.SingleOrDefaultAsync(a => a.Email == user.Email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(user.PasswordHash, admin.PasswordHash))
            {
                return Unauthorized();
            }

            var authToken = GenerateJwtToken(admin);
            return Ok(authToken);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _dbContext.Admins.ToListAsync();
        }

        [HttpPost("createAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult<Admin>> Register(Admin user)
        {
            var admin = new Admin { Email = user.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash) };
            _dbContext.Admins.Add(admin);
            await _dbContext.SaveChangesAsync();

            return Ok("admin created successfully");
        }

        private string GenerateJwtToken(Admin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OWLETTEAMSECURITYKEYISINSAFEHANDS"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
                Issuer = "FRAUD_SERVER"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDetails);

            return tokenHandler.WriteToken(token);
        }

    }
}
