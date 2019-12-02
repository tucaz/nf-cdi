using System;
using System.Linq;

namespace NF.Hotmart
{
    public class Purchase
    {
        public string Transaction { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string PaymentEngine { get; set; }
        public string Status { get; set; }
        public Price Price { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMethod { get; set; }
        public int RecurrencyNumber { get; set; }
        public int WarrantyRefund { get; set; }
        public string SalesNature { get; set; }
        public bool UnderWarranty { get; set; }
        public bool PurchaseSubscription { get; set; }
        public int InstallmentsNumber { get; set; }

        public bool Approved => (new[] {"APPROVED", "COMPLETE"}).Contains(Status);
    }
}