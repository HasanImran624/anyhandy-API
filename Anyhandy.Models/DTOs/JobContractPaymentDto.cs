using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class JobContractPaymentDto
    {
        public int? JobId { get; set; }
        public int ContractId { get; set; }
        public decimal? Amount { get; set; }
        public int MilestoneId { get; set; }
        public string PaymentReference { get; set; }

    }
}
