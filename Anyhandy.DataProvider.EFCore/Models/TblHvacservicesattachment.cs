using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblHvacservicesattachment
    {
        public int HvacServicesAttachmentsId { get; set; }
        public int? HvacServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblHvacservice HvacService { get; set; }
    }
}
