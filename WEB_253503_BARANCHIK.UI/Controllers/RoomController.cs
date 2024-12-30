using Microsoft.AspNetCore.Mvc;
using WEB_253503_BARANCHIK.Domain.Models;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Services.RoomCategoryService;
using WEB_253503_BARANCHIK.UI.Services.RoomService;
using WEB_253503_BARANCHIK.UI.Extensions;

namespace WEB_253503_BARANCHIK.UI.Controllers
{
    [Route("rooms")]
    public class RoomController : Controller
    {
        IRoomService _roomService;
        IRoomCategoryService _roomCategoryService;

        public RoomController(IRoomService roomService, IRoomCategoryService roomCategoryService)
        {
            this._roomService = roomService;
            this._roomCategoryService = roomCategoryService;
        }

        [Route("{category?}")]
        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            var categoriesResponse = await _roomCategoryService.GetRoomCategoryListAsync();
            if (!categoriesResponse.Successfull)
                return NotFound(categoriesResponse.ErrorMessage);

            var roomResponse = await _roomService.GetRoomListAsync(category, pageNo);
            if (!roomResponse.Successfull)
                return NotFound(roomResponse.ErrorMessage);
            var currentCategoryName = categoriesResponse.Data.FirstOrDefault(c =>
            c.NormalizedName.Equals(category, StringComparison.OrdinalIgnoreCase))?.Name ?? "Все категории";

            ViewBag.Categories = categoriesResponse.Data;
            ViewBag.CurrentCategory = currentCategoryName ?? "Все категории";
            ViewBag.PreviousPage = pageNo == 1 ? 1 : pageNo-1;
            ViewBag.NextPage = pageNo == roomResponse.Data.TotalPages ? roomResponse.Data.TotalPages : pageNo + 1;

            var model = new RoomListModel<Room>
            {
                Items = roomResponse.Data.Items,
                CurrentPage = pageNo,
                TotalPages = roomResponse.Data.TotalPages
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_RoomList", model);
            }


            return View(model);
        }
    }
}
