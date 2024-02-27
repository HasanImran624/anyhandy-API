using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Userprofileservice
    {
        public int UserProfileServiceId { get; set; }
        public int? UserId { get; set; }
        public int? MainServiceId { get; set; }
        public int? SubCategoryId { get; set; }

        public virtual Mainservice1 MainService { get; set; }
        public virtual Subservice1 SubCategory { get; set; }
        public virtual User User { get; set; }
    }
}
