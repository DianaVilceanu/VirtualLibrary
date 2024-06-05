using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using GameLibrary.ViewModels;

namespace GameLibrary.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly StoreContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public CategoriesController(StoreContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        //GET: Categories
        public async Task<IActionResult> HomeCategory()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> Go()
        {
            return View();
        }

        public async Task<IActionResult> newindex()
        {
            CategoryBookViewModel model = new CategoryBookViewModel();

            model.Categories = await _context.Categories.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var categories = from c in _context.Categories
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(s => s.CategoryName.Contains(searchString));
            }

            return View(await categories.ToListAsync());
        }

        [HttpPost]
        public string AddCategory(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        public async Task<IActionResult> AddCategory(string searchString)
        {
            var categories = from c in _context.Categories
                             select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(s => s.CategoryName.Contains(searchString));
            }

            return View(await categories.ToListAsync());
        }

        [HttpPost]
        public string CategoryDetails(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        public async Task<IActionResult> CategoryDetails(string searchString)
        {
            var categories = from c in _context.Categories
                             select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(s => s.CategoryName.Contains(searchString));
            }

            return View(await categories.ToListAsync());
        }

        public async Task<IActionResult> BookDetails(int? id)
        {
            CategoryBookViewModel model = new CategoryBookViewModel();
            BookDetailsViewModel books = new BookDetailsViewModel();

            if (id == null)
            {
                return NotFound();
            }

            books.Books = _context.Books
                .Where(b => b.CategoryID == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\", fileName);
                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileSteam);
                    }
                    category.CategoryImage = fileName;
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryName,CategoryDescription")] Category category)
        {
            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }

    }
}
