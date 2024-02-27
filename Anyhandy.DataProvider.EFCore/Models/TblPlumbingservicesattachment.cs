using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblPlumbingservicesattachment
    {
        public int PlumbingServicesAttachmentsId { get; set; }
        public int? PlumbingServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblPlumbingservice PlumbingService { get; set; }
    }
}
