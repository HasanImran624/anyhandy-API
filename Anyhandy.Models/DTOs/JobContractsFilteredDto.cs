﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class JobContractsFilteredDto
    {
        public int JobContractId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string ContractName { get; set; }
        public string Image { get; set; }
        public string MileStoneTitle { get; set; }
        public DateTime? ContractDueDate { get; set; }
        public string ContractStatus { get; set; }
        public string MileStoneStatus { get; set; }
        public DateTime? MileStoneStartDate { get; set; }
        public DateTime? MileStoneDueDate { get; set; }
        public decimal? ContractAmount { get; set; }
        public decimal? ContractPaidAmount { get; set; }
        public decimal? MileStoneAmount { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Service { get; set; }
        public DateTime? ContractStartDate { get; set; }

    }
}