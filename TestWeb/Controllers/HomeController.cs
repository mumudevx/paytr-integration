using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PayTrPayment.Models;
using PayTrPayment.Services.Abstracts;

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPayTrService _payTrService;

        public HomeController(IPayTrService payTrService)
        {
            _payTrService = payTrService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var merchantOptions = new MerchantOptions
            {
                MerchantId = "MyMerchantId",
                MerchantKey = "MyMerchantKey",
                MerchantSecret = "MyMerchantSecret",
                SuccessUrl = "https://mywebsite.com/odeme/basarili",
                FailureUrl = "https://mywebsite.com/odeme/basarisiz",
                TestMode = 1,
                DebugMode = 1,
                TimeoutLimit = 0,
                LanguageCode = "tr",
                MaxInstallmentNumber = 0
            };

            var order = new Order
            {
                UserIp = "11.111.111.111",
                OrderId = Guid.NewGuid().ToString("N"),
                PaymentAmount = 1.50M,
                Currency = "TL",
                InstallmentNumber = 1,
                CustomerFullName = "John Doe",
                CustomerEmailAddress = "john@demo.com",
                CustomerAddress = "Üsküdar/İstanbul",
                CustomerPhone = "05551231122",
                BasketItems = new List<BasketItem>
                {
                    new BasketItem{ProductName = "AYT Sözel VIP", ProductPrice = 1.50M, ProductAmount = 1}
                }
            };

            var tokenResponse = await _payTrService.GenerateTokenAsync(merchantOptions, order);

            return View(tokenResponse);
        }

        [Route("callback")]
        [HttpPost]
        public IActionResult Callback()
        {
            var merchantOptions = new MerchantOptions
            {
                MerchantId = "MyMerchantId",
                MerchantKey = "MyMerchantKey",
                MerchantSecret = "MyMerchantSecret",
                SuccessUrl = "https://mywebsite.com/odeme/basarili",
                FailureUrl = "https://mywebsite.com/odeme/basarisiz",
                TestMode = 1,
                DebugMode = 1,
                TimeoutLimit = 0,
                LanguageCode = "tr",
                MaxInstallmentNumber = 0
            };

            var formValues =
                Request.Form.Keys.ToDictionary<string, string, string>(key => key, key => Request.Form[key]);

            var paymentResponse = _payTrService.GetPaymentResult(formValues);

            var compareHash = _payTrService.CompareHash(paymentResponse, merchantOptions);

            if (!compareHash)
                return BadRequest("PAYTR notification failed: bad hash");

            if (paymentResponse.Status == "failed")
            {
                //TODO: Access transaction via orderId and change it's status
            }

            if (paymentResponse.Status == "success")
            {
                //TODO: Access transaction via orderId and change it's status, also give credit or course
            }

            return Ok("OK");
        }
    }
}
