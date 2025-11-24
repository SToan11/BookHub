using BookHub.API.Data;
using BookHub.API.DTOs;
using BookHub.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Route("api/staff/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
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
                }).ToListAsync();

            return Ok(products);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return Ok(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Author = product.Author,
                Description = product.Description,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                Image = product.Image,
                Categories = product.ProductCategories.Select(pc => pc.Category.Name).ToList()
            });
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Tên sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(dto.Author))
                return BadRequest("Tác giả không được để trống.");
            if (dto.Price <= 0)
                return BadRequest("Giá sản phẩm phải lớn hơn 0.");
            if (dto.QuantityInStock <= 0)
                return BadRequest("Số lượng sản phẩm phải lớn hơn 0.");

            var isDuplicate = await _context.Products
                .AnyAsync(p => p.Name.ToLower().Trim() == dto.Name.ToLower().Trim());

            if (isDuplicate)
                return BadRequest("Sản phẩm đã tồn tại.");

            var categories = await _context.Categories
                .Where(c => dto.CategoryNames.Contains(c.Name))
                .ToListAsync();

            if (categories.Count != dto.CategoryNames.Count)
                return BadRequest("Một hoặc nhiều thể loại không tồn tại.");

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Author = dto.Author.Trim(),
                Description = dto.Description?.Trim(),
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock,
                Image = string.IsNullOrWhiteSpace(dto.Image)
                    ? "https://placehold.co/600x400?text=No+Image"
                    : dto.Image,
                ProductCategories = categories.Select(c => new ProductCategory
                {
                    CategoryId = c.Id
                }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Author = product.Author,
                Description = product.Description,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                Image = product.Image,
                Categories = categories.Select(c => c.Name).ToList()
            });
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Tên sản phẩm không được để trống.");
            if (string.IsNullOrWhiteSpace(dto.Author))
                return BadRequest("Tác giả không được để trống.");
            if (dto.Price <= 0)
                return BadRequest("Giá sản phẩm phải lớn hơn 0.");
            if (dto.QuantityInStock <= 0)
                return BadRequest("Số lượng sản phẩm phải lớn hơn 0.");

            var product = await _context.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var isDuplicate = await _context.Products
                .AnyAsync(p => p.Id != id && p.Name.ToLower().Trim() == dto.Name.ToLower().Trim());

            if (isDuplicate)
                return BadRequest("Tên sản phẩm đã tồn tại.");

            var categories = await _context.Categories
                .Where(c => dto.CategoryNames.Contains(c.Name))
                .ToListAsync();

            if (categories.Count != dto.CategoryNames.Count)
                return BadRequest("Một hoặc nhiều thể loại không tồn tại.");

            product.Name = dto.Name.Trim();
            product.Author = dto.Author.Trim();
            product.Description = dto.Description?.Trim();
            product.Price = dto.Price;
            product.QuantityInStock = dto.QuantityInStock;
            product.Image = string.IsNullOrWhiteSpace(dto.Image)
                ? "https://placehold.co/600x400?text=Image"
                : dto.Image;

            product.ProductCategories.Clear();
            foreach (var category in categories)
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = category.Id
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Author = product.Author,
                Description = product.Description,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                Image = product.Image,
                Categories = categories.Select(c => c.Name).ToList()
            });
        }



        [HttpGet("search")]
        public async Task<IActionResult> Search(string keyword)
        {
            var products = await _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Where(p => p.Name.Contains(keyword) ||
                            p.Description.Contains(keyword) ||
                            p.Author.Contains(keyword))
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
                }).ToListAsync();

            return Ok(products);
        }
        //[HttpGet("categories")]
        //public async Task<ActionResult<List<string>>> GetAllCategories()
        //{
        //    var categories = await _context.Categories
        //        .Select(c => c.Name)
        //        .Distinct()
        //        .OrderBy(name => name)
        //        .ToListAsync();

        //    return Ok(categories);
        //}
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories.Select(c => c.Name).ToListAsync();
            return Ok(categories);
        }


    }
}



//using BookHub.API.Data;
//using BookHub.API.DTOs;
//using BookHub.API.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace BookHub.API.Areas.Staff.Controllers
//{
//    [Area("Staff")]
//    [Route("api/staff/products")]
//    [ApiController]
//    public class ProductController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public ProductController(AppDbContext context)
//        {
//            _context = context;
//        }


