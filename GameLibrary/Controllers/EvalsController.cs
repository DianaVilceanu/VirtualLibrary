using System;
using System.Collections.Generic;
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
    public class EvalsController : Controller
    {
        private readonly StoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EvalsController(StoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Evals
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Evals.ToListAsync());
        }

        public async Task<IActionResult> Index2()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

            return View(await _context.Evals.Where(g => g.ApplicationUserID == userId).ToListAsync());
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Evals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eval = await _context.Evals
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.EvalId == id);
            if (eval == null)
            {
                return NotFound();
            }

            return View(eval);
        }

        // GET: Evals/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Evals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvalId,EvalSubject,EvalDescription,ApplicationUserID")] Eval eval)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eval);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", eval.ApplicationUserID);
            return View(eval);
        }

        // GET: Evals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eval = await _context.Evals.FindAsync(id);
            if (eval == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", eval.ApplicationUserID);
            return View(eval);
        }

        // POST: Evals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvalId,EvalSubject,EvalDescription,ApplicationUserID")] Eval eval)
        {
            if (id != eval.EvalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eval);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvalExists(eval.EvalId))
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
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", eval.ApplicationUserID);
            return View(eval);
        }

        // GET: Evals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eval = await _context.Evals
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.EvalId == id);
            if (eval == null)
            {
                return NotFound();
            }

            return View(eval);
        }

        // POST: Evals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eval = await _context.Evals.FindAsync(id);
            _context.Evals.Remove(eval);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvalExists(int id)
        {
            return _context.Evals.Any(e => e.EvalId == id);
        }
    }
}
