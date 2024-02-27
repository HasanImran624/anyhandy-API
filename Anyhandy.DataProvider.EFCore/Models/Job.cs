using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Job
    {
        public Job()
        {
            Jobattachments = new HashSet<Jobattachment>();
            Jobcontracts = new HashSet<Jobcontract>();
            Jobproposals = new HashSet<Jobproposal>();
            Jobservices = new HashSet<Jobservice>();
            TblAppliancerepairservices = new HashSet<TblAppliancerepairservice>();
            TblCarpentryservices = new HashSet<TblCarpentryservice>();
            TblElectricalservices = new HashSet<TblElectricalservice>();
            TblGeneralservices = new HashSet<TblGeneralservice>();
            TblHomecleanings = new HashSet<TblHomecleaning>();
            TblHvacservices = new HashSet<TblHvacservice>();
            TblLandscapingservices = new HashSet<TblLandscapingservice>();
            TblPaintingservices = new HashSet<TblPaintingservice>();
            TblPestcontrolservices = new HashSet<TblPestcontrolservice>();
            TblPlumbingservices = new HashSet<TblPlumbingservice>();
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

        public virtual Useraddress JobAddress { get; set; }
        public virtual Mainservice1 MainServices { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Jobattachment> Jobattachments { get; set; }
        public virtual ICollection<Jobcontract> Jobcontracts { get; set; }
        public virtual ICollection<Jobproposal> Jobproposals { get; set; }
        public virtual ICollection<Jobservice> Jobservices { get; set; }
        public virtual ICollection<TblAppliancerepairservice> TblAppliancerepairservices { get; set; }
        public virtual ICollection<TblCarpentryservice> TblCarpentryservices { get; set; }
        public virtual ICollection<TblElectricalservice> TblElectricalservices { get; set; }
        public virtual ICollection<TblGeneralservice> TblGeneralservices { get; set; }
        public virtual ICollection<TblHomecleaning> TblHomecleanings { get; set; }
        public virtual ICollection<TblHvacservice> TblHvacservices { get; set; }
        public virtual ICollection<TblLandscapingservice> TblLandscapingservices { get; set; }
        public virtual ICollection<TblPaintingservice> TblPaintingservices { get; set; }
        public virtual ICollection<TblPestcontrolservice> TblPestcontrolservices { get; set; }
        public virtual ICollection<TblPlumbingservice> TblPlumbingservices { get; set; }
    }
}
