using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblHomeCleaning
    {
        public TblHomeCleaning()
        {
            TblHomeCleaningAttachments = new HashSet<TblHomeCleaningAttachment>();
        }

        public int HomeCleaningServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubServiceId { get; set; }
        public string MoreDetailsDescription { get; set; }
        public int? LocationTypeId { get; set; }
        public int? NumberCleaner { get; set; }
        public int? NumberHours { get; set; }
        public bool? ProvideSupplies { get; set; }
        public int? AreaTypeId { get; set; }
        public int? TypeFurnitureId { get; set; }
        public int? NumberItems { get; set; }
        public string SizeItems { get; set; }

        public virtual AreaType AreaType { get; set; }
        public virtual Job Job { get; set; }
        public virtual LocationType LocationType { get; set; }
        public virtual SubService SubService { get; set; }
        public virtual TypeFurniture TypeFurniture { get; set; }
        public virtual ICollection<TblHomeCleaningAttachment> TblHomeCleaningAttachments { get; set; }
    }
}
