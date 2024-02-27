using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblPlumbingservice
    {
        public TblPlumbingservice()
        {
            TblPlumbingservicesattachments = new HashSet<TblPlumbingservicesattachment>();
        }

        public int PlumbingServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Description { get; set; }
        public int? NumberOfItems { get; set; }

        public virtual Job Job { get; set; }
        public virtual Subservice1 SubCategory { get; set; }
        public virtual ICollection<TblPlumbingservicesattachment> TblPlumbingservicesattachments { get; set; }
    }
}
