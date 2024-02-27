using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class UserAddress
    {
        public UserAddress()
        {
            Jobs = new HashSet<Job>();
        }

        public int AddressId { get; set; }
        public string AddressType { get; set; }
        public int Country { get; set; }
        public int City { get; set; }
        public string Details { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
