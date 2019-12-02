using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static Global GlobalConfiguration { get; } = new Global();

        static async Task Main(string[] args)
        {
            Setup();

            SqLiteBaseRepository.Init();

            var transactions = await GetTransactionsFromHotmart.GetAlltransactions();
            var members = await Api.GetAllMembers();
            var nfs = new List<NotaFiscal>();

            var numeroNF = 5;
            foreach (var transaction in transactions)
            {
                var nf = CreateNotaFiscal(transaction, members, numeroNF);
                nfs.Add(nf);

                File.WriteAllText($"C:\\temp\\nfs\\{numeroNF}-env-loterps.xml", nf.NotaFiscalRequestXML);

                numeroNF++;
            }

            using (var conn = SqLiteBaseRepository.DbConnection())
            {
                conn.Open();
                conn.EnableExtensions(true);
                conn.LoadExtension(@"SQLite.Interop.dll", "sqlite3_json_init");
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var nf in nfs)
                    {
                        InsertNotaFiscal(nf, conn);
                    }

                    transaction.Commit();
                }

                conn.Close();
            }
        }

        private static void InsertNotaFiscal(NotaFiscal nf, SQLiteConnection conn)
        {
            conn.Execute(
                @"INSERT INTO nota_fiscal (member, purchase, nf_number, nf_request, nf_request_xml, sent, is_foreigner, invalid_address, created) 
VALUES (json(@member), json(@purchase), @nf_number, @nf_request, @nf_request_xml, @sent, @is_foreigner, @invalid_address, @created)",
                new
                {
                    member = JsonConvert.SerializeObject(nf.HotmartMember),
                    purchase = JsonConvert.SerializeObject(nf.HotmartTransaction),
                    nf_number = nf.Numero,
                    nf_request = JsonConvert.SerializeObject(nf.NotaFiscalRequest),
                    nf_request_xml = nf.NotaFiscalRequestXML,
                    sent = 0,
                    is_foreigner = nf.IsForeigner,
                    invalid_address = nf.InvalidAddress,
                    created = DateTime.UtcNow.ToString("o")
                });
        }

        private static NotaFiscal CreateNotaFiscal(Transaction transaction, List<Member> members, int numeroNotaFiscal)
        {
            var member = members.First(m => m.Id == transaction.Buyer.Id);
            var loteRps = GenerateNotaFiscal.Generate(numeroNotaFiscal, numeroNotaFiscal, transaction,
                out bool isForeigner, out bool invalidAddress);
            var loteRpsXML = GenerateNotaFiscal.Serialize(loteRps);

            var nf = new NotaFiscal
            {
                Numero = numeroNotaFiscal,
                Created = DateTime.Now,
                HotmartTransaction = transaction,
                HotmartMember = member,
                NotaFiscalRequest = loteRps,
                NotaFiscalRequestXML = loteRpsXML,
                IsForeigner = isForeigner,
                InvalidAddress = invalidAddress
            };

            return nf;
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

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(GlobalConfiguration);

            var serviceProvider = services.BuildServiceProvider();
        }
    }
}