using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models
{
    public class FilteredResponse<T>
    {
        public T Results { get; set; }
        public int TotalCount { get; set; }

    }
}
