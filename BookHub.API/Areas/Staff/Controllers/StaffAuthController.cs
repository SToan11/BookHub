using BookHub.API.Data;
using BookHub.API.DTOs;
using BookHub.API.Helpers;
using BookHub.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using StaffModel = BookHub.API.Models.Staff;


namespace BookHub.API.Areas.Staff.Controllers
{
    [Route("api/staff-auth")]
    [ApiController]
    public class StaffAuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public StaffAuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(StaffLoginDto dto)
        {
            var staff = await _context.Staffs.FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (staff == null || HashPassword(dto.Password) != staff.PasswordHash)
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");
            }

            var token = GenerateJwtToken(staff);

            var message = staff.Role == "owner"
                ? "Đây là tài khoản owner."
                : "Đây là tài khoản staff.";

            return Ok(new
            {
                token,
                username = staff.Username,
                role = staff.Role,
                message
            });
        }

        private string GenerateJwtToken(StaffModel staff)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, staff.Id.ToString()),
            new Claim(ClaimTypes.Name, staff.Username),
            new Claim(ClaimTypes.Role, staff.Role) // "employee" hoặc "owner"
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpiresInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Không cần xử lý gì nếu không lưu token
            return Ok(new { message = "Đăng xuất thành công. Token đã bị xoá ở phía client." });
        }

    }
}
