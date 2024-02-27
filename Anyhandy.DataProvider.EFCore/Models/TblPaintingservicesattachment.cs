using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblPaintingServicesAttachment
    {
        public int PaintingServicesAttachmentsId { get; set; }
        public int? PaintingServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblPaintingService PaintingService { get; set; }
    }
}
