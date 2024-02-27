using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblElectricalService
    {
        public TblElectricalService()
        {
            TblElectricalServicesAttachments = new HashSet<TblElectricalServicesAttachment>();
        }

        public int ElectricalServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Description { get; set; }
        public int? NumberOfItems { get; set; }

        public virtual Job Job { get; set; }
        public virtual SubService SubCategory { get; set; }
        public virtual ICollection<TblElectricalServicesAttachment> TblElectricalServicesAttachments { get; set; }
    }
}
