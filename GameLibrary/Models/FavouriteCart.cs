//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GameLibrary.Models
//{
//    public class FavouriteCart
//    {
//        private readonly StoreContext _context;

//        public FavouriteCart(StoreContext context)
//        {
//            this._context = context;
//        }

//        int? FavouriteID { get; set; }
//        public const string CartSessionKey = "CartId";

//        public static FavouriteCart GetCart(HttpContext context)
//        {

//            var cart = new FavouriteCart(_context);
//            cart.FavouriteID = cart.GetCartId(context);
//            return cart;
//        }
//        // Helper method to simplify shopping cart calls
//        public static FavouriteCart GetCart(Microsoft.AspNetCore.Mvc.Controller controller)
//        {
//            return GetCart(controller.HttpContext);
//        }
//        public void AddToCart(Book book)
//        {

//            var cartItem =  _context.Favourites.SingleOrDefault(
//                c => c.FavouriteID == FavouriteID
//                && c.BookID == book.BookID);

//            if (cartItem == null)
//            {

//                cartItem = new Favourite
//                {
//                    BookID = book.BookID,
//                    FavouriteID = FavouriteID,
//                    Count = 1,
//                    DateCreated = DateTime.Now
//                };
//                _context.Favourites.Add(cartItem);
//            }
//            else
//            {

//                cartItem.Count++;
//            }

//            _context.SaveChanges();
//        }
//        public int RemoveFromCart(int id)
//        {

//            var cartItem = _context.Favourites.Single(
//                cart => cart.FavouriteID == FavouriteID);
//            //&& cart.RecordId == id);

//            int itemCount = 0;

//            if (cartItem != null)
//            {
//                if (cartItem.Count > 1)
//                {
//                    cartItem.Count--;
//                    itemCount = cartItem.Count;
//                }
//                else
//                {
//                    _context.Favourites.Remove(cartItem);
//                }

//                _context.SaveChanges();
//            }
//            return itemCount;
//        }

//        public void EmptyCart()
//        {
//            var cartItems = _context.Favourites.Where(
//                cart => cart.FavouriteID == FavouriteID);

//            foreach (var cartItem in cartItems)
//            {
//                _context.Favourites.Remove(cartItem);
//            }

//            _context.SaveChanges();
//        }
//        public List<Favourite> GetCartItems()
//        {
//            return _context.Favourites.Where(
//                cart => cart.FavouriteID == FavouriteID).ToList();
//        }
//        public int GetCount()
//        {

//            int? count = (from cartItems in _context.Favourites
//                          where cartItems.FavouriteID == FavouriteID
//                          select (int?)cartItems.Count).Sum();

//            return count ?? 0;
//        }

//        public float? GetTotal()
//        {

//            float? total = (from cartItems in _context.Favourites
//                            where cartItems.FavouriteID == FavouriteID
//                            select (int?)cartItems.Count *
//                            cartItems.Book.BookPrice).Sum();

//            return total;
//        }



//        public int? GetCartId(HttpContext context)
//        {

//            if (context.Session.GetString(CartSessionKey) == null)
//            {
//                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
//                {
//                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
//                }
//                else
//                {

//                    var tempCartId = Guid.NewGuid();

//                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
//                }
//            }

//            return context.Session.GetInt32(CartSessionKey);

//        }
//    }
//}
