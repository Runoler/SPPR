
using WEB_253503_BARANCHIK.UI.Services.Authentication;

namespace WEB_253503_BARANCHIK.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _tokenAccessor = tokenAccessor;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete
            };
            StringContent content = new StringContent(fileName);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка удаления файла. Error: {response.StatusCode}");
            }
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            await _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            var extension = Path.GetExtension(formFile.FileName);
           
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", formFile.FileName);
   
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }
    }
}
