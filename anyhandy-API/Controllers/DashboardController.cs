using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface.Dashboard;
using Anyhandy.Interface.User;
using Anyhandy.Models;
using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace anyhandy_API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        readonly IDashboard _dashboard;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger, IDashboard dashboard)
        {
            _logger = logger;
            _dashboard = dashboard;
        }

        [HttpGet("GetMyJobsDetailsWithUserID")]
        public IActionResult GetMyJobsDetailsWithUserID(int UserID = 40)
        {
            try
            {
                List<MyRecentJobsDTO> data = _dashboard.GetMyJobsDetailsWithUserID(UserID);
                List<MyHeroJobsDTO> lst = _dashboard.GetMyHeroListWithUserID(UserID);
                return Ok(new { Message = "success", Data = data, HeroList = lst });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetMyJobsFilterWithUserID")]
        public IActionResult GetMyJobsFilterWithUserID(int UserID = 40, int PerPage = 10)
        {
            try
            {
                int totalPages = 0;

                List<MyJobsFilterDTO> data = _dashboard.GetMyJobsFilterWithUserID(UserID);
                int recordsPerPage = PerPage; // Number of records per page

                if (data.Count > 0)
                {
                    int totalRecords = data.Count;
                    totalPages = totalRecords / recordsPerPage;

                    if (totalRecords % recordsPerPage != 0)
                    {
                        totalPages++;
                    }
                }


                return Ok(new { Message = "success", Data = data, TotalPages = totalPages, TotalRecord = data.Count });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("GetSearchFilterWithSortBy")]
        public IActionResult GetSearchFilterWithSortBy(int UserID, int PageNo, DateTime? JobStartDate, DateTime? JobEndDate, DateTime? DueStartDate, DateTime? DueEndDate, string? SearchTxt, string OrderByType, string SortByColumn, int RecordsPerPage, int ActiveType)
        {
            try
            {
                int totalPages = 0;
                int recordsPerPage = 10;

                FilteredResponse<List<MyJobsFilterDTO>> data = _dashboard.GetSearchFilterWithSortBy(JobStartDate, JobEndDate, DueStartDate, DueEndDate, SearchTxt, OrderByType, UserID, PageNo, SortByColumn, RecordsPerPage, ActiveType);

                if (data.Results.Count > 0)
                {
                    int totalRecords = data.Results.Count;
                    totalPages = totalRecords / recordsPerPage;

                    if (totalRecords % recordsPerPage != 0)
                    {
                        totalPages++;
                    }
                }

                return Ok(new { Message = "success", Data = data, TotalPages = totalPages, TotalRecord = data.Results.Count });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetPropasalsReceviedSearchFilterWithSortBy")]
        public IActionResult GetPropasalsReceviedSearchFilterWithSortBy(int UserID, int PageNo, string SearchTxt, string OrderByType, string SortByColumn, int RecordsPerPage, string ActiveType)
        {
            try
            {
                int totalPages = 0;
                int recordsPerPage = 10;

                FilteredResponse<List<ProposalsReceivedFilterDTO>> data = _dashboard.GetProposalsReceivedWithSortBy(SearchTxt, OrderByType, UserID, PageNo, SortByColumn, RecordsPerPage, ActiveType);

                if (data.Results.Count > 0)
                {
                    int totalRecords = data.Results.Count;
                    totalPages = totalRecords / recordsPerPage;

                    if (totalRecords % recordsPerPage != 0)
                    {
                        totalPages++;
                    }
                }

                return Ok(new { Message = "success", Data = data, TotalPages = totalPages, TotalRecord = data.TotalCount });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("GetAllJobsStatusWithUserID")]
        public IActionResult GetAllJobsStatusWithUserID(int UserID = 39)
        {
            try
            {
                JobsStatusCount data = _dashboard.GetAllJobsStatusWithUserID(UserID);
                return Ok(new { Message = "success", Data = data });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("DeleteCancelJob")]
        public IActionResult DeleteCancelJob(int jobId, int status)
        {
            try
            {
                var resp = _dashboard.DeleteCancelJob(jobId, status);
                return Ok(new { Message = resp, IsSuccess = resp == "success" ? true : false });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("UpdateProposalStatus")]
        public IActionResult UpdateProposalStatus(int proposalId, string status)
        {
            try
            {
                var resp = _dashboard.UpdateProposalStatus(proposalId, status);
                return Ok(new { Message = resp, IsSuccess = resp == "success" ? true : false });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }



        [HttpGet("GetContractSearchFilterWithSortBy")]
        public IActionResult GetContractSearchFilterWithSortBy(int UserID, int PageNo, string SearchTxt, string OrderByType, string SortByColumn, int RecordsPerPage, int ActiveType)
        {
            try
            {
                int totalPages = 0;
                int recordsPerPage = 10;

                var data = _dashboard.GetContractSearchFilterWithSortBy(SearchTxt, OrderByType, UserID, PageNo, SortByColumn, RecordsPerPage, ActiveType);

                if (data.Results.Count > 0)
                {
                    int totalRecords = data.Results.Count;
                    totalPages = totalRecords / recordsPerPage;

                    if (totalRecords % recordsPerPage != 0)
                    {
                        totalPages++;
                    }
                }

                return Ok(new { Message = "success", Data = data, TotalPages = totalPages, TotalRecord = data.TotalCount });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("JobContractDetails")]
        public IActionResult JobContractDetails(int contractId)
        {
            try
            {
                int totalPages = 0;
                int recordsPerPage = 10;

                var data = _dashboard.JobContractDetails(contractId);

                return Ok(new { Message = "success", Data = data });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
