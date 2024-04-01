using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface.Dashboard;
using Anyhandy.Models;
using Anyhandy.Models.DTOs;
using Anyhandy.Models.ResourcesModels;
using Anyhandy.Models.ViewModels;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Anyhandy.Services.Dashboard
{
    public class DashboardServices : IDashboard
    {
        public List<MyRecentJobsDTO> GetMyJobsDetailsWithUserID(int UserID)
        {
            using var userContext = new AnyHandyDBContext<User>();

            var lstt = (from j in userContext.Jobs
                        join m in userContext.MainServices on j.MainServicesId equals m.MainServicesId
                        join u in userContext.Users on j.UserId equals u.UserId
                        where j.UserId == UserID
                        orderby j.PostedDate descending  
                        select new MyRecentJobsDTO()
                        {
                            JobID = j.JobId,
                            UserID = j.UserId ?? 0, 
                            MainServicesID = m.MainServicesId,
                            PostedDate = j.PostedDate ?? DateTime.MinValue, 
                            JobTitle = j.JobTitle ?? string.Empty, 
                            Amount = j.Amount == null?0: (double)j.Amount, 
                            Status = j.Status ?? 0, 
                            JobDetails = j.JobDetails ?? string.Empty,
                            JobAddressID = j.JobAddressId ?? 0,
                            DueDate = j.DueDate ?? DateTime.MinValue,
                            ServiceName = m.ServiceNameEn ?? string.Empty,
                            UserName = (u.FirstName ?? string.Empty) + " " + (u.LastName ?? string.Empty),
                            UserImg = u.Picture ?? string.Empty

                        }).Take(3).ToList();

            return lstt;
        }

        public List<MyJobsFilterDTO> GetMyJobsFilterWithUserID(int userID)
        {
            using var userContext = new AnyHandyDBContext<User>();

            var lstt = (from j in userContext.Jobs 
                        join m in userContext.MainServices on j.MainServicesId equals m.MainServicesId
                        where j.UserId == userID
                        orderby j.PostedDate descending
                        select new MyJobsFilterDTO()
                        {
                            JobID = j.JobId,
                            UserID = j.UserId ?? 0,
                            PostedDate = j.PostedDate ?? DateTime.MinValue,
                            JobTitle = j.JobTitle ?? string.Empty,
                            JobDetails = j.JobDetails ?? string.Empty,
                            DueDate = j.DueDate ?? DateTime.MinValue,
                            Service = m.ServiceNameEn.ToLower() ?? string.Empty,
                            Status = j.Status ?? 0,
                            TotalHired = 1,
                            TotalPropsals = 2

                        }).Take(3).ToList();

            return lstt;
        }

        public FilteredResponse<List<MyJobsFilterDTO>> GetSearchFilterWithSortBy(DateTime? JobStartDate, DateTime? JobEndDate, DateTime? DueStartDate, DateTime? DueEndDate, string? SearchTxt, string OrderByType, int UserID, int PageNo, string SortByColumn, int RecordsPerPage, int ActiveType)
        {
            var res = new FilteredResponse<List<MyJobsFilterDTO>>();

            using var userContext = new AnyHandyDBContext<User>();

            var lst = (from j in userContext.Jobs
                       join m in userContext.MainServices on j.MainServicesId equals m.MainServicesId
                       join u in userContext.Users on j.UserId equals u.UserId
                       where j.UserId == UserID
                       orderby j.PostedDate descending
                       select new MyJobsFilterResourcesModel()
                       {
                           JobID = j.JobId,
                           UserID = j.UserId ?? 0,
                           PostedDate = j.PostedDate ?? DateTime.MinValue,
                           JobTitle = j.JobTitle.ToLower() ?? string.Empty,
                           JobDetails = j.JobDetails.ToLower() ?? string.Empty,
                           DueDate = j.DueDate ?? DateTime.MinValue,
                           Service = m.ServiceNameEn.ToLower() ?? string.Empty,
                           Status = j.Status ?? 0
                       }).ToList();

            if(ActiveType != 0)
            {
                lst = lst.Where(x => x.Status == ActiveType).ToList();
            }

            if (!String.IsNullOrEmpty(SearchTxt) && SearchTxt != "null")
            {
                lst = lst.Where(x => x.JobTitle.ToLower().Contains(SearchTxt.ToLower()) || x.Service.ToLower().Contains(SearchTxt.ToLower())).ToList();
            }

            if (JobStartDate != null)
            {
                lst = lst.Where(x => x.PostedDate >= JobStartDate).ToList();
            }

            if (JobEndDate != null)
            {
                lst = lst.Where(x => x.PostedDate <= JobEndDate).ToList();
            }

            if (DueStartDate != null)
            {
                lst = lst.Where(x => x.DueDate >= DueStartDate).ToList();
            }

            if (DueEndDate != null)
            {
                lst = lst.Where(x => x.DueDate <= DueEndDate).ToList();
            }

            if (!String.IsNullOrEmpty(SortByColumn))
            {
                if (SortByColumn == "startDate")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.PostedDate).ToList();
                    }
                    else{
                        lst = lst.OrderByDescending(x => x.PostedDate).ToList();
                    }
                }

                if (SortByColumn == "dueDate")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.DueDate).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.DueDate).ToList();
                    }
                }

                if (SortByColumn == "service")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.Service).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Service).ToList();
                    }
                }

                if (SortByColumn == "jobTitle")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.JobTitle).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.JobTitle).ToList();
                    }
                }
            }

            res.TotalCount = lst.Count();
            lst = lst.Skip((int)((PageNo - 1) * RecordsPerPage)).Take(RecordsPerPage).ToList();
            //Get Hired List
            var hiredList = (from p in userContext.JobContracts
                             where (lst.Select(j => j.JobID)).Contains((int)p.JobId)
                             group new { p } by new { p.JobId } into grp
                             select new
                             {
                                 JobId = grp.Select(x => x.p.JobId).FirstOrDefault(),
                                 TotalHired = grp.Count()
                             }
                             );
            //Get Proposal List
            var proposalsList = (from p in userContext.JobProposals
                                 where (lst.Select(j => j.JobID)).Contains((int)p.JobId)
                                 group new { p } by new { p.JobId } into grp
                                 select new
                                 {
                                     JobId = grp.Select(x => x.p.JobId).FirstOrDefault(),
                                     TotalProposals = grp.Count()
                                 }
                          );

            res.Results = lst.Skip((int)((PageNo - 1) * RecordsPerPage)).Take(RecordsPerPage).Select(x => new MyJobsFilterDTO()
            {
                DueDate = x.DueDate,
                JobDetails = x.JobDetails,
                JobID = x.JobID,
                JobTitle = x.JobTitle,
                PostedDate = x.PostedDate,
                UserID = x.UserID,
                Status = x.Status,
                TotalHired = (hiredList?.Where(a => a.JobId == x.JobID).FirstOrDefault()?.TotalHired),
                TotalPropsals = (proposalsList?.Where(a => a.JobId == x.JobID).FirstOrDefault()?.TotalProposals)
            }).ToList();
            

            foreach(var item in res.Results)
            {
                item.TotalPropsals = item.TotalPropsals == null ? 0 : item.TotalPropsals;
                item.TotalHired = item.TotalHired == null ? 0 : item.TotalHired;
            }

            return res;
        }

    }
}
