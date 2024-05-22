using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.ViewModels
{
    public class JobPostVM
    {
        public int JobId { get; set; }
        public string MainServiceName { get; set; }
        public LocationVM Location { get; set; } = new LocationVM();
        public List<SubServiceVM> SubServices { get; set; } = new List<SubServiceVM>();
    }

    public class SubServiceVM
    {
        public string Name { get; set; }
        public int id { get; set; }

    }
    public class LocationVM
    {
        public int AddressId { get; set; }

    }

}
