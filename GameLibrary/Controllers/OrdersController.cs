using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GameLibrary.Controllers
{
    public class OrdersController : Controller
    {
        public string ShoppingCartId { get; set; }
        private readonly StoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ShoppingCart _shoppingCart;

        public OrdersController(StoreContext context, UserManager<ApplicationUser> userManager, ShoppingCart shoppingCart)
        {
            _context = context;
            _userManager = userManager;
            _shoppingCart = shoppingCart;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {

            return View(await _context.Orders.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Index2()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            return View(await _context.Orders.Where(g => g.ApplicationUserID == userId).ToListAsync());
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(string ShoppingCartId)
        {
            var total = _context.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                        .Select(c => c.Book.BookPrice * c.Amount).Sum();

            var model = new CreateOrderViewModel();
            model.DeliveryMethods = new SelectList(_context.DeliveryMethods, "DeliveryMethodID", "Name", 1);
            model.PaymentMethods = new SelectList(_context.PaymentMethods, "PaymentMethodID", "Name", 1);

            model.OrderPlaced = DateTime.Now;
            model.OrderCost = total;

            _shoppingCart.ClearCart();

            return View(model);

        }

      
        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                var user = await GetCurrentUserAsync();
                var userId = user?.Id;
                order.ApplicationUserID = userId;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
