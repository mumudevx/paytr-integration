namespace PayTrPayment.Models
{
    public class MerchantOptions
    {
        public string MerchantId { get; set; }
        public string MerchantKey { get; set; }
        public string MerchantSecret { get; set; }
        public string SuccessUrl { get; set; }
        public string FailureUrl { get; set; }
        public int TestMode { get; set; }
        public int DebugMode { get; set; }
        public int TimeoutLimit { get; set; }
        public string LanguageCode { get; set; }
        public int MaxInstallmentNumber { get; set; }
    }
}
