using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class SubService
    {
        public int SubServiceId { get; set; }
        public string SubServiceName { get; set; }
        public int? MainServiceId { get; set; }

        public virtual MainService MainService { get; set; }
    }
}
