using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblHvacService
    {
        public TblHvacService()
        {
            TblHvacservicesAttachments = new HashSet<TblHvacservicesAttachment>();
        }

        public int HvacServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubServiceId { get; set; }
        public string MoreDetailsDescription { get; set; }
        public int? NumberItems { get; set; }

        public virtual Job Job { get; set; }
        public virtual SubService SubService { get; set; }
        public virtual ICollection<TblHvacservicesAttachment> TblHvacservicesAttachments { get; set; }
    }
}
