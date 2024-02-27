using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblGeneralserviceattachment
    {
        public int AttachmentId { get; set; }
        public int? GeneralServiceServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblGeneralservice GeneralServiceService { get; set; }
    }
}
