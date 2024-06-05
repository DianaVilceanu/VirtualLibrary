using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class SCartItem
    {
        public int SCartItemId { get; set; }
        public Book Book { get; set; }

        public int Amount { get; set; }
        public string SCartId { get; set; }
    }
}
