﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblAppliancerepairservice
    {
        public int ApplianceRepairServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubServiceId { get; set; }
        public string MoreDetailsDescription { get; set; }
        public string TypeAppliance { get; set; }
        public int? NumberItems { get; set; }

        public virtual Job Job { get; set; }
        public virtual Subservice1 SubService { get; set; }
    }
}
