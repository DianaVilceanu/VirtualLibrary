using GameLibraryML.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class Book
    {

        [Key]
        public int BookID { get; set; }

        [Display(Name = "Author")]
        public String BookAuthor { get; set; }


        [Display(Name = "Title")]
        public String BookTitle { get; set; }


        [Display(Name = "Publishing Year")]
        public int PublishingYear { get; set; }

        [Display(Name = "Language")]
        public String Language { get; set; }

        [Display(Name = "Price")]
        public float BookPrice { get; set; }

        [Display(Name = "Image")]
        public String BookImage { get; set; }

        [Display(Name = "Description")]
        public String BookDescription { get; set; }


        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; }

        
   
       

        public ICollection<BookCart> BookCarts { get; set; }

        public ICollection<BookOrder> BookOrders { get; set; }
    
        //public IEnumerable<SelectListItem> CategorySelect { get; set; }


    }
}
