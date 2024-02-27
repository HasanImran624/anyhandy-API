using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblLandscapingserviceattachment
    {
        public int LandscapingServiceAttachmentId { get; set; }
        public int? LandscapingServiceId { get; set; }
        public string AttachmentPath { get; set; }
        public string Description { get; set; }

        public virtual TblLandscapingservice LandscapingService { get; set; }
    }
}
