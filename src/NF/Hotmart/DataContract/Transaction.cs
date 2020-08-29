namespace NF.Hotmart.DataContract
{
    public class Transaction
    {
        public Product Product { get; set; }
        public Member Buyer { get; set; }
        public Affiliate Affiliate { get; set; }
        public Comission Commission { get; set; }
        public Purchase Purchase { get; set; }
        public Offer Offer { get; set; }
    }
}