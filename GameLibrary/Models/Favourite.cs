using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class Favourite
    {
        [Key]
        public int? FavouriteID  { get; set; } //record
        public int RecordID { get; set; } //cart
        public int UserID { get; set; }
        public int BookID { get; set; }
        public float Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Book Book { get; set; }
    }
}



