using GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;
        private readonly SCart _sCart;
        public OrderRepository(StoreContext context, SCart sCart)
        {
            _context = context;
            _sCart = sCart;
        }

        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            _context.Orders.Add(order);

            var sCartItems = _sCart.SCartItems;

            foreach(var item in sCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = item.Amount,
                    BookID = item.Book.BookID,
                    OrderID = order.OrderID,
                    Price = item.Book.BookPrice
                };
                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();
        }
    }
}
