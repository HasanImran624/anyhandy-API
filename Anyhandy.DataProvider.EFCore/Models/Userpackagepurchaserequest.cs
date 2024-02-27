using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class UserPackagePurchaseRequest
    {
        public int PackageRequestId { get; set; }
        public int? PackageId { get; set; }
        public int? UserId { get; set; }
        public int? StatusId { get; set; }
        public string MessageText { get; set; }
        public TimeSpan? TimeSlotFrom { get; set; }
        public TimeSpan? TimeSlotTo { get; set; }
        public DateTime? CreateDatetime { get; set; }
        public DateTime? UpdatedDatetime { get; set; }

        public virtual UserPackage Package { get; set; }
        public virtual User User { get; set; }
    }
}
