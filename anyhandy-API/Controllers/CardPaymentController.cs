using Anyhandy.Interface;
using Anyhandy.Interface.Dashboard;
using Anyhandy.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using Stripe;
using Stripe.Infrastructure;

namespace anyhandy_API.Controllers
{
    [ApiController]
    [Route("api/CardPayment/")]
    public class CardPaymentController : Controller
    {
        readonly IJobContract _jobHire;
        private readonly ILogger<CardPaymentController> _logger;

        public CardPaymentController(ILogger<CardPaymentController> logger, IJobContract jobHire)
        {
            _logger = logger;
            _jobHire = jobHire;
        }
        [HttpPost("CreatePaymentIntent")]
        public IActionResult CreatePaymentIntent( CardPaymentDto card )
        {
            try
            {
                //Stripe.PaymentIntent paymentIntent = new Stripe.PaymentIntent()
                //{
                //    Amount = 100,
                //    Currency = "usd",
                //    ClientSecret = "sk_test_51OLB4eAHr7Y9hYhlvRshFs1jGA2WZluFMx3ZVhDXBtHh02e5tZKEdMol58L6Oop2XyxtFDPLKdsqIoMnGXy5E0Wa00Kd5eUd5G"
                //};


                Stripe.StripeConfiguration.SetApiKey("sk_test_51OLB4eAHr7Y9hYhlvRshFs1jGA2WZluFMx3ZVhDXBtHh02e5tZKEdMol58L6Oop2XyxtFDPLKdsqIoMnGXy5E0Wa00Kd5eUd5G");
                long amount = 10*100;
                try
                {
                    if (card.Price > 0)
                    {
                        amount = (long)(card.Price * 100);
                    }
                }
                catch { }
               
                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "AED",  
                    ReceiptEmail="tanweerahmedmem@gmail.com",
                    Description = "Test Payment Stripe"
                };
                var service = new PaymentIntentService();
                var paymnetIntent  = service.Create(options);

                return Ok(new { data = paymnetIntent }) ;
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
