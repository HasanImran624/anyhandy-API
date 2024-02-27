using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblPaintingservice
    {
        public TblPaintingservice()
        {
            TblPaintingservicesattachments = new HashSet<TblPaintingservicesattachment>();
        }

        public int PaintingServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubCategoryId { get; set; }
        public string MoreDetailsDescription { get; set; }
        public int? NumberItems { get; set; }
        public decimal? SizeArea { get; set; }
        public bool? ProvidePaint { get; set; }
        public string PaintColor { get; set; }
        public int? NumberofCoats { get; set; }
        public string SpecialRequest { get; set; }

        public virtual Job Job { get; set; }
        public virtual Subservice1 SubCategory { get; set; }
        public virtual ICollection<TblPaintingservicesattachment> TblPaintingservicesattachments { get; set; }
    }
}
