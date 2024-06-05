using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;

namespace GameLibrary.Controllers
{
    public class RecommendsController : Controller
    {
        private readonly StoreContext _context;

        public RecommendsController(StoreContext context)
        {
            _context = context;
        }

        // GET: Recommends
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recommends.ToListAsync());
        }

        // GET: Recommends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommends
                .FirstOrDefaultAsync(m => m.RecommendId == id);
            if (recommend == null)
            {
                return NotFound();
            }

            return View(recommend);
        }

        // GET: Recommends/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recommends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecommendId,Name,ItiPlace,Dislike")] Recommend recommend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recommend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recommend);
        }

        // GET: Recommends/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommends.FindAsync(id);
            if (recommend == null)
            {
                return NotFound();
            }
            return View(recommend);
        }

        // POST: Recommends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecommendId,Name,ItiPlace,Dislike")] Recommend recommend)
        {
            if (id != recommend.RecommendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recommend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecommendExists(recommend.RecommendId))
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
            return View(recommend);
        }

        // GET: Recommends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommend = await _context.Recommends
                .FirstOrDefaultAsync(m => m.RecommendId == id);
            if (recommend == null)
            {
                return NotFound();
            }

            return View(recommend);
        }

        // POST: Recommends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recommend = await _context.Recommends.FindAsync(id);
            _context.Recommends.Remove(recommend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecommendExists(int id)
        {
            return _context.Recommends.Any(e => e.RecommendId == id);
        }
    }
}
