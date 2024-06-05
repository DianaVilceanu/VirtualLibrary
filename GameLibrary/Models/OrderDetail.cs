using System;
using System.Collections.Generic;

namespace GameLibrary.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int BookID { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public virtual Book Book { get; set; }
        public virtual Order Order { get; set; }

        //public String ApplicationUserID { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        //public int BookOrderID { get; set; }
        public ICollection<BookOrder> BookOrders { get; set; }
    }
}