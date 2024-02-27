using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblPlumbingService
    {
        public TblPlumbingService()
        {
            TblPlumbingServicesAttachments = new HashSet<TblPlumbingServicesAttachment>();
        }

        public int PlumbingServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Description { get; set; }
        public int? NumberOfItems { get; set; }

        public virtual Job Job { get; set; }
        public virtual SubService SubCategory { get; set; }
        public virtual ICollection<TblPlumbingServicesAttachment> TblPlumbingServicesAttachments { get; set; }
    }
}
