using Anyhandy.Interface.User;
using Anyhandy.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Anyhandy.Common;
using Anyhandy.Models.ViewModels;
using Anyhandy.DataProvider.EFCore.Models;

namespace anyhandy_API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        readonly IUser _user;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUser user)
        {
            _logger = logger;
            _user = user;
        }


        [HttpPost("Sign-up")]
        public IActionResult Signup([FromBody] UserDTO user)
        {
            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _user.CreateUser(user);
                return Ok(new { Message = "User signed up successfully!" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("healthcheck")]
        public IActionResult HealthCheck()
        {
            return Ok("Server is healthy!");
        }


        [HttpPost("validate-login")]
        public IActionResult ValidateLogin([FromBody] UserDTO user)
        {
            
            // Validate user input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate user credentials (add your authentication logic here)
            LoginDetailsVM loginDetailsVM = _user.ValidateUserCredentials(user);

            if (loginDetailsVM.IsValidUser)
            {
                var tokenString = GenerateJSONWebToken(user, loginDetailsVM.UserId);
                return Ok(new { Message = "Login successful!", token = tokenString, username = loginDetailsVM.UserName,
                    UserId = loginDetailsVM.UserId, firstNmae = loginDetailsVM.FirstName, lastName = loginDetailsVM.LastName });
            }
            else
            {
                // Invalid credentials
                return Unauthorized(new { Message = "Invalid username or password." });
            }
        }

        private string GenerateJSONWebToken(UserDTO user, int UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigrationManager.AppSettings["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Sub, user.FullName),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
         new Claim("UserId", UserId.ToString()),
         new Claim(JwtRegisteredClaimNames.Aud, "web-api"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
         };

            var token = new JwtSecurityToken(AppConfigrationManager.AppSettings["Jwt:Issuer"],
                AppConfigrationManager.AppSettings["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromHeader] string authorization)
        {
            if (string.IsNullOrEmpty(authorization))
            {
                return BadRequest("Authorization header missing");
            }

            if (!authorization.StartsWith("Bearer "))
            {
                return BadRequest("Invalid authorization header format");
            }

            var tokenString = authorization.Substring("Bearer ".Length);

            try
            {
                var principal = ValidateTokenString(tokenString);
                if (principal != null)
                {
                    // Token is valid, extract user information from claims
                    var userId = principal.FindFirstValue("UserId");

                    // You can potentially return additional information based on user claims
                    return Ok(new { Message = "Token is valid", UserId = userId });
                }
                else
                {
                    return Unauthorized("Invalid token");
                }
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }

        private ClaimsPrincipal ValidateTokenString(string tokenString)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigrationManager.AppSettings["Jwt:Key"])),
                ValidateIssuer = true,
                ValidIssuer = AppConfigrationManager.AppSettings["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = AppConfigrationManager.AppSettings["Jwt:Audience"],
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out SecurityToken validatedToken);
            return securityToken;
        }

    }
}