using System.Collections.Generic;
using System.Threading.Tasks;
using NF.Hotmart;
using NF.Hotmart.DataContract;

namespace NF.NotaFiscal
{
    public static class GetTransactionsFromHotmart
    {
        public static async Task<List<Transaction>> GetAlltransactions()
        {
            var transactions = await Api.GetAllTransactions(MyProduct.Subscription, true);
            return transactions;
        }
    }
}