using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class CreateOrderViewModel
    {
        public int OrderID { get; set; }
        //public System.DateTime OrderDate { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }

        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        public SelectList DeliveryMethods { get; set; }
        public int DeliveryMethodID { get; set; }
        public SelectList PaymentMethods { get; set; }
        public int PaymentMethodID { get; set; }

        [Display(Name = "Credit Card Number")]
        public string CreditCardNo { get; set; }

        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Display(Name = "Cost")]
        public float OrderCost { get; set; }

        public DateTime OrderPlaced { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
