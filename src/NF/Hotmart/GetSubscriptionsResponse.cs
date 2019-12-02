using System.Collections.Generic;

namespace NF.Hotmart
{
    public class GetSubscriptionsResponse
    {
        public int Size { get; set; }
        public List<Subscription> Data { get; set; }
        public int Page { get; set; }
    }
}