//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var products = await _context.Products
//                .Include(p => p.ProductCategories)
//                .ThenInclude(pc => pc.Category)
//                .Select(p => new ProductDto
//                {
//                    Id = p.Id,
//                    Name = p.Name,
//                    Description = p.Description,
//                    Price = p.Price,
//                    QuantityInStock = p.QuantityInStock,
//                    Categories = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
//                }).ToListAsync();

//            return Ok(products);
//        }


//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(Guid id)
//        {
//            var product = await _context.Products
//                .Include(p => p.ProductCategories)
//                    .ThenInclude(pc => pc.Category)
//                .FirstOrDefaultAsync(p => p.Id == id);

//            if (product == null) return NotFound();

//            return Ok(new ProductDto
//            {
//                Id = product.Id,
//                Name = product.Name,
//                Description = product.Description,
//                Price = product.Price,
//                QuantityInStock = product.QuantityInStock,
//                Categories = product.ProductCategories.Select(pc => pc.Category.Name).ToList()
//            });
//        }


//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
//        {
//            var product = await _context.Products
//                .Include(p => p.ProductCategories)
//                .FirstOrDefaultAsync(p => p.Id == id);

//            if (product == null) return NotFound();

//            var categories = await _context.Categories
//                .Where(c => dto.CategoryNames.Contains(c.Name))
//                .ToListAsync();

//            if (categories.Count != dto.CategoryNames.Count)
//            {
//                return BadRequest("Một hoặc nhiều thể loại không tồn tại.");
//            }

//            product.Name = dto.Name;
//            product.Description = dto.Description;
//            product.Price = dto.Price;
//            product.QuantityInStock = dto.QuantityInStock;

//            // Xóa các liên kết cũ
//            product.ProductCategories.Clear();

//            // Thêm các liên kết mới
//            foreach (var category in categories)
//            {
//                product.ProductCategories.Add(new ProductCategory
//                {
//                    ProductId = product.Id,
//                    CategoryId = category.Id
//                });
//            }

//            await _context.SaveChangesAsync();

//            return Ok(new ProductDto
//            {
//                Id = product.Id,
//                Name = product.Name,
//                Description = product.Description,
//                Price = product.Price,
//                QuantityInStock = product.QuantityInStock,
//                Categories = categories.Select(c => c.Name).ToList()
//            });
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            var product = await _context.Products.FindAsync(id);
//            if (product == null) return NotFound();

//            _context.Products.Remove(product);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }



//        [HttpPost]
//        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
//        {
//            var categories = await _context.Categories
//                .Where(c => dto.CategoryNames.Contains(c.Name))
//                .ToListAsync();

//            if (categories.Count != dto.CategoryNames.Count)
//            {
//                return BadRequest("Một hoặc nhiều thể loại không tồn tại.");
//            }

//            var product = new Product
//            {
//                Id = Guid.NewGuid(),
//                Name = dto.Name,
//                Description = dto.Description,
//                Price = dto.Price,
//                QuantityInStock = dto.QuantityInStock,
//                Image = dto.Image,
//                ProductCategories = categories.Select(c => new ProductCategory
//                {
//                    CategoryId = c.Id
//                }).ToList()
//            };

//            _context.Products.Add(product);
//            await _context.SaveChangesAsync();

//            return Ok(new ProductDto
//            {
//                Id = product.Id,
//                Name = product.Name,
//                Description = product.Description,
//                Price = product.Price,
//                QuantityInStock = product.QuantityInStock,
//                Categories = categories.Select(c => c.Name).ToList()
//            });
//        }






//        // /api/staff/products/search?keyword=thám hiểm
//        [HttpGet("search")]
//        public async Task<IActionResult> Search(string keyword)
//        {
//            var products = await _context.Products
//                .Include(p => p.ProductCategories)
//                    .ThenInclude(pc => pc.Category)
//                .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
//                .Select(p => new ProductDto
//                {
//                    Id = p.Id,
//                    Name = p.Name,
//                    Description = p.Description,
//                    Price = p.Price,
//                    QuantityInStock = p.QuantityInStock,
//                    Categories = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
//                }).ToListAsync();

//            return Ok(products);
//        }

//    }
//}




