using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblHvacservicesAttachment
    {
        public int HvacServicesAttachmentsId { get; set; }
        public int? HvacServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblHvacService HvacService { get; set; }
    }
}
