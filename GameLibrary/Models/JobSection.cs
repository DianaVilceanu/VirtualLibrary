using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public class JobSection
    {
        public int JobSectionID { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }


        public virtual ICollection<JobApplication> JobApplications { get; set; }
        
    }
}