//[HttpGet("search")]
//public async Task<IActionResult> SearchProduct([FromQuery] string keyword)
//{
//    if (string.IsNullOrWhiteSpace(keyword))
//        return BadRequest("Từ khóa tìm kiếm không được để trống.");

//    var results = await _context.Products
//        .Include(p => p.Category)
//        .Where(p =>
//            p.Name.Contains(keyword) ||
//            p.Description.Contains(keyword) ||
//            p.Category.Name.Contains(keyword))
//        .ToListAsync();

//    return Ok(results);
//}

//[HttpGet]
//public async Task<IActionResult> GetAll()
//{
//    var products = await _context.Products.Include(p => p.Category).ToListAsync();
//    return Ok(products);
//}

//[HttpGet("{id}")]
//public async Task<IActionResult> GetById(Guid id)
//{
//    var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
//    if (product == null) return NotFound();
//    return Ok(product);
//}

//[HttpPost]
//public async Task<IActionResult> Create(Product product)
//{
//    if (!await _context.Categories.AnyAsync(c => c.Id == product.CategoryId))
//        return BadRequest("Invalid category.");

//    product.Id = Guid.NewGuid();
//    _context.Products.Add(product);
//    await _context.SaveChangesAsync();

//    return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
//}

//[HttpPut("{id}")]
//public async Task<IActionResult> Update(Guid id, Product product)
//{
//    if (id != product.Id) return BadRequest();

//    if (!await _context.Categories.AnyAsync(c => c.Id == product.CategoryId))
//        return BadRequest("Invalid category.");

//    _context.Entry(product).State = EntityState.Modified;
//    await _context.SaveChangesAsync();

//    return NoContent();
//}

//[HttpDelete("{id}")]
//public async Task<IActionResult> Delete(Guid id)
//{
//    var product = await _context.Products.FindAsync(id);
//    if (product == null) return NotFound();

//    _context.Products.Remove(product);
//    await _context.SaveChangesAsync();

//    return NoContent();
//}


//[HttpPost]
//public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
//{
//    var categories = await _context.Categories
//        .Where(c => dto.CategoryNames.Contains(c.Name))
//        .ToListAsync();

//    if (categories.Count != dto.CategoryNames.Count)
//    {
//        return BadRequest("Một hoặc nhiều thể loại không tồn tại.");
//    }

//    var product = new Product
//    {
//        Id = Guid.NewGuid(),
//        Name = dto.Name,
//        Description = dto.Description,
//        Price = dto.Price,
//        QuantityInStock = dto.QuantityInStock,
//        Image = dto.Image,
//        ProductCategories = categories.Select(c => new ProductCategory
//        {
//            CategoryId = c.Id
//        }).ToList()
//    };

//    _context.Products.Add(product);
//    await _context.SaveChangesAsync();

//    return Ok(new ProductDto
//    {
//        Id = product.Id,
//        Name = product.Name,
//        Description = product.Description,
//        Price = product.Price,
//        QuantityInStock = product.QuantityInStock,
//        Image = product.Image,
//        Categories = categories.Select(c => c.Name).ToList()
//    });
//}


//[HttpPut("{id}")]
//public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto dto)
//{
//    var product = await _context.Products
//        .Include(p => p.ProductCategories)
//        .FirstOrDefaultAsync(p => p.Id == id);

//    if (product == null) return NotFound();

//    var categories = await _context.Categories
//        .Where(c => dto.CategoryNames.Contains(c.Name))
//        .ToListAsync();

//    if (categories.Count != dto.CategoryNames.Count)
//    {
//        return BadRequest("Một hoặc nhiều thể loại không tồn tại.");
//    }

//    product.Name = dto.Name;
//    product.Description = dto.Description;
//    product.Price = dto.Price;
//    product.QuantityInStock = dto.QuantityInStock;
//    product.Image = dto.Image ?? product.Image;

//    product.ProductCategories.Clear();

//    foreach (var category in categories)
//    {
//        product.ProductCategories.Add(new ProductCategory
//        {
//            ProductId = product.Id,
//            CategoryId = category.Id
//        });
//    }

//    await _context.SaveChangesAsync();

//    return Ok(new ProductDto
//    {
//        Id = product.Id,
//        Name = product.Name,
//        Description = product.Description,
//        Price = product.Price,
//        QuantityInStock = product.QuantityInStock,
//        Image = product.Image,
//        Categories = categories.Select(c => c.Name).ToList()
//    });
//}