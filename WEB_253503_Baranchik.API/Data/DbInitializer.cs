using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using WEB_253503_BARANCHIK.Domain.Entities;

namespace WEB_253503_Baranchik.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            string apiURL = app.Configuration.GetSection("ApiSettings").GetValue<string>("ApiURL");

            await context.Database.MigrateAsync();

            if (context.Rooms.Any() && context.RoomCategories.Any())
                return;
            if (!context.RoomCategories.Any())
            {
                context.RoomCategories.AddRange(
                    new List<RoomCategory>
                    {
                        new RoomCategory(1, "Category1", "CAT1"),
                        new RoomCategory (2, "Category2", "CAT2"),
                    });
            }

            await context.SaveChangesAsync();
            var categories = context.RoomCategories.ToList();
            Console.WriteLine(categories);

            if (!context.Rooms.Any())
            {
                context.Rooms.AddRange(
                    new List<Room>
                    {
                        new Room ( 1, "Room 1", "", categories[0], 1, "" ),
                        new Room ( 2, "Room 2", "", categories[1], 1, "" ),
                        new Room ( 3, "Room 3", "", categories[0], 1, "" ),
                        new Room ( 4, "Room 4", "", categories[1], 1, "" ),
                        new Room ( 5, "Room 5", "", categories[0], 1, "" ),
                    });
            }

            Console.WriteLine(context.Rooms.ToList());

            await context.SaveChangesAsync();
        }
    }
}
