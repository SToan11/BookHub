namespace BookHub.Client.Models
{
    // Models/ProductDto.cs
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string Image { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
    }

    // Models/ProductCreateDto.cs
    public class ProductCreateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string Image { get; set; } = string.Empty;
        public List<string> CategoryNames { get; set; } = new();
    }



    public class ProductUpdateDto : ProductCreateDto
    {
    }


}
