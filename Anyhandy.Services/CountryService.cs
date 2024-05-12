using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface;
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
    public class CountryService : ICountry
    {

        public async Task<List<CountryVM>> GetCountriesWithCities()
        {

            using var countryContext = new AnyHandyDBContext<Country>();
            return await countryContext.Countries
            .Include(c => c.Cities)
            .Select(c => new CountryVM
            {
                Name = c.Name,
                Cities = c.Cities.Select(city => new CityVM { Name = city.Name }).ToList()
            })
            .ToListAsync();
        }

    }
}
