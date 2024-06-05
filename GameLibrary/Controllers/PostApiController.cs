using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameLibrary.Controllers
{
    [Route("api/book")]
    public class PostApiController : Controller
    {
        private readonly StoreContext storeContext;

        public PostApiController(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = storeContext.Books.Where(p => p.BookTitle.StartsWith(term))
                                                .Select(p => p.BookTitle).ToList();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}