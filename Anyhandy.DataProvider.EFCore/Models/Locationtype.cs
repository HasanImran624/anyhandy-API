using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Locationtype
    {
        public Locationtype()
        {
            TblHomecleanings = new HashSet<TblHomecleaning>();
            TblPestcontrolservices = new HashSet<TblPestcontrolservice>();
        }

        public int LocationTypeId { get; set; }
        public string LocationTypeName { get; set; }

        public virtual ICollection<TblHomecleaning> TblHomecleanings { get; set; }
        public virtual ICollection<TblPestcontrolservice> TblPestcontrolservices { get; set; }
    }
}
