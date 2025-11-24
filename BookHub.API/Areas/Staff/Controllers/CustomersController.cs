using BookHub.API.Data;
using BookHub.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Areas.Staff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // GET: api/customers/search?query=abc
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Từ khóa tìm kiếm không được để trống.");

            query = query.ToLower();

            var results = await _context.Customers
                .Where(c =>
                    c.Username.ToLower().Contains(query) ||
                    c.FullName.ToLower().Contains(query) ||
                    c.Email.ToLower().Contains(query) ||
                    c.PhoneNumber.ToLower().Contains(query))
                .ToListAsync();

            return Ok(results);
        }
    }
}
