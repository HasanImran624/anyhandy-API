using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblPlumbingServicesAttachment
    {
        public int PlumbingServicesAttachmentsId { get; set; }
        public int? PlumbingServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblPlumbingService PlumbingService { get; set; }
    }
}
