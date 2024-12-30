using Microsoft.EntityFrameworkCore;
using WEB_253503_BARANCHIK.Domain.Entities;

namespace WEB_253503_Baranchik.API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomCategory> RoomCategories { get; set; }
    }
}
