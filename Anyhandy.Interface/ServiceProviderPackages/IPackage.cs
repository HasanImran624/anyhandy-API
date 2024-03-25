using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Interface.Packages
{


    public interface IPackage
    {
        Task<List<PackageDetailVM>> GetPackageDetailsByMainCategory(int mainCategoryId);
    }
}
