using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblGeneralservice
    {
        public TblGeneralservice()
        {
            TblGeneralserviceattachments = new HashSet<TblGeneralserviceattachment>();
        }

        public int GeneralServiceId { get; set; }
        public int? JobId { get; set; }
        public string MoreDetailsDescription { get; set; }

        public virtual Job Job { get; set; }
        public virtual ICollection<TblGeneralserviceattachment> TblGeneralserviceattachments { get; set; }
    }
}
