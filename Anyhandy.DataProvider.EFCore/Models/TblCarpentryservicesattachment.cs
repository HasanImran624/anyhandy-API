using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblCarpentryservicesattachment
    {
        public int CarpentryServicesAttachmentsId { get; set; }
        public int? CarpentryServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblCarpentryservice CarpentryService { get; set; }
    }
}
