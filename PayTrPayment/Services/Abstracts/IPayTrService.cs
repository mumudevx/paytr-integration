using System.Collections.Generic;
using System.Threading.Tasks;
using PayTrPayment.Models;
using PayTrPayment.Models.Responses;

namespace PayTrPayment.Services.Abstracts
{
    public interface IPayTrService
    {
        Task<TokenResponse> GenerateTokenAsync(MerchantOptions merchantOptions, Order order);
        PaymentResponse GetPaymentResult(Dictionary<string, string> formValues);
        bool CompareHash(PaymentResponse paymentResponse, MerchantOptions merchantOptions);
    }
}
