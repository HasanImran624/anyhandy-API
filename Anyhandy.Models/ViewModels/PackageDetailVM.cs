using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.ViewModels
{
    public class PackageDetailVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserName { get; set; }
        public float Rating { get; set; }
    }
}
