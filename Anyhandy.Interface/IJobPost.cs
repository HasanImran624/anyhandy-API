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
        public JobPostVM submitJobPost(MainServiceDTO jobDTO, int userID);
        public bool IsJobPostExistsById(int id);
        public void editJobPost(MainServiceDTO jobDTO, int userID, int jobId);
    }
}
