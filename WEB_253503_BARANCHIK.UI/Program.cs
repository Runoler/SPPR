using WEB_253503_BARANCHIK.UI;
using WEB_253503_BARANCHIK.UI.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using WEB_253503_BARANCHIK.UI.HelperClasses;
using WEB_253503_BARANCHIK.UI.Services.Authentication;
using Serilog.Filters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
string uriDataString = builder.Configuration.GetSection("UriData").GetValue<string>("UriApi");
UriData uriData = new UriData(uriDataString);

HostingExtensions.RegisterCustomServices(builder);

var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority = $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid"); // Customize scopes as needed
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; // позволяет обращаться к локальному Keycloak по http
    options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});

// Add services to the container.
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Customize as needed
    WriteIndented = true
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .Filter.ByIncludingOnly(Matching.WithProperty<int>("StatusCode", status => status != 200))
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); // Maps Razor Pages
app.MapControllers(); // Maps MVC controllers

app.MapGet("/menu/{category}", (string category, int pageno) => {

    return Results.Ok(new
    {
        Category = category,
        PageNumber = pageno,
        Items = new List<string> { "Item 1", "Item 2" }
    });
});


app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "AdminArea",
    pattern: "AdminArea/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
