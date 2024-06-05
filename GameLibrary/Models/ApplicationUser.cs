using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String Name { get; set; }
        public String BirthDate { get; set; }
        public String Address { get; set; }
        public String Gender { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Country { get; set; }
        

    }
}
