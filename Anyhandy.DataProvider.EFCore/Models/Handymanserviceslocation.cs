using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class HandymanServicesLocation
    {
        public int ServiceLocationId { get; set; }
        public int? CityId { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
