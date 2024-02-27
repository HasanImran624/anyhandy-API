using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblCarpentryservice
    {
        public TblCarpentryservice()
        {
            TblCarpentryservicesattachments = new HashSet<TblCarpentryservicesattachment>();
        }

        public int CarpentryServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Description { get; set; }
        public int? NumberOfItems { get; set; }

        public virtual Job Job { get; set; }
        public virtual Subservice1 SubCategory { get; set; }
        public virtual ICollection<TblCarpentryservicesattachment> TblCarpentryservicesattachments { get; set; }
    }
}
