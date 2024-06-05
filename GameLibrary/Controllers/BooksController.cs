using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.pdf.parser;
using GameLibraryML.Model;

namespace OnlineBookStore.Controllers
{
    public class BooksController : Controller
    {

        private readonly StoreContext _context;
        private IWebHostEnvironment _hostingEnvironment;

        public BooksController(StoreContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        ///
        ////////
        public string LuceneSearching()
        {
            string dataHTML = "";
            string pathIndexFolder = _hostingEnvironment.WebRootPath + "\\indexDirectory\\";
            var textToSearch = Request.Form["textToSearch"].ToString();

            var books = from b in _context.Books
                        select b;

            Lucene.Net.Store.Directory directory = FSDirectory.Open(pathIndexFolder);
            IndexReader indexReader = IndexReader.Open(directory);
            IndexSearcher searcher = new IndexSearcher(indexReader);
            Analyzer analyser = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            QueryParser parser = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "content", analyser);
            Lucene.Net.Search.Query query = parser.Parse(textToSearch.Trim());

            var hits_limit = 40;
            var hits = searcher.Search(query, hits_limit).ScoreDocs;
            string pathFile = _hostingEnvironment.WebRootPath + "\\pictures\\";


            foreach (var hit in hits)
            {
                Document hitDoc = searcher.Doc(hit.Doc);

                books = books.Where(s => s.BookTitle.Equals(hitDoc.Get("name")));
                dataHTML += string.Format("<div class='panel panel-default'>");

                dataHTML += string.Format("<center><ul><a href='@Url.Action('Details', 'Books', new {id = books.BookID})' title='@item.BookTitle'  target='_blank' " +
                    "class='links'><img class='img-responsive' src='@Url.Content('~/images/' + books.BookImage)' height='350' width='300'  target='_blank'" +
                    "alt=''></a></ul>");
                dataHTML += string.Format("<p>Title: books.BookTitle</p><br>");
                dataHTML += string.Format("<p>Author: books.BookAuthor</p>");

                var pathImage = System.IO.Path.Combine(pathFile, hitDoc.Get("name"));
                //dataHTML += string.Format("<embed src='{0}' width='800px' height='500px'/>", pathImage);
            }
            return dataHTML;
        }

        public ActionResult LuceneIndexing()
        {
            var description = Request.Form["description"].ToString();
            IFormFile filePicture = Request.Form.Files["filePicture"];

            if (description != null && filePicture != null)
            {
                string fileName = filePicture.FileName;
                string pathFile = _hostingEnvironment.WebRootPath + "\\pictures\\";
                var pathImage = System.IO.Path.Combine(pathFile, fileName);

                using (var stream = new FileStream(pathImage, FileMode.Create))
                {
                    filePicture.CopyTo(stream);
                }

                string pathIndexFolder = _hostingEnvironment.WebRootPath + "\\indexDirectory\\";

                Lucene.Net.Store.Directory directory = FSDirectory.Open(pathIndexFolder);
                Analyzer analyser = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
                IndexWriterConfig indexWriterConfig = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyser);

                IndexWriter indexWriter = new IndexWriter(directory, indexWriterConfig);

                using PdfReader reader = new PdfReader(pathImage);
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    Document doc = new Document();
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    doc.Add(new Field("path", pathImage, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("name", fileName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("content", text.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    indexWriter.AddDocument(doc);
                }
                indexWriter.Dispose();
            }
            return View("Index");
        }

        /// ////////////////////////////

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var bookTitle = _context.Books.Where(p => p.BookTitle.Contains(term))
                                                .Select(p => p.BookTitle).ToList();
                return Ok(bookTitle);
            }
            catch
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> BooksDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(b => b.CategoryID == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var books = from b in _context.Books
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookTitle.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }

        public async Task<IActionResult> BookListSearch(string searchString)
        {
            var books = from b in _context.Books
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.BookTitle.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }


        //GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);


        }

