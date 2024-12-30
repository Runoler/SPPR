using Microsoft.EntityFrameworkCore;
using WEB_253503_Baranchik.API.Data;
using WEB_253503_Baranchik.API.Services.RoomService;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;

namespace WEB_253503_BARANCHIK.Tests
{
    public class RoomServiceTests
    {
        private readonly AppDbContext _context;
        private readonly RoomService _service;

        public RoomServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _service = new RoomService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _context.RoomCategories.RemoveRange(_context.RoomCategories);
            _context.Rooms.RemoveRange(_context.Rooms);
            _context.SaveChanges();

            var categories = new List<RoomCategory>
            {
                new RoomCategory(1, "Стандарт", "standart"),
                new RoomCategory(2, "Премиум", "premium")
            };

            _context.RoomCategories.AddRange(categories);
            _context.SaveChanges();

            var rooms = new List<Room>
            {
                new Room(1, "Номер 1", "Описание 1", categories[0], 100, "1.jpg"),
                new Room(2, "Номер 2", "Описание 2", categories[0], 150, "2.jpg")
            };

            _context.Rooms.AddRange(rooms);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetRoomListAsync_ReturnsFirstPageWithThreeItems_WhenCalledWithDefaultParameters()
        {
            // Act
            var result = await _service.GetRoomListAsync(null);

            // Assert
            var response = Assert.IsType<ResponseData<RoomListModel<Room>>>(result);
            Assert.True(response.Successfull);
            Assert.Equal(2, response.Data.Items.Count);
            Assert.Equal(1, response.Data.TotalPages);
            Assert.Equal(1, response.Data.CurrentPage);
            _context.Database.EnsureDeleted();

        }

        [Fact]
        public async Task GetRoomListAsync_ReturnsCorrectPage_WhenPageNumberIsSpecified()
        {
            // Act
            var result = await _service.GetRoomListAsync(null, pageNo: 1);

            // Assert
            var response = Assert.IsType<ResponseData<RoomListModel<Room>>>(result);
            Assert.True(response.Successfull);
            Assert.Equal(2, response.Data.Items.Count);
            Assert.Equal(1, response.Data.TotalPages);
            Assert.Equal(1, response.Data.CurrentPage);
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetRoomListAsync_FiltersByCategory_WhenCategoryIsProvided()
        {
            // Act
            var result = await _service.GetRoomListAsync("standart");

            // Assert
            var response = Assert.IsType<ResponseData<RoomListModel<Room>>>(result);
            Assert.True(response.Successfull);
            Assert.Equal(2, response.Data.Items.Count);
            Assert.All(response.Data.Items, item => Assert.Equal("standart", item.Category.NormalizedName));
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetRoomListAsync_DoesNotAllowPageSizeGreaterThanMax()
        {
            // Act
            var result = await _service.GetRoomListAsync(null, pageSize: 25);

            // Assert
            var response = Assert.IsType<ResponseData<RoomListModel<Room>>>(result);
            Assert.True(response.Successfull);
            Assert.Equal(2, response.Data.Items.Count);
            Assert.Equal(1, response.Data.TotalPages);
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetRoomListAsync_ReturnsError_WhenPageNumberExceedsTotalPages()
        {
            // Act
            var result = await _service.GetRoomListAsync(null, pageNo: 3);

            // Assert
            var response = Assert.IsType<ResponseData<RoomListModel<Room>>>(result);
            Assert.False(response.Successfull);
            Assert.Equal("No such page", response.ErrorMessage);
            _context.Database.EnsureDeleted();
        }
    }
}
