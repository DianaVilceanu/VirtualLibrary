using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;
using GameLibrary.ViewModels;

namespace GameLibrary.Controllers
{
    public class SCartsController : Controller
    {
        private readonly StoreContext _context;
        private readonly IBookRepository _bookRepository;
        private readonly SCart _sCart;

        public SCartsController(StoreContext context, IBookRepository bookRepository, SCart sCart)
        {
            _context = context;
            _bookRepository = bookRepository;
            _sCart = sCart;
        }

        public ViewResult Index()
        {
            var items = _sCart.GetShoppingCartItems();
            _sCart.SCartItems = items;

            var sCVM = new SCartViewModel
            {
                SCart = _sCart,
                SCartTotal = _sCart.GetShoppingCartTotal()
            };

            return View(sCVM);
        }

        public RedirectToActionResult AddToShoppingCart(int bookId)
        {
            var selectedBook = _bookRepository.Books.FirstOrDefault(p => p.BookID == bookId);
            if (selectedBook != null)
            {
                _sCart.AddToCart(selectedBook, 1);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int bookId)
        {
            var selectedBook = _bookRepository.Books.FirstOrDefault(p => p.BookID == bookId);
            if (selectedBook != null)
            {
                _sCart.RemoveFromCart(selectedBook);
            }
            return RedirectToAction("Index");
        }
    }
}
