using System.Collections.Generic;

namespace NF.Hotmart
{
    public class Affiliate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ucode { get; set; }
        public string Locale { get; set; }
        public List<Document> Documents { get; set; }
    }
}