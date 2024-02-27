using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Jobproposal
    {
        public Jobproposal()
        {
            Jobcontracts = new HashSet<Jobcontract>();
            Jobproposalattachments = new HashSet<Jobproposalattachment>();
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
        public virtual ICollection<Jobcontract> Jobcontracts { get; set; }
        public virtual ICollection<Jobproposalattachment> Jobproposalattachments { get; set; }
    }
}
