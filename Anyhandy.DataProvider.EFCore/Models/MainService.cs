using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class MainService
    {
        public MainService()
        {
            SubServices = new HashSet<SubService>();
        }

        public int MainServiceId { get; set; }
        public string MainServiceName { get; set; }

        public virtual ICollection<SubService> SubServices { get; set; }
    }
}
