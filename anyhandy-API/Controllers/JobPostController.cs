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
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace anyhandy_API.Controllers
{
    [ApiController]
    [Route("api/job")]
    public class JobPostController : ControllerBase
    {
        readonly IUser _user;
        private readonly ILogger<PackageController> _logger;
        readonly IJobPost _jobPost;
        public JobPostController(ILogger<PackageController> logger, IUser user, IJobPost jobPost)
        {
            _logger = logger;
            _user = user;
            _jobPost = jobPost;
        }

        //[Authorize]
        [HttpPost]
        [Route("submit-job-post")]
        public async Task<IActionResult> SubmitJobPost(IFormCollection formdata)
        {

            var userID = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var files = HttpContext.Request.Form.Files;
            var value = formdata.TryGetValue("form_attributes", out var values) ? values.FirstOrDefault() : null;
            MainServiceDTO jobDTO = JsonConvert.DeserializeObject<MainServiceDTO>(value);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileNameParts = Path.GetFileNameWithoutExtension(file.FileName).Split('_');

                    if (fileNameParts.Length >= 2) //
                    {
                        var code = fileNameParts[0];
                        var uuid = fileNameParts[1];
                        var subService = jobDTO?.SubServices.FirstOrDefault(s => s.Code == code && s.uuid == uuid);

                        if (subService != null)
                        {
                            // first add job and location
                            var subServiceImage = new SubServiceImage
                            {
                                ContentType = file.ContentType,
                                FileName = file.Name,

                                Content = await ReadFileContentAsByteArray(file)
                            };
                            subService.Images.Add(subServiceImage);
                        }
                    }
                }
            }

            _jobPost.submitJobPost(jobDTO, 25);
          


            return Ok("Job post submitted successfully");
        }

        private async Task<byte[]> ReadFileContentAsByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }


    }
}