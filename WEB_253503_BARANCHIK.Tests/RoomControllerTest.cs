using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NSubstitute;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.Domain.Models;
using WEB_253503_BARANCHIK.UI.Controllers;
using WEB_253503_BARANCHIK.UI.Services.RoomCategoryService;
using WEB_253503_BARANCHIK.UI.Services.RoomService;

namespace WEB_253503_BARANCHIK.Tests
{
    public class RoomControllerTests
    {
        private readonly IRoomService _roomService;
        private readonly IRoomCategoryService _roomCategoryService;
        private readonly RoomController _controller;

        public RoomControllerTests()
        {
            _roomService = Substitute.For<IRoomService>();
            _roomCategoryService = Substitute.For<IRoomCategoryService>();
            _controller = new RoomController(_roomService, _roomCategoryService);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenCategoryListFails()
        {
            // Arrange
            var productResponse = ResponseData<RoomListModel<Room>>.Success(new RoomListModel<Room>());
            productResponse.Successfull = true;

            _roomService.GetRoomListAsync(Arg.Any<string>(), Arg.Any<int>())
                .Returns(Task.FromResult(productResponse));
            _roomCategoryService.GetRoomCategoryListAsync().Returns(Task.FromResult(ResponseData<List<RoomCategory>>.Error("Error fetching categories")));

            // Act
            var result = await _controller.Index("some-category");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error fetching categories", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenProductListFails()
        {
            // Arrange
            _roomCategoryService.GetRoomCategoryListAsync().Returns(Task.FromResult(ResponseData<List<RoomCategory>>.Success(new List<RoomCategory>())));

            var productResponse = ResponseData<RoomListModel<Room>>.Error("Error fetching products");

            _roomService.GetRoomListAsync(Arg.Any<string>(), Arg.Any<int>())
                .Returns(Task.FromResult(productResponse));

            // Act
            var result = await _controller.Index("some-category");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error fetching products", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_ReturnsViewWithValidData_WhenSuccessful()
        {
            // Arrange
            var categories = new List<RoomCategory>
        {
            new RoomCategory(1, "Стандарт", "standart"),
            new RoomCategory(2, "Премиум", "premium")
        };

            var productList = new List<Room>
        {
            new Room(1, "Номер 1", "Описание 1", categories[0], 100, "1.jpg"),
            new Room(2, "Номер 2", "Описание 2", categories[0], 150, "2.jpg")
        };

            var productResponse = ResponseData<RoomListModel<Room>>.Success(new RoomListModel<Room>
            {
                Items = productList,
                CurrentPage = 1,
                TotalPages = 1
            });

            var categoryResponse = ResponseData<List<RoomCategory>>.Success(categories);

            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            var controller = new RoomController(_roomService, _roomCategoryService) { ControllerContext = controllerContext };

            _roomService.GetRoomListAsync(Arg.Any<string>(), Arg.Any<int>())
                .Returns(Task.FromResult(productResponse));
            _roomCategoryService.GetRoomCategoryListAsync().Returns(Task.FromResult(categoryResponse));

            // Act
            var result = await controller.Index("standart");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RoomListModel<Room>>(viewResult.Model);

            Assert.Equal(2, model.Items.Count);
            Assert.Equal("Стандарт", viewResult.ViewData["CurrentCategory"]);
            Assert.Equal(categories, viewResult.ViewData["Categories"]);
        }
    }
}