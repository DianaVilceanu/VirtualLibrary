using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class BookCart
    {

        [Key, Column(Order = 0)]
        public int ISBN { get; set; }
        [Key, Column(Order = 1)]
        public int CartID { get; set; }


        public Book Book { get; set; }
        public Cart Cart { get; set; }
    }
}
