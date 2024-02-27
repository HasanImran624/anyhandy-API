using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblCarpentryServicesAttachment
    {
        public int CarpentryServicesAttachmentsId { get; set; }
        public int? CarpentryServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblCarpentryService CarpentryService { get; set; }
    }
}
