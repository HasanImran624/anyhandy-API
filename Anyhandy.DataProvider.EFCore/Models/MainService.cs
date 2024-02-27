using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class MainService
    {
        public MainService()
        {
            Jobs = new HashSet<Job>();
            SubServices = new HashSet<SubService>();
            UserPackages = new HashSet<UserPackage>();
            UserProfileServices = new HashSet<UserProfileService>();
        }

        public int MainServicesId { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<SubService> SubServices { get; set; }
        public virtual ICollection<UserPackage> UserPackages { get; set; }
        public virtual ICollection<UserProfileService> UserProfileServices { get; set; }
    }
}
