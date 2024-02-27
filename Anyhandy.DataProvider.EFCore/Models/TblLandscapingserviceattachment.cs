using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblLandscapingServiceAttachment
    {
        public int LandscapingServiceAttachmentId { get; set; }
        public int? LandscapingServiceId { get; set; }
        public string AttachmentPath { get; set; }
        public string Description { get; set; }

        public virtual TblLandscapingService LandscapingService { get; set; }
    }
}
