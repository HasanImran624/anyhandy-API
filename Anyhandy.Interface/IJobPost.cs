using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Interface
{
    public interface IJobPost
    {
        public void submitJobPost(MainServiceDTO jobDTO, int userID);
    }
}