        public IActionResult Create()
        {
            var model = new CreateBookViewModel();
            model.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName", 1);
            

            return View(model);
        }

        public async Task<IActionResult> BookList(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["PriceSortParm"] = sortOrder == "0" ? "10" : "50";
            var books = from s in _context.Books
                        select s;
            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(s => s.BookTitle);
                    break;
                case "Date":
                    books = books.OrderBy(s => s.PublishingYear);
                    break;
                case "date_desc":
                    books = books.OrderByDescending(s => s.PublishingYear);
                    break;
                case "10":
                    books = books.OrderBy(s => s.BookPrice > 10);
                    break;
                default:
                    books = books.OrderBy(s => s.BookTitle);
                    break;
            }
            return View(await books.AsNoTracking().ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = System.IO.Path.GetFileName(image.FileName);
                    var filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\images\\", fileName);
                    using (var fileSteam = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileSteam);
                    }
                    book.BookImage = fileName;
                }
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }


        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpGet]
        public IActionResult Predict()
        {
            return View();
        }

        public ActionResult Predict(ModelInput input)
        {
            var prediction = ConsumeModel.Predict(input);
            ViewBag.Result = prediction;
            return View();

        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }

        public async Task<IActionResult> SearchBook(string searchString)
        {
            var books = from b in _context.Books
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Category.CategoryName.Contains(searchString));
            }

            return View(await books.ToListAsync());
        }

        public string Searching()
        {
            string dataHTML = "";
            string pathIndexFolder = _hostingEnvironment.WebRootPath + "\\indexDirectory\\";
            var textToSearch = Request.Form["textToSearch"].ToString();

            Lucene.Net.Store.Directory directory = FSDirectory.Open(pathIndexFolder);
            IndexReader indexReader = IndexReader.Open(directory);
            IndexSearcher searcher = new IndexSearcher(indexReader);
            Analyzer analyser = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            QueryParser parser = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "content", analyser);
            Lucene.Net.Search.Query query = parser.Parse(textToSearch.Trim());

            var hits_limit = 40;
            var hits = searcher.Search(query, hits_limit).ScoreDocs;

            foreach (var hit in hits)
            {
                Document hitDoc = searcher.Doc(hit.Doc);
                dataHTML += "<div class='panel panel-default'>";
                //dataHTML += string.Format("<img src='\\pictures\\{0}' width='150px' height='150'/>", hitDoc.Get("path"));


                if (hitDoc.Get("name") == "It.pdf")
                {
                    dataHTML += string.Format("<center><a href='https://localhost:44387/Books/Details/2' target='_blank'>" +
                        "<img src = 'https://images-na.ssl-images-amazon.com/images/I/71dIjJTeOSL.jpg' width = '300' height = '350'></br>" +
                        "Title: It</br>" +
                        "Author: Stephen King</br>");
                    //dataHTML += string.Format("<embed src='https://docs.google.com/document/d/1BlABPbf7JiXUPrrgMfiVl7Cu1bDj1gZrGvpstwzfbBg/edit' width='700px' height='500px'  target='_blank'/></a></center></br>", hitDoc.Get("path"));

                }
                else if (hitDoc.Get("name") == "Amintiri din copilarie.pdf")
                {
                    dataHTML += string.Format("<center><a href='https://localhost:44387/Books/Details/5'  target='_blank'>" +
                        "<img src = 'https://www.librariaeminescu.ro/upload/produse/Ion-Creanga__Amintiri-din-copilarie__606-8395-19-7-785334231657.jpg' width = '300' height = '350' ></br>" +
                        "Title: Amintiri din copilarie</br>" +
                        "Author: Ion Creanga</br>");
                    //dataHTML += string.Format("<embed src='https://docs.google.com/document/d/1xbFBtrv_RmM9-dd9o1jLA5tecxVOk8Zm3hKQSlRR5QA/edit' width='700px' height='500px'/></a></center></br>", hitDoc.Get("path"));

                }
                else if (hitDoc.Get("name") == "pride and prejudice.pdf")
                {
                    dataHTML += string.Format("<center><a href='https://localhost:44387/Books/Details/4'  target='_blank'>" +
                        "<img src = 'https://kbimages1-a.akamaihd.net/94a264bb-8f67-445f-a993-fd5fc96d89ed/1200/1200/False/jane-austen-s-pride-prejudice.jpg' width = '300' height = '350' ></br>" +
                        "Title: Pride and prejudice</br>" +
                        "Author: Jane Austen</br>");
                    //dataHTML += string.Format("<embed src='https://docs.google.com/document/d/18HPV0Wi-b4OdIZsTCpC3S1xGpy7DvkF_zP2-UuRDz1s/edit' width='700px' height='500px'/></a></center></br>", hitDoc.Get("path"));

                }
                else if (hitDoc.Get("name") == "39 steps.pdf")
                {
                    dataHTML += string.Format("<center><a href='https://localhost:44387/Books/Details/3'  target='_blank'>" +
                        "<img src = 'https://pictures.abebooks.com/LELIVRE/22550473964.jpg' width = '300' height = '350' ></br>" +
                        "Title: The thirty nine steps</br>" +
                        "Author: John Buchan</br>");
                    //dataHTML += string.Format("<embed src='https://docs.google.com/document/d/1aZlXTiZ6Bk8GvHz9TdRksYW4o6f18jw1buUEGPBwr_A/edit' width='700px' height='500px'/></a></center></br>", hitDoc.Get("path"));

                }
                else if (hitDoc.Get("name") == "To kill a mockinbird.pdf")
                {
                    dataHTML += string.Format("<center><a href='https://localhost:44387/Books/Details/1'  target='_blank'>" +
                        "<img src = 'https://cdn.britannica.com/21/182021-050-666DB6B1/book-cover-To-Kill-a-Mockingbird-many-1961.jpg' width = '300' height = '350' ></br>" +
                        "Title: To kill a mockingbird</br>" +
                        "Author: Harper Lee</br>");
                    //dataHTML += string.Format("<embed src='https://docs.google.com/document/d/1L-RaIe6-AXkeQSC6R2CLGrniKGoZTadux91dlPNViDk/edit' width='700px' height='500px'/></a></center></br>", hitDoc.Get("path"));
                }
            }
            return dataHTML;
        }

        public ActionResult Indexing()
        {
            var description = Request.Form["description"].ToString();
            IFormFile filePicture = Request.Form.Files["filePicture"];

            if (description != null && filePicture != null)
            {
                string fileName = filePicture.FileName;
                string pathFile = _hostingEnvironment.WebRootPath + "\\pictures\\";
                var pathImage = System.IO.Path.Combine(pathFile, fileName);

                using (var stream = new FileStream(pathImage, FileMode.Create))
                {
                    filePicture.CopyTo(stream);
                }

                string pathIndexFolder = _hostingEnvironment.WebRootPath + "\\indexDirectory\\";

                Lucene.Net.Store.Directory directory = FSDirectory.Open(pathIndexFolder);
                Analyzer analyser = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
                IndexWriterConfig indexWriterConfig = new IndexWriterConfig(Lucene.Net.Util.LuceneVersion.LUCENE_48, analyser);

                IndexWriter indexWriter = new IndexWriter(directory, indexWriterConfig);

                using PdfReader reader = new PdfReader(pathImage);
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    Document doc = new Document();
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    doc.Add(new Field("path", pathImage, Field.Store.YES, Field.Index.ANALYZED));
                    doc.Add(new Field("name", fileName, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("content", text.ToString(), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    indexWriter.AddDocument(doc);
                }
                indexWriter.Dispose();
            }
            return View("Index");
        }
    }
}
