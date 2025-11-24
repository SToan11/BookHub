using BookHub.API.Data;
using BookHub.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace BookHub.API.Controllers
{
    [Route("api/customer")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        // Update thông tin người dùng
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateCustomerDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _context.Customers.FindAsync(Guid.Parse(userId!));

            if (customer == null) return NotFound();

            customer.FullName = dto.FullName;
            customer.Email = dto.Email;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật thành công.");
        }

        // Đổi mật khẩu
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _context.Customers.FindAsync(Guid.Parse(userId!));

            if (customer == null) return NotFound();

            if (HashPassword(dto.OldPassword) != customer.PasswordHash)
            {
                return BadRequest("Mật khẩu cũ không chính xác.");
            }

            if (!ValidatePassword(dto.NewPassword))
            {
                return BadRequest("Mật khẩu mới không hợp lệ. Phải có ít nhất 8 kí tự, 1 chữ hoa, 1 chữ thường, 1 số và 1 kí tự đặc biệt.");
            }

            customer.PasswordHash = HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok("Đổi mật khẩu thành công.");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _context.Customers.FindAsync(Guid.Parse(userId!));

            if (customer == null) return NotFound();

            return Ok(new
            {
                customer.Id,
                customer.Username,
                customer.FullName,
                customer.Email,
                customer.PhoneNumber,
                customer.Address
            });
        }


        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool ValidatePassword(string password)
        {
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }

}
