using Microsoft.AspNetCore.Mvc;
using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Extensions;

namespace WEB_253503_BARANCHIK.UI.Components
{
    public class CartViewComponent: ViewComponent
    {
        private readonly Cart _cart;

        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_cart);
        }
    }
}
