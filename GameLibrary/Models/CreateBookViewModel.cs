using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class CreateBookViewModel
    {
        public SelectList Categories { get; set; }
        public int BookID { get; set; }
        public String BookAuthor { get; set; }
        public String BookTitle { get; set; }
        public int PublishingYear { get; set; }
        public String Language { get; set; }
        public float BookPrice { get; set; }
        public String BookImage { get; set; }
        public String BookDescription { get; set; }
        public int Quantity { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int AuthorID { get; set; }
        public SelectList Authors { get; set; }
    }
}
