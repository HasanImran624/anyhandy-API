using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Userpackage
    {
        public Userpackage()
        {
            Userpackagepurchaserequests = new HashSet<Userpackagepurchaserequest>();
        }

        public int PackageId { get; set; }
        public int? HandymanUserId { get; set; }
        public string Title { get; set; }
        public int? MainCategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public byte? AutoPurchase { get; set; }

        public virtual User HandymanUser { get; set; }
        public virtual Mainservice1 MainCategory { get; set; }
        public virtual Subservice1 SubCategory { get; set; }
        public virtual ICollection<Userpackagepurchaserequest> Userpackagepurchaserequests { get; set; }
    }
}
