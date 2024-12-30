namespace WEB_253503_BARANCHIK.UI.Services.Authentication
{
    public interface ITokenAccessor
    {
        Task<string> GetAccessTokenAsync();
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }
}
