using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Mainservice1
    {
        public Mainservice1()
        {
            Jobs = new HashSet<Job>();
            Subservice1s = new HashSet<Subservice1>();
            Userpackages = new HashSet<Userpackage>();
            Userprofileservices = new HashSet<Userprofileservice>();
        }

        public int MainServicesId { get; set; }
        public string ServiceNameEn { get; set; }
        public string ServiceNameAr { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
        public virtual ICollection<Subservice1> Subservice1s { get; set; }
        public virtual ICollection<Userpackage> Userpackages { get; set; }
        public virtual ICollection<Userprofileservice> Userprofileservices { get; set; }
    }
}
