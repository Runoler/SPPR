using System.Text;
using System.Text.Json;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;
using WEB_253503_BARANCHIK.UI.Services.Authentication;
using WEB_253503_BARANCHIK.UI.Services.FileService;

namespace WEB_253503_BARANCHIK.UI.Services.RoomService
{
    public class ApiRoomService : IRoomService

    {
        private HttpClient _httpClient;
        private string _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiRoomService> _logger;
        private readonly IFileService _fileService;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiRoomService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiRoomService> logger, IFileService fileService,
            ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
            _tokenAccessor = tokenAccessor;
        }
        public async Task<ResponseData<Room>> CreateRoomAsync(Room room, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            using var content = new MultipartFormDataContent();

            room.Image = "https://localhost:7002/Images/noimage.jpg";
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                    room.Image = imageUrl;
            }

            var jsonRoom= JsonSerializer.Serialize(room, _serializerOptions);
            content.Add(new StringContent(jsonRoom, Encoding.UTF8, "application/json"), "room");

            var response = await _httpClient.PostAsync("rooms", content);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Room>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<Room>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return ResponseData<Room>.Error($"Данные не получены от сервера. Error:{response.StatusCode}");
        }

        public async Task DeleteRoomAsync(int id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.DeleteAsync($"rooms/{id}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Ошибка удаления продукта. Error: {response.StatusCode}");
                throw new Exception($"Ошибка удаления продукта. Error: {response.StatusCode}");
            }
        }

        public async Task<ResponseData<Room>> GetRoomByIdAsync(int id)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.GetAsync($"rooms/{id}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Room>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<Room>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"-----> Продукт не найден. Error: {response.StatusCode}");
            return ResponseData<Room>.Error($"Продукт не найден. Error: {response.StatusCode}");
        }

        public async Task<ResponseData<RoomListModel<Room>>> GetRoomListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}rooms?");
            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                urlString.Append($"categoryNormalizedName={Uri.EscapeDataString(categoryNormalizedName)}&");
            }

            if (pageNo > 1)
            {
                urlString.Append($"pageNo={pageNo}&");
            }

            if (!_pageSize.Equals("3"))
            {
                urlString.Append($"pageSize={_pageSize}");
            }

            if (urlString[urlString.Length - 1] == '&')
            {
                urlString.Length--;
            }

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<RoomListModel<Room>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<RoomListModel<Room>>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
            return ResponseData<RoomListModel<Room>>.Error($"Данные не получены от сервера. Error:{response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<Room>> UpdateRoomAsync(int id, Room room, IFormFile? formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var token = _httpClient.DefaultRequestHeaders.Authorization?.Parameter;
            _logger.LogInformation($"Используем токен: {token}");


            using var content = new MultipartFormDataContent();

            if (formFile != null)
            {
                var getResponse = await _httpClient.GetAsync($"rooms/{id}");
                if (getResponse.IsSuccessStatusCode)
                {
                    var oldRoom = getResponse.Content.ReadFromJsonAsync<ResponseData<Room>>(_serializerOptions);
                    await _fileService.DeleteFileAsync(oldRoom.Result.Data.Image);
                    var imageUrl = await _fileService.SaveFileAsync(formFile);
                    if (!string.IsNullOrEmpty(imageUrl))
                        room.Image = imageUrl;
                }
                else
                {
                    _logger.LogError($"-----> Ошибка обновления продукта. Error: {getResponse.StatusCode}");
                    throw new Exception($"Ошибка обновления продукта. Error: {getResponse.StatusCode}");
                }
            }

            var jsonRoom = JsonSerializer.Serialize(room, _serializerOptions);
            content.Add(new StringContent(jsonRoom, Encoding.UTF8, "application/json"), "room");
            

            var response = await _httpClient.PutAsync($"rooms/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<Room>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    throw new Exception($"Ошибка десериализации: {ex.Message}");
                }
            }

            _logger.LogError($"-----> Ошибка обновления продукта. Error: {response.StatusCode}");
            throw new Exception($"Ошибка обновления продукта. Error: {response.StatusCode}");
        }
    }
}
