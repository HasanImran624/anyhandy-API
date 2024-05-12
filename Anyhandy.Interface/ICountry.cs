using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Interface
{
    public interface ICountry
    {
        Task<List<CountryVM>> GetCountriesWithCities();
    }
}
