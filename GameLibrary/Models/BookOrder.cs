using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class BookOrder
    {
        [Key, Column(Order = 0)]
        public int ISBN { get; set; }
        [Key, Column(Order = 1)]
        public int OrderID { get; set; }


        public Book Book { get; set; }
        public Order Order { get; set; }
    }
}
