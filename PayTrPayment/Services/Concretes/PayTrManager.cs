using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayTrPayment.Models;
using PayTrPayment.Models.Responses;
using PayTrPayment.Services.Abstracts;

namespace PayTrPayment.Services.Concretes
{
    public class PayTrManager : IPayTrService
    {
        public async Task<TokenResponse> GenerateTokenAsync(MerchantOptions merchantOptions, Order order)
        {
            var totalAmount = (int)(order.PaymentAmount * 100);

            var data = new NameValueCollection
            {
                ["merchant_id"] = merchantOptions.MerchantId,
                ["user_ip"] = order.UserIp,
                ["merchant_oid"] = order.OrderId,
                ["email"] = order.CustomerEmailAddress,
                ["payment_amount"] = totalAmount.ToString()
            };

            // Generate Basket
            var userBasketJson = JsonConvert.SerializeObject(order.BasketItems);
            var userBasketString = Convert.ToBase64String(Encoding.UTF8.GetBytes(userBasketJson));
            data["user_basket"] = userBasketString;

            // Generate Token
            var tokenInformation = string.Concat(merchantOptions.MerchantId, order.UserIp, order.OrderId, order.CustomerEmailAddress,
                totalAmount.ToString(), userBasketString, order.InstallmentNumber, merchantOptions.MaxInstallmentNumber, order.Currency, merchantOptions.TestMode,
                merchantOptions.MerchantSecret);
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchantOptions.MerchantKey));
            var generatedToken = hmac.ComputeHash(Encoding.UTF8.GetBytes(tokenInformation));
            data["paytr_token"] = Convert.ToBase64String(generatedToken);

            data["debug_on"] = merchantOptions.DebugMode.ToString();
            data["test_mode"] = merchantOptions.TestMode.ToString();
            data["no_installment"] = order.InstallmentNumber.ToString();
            data["max_installment"] = merchantOptions.MaxInstallmentNumber.ToString();
            data["user_name"] = order.CustomerFullName;
            data["user_address"] = order.CustomerAddress;
            data["user_phone"] = order.CustomerPhone;
            data["merchant_ok_url"] = merchantOptions.SuccessUrl;
            data["merchant_fail_url"] = merchantOptions.FailureUrl;
            data["timeout_limit"] = merchantOptions.TimeoutLimit.ToString();
            data["currency"] = order.Currency;
            data["lang"] = merchantOptions.LanguageCode;

            using var client = new WebClient();

            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var result = await client.UploadValuesTaskAsync("https://www.paytr.com/odeme/api/get-token", "POST", data);
            var resultAuthTicket = Encoding.UTF8.GetString(result);

            var tokenResult = JsonConvert.DeserializeObject<TokenResponse>(resultAuthTicket);

            return tokenResult;
        }

        public PaymentResponse GetPaymentResult(Dictionary<string, string> formValues)
        {
            var properties = typeof(PaymentResponse).GetProperties();
            var newPaymentResponse = new PaymentResponse();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(DataMemberAttribute), true);
                foreach (DataMemberAttribute dma in attributes)
                {
                    var propertyInformation = typeof(PaymentResponse).GetProperty(property.Name);
                    var valueOfProperty = formValues.FirstOrDefault(fv => fv.Key == dma.Name).Value;
                    propertyInformation?.SetValue(newPaymentResponse, valueOfProperty);
                }
            }

            return newPaymentResponse;
        }

        public bool CompareHash(PaymentResponse paymentResponse, MerchantOptions merchantOptions)
        {
            var tokenInformation = string.Concat(paymentResponse.OrderId, merchantOptions.MerchantSecret,
                paymentResponse.Status, paymentResponse.TotalAmount);

            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchantOptions.MerchantKey));
            var tokenBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(tokenInformation));
            var generatedToken = Convert.ToBase64String(tokenBytes);

            return paymentResponse.Hash == generatedToken;
        }
    }
}
