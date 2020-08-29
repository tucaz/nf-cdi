using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using NF.DataAccess;
using NF.Hotmart.DataContract;

namespace Run
{
    public class TransactionsReport
    {
        public static async Task Load()
        {
            const string sql = "SELECT member as Member, purchase as `Transaction` FROM nota_fiscal";

            SqLiteBaseRepository.Init(null);

            var data = await SqLiteBaseRepository.DbConnection().Query<TransactionData>(sql);

            using (var writer = new StreamWriter(@"C:\temp\report.csv"))
            {
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(data);
                }
            }
        }

        public class TransactionData
        {
            public Transaction Transaction { get; set; }
            public Member Member { get; set; }
        }
    }
}