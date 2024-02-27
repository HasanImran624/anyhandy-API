using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Jobservice
    {
        public int JobServiceId { get; set; }
        public int? JobId { get; set; }
        public int? JobSubServiceId { get; set; }
        public string Description { get; set; }

        public virtual Job Job { get; set; }
        public virtual Subservice1 JobSubService { get; set; }
    }
}
