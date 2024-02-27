using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Areatype
    {
        public Areatype()
        {
            TblHomecleanings = new HashSet<TblHomecleaning>();
        }

        public int AreaTypeId { get; set; }
        public string AreaTypeName { get; set; }

        public virtual ICollection<TblHomecleaning> TblHomecleanings { get; set; }
    }
}
