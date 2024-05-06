using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class MyHeroJobsDTO
    {
        public int JobID { get; set; }
        public int UserID { get; set; }
        public int MainServicesID { get; set; }
        public string? ServiceName { get; set; }
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public int JobsCompleted { get; set; }
        public float Rating { get; set; }
    }
}
