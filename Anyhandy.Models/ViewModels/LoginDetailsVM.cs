using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Models.ViewModels
{
    public class LoginDetailsVM
    {
        public bool IsValidUser { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
    }
}
