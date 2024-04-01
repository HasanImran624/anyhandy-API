using Anyhandy.Models;
using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Interface.Dashboard
{
    public interface IDashboard
    {
        public List<MyRecentJobsDTO> GetMyJobsDetailsWithUserID(int userID);
        public List<MyJobsFilterDTO> GetMyJobsFilterWithUserID(int userID);

        public FilteredResponse<List<MyJobsFilterDTO>> GetSearchFilterWithSortBy(DateTime? JobStartDate, DateTime? JobEndDate, DateTime? DueStartDate, DateTime? DueEndDate, string SearchTxt, string OrderByType, int UserID, int PageNo, string SortByColumn, int RecordsPerPage, int ActiveType);
    }
}
