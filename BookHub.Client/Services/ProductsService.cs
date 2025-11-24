using BookHub.Client.Models;
using System.Net.Http.Json;

namespace BookHub.Client.Services
{
    public class ProductsService
    {
        private readonly HttpClient _http;

        public ProductsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _http.GetFromJsonAsync<List<ProductDto>>("api/products") ?? new();
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(string categoryName)
        {
            return await _http.GetFromJsonAsync<List<ProductDto>>($"api/products/{categoryName}") ?? new();
        }

        public async Task<List<ProductDto>> SearchProductsAsync(string keyword)
        {
            return await _http.GetFromJsonAsync<List<ProductDto>>($"api/products/search?keyword={keyword}") ?? new();
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<ProductDto>($"api/products/{id}");
        }
    }

}
