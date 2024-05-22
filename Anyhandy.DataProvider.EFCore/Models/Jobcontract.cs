using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class JobContract
    {
        public JobContract()
        {
            ContractMilestones = new HashSet<ContractMilestone>();
        }

        public int JobContractId { get; set; }
        public int? JobId { get; set; }
        public int? SelectedHandyManId { get; set; }
        public int? JobProposalId { get; set; }
        public string JobDetails { get; set; }
        public string AdditionalTerms { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? ContractStart { get; set; }
        public DateTime? ContractEnd { get; set; }
        public short? Status { get; set; }
        public bool? IsMileStone { get; set; }
        public virtual Job Job { get; set; }
        public virtual JobProposal JobProposal { get; set; }
        public virtual User SelectedHandyMan { get; set; }
        public virtual ICollection<ContractMilestone> ContractMilestones { get; set; }
    }
}
