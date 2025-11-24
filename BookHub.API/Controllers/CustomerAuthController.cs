using BookHub.API.Data;
using BookHub.API.DTOs;
using BookHub.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using BookHub.API.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookHub.API.Controllers
{

    [Route("api/customer-auth")]
    [ApiController]
    public class CustomerAuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;


        public CustomerAuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerRegisterDto dto)
        {
            // Kiểm tra mật khẩu mạnh
            if (!IsStrongPassword(dto.Password))
            {
                return BadRequest("Mật khẩu phải có ít nhất 8 ký tự, gồm 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.");
            }

            // Kiểm tra username đã tồn tại chưa
            if (await _context.Customers.AnyAsync(x => x.Username == dto.Username))
            {
                return BadRequest("Username đã tồn tại.");
            }

            if(dto.Password != null)
            {
                return BadRequest("Mật khẩu không được để trống");
            }
            if(dto.Address != null )
            {
                return BadRequest("Địa chỉ không được để trống");
            }

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                PasswordHash = HashPassword(dto.Password),
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok("Đăng ký thành công!");
        }

        // Hàm kiểm tra mật khẩu mạnh
        private bool IsStrongPassword(string password)
        {
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
            return regex.IsMatch(password);
        }

        // Hàm hash mật khẩu bằng SHA256
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerLoginDto dto)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null || HashPassword(dto.Password) != user.PasswordHash)
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                username = user.Username,
                fullName = user.FullName
            });
        }

        private string GenerateJwtToken(Customer user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, "Customer") // Gắn role nếu cần phân quyền
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


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Không cần xử lý gì nếu không lưu token
            return Ok(new { message = "Đăng xuất thành công. Token đã bị xoá ở phía client." });
        }

    }
}
