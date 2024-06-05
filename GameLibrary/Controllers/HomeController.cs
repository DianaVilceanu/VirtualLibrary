using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GameLibrary.Models;
using Microsoft.AspNetCore.Http;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers.Classic;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.pdf.parser;


namespace GameLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LuceneView()
        {
            return View();
        }

        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult newindex()
        {
            return View();
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
            string pathFile = _hostingEnvironment.WebRootPath + "\\pictures\\";


            foreach (var hit in hits)
            {
                Document hitDoc = searcher.Doc(hit.Doc);
                dataHTML += "<div class='panel panel-default'>";
                var pathImage = System.IO.Path.Combine(pathFile, hitDoc.Get("name"));

                dataHTML += string.Format("<embed src='{0}' width='800px' height='500px'/>", pathImage);
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
