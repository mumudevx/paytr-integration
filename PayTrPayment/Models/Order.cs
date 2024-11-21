using System.Collections.Generic;

namespace PayTrPayment.Models
{
    public class Order
    {
        public string UserIp { get; set; }
        public string OrderId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; }
        public int InstallmentNumber { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public IList<BasketItem> BasketItems { get; set; }
    }
}
