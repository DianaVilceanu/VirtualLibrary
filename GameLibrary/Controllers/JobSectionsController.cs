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
    public class JobSectionsController : Controller
    {
        private readonly StoreContext _context;

        public JobSectionsController(StoreContext context)
        {
            _context = context;
        }

        // GET: JobSections
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobSections.ToListAsync());
        }

        // GET: JobSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSection = await _context.JobSections
                .FirstOrDefaultAsync(m => m.JobSectionID == id);
            if (jobSection == null)
            {
                return NotFound();
            }

            return View(jobSection);
        }

        // GET: JobSections/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobSectionID,JobName,JobDescription")] JobSection jobSection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobSection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobSection);
        }

        // GET: JobSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSection = await _context.JobSections.FindAsync(id);
            if (jobSection == null)
            {
                return NotFound();
            }
            return View(jobSection);
        }

        // POST: JobSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobSectionID,JobName,JobDescription")] JobSection jobSection)
        {
            if (id != jobSection.JobSectionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobSection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobSectionExists(jobSection.JobSectionID))
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
            return View(jobSection);
        }

        // GET: JobSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSection = await _context.JobSections
                .FirstOrDefaultAsync(m => m.JobSectionID == id);
            if (jobSection == null)
            {
                return NotFound();
            }

            return View(jobSection);
        }

        // POST: JobSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobSection = await _context.JobSections.FindAsync(id);
            _context.JobSections.Remove(jobSection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobSectionExists(int id)
        {
            return _context.JobSections.Any(e => e.JobSectionID == id);
        }
    }
}
