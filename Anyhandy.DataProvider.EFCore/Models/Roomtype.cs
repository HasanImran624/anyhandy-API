using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Roomtype
    {
        public Roomtype()
        {
            TblPestcontrolservices = new HashSet<TblPestcontrolservice>();
        }

        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }

        public virtual ICollection<TblPestcontrolservice> TblPestcontrolservices { get; set; }
    }
}
