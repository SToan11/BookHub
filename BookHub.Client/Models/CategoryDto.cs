using System.ComponentModel.DataAnnotations;

namespace BookHub.Client.Models
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự")]
        public string Name { get; set; } = string.Empty;
    }


    public class CategoryUpdateDto
    {
        public string Name { get; set; } = string.Empty;
    }

}
