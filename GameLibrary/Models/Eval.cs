using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class Eval
    {
        public int EvalId { get; set; }
        public string EvalSubject { get; set; }
        public string EvalDescription { get; set; }

        public String ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
