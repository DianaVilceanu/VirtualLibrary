using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class JobApplication
    {
        public int JobApplicationId { get; set; }
        public string Nume { get; set; }
        public string Ocupatie { get; set; }
        public string Telefon { get; set; }

        public int JobSectionId { get; set; }
        public virtual JobSection JobSection { get; set; }
    }
}
