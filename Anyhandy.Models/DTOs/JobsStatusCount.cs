using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class JobsStatusCount
    {
        public int UserId { get; set; }
        public int MyJobsTotal { get; set; }
        public int ProposalsReceivedTotal { get; set; }
        public int ContractsTotal { get; set; }
        public int CancelledJobsTotal { get; set; }
    }
}
