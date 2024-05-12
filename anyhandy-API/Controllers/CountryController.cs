using Anyhandy.Interface.User;
using Anyhandy.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Anyhandy.Common;
using Microsoft.EntityFrameworkCore;
using Anyhandy.Interface.Packages;
using Anyhandy.Interface;

namespace anyhandy_API.Controllers
{
    [ApiController]
    [Route("api/country")]
    public class CountryController : ControllerBase
    {
        readonly IUser _user;
        private readonly ILogger<PackageController> _logger;
        readonly ICountry _country;
        public CountryController(ILogger<PackageController> logger, ICountry country)
        {
            _logger = logger;
            _country = country;
        }

        [HttpGet("cities-info")]
        public async Task<IActionResult> GetCountriesWithCities()
        {
            return Ok(await _country.GetCountriesWithCities());
        }


      
    }
}