using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class SubService1
    {
        public int SubServiceId { get; set; }
        public string SubServiceName { get; set; }
        public int? MainServiceId { get; set; }

        public virtual MainService1 MainService { get; set; }
    }
}
