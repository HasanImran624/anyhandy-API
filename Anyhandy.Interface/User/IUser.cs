using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
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
        LoginDetailsVM ValidateUserCredentials(UserDTO userDTO);
    }
}
