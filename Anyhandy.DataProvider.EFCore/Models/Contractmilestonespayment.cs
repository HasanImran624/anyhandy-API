using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Contractmilestonespayment
    {
        public int ContractMilestonesPaymentId { get; set; }
        public int? MilestoneId { get; set; }
        public string PaymentReference { get; set; }
        public decimal Amount { get; set; }

        public virtual Contractmilestone Milestone { get; set; }
    }
}
