using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblElectricalservicesattachment
    {
        public int ElectricalServicesAttachmentsId { get; set; }
        public int? ElectricalServiceId { get; set; }
        public string FilePath { get; set; }

        public virtual TblElectricalservice ElectricalService { get; set; }
    }
}
