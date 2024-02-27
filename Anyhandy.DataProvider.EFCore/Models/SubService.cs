using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class SubService
    {
        public SubService()
        {
            JobServices = new HashSet<JobService>();
            TblApplianceRepairServices = new HashSet<TblApplianceRepairService>();
            TblCarpentryServices = new HashSet<TblCarpentryService>();
            TblElectricalServices = new HashSet<TblElectricalService>();
            TblHomeCleanings = new HashSet<TblHomeCleaning>();
            TblHvacServices = new HashSet<TblHvacService>();
            TblLandscapingServices = new HashSet<TblLandscapingService>();
            TblPaintingServices = new HashSet<TblPaintingService>();
            TblPestControlServices = new HashSet<TblPestControlService>();
            TblPlumbingServices = new HashSet<TblPlumbingService>();
            UserPackages = new HashSet<UserPackage>();
            UserProfileServices = new HashSet<UserProfileService>();
        }

        public int SubServicesId { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }
        public int MainServicesId { get; set; }

        public virtual MainService MainServices { get; set; }
        public virtual ICollection<JobService> JobServices { get; set; }
        public virtual ICollection<TblApplianceRepairService> TblApplianceRepairServices { get; set; }
        public virtual ICollection<TblCarpentryService> TblCarpentryServices { get; set; }
        public virtual ICollection<TblElectricalService> TblElectricalServices { get; set; }
        public virtual ICollection<TblHomeCleaning> TblHomeCleanings { get; set; }
        public virtual ICollection<TblHvacService> TblHvacServices { get; set; }
        public virtual ICollection<TblLandscapingService> TblLandscapingServices { get; set; }
        public virtual ICollection<TblPaintingService> TblPaintingServices { get; set; }
        public virtual ICollection<TblPestControlService> TblPestControlServices { get; set; }
        public virtual ICollection<TblPlumbingService> TblPlumbingServices { get; set; }
        public virtual ICollection<UserPackage> UserPackages { get; set; }
        public virtual ICollection<UserProfileService> UserProfileServices { get; set; }
    }
}
