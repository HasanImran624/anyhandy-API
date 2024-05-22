using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class JobContractCreateDto
    {
        public int JobId { get; set; }
        public int ProposalId { get; set; }
        public int UserId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? ContractDueDate { get; set; }
        public string JobDetails { get; set; }
        public string AdditionalTerms { get; set; }
        public List<ContractMilestoneCrateDto> MilestoneList { get; set; }
    }


    public class ContractMilestoneCrateDto
    {
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }
        public string Details { get; set; }
    }
}
