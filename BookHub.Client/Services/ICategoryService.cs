using BookHub.Client.Models;

namespace BookHub.Client.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task<CategoryDto?> CreateAsync(CategoryCreateDto dto);
        Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<CategoryDto>> SearchAsync(string keyword);
    }

}
