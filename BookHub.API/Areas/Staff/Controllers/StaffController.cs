using BookHub.API.Data;
using BookHub.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace BookHub.API.Areas.Staff.Controllers
{

    [Area("Staff")]
    [Route("api/staff")]
    [ApiController]
    [Authorize(Roles = "employee,owner")]
    public class StaffController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StaffController(AppDbContext context)
        {
            _context = context;
        }

        // 📌 Lấy thông tin profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var staff = await _context.Staffs.FindAsync(Guid.Parse(userId!));

            if (staff == null) return NotFound();

            return Ok(new
            {
                staff.Id,
                staff.Username,
                staff.Email,
                staff.PhoneNumber,
                staff.Role
            });
        }

        // 📌 Cập nhật thông tin nhân viên
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateStaffDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var staff = await _context.Staffs.FindAsync(Guid.Parse(userId!));

            if (staff == null) return NotFound();

            staff.Email = dto.Email;
            staff.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật thành công.");
        }

        // 📌 Đổi mật khẩu
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var staff = await _context.Staffs.FindAsync(Guid.Parse(userId!));

            if (staff == null) return NotFound();

            if (HashPassword(dto.OldPassword) != staff.PasswordHash)
            {
                return BadRequest("Mật khẩu cũ không đúng.");
            }

            if (!ValidatePassword(dto.NewPassword))
            {
                return BadRequest("Mật khẩu mới không hợp lệ.");
            }

            staff.PasswordHash = HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok("Đổi mật khẩu thành công.");
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private bool ValidatePassword(string password)
        {
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }

}
