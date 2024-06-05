using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int NoBooks { get; set; }
        public float TotalPrice { get; set; }


        //public int UserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<BookCart> BookCarts { get; set; }
    }
}
