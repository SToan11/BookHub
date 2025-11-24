using BookHub.Client.Models;
using System.Net.Http.Json;

namespace BookHub.Client.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;
        private const string ApiUrl = "api/staff/categories";

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<CategoryDto>>($"{ApiUrl}")
                   ?? new List<CategoryDto>();
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<CategoryDto>($"{ApiUrl}/{id}");
        }

        public async Task<CategoryDto?> CreateAsync(CategoryCreateDto dto)
        {
            var response = await _http.PostAsJsonAsync($"{ApiUrl}", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryDto>();
            }

            // Nếu là lỗi trùng tên
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();
                // Optionally log: Console.WriteLine(error);
                return null; // Trả null để xử lý ở phía client
            }

            throw new ApplicationException($"Lỗi tạo thể loại: {await response.Content.ReadAsStringAsync()}");
        }


        public async Task<bool> UpdateAsync(Guid id, CategoryUpdateDto dto)
        {
            var response = await _http.PutAsJsonAsync($"{ApiUrl}/{id}", dto);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // Nếu là lỗi trùng tên
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var error = await response.Content.ReadAsStringAsync();

                // Nếu bạn muốn log ra lỗi cụ thể, có thể:
                Console.WriteLine($"Lỗi cập nhật thể loại: {error}");

                return false;
            }

            // Các lỗi khác
            var fullError = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Lỗi cập nhật thể loại: {fullError}");
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"{ApiUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryDto>> SearchAsync(string keyword)
        {
            var result = await _http.GetFromJsonAsync<List<CategoryDto>>($"{ApiUrl}/search?keyword={keyword}");
            return result ?? new List<CategoryDto>();
        }
    }
}
