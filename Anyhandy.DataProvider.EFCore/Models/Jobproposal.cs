using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class JobProposal
    {
        public JobProposal()
        {
            JobContracts = new HashSet<JobContract>();
            JobProposalAttachments = new HashSet<JobProposalAttachment>();
        }

        public int JobProposalId { get; set; }
        public int? JobId { get; set; }
        public int? UserId { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? JobExpectedEnd { get; set; }
        public DateTime? JobExpectedStart { get; set; }

        public virtual Job Job { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<JobContract> JobContracts { get; set; }
        public virtual ICollection<JobProposalAttachment> JobProposalAttachments { get; set; }
    }
}
