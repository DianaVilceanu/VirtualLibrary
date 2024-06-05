using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;
using Microsoft.AspNetCore.Http;
using GameLibrary.ViewModels;
using System.Web;

namespace GameLibrary.Controllers
{
    public class FavouritesController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly StoreContext _context;

        public FavouritesController(StoreContext context)
        {
            _context = context;
        }


        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult CartPartial()
        //{
        //    Favourite fav = new Favourite();

        //    int qty = 0;

        //    if(Session.GetString("cart") != null)
        //    {

        //        var list =(List<Favourite>)Session;
        //        foreach (var item in list)
        //        {
        //            qty += item.Count;

        //        }
        //    }
        //    else
        //    {
        //        fav.Count = 0;
        //    }


        //    return View();
        //}
        //public ActionResult Index()
        //{
        //    var cart = FavouriteCart.GetCart(this.HttpContext);


        //    var viewModel = new FavouriteCartViewModel
        //    {
        //        CartItems = cart.GetCartItems(),
        //        CartTotal = cart.GetTotal()
        //    };

        //    return View(viewModel);
        //}

        //public ActionResult AddToCart(int id)
        //{

        //    var addedItem =
        //        _context.Books
        //        .Single(item => item.BookID == id);


        //    var cart = FavouriteCart.GetCart(this.HttpContext);

        //    cart.AddToCart(addedItem);


        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public ActionResult RemoveFromCart(int id)
        //{

        //    var cart = FavouriteCart.GetCart(this.HttpContext);


        //    string itemName = _context.Favourites
        //        .Single(item => item.FavouriteID == id).Book.BookTitle;


        //    int itemCount = cart.RemoveFromCart(id);


        //    var results = new FavouriteCartRemoveViewModel
        //    {
        //        //Message = Server.HtmlEncode(itemName) +
        //        //    " has been removed from your shopping cart.",
        //        CartTotal = cart.GetTotal(),
        //        CartCount = cart.GetCount(),
        //        ItemCount = itemCount,
        //        DeleteId = id
        //    };
        //    return Json(results);
        //}

        ////[ChildActionOnly]
        //public ActionResult CartSummary()
        //{
        //    var cart = FavouriteCart.GetCart(this.HttpContext);

        //    ViewData["CartCount"] = cart.GetCount();
        //    return PartialView("CartSummary");
        //}

        //// GET: Favourites
        //public async Task<IActionResult> Index()
        //{
        //    var storeContext = _context.Favourites.Include(f => f.Book);
        //    return View(await storeContext.ToListAsync());
        //}

        //// GET: Favourites/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var favourite = await _context.Favourites
        //        .Include(f => f.Book)
        //        .FirstOrDefaultAsync(m => m.FavouriteID == id);
        //    if (favourite == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(favourite);
        //}

        //// GET: Favourites/Create
        //public IActionResult Create()
        //{
        //    ViewData["BookID"] = new SelectList(_context.Books, "BookID", "BookID");
        //    return View();
        //}

        //// POST: Favourites/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Favourite favourite)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(favourite);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BookID"] = new SelectList(_context.Books, "BookID", "BookID", favourite.BookID);
        //    return View(favourite);
        //}

        //// GET: Favourites/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var favourite = await _context.Favourites.FindAsync(id);
        //    if (favourite == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["BookID"] = new SelectList(_context.Books, "BookID", "BookID", favourite.BookID);
        //    return View(favourite);
        //}

        //// POST: Favourites/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("FavouriteID,UserID,BookID")] Favourite favourite)
        //{
        //    if (id != favourite.FavouriteID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(favourite);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FavouriteExists(favourite.FavouriteID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["BookID"] = new SelectList(_context.Books, "BookID", "BookID", favourite.BookID);
        //    return View(favourite);
        //}

        //// GET: Favourites/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var favourite = await _context.Favourites
        //        .Include(f => f.Book)
        //        .FirstOrDefaultAsync(m => m.FavouriteID == id);
        //    if (favourite == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(favourite);
        //}

        //// POST: Favourites/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var favourite = await _context.Favourites.FindAsync(id);
        //    _context.Favourites.Remove(favourite);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool FavouriteExists(int id)
        //{
        //    return _context.Favourites.Any(e => e.FavouriteID == id);
        //}
    }
}
