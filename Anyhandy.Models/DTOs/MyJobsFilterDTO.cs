using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class MyJobsFilterDTO
    {
        public int JobID { get; set; }
        public int UserID { get; set; }
        public DateTime PostedDate { get; set; }
        public string JobTitle { get; set; }
        public string JobDetails { get; set; }
        public DateTime DueDate { get; set; }
        public string Service { get; set; }
        public int Status { get; set; }
        public int? TotalHired { get; set; }
        public int? TotalPropsals { get; set; }

        //public int TotalPages { get; set; }
    }
}
