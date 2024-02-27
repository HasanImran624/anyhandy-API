using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class UserProfileService
    {
        public int UserProfileServiceId { get; set; }
        public int? UserId { get; set; }
        public int? MainServiceId { get; set; }
        public int? SubCategoryId { get; set; }

        public virtual MainService MainService { get; set; }
        public virtual SubService SubCategory { get; set; }
        public virtual User User { get; set; }
    }
}
