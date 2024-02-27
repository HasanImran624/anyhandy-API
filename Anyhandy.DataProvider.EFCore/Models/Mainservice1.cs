using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class MainService1
    {
        public MainService1()
        {
            SubService1s = new HashSet<SubService1>();
        }

        public int MainServiceId { get; set; }
        public string MainServiceName { get; set; }

        public virtual ICollection<SubService1> SubService1s { get; set; }
    }
}
