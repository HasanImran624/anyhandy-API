using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.ViewModels
{
    public class CityVM
    {
        public string Name { get; set; }
    }

    public class CountryVM
    {
        public string Name { get; set; }
        public List<CityVM> Cities { get; set; }
    }

}
