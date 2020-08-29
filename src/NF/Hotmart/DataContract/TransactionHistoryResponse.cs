using System.Collections.Generic;

namespace NF.Hotmart.DataContract
{
    public class TransactionHistoryResponse
    {
        public List<Summary> Summary { get; set; }
        public List<Transaction> Data { get; set; }

        public int Size { get; set; }
        public int Page { get; set; }
    }
}