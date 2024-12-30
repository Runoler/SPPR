using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Services.RoomService;
using WEB_253503_BARANCHIK.UI.Extensions;

namespace WEB_253503_BARANCHIK.UI.Controllers
{

    public class CartController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly Cart _cart;

        public CartController(IRoomService roomService, Cart cart)
        {
            _roomService = roomService;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult ViewCart()
        {
            return View(_cart);
        }

        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _roomService.GetRoomByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }
    }
}
