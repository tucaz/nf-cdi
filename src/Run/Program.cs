using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CsvHelper;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NF;
using NF.DataAccess;
using NF.Hotmart;
using NF.NotaFiscal;

namespace Run
{
    public class TransactionsReport
    {
        public static async Task Load()
        {
            const string sql = "SELECT member as Member, purchase as `Transaction` FROM nota_fiscal";

            SqLiteBaseRepository.Init();
            
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

    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static Global GlobalConfiguration { get; } = new Global();

        static async Task Main(string[] args)
        {
            Setup();


//            await CreateNotasFiscaisFromHotmartTransactions();
//            await SendNotasFiscaisToValidation();
//            await ProcessValidationResults();
            await TransmitNotasFiscais();
//            await ProcessTransmissionResults();

        }

        private static async Task ProcessTransmissionResults()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForTransmissionResult)
                .ToList();

            await NotasFiscais.ProcessResultsFromTransmission(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Retorno\",
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Erro\");
        }

        private static async Task TransmitNotasFiscais()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForTransmission)
                .OrderBy(nf => nf.Numero)
                .Take(5)
                .ToList();
            
            await NotasFiscais.SendNotasFiscaisToReceita(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Envio\");
        }

        private static async Task ProcessValidationResults()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForValidation)
                .ToList();

            var files = NotasFiscais.ProcessResultsFromValidation(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Retorno");
        }

        private static async Task SendNotasFiscaisToValidation()
        {
            var nfs = (await NotasFiscaisDb.LoadAllNotasFiscais())
                .Where(nf => nf.ReadyForValidation)
                .ToList();


            await NotasFiscais.SendNotasFiscaisToValidation(nfs,
                @"C:\Program Files\Unimake\UniNFe\34231972000109\nfse\Validar\");
        }

        private static async Task CreateNotasFiscaisFromHotmartTransactions()
        {
            await NotasFiscais.GenerateNotasFiscais(5);
        }

        private static void Setup()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            var builder = new ConfigurationBuilder();
            // tell the builder to look for the appsettings.json file
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //only add secrets in development
            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();
            Configuration.Bind(GlobalConfiguration);
            
            SqLiteBaseRepository.Init();
        }
    }
}