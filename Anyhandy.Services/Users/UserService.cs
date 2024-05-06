using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface.User;
using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Anyhandy.Services.Users
{
    public class UserService : IUser
    {

        public void CreateUser(UserDTO userDTO)
        {
            using var userContext = new AnyHandyDBContext<User>();

            try
            {
                var lst = userContext.Jobs.ToList();

                var lstt = (from j in userContext.Jobs
                            join m in userContext.MainServices on j.MainServicesId equals m.MainServicesId
                           
                            select new UserDTO
                            {
                                Email = j.JobTitle
                            }

                            ).ToList();
                // Check if a user with the provided email already exists
                if (userContext.Users.Any(u => u.Email == userDTO.Email))
                {
                    // Throw an exception or handle the case as needed
                    throw new InvalidOperationException("User with the provided email already exists.");
                }

                string[] nameParts = userDTO.FullName.Split(' ');

                var userEntity = new User
                {
                    FirstName = nameParts[0],
                    LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty,
                    Paswword = userDTO.Password,
                    Email = userDTO.Email,
                    MobileNumber = userDTO.MobileNumber,
                    IsHandyman = userDTO.IsHandyman,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsActive = true

                    // Map other properties as needed
                };
                userContext.Save(userEntity);
            }
            catch (Exception ex)
            {
                // Handle the exception (log, return an error response, etc.)
                // For simplicity, rethrow the exception in this example
                throw;
            }
        }

        public LoginDetailsVM ValidateUserCredentials(UserDTO userDTO)
        {
            using var userContext = new AnyHandyDBContext<User>(); // Assuming your context is non-generic
            bool isValidCredentials = false;
            // Find a user with the provided email
            var userEntity = userContext.Users
                .FirstOrDefault(u => u.Email == userDTO.Email);

            // If a user with the provided email is found
            if (userEntity != null)
            {
                if(userDTO.IsHandyman)
                {
                    
                     isValidCredentials = (userEntity.Paswword == userDTO.Password && (bool)userEntity.IsHandyman);
                }
                else
                {
                    isValidCredentials = (userEntity.Paswword == userDTO.Password && !(bool)userEntity.IsHandyman);
                }
                // Compare the stored password with the provided password
                

                return new LoginDetailsVM { IsValidUser = isValidCredentials, UserName = userEntity.FirstName + " " + userEntity.LastName, UserId = userEntity.UserId, FirstName = userEntity.FirstName, LastName = userEntity.LastName };
            }

            // If no user with the provided email is found
            return new LoginDetailsVM { IsValidUser = isValidCredentials, UserName = string.Empty }; ;
        }


    }
}
