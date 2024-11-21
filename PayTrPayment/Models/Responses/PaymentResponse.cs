using System.Runtime.Serialization;

namespace PayTrPayment.Models.Responses
{
    public class PaymentResponse
    {
        [DataMember(Name = "merchant_oid")]
        public string OrderId { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "total_amount")]
        public string TotalAmount { get; set; }

        [DataMember(Name = "hash")]
        public string Hash { get; set; }

        [DataMember(Name = "failed_reason_code", IsRequired = false)]
        public string FailedReasonCode { get; set; }

        [DataMember(Name = "failed_reason_msg", IsRequired = false)]
        public string FailedReasonMessage { get; set; }

        [DataMember(Name = "test_mode")]
        public string TestMode { get; set; }

        [DataMember(Name = "payment_type")]
        public string PaymentType { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "payment_amount")]
        public string PaymentAmount { get; set; }
    }
}
