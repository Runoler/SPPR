using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;
using WEB_253503_BARANCHIK.BlazorWasm.Services.DataService;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

public class DataService : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly IAccessTokenProvider _tokenProvider;
    private readonly string _pageSize = "3";

    public DataService(HttpClient httpClient, IAccessTokenProvider tokenProvider)
    {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public List<RoomCategory> Categories { get; set; }
    public List<Room> Rooms { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public RoomCategory SelectedCategory { get; set; }

    public event Action DataLoaded;

    public async Task GetRoomCategoryListAsync()
    {
        var tokenRequest = await _tokenProvider.RequestAccessToken();
        if (tokenRequest.TryGetToken(out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
        }
        var route = new StringBuilder("telescopecategories/");
        var response = await _httpClient.GetAsync(route.ToString());
        if (response.IsSuccessStatusCode)
        {
            Categories = (await response.Content.ReadFromJsonAsync<ResponseData<List<RoomCategory>>>(_serializerOptions)).Data;
        }
    }

    public async Task GetRoomListAsync(int pageNo = 1)
    {
        var tokenRequest = await _tokenProvider.RequestAccessToken();
        if (tokenRequest.TryGetToken(out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
        }

        var route = new StringBuilder("rooms/");
        List<KeyValuePair<string, string>> queryData = new();
        if (SelectedCategory is not null)
        {
            Console.WriteLine(SelectedCategory.Name);
            queryData.Add(KeyValuePair.Create("category", SelectedCategory.NormalizedName));
        }
        if (pageNo > 1)
        {
            queryData.Add(KeyValuePair.Create("pageNo", pageNo.ToString()));
        }

        if (queryData.Count > 0)
        {
            var queryString = QueryHelpers.AddQueryString("", queryData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            route.Append(queryString);
        }

        var response = await _httpClient.GetAsync(route.ToString());
        if (response.IsSuccessStatusCode)
        {
            var data = (await response.Content.ReadFromJsonAsync<ResponseData<RoomListModel<Room>>>(_serializerOptions)).Data;
            Rooms = data.Items;
            TotalPages = data.TotalPages;
            CurrentPage = data.CurrentPage;
            DataLoaded.Invoke();
        }

    }
}