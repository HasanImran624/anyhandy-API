using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Job
    {
        public Job()
        {
            JobAttachments = new HashSet<JobAttachment>();
            JobContracts = new HashSet<JobContract>();
            JobProposals = new HashSet<JobProposal>();
            JobServices = new HashSet<JobService>();
            TblApplianceRepairServices = new HashSet<TblApplianceRepairService>();
            TblCarpentryServices = new HashSet<TblCarpentryService>();
            TblElectricalServices = new HashSet<TblElectricalService>();
            TblGeneralServices = new HashSet<TblGeneralService>();
            TblHomeCleanings = new HashSet<TblHomeCleaning>();
            TblHvacServices = new HashSet<TblHvacService>();
            TblLandscapingServices = new HashSet<TblLandscapingService>();
            TblPaintingServices = new HashSet<TblPaintingService>();
            TblPestControlServices = new HashSet<TblPestControlService>();
            TblPlumbingServices = new HashSet<TblPlumbingService>();
        }

        public int JobId { get; set; }
        public int? MainServicesId { get; set; }
        public DateTime? PostedDate { get; set; }
        public decimal? Amount { get; set; }
        public int? Status { get; set; }
        public string JobTitle { get; set; }
        public string JobDetails { get; set; }
        public int? UserId { get; set; }
        public int? JobAddressId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? JobDue { get; set; }
        public DateTime? JobExpectedEnd { get; set; }

        public virtual UserAddress JobAddress { get; set; }
        public virtual MainService MainServices { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<JobAttachment> JobAttachments { get; set; }
        public virtual ICollection<JobContract> JobContracts { get; set; }
        public virtual ICollection<JobProposal> JobProposals { get; set; }
        public virtual ICollection<JobService> JobServices { get; set; }
        public virtual ICollection<TblApplianceRepairService> TblApplianceRepairServices { get; set; }
        public virtual ICollection<TblCarpentryService> TblCarpentryServices { get; set; }
        public virtual ICollection<TblElectricalService> TblElectricalServices { get; set; }
        public virtual ICollection<TblGeneralService> TblGeneralServices { get; set; }
        public virtual ICollection<TblHomeCleaning> TblHomeCleanings { get; set; }
        public virtual ICollection<TblHvacService> TblHvacServices { get; set; }
        public virtual ICollection<TblLandscapingService> TblLandscapingServices { get; set; }
        public virtual ICollection<TblPaintingService> TblPaintingServices { get; set; }
        public virtual ICollection<TblPestControlService> TblPestControlServices { get; set; }
        public virtual ICollection<TblPlumbingService> TblPlumbingServices { get; set; }
    }
}
