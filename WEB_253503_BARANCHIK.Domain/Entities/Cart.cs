using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_BARANCHIK.Domain.Entities
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        public virtual void AddToCart(Room room)
        {
            int roomId = room.Id;
            if (CartItems.TryGetValue(roomId, out CartItem? value)) 
            {
                value.Count += 1;
            }
            else
            {
                var cartItem = new CartItem
                {
                    Room = room,
                    Count = 1
                };
                CartItems.Add(roomId, cartItem);
            }
        }

        public virtual void RemoveItems(int id) 
        {
            CartItems.Remove(id);
        }

        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        public int Count { get => CartItems.Sum(item => item.Value.Count); }

        public int TotalPrice { get => CartItems?.Sum(item => item.Value.Room.Cost * item.Value.Count) ?? 0; }

    }
}
