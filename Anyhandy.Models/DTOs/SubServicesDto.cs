using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.DTOs
{
    public class SubServicesDto
    {
        public string SubSrviceName { get; set; }
        public List<Item> ListItemData { get; set; }
    }



    public class Item
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
