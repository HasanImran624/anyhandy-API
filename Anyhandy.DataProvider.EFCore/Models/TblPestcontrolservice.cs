using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TblPestcontrolservice
    {
        public int PestControlServiceId { get; set; }
        public int? JobId { get; set; }
        public int? SubServiceId { get; set; }
        public string MoreDetailsDescription { get; set; }
        public int? NumberRooms { get; set; }
        public int? LocationTypeId { get; set; }
        public decimal? LocationSizeId { get; set; }
        public int? RoomTypeId { get; set; }

        public virtual Job Job { get; set; }
        public virtual Locationtype LocationType { get; set; }
        public virtual Roomtype RoomType { get; set; }
        public virtual Subservice1 SubService { get; set; }
    }
}
