using Anyhandy.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Interface.User
{
    public interface IUser
    {
        void CreateUser(UserDTO userDTO);
        bool ValidateUserCredentials(UserDTO userDTO);
    }
}
