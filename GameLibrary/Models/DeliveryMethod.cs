using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    [Table("DeliveryMethod")]
    public class DeliveryMethod
    {
        [Key]
        public int DeliveryMethodID { get; set; }

        public string Name { get; set; }

        //public List<Order> Orders { get; set; }
    }
}
