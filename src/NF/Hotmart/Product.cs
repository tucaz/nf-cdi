namespace NF.Hotmart
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ucode { get; set; }
        public bool HasCoProduction { get; set; }
        public bool IsSubscription { get; set; }
    }

    public static class MyProduct
    {
        public static int Subscription = 423239;
        public static int Product = 417452;
    }
}