using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblHomecleaningattachment
    {
        public int HomeCleaningAttachmentId { get; set; }
        public int? HomeCleaningServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblHomecleaning HomeCleaningService { get; set; }
    }
}
