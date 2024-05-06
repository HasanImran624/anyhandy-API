using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class UserType
    {
        public short UserTypeId { get; set; }
        public string UserTypeInfo { get; set; }
        public int? UserId { get; set; }
    }
}
