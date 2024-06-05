using GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.ViewModels
{
    public class BookDetailsViewModel
    {
        public IEnumerable<Book> Books { get; set; }
    }
}
