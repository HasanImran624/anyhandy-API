using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            TblPestControlServices = new HashSet<TblPestControlService>();
        }

        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }

        public virtual ICollection<TblPestControlService> TblPestControlServices { get; set; }
    }
}
