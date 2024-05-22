using Anyhandy.Interface;
using Anyhandy.Interface.Dashboard;
using Anyhandy.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace anyhandy_API.Controllers
{
    [ApiController]
    [Route("api/JobContract/")]
    public class JobContractController : Controller
    {
        readonly IJobContract _jobContract;
        private readonly ILogger<JobContractController> _logger;

        public JobContractController(ILogger<JobContractController> logger, IJobContract jobContract)
        {
            _logger = logger;
            _jobContract = jobContract;
        }
        [HttpPost("CreateContract")]
        public IActionResult CreateContract(JobContractCreateDto contract)
        {
            try
            {
                var resp = _jobContract.CreateContract(contract);
                return Ok(new { Message = resp.Item1, IsSuccess = resp.Item1 == "success" ? true : false, data = resp.Item2 });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("PaymentConfiramtion")]
        public IActionResult PaymentConfiramtion(JobContractPaymentDto paymentDto)
        {
            try
            {
                var resp = _jobContract.PaymentConfiramtion(paymentDto);
                return Ok(new { Message = resp, IsSuccess = resp== "success" ? true : false});
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
