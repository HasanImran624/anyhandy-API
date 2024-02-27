using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Subservice1
    {
        public Subservice1()
        {
            Jobservices = new HashSet<Jobservice>();
            TblAppliancerepairservices = new HashSet<TblAppliancerepairservice>();
            TblCarpentryservices = new HashSet<TblCarpentryservice>();
            TblElectricalservices = new HashSet<TblElectricalservice>();
            TblHomecleanings = new HashSet<TblHomecleaning>();
            TblHvacservices = new HashSet<TblHvacservice>();
            TblLandscapingservices = new HashSet<TblLandscapingservice>();
            TblPaintingservices = new HashSet<TblPaintingservice>();
            TblPestcontrolservices = new HashSet<TblPestcontrolservice>();
            TblPlumbingservices = new HashSet<TblPlumbingservice>();
            Userpackages = new HashSet<Userpackage>();
            Userprofileservices = new HashSet<Userprofileservice>();
        }

        public int SubServicesId { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
        public int MainServicesId { get; set; }

        public virtual Mainservice1 MainServices { get; set; }
        public virtual ICollection<Jobservice> Jobservices { get; set; }
        public virtual ICollection<TblAppliancerepairservice> TblAppliancerepairservices { get; set; }
        public virtual ICollection<TblCarpentryservice> TblCarpentryservices { get; set; }
        public virtual ICollection<TblElectricalservice> TblElectricalservices { get; set; }
        public virtual ICollection<TblHomecleaning> TblHomecleanings { get; set; }
        public virtual ICollection<TblHvacservice> TblHvacservices { get; set; }
        public virtual ICollection<TblLandscapingservice> TblLandscapingservices { get; set; }
        public virtual ICollection<TblPaintingservice> TblPaintingservices { get; set; }
        public virtual ICollection<TblPestcontrolservice> TblPestcontrolservices { get; set; }
        public virtual ICollection<TblPlumbingservice> TblPlumbingservices { get; set; }
        public virtual ICollection<Userpackage> Userpackages { get; set; }
        public virtual ICollection<Userprofileservice> Userprofileservices { get; set; }
    }
}
