using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class JobProposalDetailsDto
    {
       
        public int JobId { get; set; }
        public int PorposalId { get; set; }
        public string? JobTitle { get; set; }
        public string? ContractorName { get; set; }     
        public int UserId { get; set; }
        public string? Service { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? JobDueDate { get; set; }
        public int ServiceId { get; set; }
        public List<SubServicesDto> SubServiceList { get; set; }
        public string? JobDetails { get; set; }

    }
}
