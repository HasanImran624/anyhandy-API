using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblHomecleaning
    {
        public TblHomecleaning()
        {
            TblHomecleaningattachments = new HashSet<TblHomecleaningattachment>();
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

        public virtual Areatype AreaType { get; set; }
        public virtual Job Job { get; set; }
        public virtual Locationtype LocationType { get; set; }
        public virtual Subservice1 SubService { get; set; }
        public virtual Typefurniture TypeFurniture { get; set; }
        public virtual ICollection<TblHomecleaningattachment> TblHomecleaningattachments { get; set; }
    }
}
