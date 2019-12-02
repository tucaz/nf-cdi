using System.Collections.Generic;
using System.Threading.Tasks;
using NF.Hotmart;

namespace NF.NotaFiscal
{
    public static class GetTransactionsFromHotmart
    {
        public static async Task<List<Transaction>> GetAlltransactions()
        {
            var transactions = await Api.GetAllTransactions(MyProduct.Product, true);
            return transactions;
        }
    }
}