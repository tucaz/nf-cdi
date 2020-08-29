using System;

namespace NF.Hotmart.DataContract
{
    public class Subscription
    {
        public string SubscriberCode { get; set; }
        public int SubscriptionId { get; set; }
        public string Status { get; set; }
        public DateTime AccessionDate { get; set; }
        public DateTime RequestDate { get; set; }
        public bool Trial { get; set; }
        public SubscriptionPlan Plan { get; set; }
        public Product Product { get; set; }
        public Price Price { get; set; }
        public Subscriber Subscriber { get; set; }

        public bool IsActive => Status == "ACTIVE";
    }
}