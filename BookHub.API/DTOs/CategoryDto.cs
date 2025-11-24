namespace BookHub.API.DTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
    }

    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class CategoryUpdateDto
    {
        public string Name { get; set; }
    }

}
