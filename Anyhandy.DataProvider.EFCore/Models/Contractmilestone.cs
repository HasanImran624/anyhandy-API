using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class ContractMilestone
    {
        public ContractMilestone()
        {
            ContractMilestonesPayments = new HashSet<ContractMilestonesPayment>();
        }

        public int MilestoneId { get; set; }
        public int? JobContractId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Amount { get; set; }
        public string Details { get; set; }
        public int? Status { get; set; }

        public virtual JobContract JobContract { get; set; }
        public virtual ICollection<ContractMilestonesPayment> ContractMilestonesPayments { get; set; }
    }
}
