using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Authorization;
using WEB_253503_BARANCHIK.UI.HelperClasses;
using WEB_253503_BARANCHIK.UI.Services.Authentication;
using WEB_253503_BARANCHIK.UI.Services.CartService;
using WEB_253503_BARANCHIK.UI.Services.FileService;
using WEB_253503_BARANCHIK.UI.Services.RoomCategoryService;
using WEB_253503_BARANCHIK.UI.Services.RoomService;

namespace WEB_253503_BARANCHIK.UI.Extensions
{
    public static class HostingExtensions
    {
        public static void RegisterCustomServices(this WebApplicationBuilder builder)
        {
            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();
            builder.Services.AddHttpClient<IRoomService, ApiRoomService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<IRoomCategoryService, ApiRoomCategoryService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
            builder.Services.AddHttpClient<IFileService, ApiFileService>(opt => opt.BaseAddress = new Uri($"{uriData.ApiUri}Files"));
            builder.Services.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
            builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
            builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();
            builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
        }
    }   
}
