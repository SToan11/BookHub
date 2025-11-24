using BCrypt.Net;
using BookHub.API.Data;
using BookHub.API.Models;
using BookHub.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Areas.Staff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StaffsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/staffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAll()
        {
            return await _context.Staffs
                .Select(s => new StaffDto
                {
                    Id = s.Id,
                    Username = s.Username,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Role = s.Role
                })
                .ToListAsync();
        }

        // GET: api/staffs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetById(Guid id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null) return NotFound();

            return new StaffDto
            {
                Id = staff.Id,
                Username = staff.Username,
                Email = staff.Email,
                PhoneNumber = staff.PhoneNumber,
                Role = staff.Role
            };
        }

        // POST: api/staffs/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(StaffCreateDto dto)
        {
            if (_context.Staffs.Any(s => s.Username == dto.Username))
                return BadRequest("Username đã tồn tại");

            var staff = new BookHub.API.Models.Staff
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }

        // PUT: api/staffs/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, StaffUpdateDto dto)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null) return NotFound();

            staff.Email = dto.Email;
            staff.PhoneNumber = dto.PhoneNumber;
            staff.Role = dto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/staffs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null) return NotFound();

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/staffs/search?keyword=abc
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<StaffDto>>> Search(string keyword)
        {
            var result = await _context.Staffs
                .Where(s =>
                    s.Username.Contains(keyword) ||
                    s.Email.Contains(keyword) ||
                    s.PhoneNumber.Contains(keyword))
                .Select(s => new StaffDto
                {
                    Id = s.Id,
                    Username = s.Username,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Role = s.Role
                })
                .ToListAsync();

            return result;
        }
    }

}
