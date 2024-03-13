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

namespace anyhandy_API.Controllers
{
    [ApiController]
    [Route("api/package")]
    public class PackageController : ControllerBase
    {
        readonly IUser _user;
        private readonly ILogger<PackageController> _logger;
        readonly IPackage _package;
        public PackageController(ILogger<PackageController> logger, IUser user, IPackage package)
        {
            _logger = logger;
            _user = user;
            _package = package;
        }

        [HttpGet("GetPackageDetailsByMainCategory/{mainCategoryId}")]
        public async Task<IActionResult> GetPackageDetailsByMainCategory(int mainCategoryId)
        {
            return Ok(await _package.GetPackageDetailsByMainCategory(mainCategoryId));
        }


      
    }
}