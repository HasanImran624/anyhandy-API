using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface.Packages;
using Anyhandy.Interface.User;
using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Anyhandy.Services.Users
{
    public class PackagesService : IPackage
    {
        public async Task<List<PackageDetailVM>> GetPackageDetailsByMainCategory(int mainCategoryId)
        {
            using var userPackageContext = new AnyHandyDBContext<UserPackage>();

            using var userProfileRatingContext = new AnyHandyDBContext<UserProfileRating>();
            var packages = await userPackageContext.UserPackages
           .Where(p => p.MainCategoryId == mainCategoryId && p.Active)
           .Include(p => p.HandymanUser.UserProfileRatingUsers) // Assuming there's a navigation property for the HandymanUser
           .Select(p => new PackageDetailVM
           {
               Title = p.Title,
               Description = p.Description,
               Price = p.Price,
               UserName = p.HandymanUser.FirstName + " " + p.HandymanUser.LastName,
               Rating = p.HandymanUser.UserProfileRatingUsers
                            .Where(r => r.UserId == p.HandymanUserId)
                            .Average(r => r.Rating)
           })
           .ToListAsync();

            return (packages);
        }

    }
}
