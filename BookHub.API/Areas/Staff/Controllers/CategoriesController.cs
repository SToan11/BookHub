using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookHub.API.Data;
using BookHub.API.Models;
using BookHub.API.DTOs;

namespace BookHub.API.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("api/staff/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Tên thể loại không được để trống.");

            // Kiểm tra tên trùng (không phân biệt hoa thường)
            var exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == dto.Name.Trim().ToLower());

            if (exists)
                return BadRequest("Tên thể loại đã tồn tại.");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim()
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CategoryUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Tên thể loại không được để trống.");

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var isDuplicate = await _context.Categories
                .AnyAsync(c => c.Id != id && c.Name.ToLower() == dto.Name.Trim().ToLower());

            if (isDuplicate)
                return BadRequest("Tên thể loại đã tồn tại.");

            category.Name = dto.Name.Trim();
            await _context.SaveChangesAsync();

            return Ok(new CategoryDto { Id = category.Id, Name = category.Name });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    


        // /api/staff/categories/search?keyword=kinh
        [HttpGet("search")]
        public async Task<IActionResult> SearchCategory([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Từ khóa tìm kiếm không được để trống.");

            var results = await _context.Categories
                .Where(c => c.Name.Contains(keyword))
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterProduct([FromQuery] Guid categoryId)
        {
            var products = await _context.Products
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    Categories = p.ProductCategories
                        .Select(pc => pc.Category.Name)
                        .ToList()
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}


//[HttpGet]
//public async Task<IActionResult> GetAll()
//{
//    var categories = await _context.Categories.ToListAsync();
//    return Ok(categories);
//}

//[HttpGet("{id}")]
//public async Task<IActionResult> GetById(Guid id)
//{
//    var category = await _context.Categories.FindAsync(id);
//    if (category == null) return NotFound();
//    return Ok(category);
//}

//[HttpPost]
//public async Task<IActionResult> Create(Category category)
//{
//    category.Id = Guid.NewGuid();
//    _context.Categories.Add(category);
//    await _context.SaveChangesAsync();

//    return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
//}

//[HttpPut("{id}")]
//public async Task<IActionResult> Update(Guid id, Category category)
//{
//    if (id != category.Id) return BadRequest();

//    _context.Entry(category).State = EntityState.Modified;
//    await _context.SaveChangesAsync();

//    return NoContent();
//}

//[HttpDelete("{id}")]
//public async Task<IActionResult> Delete(Guid id)
//{
//    var category = await _context.Categories.FindAsync(id);
//    if (category == null) return NotFound();

//    _context.Categories.Remove(category);
//    await _context.SaveChangesAsync();

//    return NoContent();
//}