using Anyhandy.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Interface
{
    public interface IJobContract
    {
        public Tuple<string, JobContractPaymentDto> CreateContract(JobContractCreateDto contractCreate);
        public string PaymentConfiramtion(JobContractPaymentDto paymentDto);
    }
}
