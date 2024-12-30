using WEB_253503_BARANCHIK.Domain.Entities;
using WEB_253503_BARANCHIK.UI.Extensions;

namespace WEB_253503_BARANCHIK.UI.Services.CartService
{
    public class SessionCart : Cart
    {
        public ISession? Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        public override void AddToCart(Room room)
        {
            base.AddToCart(room);
            Session?.Set<Cart>("Cart", this);
        }
        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set<Cart>("Cart", this);
        }
        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
