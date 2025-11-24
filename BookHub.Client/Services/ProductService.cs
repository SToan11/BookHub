using System.Net.Http;
using System.Net.Http.Json;
using BookHub.Client.Models;

public class ProductService
{
    private readonly HttpClient _http;
    private const string ApiUrl = "api/staff/products";

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<ProductDto>>(ApiUrl) ?? new();
    }

    public async Task<List<string>> GetAllCategoriesAsync()
    {
        return await _http.GetFromJsonAsync<List<string>>($"{ApiUrl}/categories") ?? new();
    }

    public async Task CreateAsync(ProductCreateDto dto)
    {
        await _http.PostAsJsonAsync(ApiUrl, dto);
    }

    public async Task UpdateAsync(ProductCreateDto dto)
    {
        await _http.PutAsJsonAsync($"{ApiUrl}/{dto.Id}", dto);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _http.DeleteAsync($"{ApiUrl}/{id}");
    }
    public async Task<List<ProductDto>> SearchAsync(string keyword)
    {
        var response = await _http.GetFromJsonAsync<List<ProductDto>>($"api/staff/products/search?keyword={keyword}");
        return response ?? new List<ProductDto>();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<ProductDto>($"{ApiUrl}/{id}");
    }

    public async Task UpdateAsync(Guid id, ProductUpdateDto dto)
    {
        var response = await _http.PutAsJsonAsync($"{ApiUrl}/{id}", dto);
        response.EnsureSuccessStatusCode();
    }

}