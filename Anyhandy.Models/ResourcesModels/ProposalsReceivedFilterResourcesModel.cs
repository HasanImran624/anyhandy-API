using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.ResourcesModels
{
    public class ProposalsReceivedFilterResourcesModel
    {
        public int JobID { get; set; }
        public int ProposalId { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Service { get; set; }
        public DateTime LocalTime { get; set; }
        public string JobTitle { get; set; }
        public string JobDetails { get; set; }
        public DateTime ReceivedDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string UserTypeInfo { get; set; }

    }
}
