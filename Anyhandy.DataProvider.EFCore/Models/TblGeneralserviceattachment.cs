using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblGeneralServiceAttachment
    {
        public int AttachmentId { get; set; }
        public int? GeneralServiceServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblGeneralService GeneralServiceService { get; set; }
    }
}
