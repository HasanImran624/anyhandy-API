using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface.Dashboard;
using Anyhandy.Models;
using Anyhandy.Models.DTOs;
using Anyhandy.Models.Enums;
using Anyhandy.Models.ResourcesModels;
using Anyhandy.Models.ViewModels;
using System.Reflection;


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
                            Amount = j.Amount == null ? 0 : (double)j.Amount,
                            Status = j.Status ?? 0,
                            JobDetails = j.JobDetails ?? string.Empty,
                            JobAddressID = j.JobAddressId ?? 0,
                            DueDate = j.DueDate ?? DateTime.MinValue,
                            ServiceName = m.ServiceNameEn ?? string.Empty,
                            UserName = (u.FirstName ?? string.Empty) + " " + (u.LastName ?? string.Empty),
                            UserImg = u.Picture ?? string.Empty

                        }).Take(5).ToList();

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

                        }).Take(5).ToList();

            return lstt;
        }

        public FilteredResponse<List<MyJobsFilterDTO>> GetSearchFilterWithSortBy(DateTime? JobStartDate, DateTime? JobEndDate, DateTime? DueStartDate, DateTime? DueEndDate, string? SearchTxt, string OrderByType, int UserID, int PageNo, string SortByColumn, int RecordsPerPage, int ActiveType)
        {
            var res = new FilteredResponse<List<MyJobsFilterDTO>>();

            using var userContext = new AnyHandyDBContext<User>();

            var lst = (from j in userContext.Jobs
                       join m in userContext.MainServices on j.MainServicesId equals m.MainServicesId
                       join u in userContext.Users on j.UserId equals u.UserId
                       where j.UserId == UserID && j.Status != 4
                       orderby j.PostedDate descending
                       select new MyJobsFilterResourcesModel()
                       {
                           Amount = j.Amount,
                           JobID = j.JobId,
                           UserID = j.UserId ?? 0,
                           PostedDate = j.PostedDate ?? DateTime.MinValue,
                           JobTitle = j.JobTitle.ToLower() ?? string.Empty,
                           JobDetails = j.JobDetails.ToLower() ?? string.Empty,
                           DueDate = j.DueDate ?? DateTime.MinValue,
                           Service = m.ServiceNameEn.ToLower() ?? string.Empty,
                           Status = j.Status ?? 0
                       }).ToList();

            if (ActiveType != 0)
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
                    else
                    {
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
                Amount = x.Amount,
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


            foreach (var item in res.Results)
            {
                item.TotalPropsals = item.TotalPropsals == null ? 0 : item.TotalPropsals;
                item.TotalHired = item.TotalHired == null ? 0 : item.TotalHired;
            }

            return res;
        }


        public FilteredResponse<List<ProposalsReceivedFilterDTO>> GetProposalsReceivedWithSortBy(string SearchTxt, string OrderByType, int UserID, int PageNo, string SortByColumn, int RecordsPerPage, string ActiveType)
        {
            var res = new FilteredResponse<List<ProposalsReceivedFilterDTO>>();

            using var userContext = new AnyHandyDBContext<User>();

            var lst = (from j in userContext.Jobs

                       join p in userContext.JobProposals on j.JobId equals p.JobId
                       join u in userContext.Users on p.UserId equals u.UserId
                       join ut in userContext.UserTypes on u.UserId equals ut.UserId into gut
                       from ut in gut.DefaultIfEmpty()
                       join up in userContext.UserProfileServices on u.UserId equals up.UserId into gup
                       from up in gup.DefaultIfEmpty()
                       join m in userContext.MainServices on up.MainServiceId equals m.MainServicesId into gm
                       from m in gm.DefaultIfEmpty()

                       where j.UserId == UserID && p.Status != "Deleted"
                       orderby j.PostedDate descending
                       select new ProposalsReceivedFilterResourcesModel()
                       {
                           DueDate = j.DueDate,
                           ProposalId = p.JobProposalId,
                           JobID = j.JobId,
                           UserID = p.UserId ?? 0,
                           UserName = u.FirstName + " " + u.LastName ?? string.Empty,
                           Service = m == null ? "" : m.ServiceNameEn ?? string.Empty,
                           LocalTime = p.JobExpectedStart ?? DateTime.MinValue,  // SubmittedDate is missing in JobProposals  time only
                           JobTitle = j.JobTitle ?? string.Empty,
                           JobDetails = j.JobDetails ?? string.Empty,
                           ReceivedDate = p.JobExpectedStart ?? DateTime.MinValue, // SubmittedDate is missing in JobProposals   date only
                           Amount = p.Amount ?? decimal.MinValue,
                           Status = p.Status ?? string.Empty,
                           Description = p.Description ?? string.Empty,
                           UserTypeInfo = ut == null ? "" : ut.UserTypeInfo
                       }).ToList();

            if (ActiveType != "0")
            {
                lst = lst.Where(x => x.Status == ActiveType).ToList();
            }

            if (!String.IsNullOrEmpty(SearchTxt) && SearchTxt != "null")
            {
                lst = lst.Where(x => x.JobTitle.ToLower().Contains(SearchTxt.ToLower()) || x.UserName.ToLower().Contains(SearchTxt.ToLower()) || x.Service.ToLower().Contains(SearchTxt.ToLower()) || x.Description.ToLower().Contains(SearchTxt.ToLower())).ToList();
            }


            if (!String.IsNullOrEmpty(SortByColumn))
            {
                if (SortByColumn == "heroName")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.UserName).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.UserName).ToList();
                    }
                }

                if (SortByColumn == "date")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.ReceivedDate).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.ReceivedDate).ToList();
                    }
                }

                if (SortByColumn == "lowerPrice")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.Amount).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Amount).ToList();
                    }
                }

                if (SortByColumn == "higherPrice")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.Amount).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.Amount).ToList();
                    }
                }
            }

            res.TotalCount = lst.Count();
            lst = lst.Skip((int)((PageNo - 1) * RecordsPerPage)).Take(RecordsPerPage).ToList();
            //Get Hired List
            var hiredList = (from c in userContext.JobContracts
                             where (lst.Select(j => j.UserID)).Contains((int)c.SelectedHandyManId)
                             group new { c } by new { c.SelectedHandyManId } into grp
                             select new
                             {
                                 UserId = grp.Select(x => x.c.SelectedHandyManId).FirstOrDefault(),
                                 TotalHired = grp.Count()
                             }
                             );

            res.Results = lst.Skip((int)((PageNo - 1) * RecordsPerPage)).Take(RecordsPerPage).Select(x => new ProposalsReceivedFilterDTO()
            {
                JobID = x.JobID,
                ProposalId = x.ProposalId,
                UserID = x.UserID,
                UserName = x.UserName,
                Service = x.Service,
                LocalTime = x.LocalTime,
                JobTitle = x.JobTitle,
                JobDetails = x.JobDetails,
                ReceivedDate = x.LocalTime,
                Amount = x.Amount,
                Status = x.Status,
                Description = x.Description,
                JobsCompleted = (hiredList?.Where(a => a.UserId == x.UserID).FirstOrDefault()?.TotalHired),
                UserTypeInfo = x.UserTypeInfo
            }).ToList();


            var listAvg = userContext.UserProfileRatings.GroupBy(g => g.UserId, r => r.Rating).Select(g => new
            {
                UserId = g.Key,
                Rating = g.Average()
            });

            foreach (var item in res.Results)
            {
                decimal rating = 0;
                try
                {
                    rating = (decimal)listAvg.Where(x => x.UserId == item.UserID).FirstOrDefault().Rating;
                }
                catch { }

                item.JobsCompleted = item.JobsCompleted == null ? 0 : item.JobsCompleted;
                item.Rating = rating;
            }

            return res;
        }

        public JobsStatusCount GetAllJobsStatusWithUserID(int userID)
        {
            JobsStatusCount ob = new JobsStatusCount();
            ob.UserId = userID;
            var dbContext = new AnyHandyDBContext<Job>();
            ob.CancelledJobsTotal = dbContext.Jobs.Where(x => x.UserId == userID && x.Status == 3).Count();
            ob.ContractsTotal = (from c in dbContext.JobContracts
                                 join j in dbContext.Jobs on c.JobId equals j.JobId
                                 where j.UserId == userID
                                 select new { }
                                 ).Count();

            ob.ProposalsReceivedTotal = (from p in dbContext.JobProposals
                                         join j in dbContext.Jobs on p.JobId equals j.JobId
                                         where j.UserId == userID
                                         select new { }
                                 ).Count();

            ob.MyJobsTotal = dbContext.Jobs.Where(x => x.UserId == userID && (x.Status != 4)).Count();

            return ob;
        }

        public List<MyHeroJobsDTO> GetMyHeroListWithUserID(int UserID)
        {
            List<MyHeroJobsDTO> ob = new List<MyHeroJobsDTO>();

            var dbContext = new AnyHandyDBContext<Job>();

            var listAvg = dbContext.UserProfileRatings.GroupBy(g => g.UserId, r => r.Rating).Select(g => new
            {
                UserId = g.Key,
                Rating = g.Average()
            });

            listAvg = listAvg.Where(x => x.Rating >= 4).OrderByDescending(x => Guid.NewGuid()).Take(5);



            var list = (from avg in listAvg
                        join u in dbContext.Users on avg.UserId equals u.UserId

                        select new MyHeroJobsDTO()
                        {
                            JobsCompleted = (from c in dbContext.JobContracts
                                             join j in dbContext.Jobs on c.JobId equals j.JobId
                                             where j.UserId == u.UserId
                                             select new { }).Count(),
                            Rating = avg.Rating,
                            UserName = u.FirstName + " " + u.LastName,
                            UserImg = u.Picture,
                            UserID = u.UserId
                        }
                        ).ToList();


            foreach (var item in list)
            {
                int? userId = item.UserID;
                item.ServiceName = (from s in dbContext.UserProfileServices
                                    join m in dbContext.MainServices on s.MainServiceId equals m.MainServicesId
                                    where s.UserId == userId
                                    select new { Service = m.ServiceNameEn }
                                           ).FirstOrDefault()?.Service;
            }

            return list;
        }


        public string DeleteCancelJob(int jobId, int status)
        {
            string message = "success";
            try
            {
                var dbContext = new AnyHandyDBContext<Job>();
                var job = dbContext.Jobs.Where(x => x.JobId == jobId).FirstOrDefault();

                if (job != null)
                {
                    job.Status = status;


                    dbContext.Entry(job).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    dbContext.SaveChanges();
                }
                else
                {
                    message = "NotFound";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }


        public string UpdateProposalStatus(int proposalId, string status)
        {
            string message = "success";
            try
            {
                var dbContext = new AnyHandyDBContext<Job>();
                var proposal = dbContext.JobProposals.Where(x => x.JobProposalId == proposalId).FirstOrDefault();

                if (proposal != null)
                {
                    proposal.Status = status;


                    dbContext.Entry(proposal).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    dbContext.SaveChanges();
                }
                else
                {
                    message = "NotFound";
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        public FilteredResponse<List<JobContractsFilteredDto>> GetContractSearchFilterWithSortBy(string SearchTxt, string OrderByType, int UserID, int PageNo, string SortByColumn, int RecordsPerPage, int ActiveType)
        {
            var res = new FilteredResponse<List<JobContractsFilteredDto>>();

            using var userContext = new AnyHandyDBContext<User>();

            var lst = (from j in userContext.Jobs
                       join jc in userContext.JobContracts on j.JobId equals jc.JobId
                       join u in userContext.Users on jc.SelectedHandyManId equals u.UserId
                       join cm in userContext.ContractMilestones on jc.JobContractId equals cm.JobContractId into cmg
                       from cm in cmg.DefaultIfEmpty()
                       join cp in userContext.JobContractPayments on jc.JobContractId equals cp.JobContractId into cpg
                       from cp in cpg.DefaultIfEmpty()
                       join up in userContext.UserProfileServices on u.UserId equals up.UserId into gup
                       from up in gup.DefaultIfEmpty()
                       join m in userContext.MainServices on up.MainServiceId equals m.MainServicesId into gm
                       from m in gm.DefaultIfEmpty()

                       where j.UserId == UserID
                       orderby cm.StartDate descending
                       select new ConctractFilterResourceModel()
                       {
                           JobContractId = jc.JobContractId,
                           ConctracStatus = jc.Status.ToString(),
                           ContractAmount = jc.Amount,
                           ContractPaidAmount = cp == null ? 0 : cp.Amount,
                           ContractDueDate = jc.ContractEnd,
                           //ContractName = "Contract Name",
                           Image = "",
                           JobId = j.JobId,
                           JobTitle = j.JobTitle,
                           MileStoneAmount = cm == null ? 0 : cm.Amount,
                           MileStoneDueDate = cm == null ? null : cm.EndDate,
                           MileStoneStartDate = cm == null ? null : cm.StartDate,
                           MileStoneStatus = cm == null ? "" : (cm.Status == null ? 0 : cm.Status).ToString(),
                           MileStoneTitle = cm.Details,
                           UserId = u.UserId,
                           UserName = u.FirstName.Trim() + " " + u.LastName.Trim() ?? string.Empty,
                           Service = m == null ? "" : m.ServiceNameEn ?? string.Empty,
                           ContractStartDate = jc.ContractStart

                       }).ToList();

            if (ActiveType != 0)
            {
                lst = lst.Where(x => x.ConctracStatus == ActiveType.ToString()).ToList();
            }

            if (!String.IsNullOrEmpty(SearchTxt) && SearchTxt != "null")
            {
                lst = lst.Where(x => x.JobTitle.ToLower().Contains(SearchTxt.ToLower()) || x.UserName.ToLower().Contains(SearchTxt.ToLower()) || x.Service.ToLower().Contains(SearchTxt.ToLower()) || x.MileStoneTitle.ToLower().Contains(SearchTxt.ToLower())).ToList();
            }


            if (!String.IsNullOrEmpty(SortByColumn))
            {
                if (SortByColumn == "heroName")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.UserName).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.UserName).ToList();
                    }
                }

                if (SortByColumn == "date")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.ContractDueDate).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.ContractDueDate).ToList();
                    }
                }

                if (SortByColumn == "lowerPrice")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.ContractAmount).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.ContractAmount).ToList();
                    }
                }

                if (SortByColumn == "higherPrice")
                {
                    if (OrderByType == "asc")
                    {
                        lst = lst.OrderBy(x => x.ContractAmount).ToList();
                    }
                    else
                    {
                        lst = lst.OrderByDescending(x => x.ContractAmount).ToList();
                    }
                }
            }

            res.TotalCount = lst.Count();
            lst = lst.Skip((int)((PageNo - 1) * RecordsPerPage)).Take(RecordsPerPage).ToList();


            res.Results = lst.Skip((int)((PageNo - 1) * RecordsPerPage)).Take(RecordsPerPage).Select(x => new JobContractsFilteredDto()
            {
                JobContractId = x.JobContractId,
                ContractStatus = x.ConctracStatus,
                ContractAmount = x.ContractAmount,
                ContractDueDate = x.ContractDueDate,
                //ContractName = x.ContractName,
                Image = x.Image,
                JobId = x.JobId,
                JobTitle = x.JobTitle,
                MileStoneAmount = x.MileStoneAmount,
                MileStoneDueDate = x.MileStoneDueDate,
                MileStoneStartDate = x.MileStoneStartDate,
                MileStoneStatus = x.MileStoneStatus,
                MileStoneTitle = x.MileStoneTitle,
                UserId = x.UserId,
                UserName = x.UserName,
                Service = x.Service,
                ContractStartDate = x.ContractStartDate
            }).ToList();


            var listAvg = userContext.UserProfileRatings.GroupBy(g => g.UserId, r => r.Rating).Select(g => new
            {
                UserId = g.Key,
                Rating = g.Average()
            });

            foreach (var item in res.Results)
            {
                item.ContractStatus = ((JobContractStatus)Convert.ToInt32(item.ContractStatus)).ToString();
            }

            return res;
        }



        public JobContractDetails JobContractDetails(int contractId)
        {
            var res = new JobContractDetails();

            using var userContext = new AnyHandyDBContext<User>();

            res = (from j in userContext.Jobs
                   join jc in userContext.JobContracts on j.JobId equals jc.JobId
                   join u in userContext.Users on jc.SelectedHandyManId equals u.UserId
                   join cm in userContext.ContractMilestones on jc.JobContractId equals cm.JobContractId into cmg
                   from cm in cmg.DefaultIfEmpty()
                   join cp in userContext.JobContractPayments on jc.JobContractId equals cp.JobContractId into cpg
                   from cp in cpg.DefaultIfEmpty()
                   join up in userContext.UserProfileServices on u.UserId equals up.UserId into gup
                   from up in gup.DefaultIfEmpty()
                   join m in userContext.MainServices on j.MainServicesId equals m.MainServicesId into gm
                   from m in gm.DefaultIfEmpty()

                   where jc.JobContractId == contractId
                   orderby cm.StartDate descending
                   select new JobContractDetails()
                   {
                       JobContractId = jc.JobContractId,
                       ContractStatus = jc.Status.ToString(),
                       ContractAmount = jc.Amount,
                       ContractPaidAmount = cp == null ? 0 : cp.Amount,
                       ContractDueDate = jc.ContractEnd,
                       Image = "",
                       JobId = j.JobId,
                       JobTitle = j.JobTitle,
                       MileStoneAmount = cm == null ? 0 : cm.Amount,
                       MileStoneDueDate = cm == null ? null : cm.EndDate,
                       MileStoneStartDate = cm == null ? null : cm.StartDate,
                       MileStoneStatus = cm == null ? "" : (cm.Status == null ? 0 : cm.Status).ToString(),
                       MileStoneTitle = cm.Details,
                       UserId = u.UserId,
                       UserName = u.FirstName.Trim() + " " + u.LastName.Trim() ?? string.Empty,
                       Service = m == null ? "" : m.ServiceNameEn ?? string.Empty,
                       BonusAmount = cp == null ? 0 : cp.BonusAmount,
                       JobDetails = j.JobDetails,
                       ServiceId = m == null ? 0 : m.MainServicesId,
                       ContractStartDate = jc.ContractStart

                   }).FirstOrDefault();

            if (res != null)
            {
                res.ContractStatus = ((JobContractStatus)Convert.ToInt32(res.ContractStatus)).ToString();
                if (res.ServiceId > 0)
                {
                    res.SubServiceList = GetSubServicesList(res.JobId, res.ServiceId);//userContext.SubServices.Where(x => x.MainServicesId == res.ServiceId).Select(s => s.ServiceNameEn).ToList();
                }
            }

            return res;
        }


        private List<SubServicesDto> GetSubServicesList(int jobId, int mainServiceId)
        {
            List<SubServicesDto> lst = new List<SubServicesDto>();
            using var userContext = new AnyHandyDBContext<User>();

            if (mainServiceId == 8)
            {
                List<TblCarpentryService> lstCarpentryServices = userContext.TblCarpentryServices.Where(x => x.JobId == jobId).OrderBy(x => x.SubCategoryId).ToList();
                foreach (var service in lstCarpentryServices)
                {
                    SubServicesDto obj = new SubServicesDto();
                    try
                    {
                        obj.SubSrviceName = userContext.SubServices.Where(x => x.SubServicesId == service.SubCategoryId).FirstOrDefault()?.ServiceNameEn;
                    }
                    catch { }

                    obj.ListItemData = GetProperties<TblCarpentryService>(service, userContext);
                    lst.Add(obj);
                }
            }
            else if (mainServiceId == 6)
            {
                List<TblPlumbingService> lstPlumbingService = userContext.TblPlumbingServices.Where(x => x.JobId == jobId).ToList();
                foreach (var service in lstPlumbingService)
                {
                    SubServicesDto obj = new SubServicesDto();
                    try
                    {
                        obj.SubSrviceName = userContext.SubServices.Where(x => x.SubServicesId == service.SubCategoryId).FirstOrDefault()?.ServiceNameEn;
                    }
                    catch { }

                    obj.ListItemData = GetProperties<TblPlumbingService>(service, userContext);
                    lst.Add(obj);
                }
            }
            else if (mainServiceId == 1)
            {
                List<TblHomeCleaning> lstHomeCleaning = userContext.TblHomeCleanings.Where(x => x.JobId == jobId).ToList();
                foreach (var service in lstHomeCleaning)
                {
                    SubServicesDto obj = new SubServicesDto();
                    try
                    {
                        obj.SubSrviceName = userContext.SubServices.Where(x => x.SubServicesId == service.SubServiceId).FirstOrDefault()?.ServiceNameEn;
                    }
                    catch { }

                    obj.ListItemData = GetProperties<TblHomeCleaning>(service, userContext);
                    lst.Add(obj);
                }
            }
            else if (mainServiceId == 7)
            {
                List<TblHvacService> lstHvacService = userContext.TblHvacServices.Where(x => x.JobId == jobId).ToList();
                foreach (var service in lstHvacService)
                {
                    SubServicesDto obj = new SubServicesDto();
                    try
                    {
                        obj.SubSrviceName = userContext.SubServices.Where(x => x.SubServicesId == service.SubServiceId).FirstOrDefault()?.ServiceNameEn;
                    }
                    catch { }
                    
                    obj.ListItemData = GetProperties<TblHvacService>(service, userContext);
                    lst.Add(obj);
                }
            }


            return lst;

        }


        private List<Item> GetProperties<T>(T myObject, AnyHandyDBContext<User> context)
        {
            List<Item> items = new List<Item>();
            string[] arrExcludeColumns = new string[] { "JobId", "SubServiceId", "SubCategoryId", "CarpentryServiceId", "PlumbingServiceId", "HomeCleaningServiceId", "HvacServiceId" };
            Type myType = myObject.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                string propertyName = prop.Name;
                bool isVirtual = typeof(T).GetProperty(propertyName).GetGetMethod().IsVirtual;
                if (!arrExcludeColumns.Contains(propertyName) && !isVirtual)
                {
                    if (propertyName == "LcoationTypeId")
                    {
                        object value = null;
                        try
                        {
                            value = context.LocationTypes.Where(x => x.LocationTypeId == Convert.ToInt32(prop.GetValue(myObject, null))).FirstOrDefault().LocationTypeName;
                        }
                        catch 
                        {
                            value = prop.GetValue(myObject, null);
                        }

                        items.Add(new Item() { Key = "LocationTypeName", Value = value });
                    }
                    else if (propertyName == "AreaTypeId")
                    {
                        object value = null;
                        try
                        {
                            value = context.AreaTypes.Where(x => x.AreaTypeId == Convert.ToInt32(prop.GetValue(myObject, null))).FirstOrDefault().AreaTypeName;
                        }
                        catch
                        {
                            value = prop.GetValue(myObject, null);
                        }

                        items.Add(new Item() { Key = "AreaTypeName", Value = value });
                    }
                    else if (propertyName == "TypeFurnitureId")
                    {
                        object value = null;
                        try
                        {
                            value = context.TypeFurnitures.Where(x => x.TypeFurnitureId == Convert.ToInt32(prop.GetValue(myObject, null))).FirstOrDefault().TypeFurnitureName;
                        }
                        catch
                        {
                            value = prop.GetValue(myObject, null);
                        }

                        items.Add(new Item() { Key = "TypeFurnitureName", Value = value });
                    }
                    else
                    {
                        items.Add(new Item() { Key = propertyName, Value = prop.GetValue(myObject, null) });
                    }
                }
            }

            return items;
        }




    }
}
