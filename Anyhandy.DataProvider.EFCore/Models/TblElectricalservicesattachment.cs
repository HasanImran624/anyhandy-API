using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblElectricalServicesAttachment
    {
        public int ElectricalServicesAttachmentsId { get; set; }
        public int? ElectricalServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblElectricalService ElectricalService { get; set; }
    }
}
