using System;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static ServiceProvider ServiceProvider { get; set; }
        public static Global GlobalConfiguration { get; } = new Global();

        static async Task Main(string[] args)
        {
            Setup();

            var logger = ServiceProvider.GetRequiredService<ILogger<Program>>();

            //TODO: Add an initial checking step that will offer options depending on what's already in the database and in the disk
            //Check stuff like: 
            //1. Is there a database?
            //2. Are there files being validated?
            
            //TODO: Make consulta LoteRPS based on protocol to get final information about what worked and what didn't
            //TODO: 1 nota fiscal between 20 and 70 failed (starting from 5)

            try
            {
                var process = ServiceProvider.GetService<ProcessNotasFiscais>();

//                await process.CreateNotasFiscaisFromHotmartTransactions();
//                await process.SendNotasFiscaisToValidation();
//                await process.ProcessValidationResults();
//                await process.TransmitNotasFiscais();
                await process.ProcessTransmissionResults();
            }
            catch (Exception e)
            {
                logger.LogError(e, "CRITICAL EXCEPTION! OMG!");
                throw;
            }
        }

        private static void Setup()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            IServiceCollection collection = new ServiceCollection();

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

            collection
                .AddLogging(configure =>
                    configure
                        .ClearProviders()
                        .AddConsole()
                        .AddFile()
                )
                .AddSingleton<NotasFiscais>()
                .AddTransient<ProcessNotasFiscais>();

            ServiceProvider = collection.BuildServiceProvider();

            var logger = ServiceProvider.GetService<ILogger<Program>>();
            SqLiteBaseRepository.Init(logger);
        }
    }
}