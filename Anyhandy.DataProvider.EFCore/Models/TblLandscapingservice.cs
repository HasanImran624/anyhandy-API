using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblLandscapingService
    {
        public TblLandscapingService()
        {
            TblLandscapingServiceAttachments = new HashSet<TblLandscapingServiceAttachment>();
        }

        public int LandscapingServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubServicesId { get; set; }
        public string MoreDetailsDescription { get; set; }
        public decimal? AreaSize { get; set; }

        public virtual Job Job { get; set; }
        public virtual SubService SubServices { get; set; }
        public virtual ICollection<TblLandscapingServiceAttachment> TblLandscapingServiceAttachments { get; set; }
    }
}
