using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class TypeFurniture
    {
        public TypeFurniture()
        {
            TblHomeCleanings = new HashSet<TblHomeCleaning>();
        }

        public int TypeFurnitureId { get; set; }
        public string TypeFurnitureName { get; set; }

        public virtual ICollection<TblHomeCleaning> TblHomeCleanings { get; set; }
    }
}
