using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Typefurniture
    {
        public Typefurniture()
        {
            TblHomecleanings = new HashSet<TblHomecleaning>();
        }

        public int TypeFurnitureId { get; set; }
        public string TypeFurnitureName { get; set; }

        public virtual ICollection<TblHomecleaning> TblHomecleanings { get; set; }
    }
}
