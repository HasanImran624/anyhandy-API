using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblLandscapingservice
    {
        public TblLandscapingservice()
        {
            TblLandscapingserviceattachments = new HashSet<TblLandscapingserviceattachment>();
        }

        public int LandscapingServiceId { get; set; }
        public string LandscapingService { get; set; }
        public int? JobId { get; set; }
        public int? SubServicesId { get; set; }
        public string MoreDetailsDescription { get; set; }
        public decimal? AreaSize { get; set; }

        public virtual Job Job { get; set; }
        public virtual Subservice1 SubServices { get; set; }
        public virtual ICollection<TblLandscapingserviceattachment> TblLandscapingserviceattachments { get; set; }
    }
}
