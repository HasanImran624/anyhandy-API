using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.ResourcesModels
{
    public class MyJobsFilterResourcesModel
    {
        public int JobID { get; set; }
        public int UserID { get; set; }
        public DateTime PostedDate { get; set; }
        public string JobTitle { get; set; }
        public string JobDetails { get; set; }
        public DateTime DueDate { get; set; }
        public string Service { get; set; }
        public int Status { get; set; }
    }
}
