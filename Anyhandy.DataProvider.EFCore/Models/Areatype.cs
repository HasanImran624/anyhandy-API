using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class AreaType
    {
        public AreaType()
        {
            TblHomeCleanings = new HashSet<TblHomeCleaning>();
        }

        public int AreaTypeId { get; set; }
        public string AreaTypeName { get; set; }

        public virtual ICollection<TblHomeCleaning> TblHomeCleanings { get; set; }
    }
}
