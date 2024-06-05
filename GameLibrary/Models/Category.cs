using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Display (Name = "Name")]
        public String CategoryName { get; set; }

        [Display(Name = "Description")]
        public String CategoryDescription { get; set; }
        [Display(Name = "Image")]
        public String CategoryImage { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
