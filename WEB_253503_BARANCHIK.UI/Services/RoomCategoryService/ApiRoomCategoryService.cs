using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;
using WEB_253503_BARANCHIK.UI.Services.RoomService;

namespace WEB_253503_BARANCHIK.UI.Services.RoomCategoryService
{
    public class ApiRoomCategoryService : IRoomCategoryService
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiRoomCategoryService> _logger;

        public ApiRoomCategoryService(HttpClient httpClient, JsonSerializerOptions serializerOptions, ILogger<ApiRoomCategoryService> logger)
        {
            _httpClient = httpClient;
            _serializerOptions = serializerOptions;
            _logger = logger;
        }

        public async Task<ResponseData<List<RoomCategory>>> GetRoomCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}roomCategories/");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<RoomCategory>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<List<RoomCategory>>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return ResponseData<List<RoomCategory>>.Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        }
    }
}
