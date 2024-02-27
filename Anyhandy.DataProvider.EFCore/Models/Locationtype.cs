using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class LocationType
    {
        public LocationType()
        {
            TblHomeCleanings = new HashSet<TblHomeCleaning>();
            TblPestControlServices = new HashSet<TblPestControlService>();
        }

        public int LocationTypeId { get; set; }
        public string LocationTypeName { get; set; }

        public virtual ICollection<TblHomeCleaning> TblHomeCleanings { get; set; }
        public virtual ICollection<TblPestControlService> TblPestControlServices { get; set; }
    }
}
