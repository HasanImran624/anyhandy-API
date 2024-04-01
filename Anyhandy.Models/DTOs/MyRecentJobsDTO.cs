using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.ViewModels
{
    public class MyRecentJobsDTO
    {
        public int JobID { get; set; }
        public int UserID { get; set; }
        public int MainServicesID { get; set; }
        public DateTime PostedDate { get; set; }
        public string JobTitle { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
        public string JobDetails { get; set; }
        public int JobAddressID { get; set; }
        public DateTime DueDate { get; set; }
        public string ServiceName { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }


    }
}
