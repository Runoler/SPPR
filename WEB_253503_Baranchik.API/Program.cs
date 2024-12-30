using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json;
using WEB_253503_Baranchik.API.Data;
using WEB_253503_Baranchik.API.Models;
using WEB_253503_Baranchik.API.Services.RoomCategoryService;
using WEB_253503_Baranchik.API.Services.RoomService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();  
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomCategoryService, RoomCategoryService>();
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
});

var authServer = builder.Configuration.GetSection("AuthServer").Get<AuthServerData>();
// Добавить сервис аутентификации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        // Адрес метаданных конфигурации OpenID
        o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";
        // Authority сервера аутентификации
        o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";
        // Audience для токена JWT
        o.Audience = "account";
        // Запретить HTTPS для использования локальной версии Keycloak
        // В рабочем проекте должно быть true
        o.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireClaim("Role").RequireRole("POWER-USER"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:7153")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));

var app = builder.Build();

DbInitializer.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseStaticFiles();

}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
