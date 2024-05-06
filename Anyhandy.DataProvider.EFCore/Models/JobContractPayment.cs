using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class JobContractPayment
    {
        public int JobContractPaymentId { get; set; }
        public int? JobContractId { get; set; }
        public int? JobId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BonusAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
