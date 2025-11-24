using BookHub.API.Data;
using BookHub.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Description = p.Description,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    Image = p.Image,
                    Categories = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("/{categoryName}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetByCategoryName(string categoryName)
        {
            var products = await _context.Products
                .Where(p => p.ProductCategories.Any(pc => pc.Category.Name == categoryName))
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Description = p.Description,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    Image = p.Image,
                    Categories = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }


        // GET: api/products/search?keyword=abc
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Search(string keyword)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(keyword) || p.Author.Contains(keyword))
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Description = p.Description,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    Image = p.Image,
                    Categories = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Description = p.Description,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    Image = p.Image,
                    Categories = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
                })
                .FirstOrDefaultAsync();

            if (product == null) return NotFound();

            return Ok(product);
        }
        /*
        // GET: api/user/products?minPrice=0&maxPrice=1000&page=1&pageSize=10&sort=name_asc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sort = null)
        {
            var query = _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsQueryable();

            // Lọc theo giá
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            // Sắp xếp
            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                _ => query.OrderBy(p => p.Name)
            };

            // Phân trang
            var totalItems = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Author = p.Author,
                    Description = p.Description,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock,
                    Image = p.Image,
                    Categories = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
                })
                .ToListAsync();

            var response = new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = products
            };

            return Ok(response);
        }
        */
    }
}
