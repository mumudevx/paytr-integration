using System.Runtime.Serialization;

namespace PayTrPayment.Models.Responses
{
    public class TokenResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "reason", IsRequired = false)]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
