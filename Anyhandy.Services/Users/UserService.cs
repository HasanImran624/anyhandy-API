using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface.User;
using Anyhandy.Models.DTOs;
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

        public bool ValidateUserCredentials(UserDTO userDTO)
        {
            using var userContext = new AnyHandyDBContext<User>(); // Assuming your context is non-generic

            // Find a user with the provided email
            var userEntity = userContext.Users
                .FirstOrDefault(u => u.Email == userDTO.Email);

            // If a user with the provided email is found
            if (userEntity != null)
            {
                // Compare the stored password with the provided password
                bool isValidCredentials = userEntity.Paswword == userDTO.Password;

                return isValidCredentials;
            }

            // If no user with the provided email is found
            return false;
        }


    }
}
