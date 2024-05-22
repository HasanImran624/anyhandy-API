using Anyhandy.DataProvider.EFCore.Context;
using Anyhandy.DataProvider.EFCore.Models;
using Anyhandy.Interface;
using Anyhandy.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Services.Dashboard
{
    public class JobContractService : IJobContract
    {
        public Tuple<string,JobContractPaymentDto> CreateContract(JobContractCreateDto contractCreate)
        {
            var jobContractPaymnetInfo = new JobContractPaymentDto();
            string msg = "success";
            decimal amount = 0;
            try
            {
                bool isMilestone = false;


                if (contractCreate.MilestoneList != null)
                {
                    if (contractCreate.MilestoneList.Count > 0)
                    {
                        isMilestone = true;
                    }
                }
                using (var context = new AnyHandyDBContext<User>())
                {
                    var jobContract = new JobContract()
                    {
                        JobId = contractCreate.JobId,
                        SelectedHandyManId = contractCreate.UserId,
                        JobProposalId = contractCreate.ProposalId,
                        Amount = contractCreate.Amount,
                        ContractEnd = contractCreate.ContractDueDate,
                        Status = 1,
                        IsMileStone = isMilestone,
                        JobDetails = contractCreate.JobDetails,
                        AdditionalTerms = contractCreate.AdditionalTerms
                    };

                    context.JobContracts.Add(jobContract);
                    context.SaveChanges();

                    var milestoneList = contractCreate.MilestoneList;
                    int milestoneId = 0;
                    if (milestoneList != null)
                    {
                        if (milestoneList.Count > 0)
                        {
                            amount = (decimal)milestoneList[0].Amount;
                            foreach (var milestone in milestoneList)
                            {
                                var milestoneObj = new ContractMilestone()
                                {
                                    JobContractId = jobContract.JobContractId,
                                    EndDate = milestone.DueDate,
                                    Amount = milestone.Amount,
                                    Details = milestone.Details
                                };
                                context.ContractMilestones.Add(milestoneObj);
                                context.SaveChanges();
                                if (milestoneId == 0)
                                {
                                    milestoneId = milestoneObj.MilestoneId;
                                }
                            }
                        }
                    }
                   


                    jobContractPaymnetInfo = new JobContractPaymentDto()
                    {
                        ContractId = jobContract.JobContractId,
                        MilestoneId = milestoneId,
                        JobId = jobContract.JobId,
                        Amount = amount == 0? jobContract.Amount:amount
                    };
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }

            return new Tuple<string, JobContractPaymentDto>(msg, jobContractPaymnetInfo);
        }

        public string PaymentConfiramtion(JobContractPaymentDto paymentDto)
        {
            string msg = "success";
            try
            {
                using (var context = new AnyHandyDBContext<User>())
                {
                    

                    if (paymentDto.MilestoneId > 0)
                    {
                        var objMilestonePayment = new ContractMilestonesPayment()
                        {
                            MilestoneId = paymentDto.MilestoneId,
                            Amount = (decimal)paymentDto.Amount,
                            PaymentReference = paymentDto.PaymentReference
                        };
                        context.ContractMilestonesPayments.Add(objMilestonePayment);
                        context.SaveChanges();
                    }

                    List<int?> lstMilestoneIds = context.ContractMilestones.Where(x => x.JobContractId == paymentDto.ContractId).Select(x => (int?)x.MilestoneId).ToList();

                    decimal totalAmount = context.ContractMilestonesPayments.Where(x => lstMilestoneIds.Contains((int)x.MilestoneId)).Sum(x => x.Amount);


                    var exist = context.JobContractPayments.Where(x => x.JobContractId == paymentDto.ContractId).FirstOrDefault();


                    if (exist != null)
                    {
                        exist.Amount = totalAmount;
                        context.Entry(exist).State = Microsoft.EntityFrameworkCore.EntityState.Modified;                      
                    }
                    else
                    {
                        var objContractPayment = new JobContractPayment()
                        {
                            JobContractId = paymentDto.ContractId,
                            Amount = totalAmount,
                            CreatedDate = DateTime.Now,
                            JobId = paymentDto.JobId
                        };
                        context.JobContractPayments.Add(objContractPayment);
                    }
                   
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }

            return msg;
        }
    }
}
