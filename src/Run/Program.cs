using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NF;
using NF.DataAccess;
using NF.NotaFiscal;

namespace Run
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static ServiceProvider ServiceProvider { get; set; }
        public static Global GlobalConfiguration { get; } = new Global();

        static async Task Main(string[] args)
        {
            // var c = new ConsultarLoteRpsEnvio();
            // c.Prestador = new tcIdentificacaoPrestador();
            // c.Prestador.Cnpj = "34231972000109";
            // c.Prestador.InscricaoMunicipal = "269905";
            // c.Protocolo = "347773059";
            //
            // var c = new ConsultarNfseRpsEnvio();
            // c.Prestador = new tcIdentificacaoPrestador();
            // c.Prestador.Cnpj = "34231972000109";
            // c.Prestador.InscricaoMunicipal = "269905";
            // c.IdentificacaoRps = new tcIdentificacaoRps();
            // c.IdentificacaoRps.Numero = "495";
            // c.IdentificacaoRps.Serie = "NFSE";
            // c.IdentificacaoRps.Tipo = 1;
            //
            // var xml = NotaFiscal.Serialize(c);
            // File.WriteAllText(@"C:\temp\1-ped-sitnfserps.xml", xml);

            // return;

            Setup();

            var r = new Regex(@"(\d+)", RegexOptions.Compiled);

            var c = 1553;
            var files = Directory.GetFiles(@"C:\Users\tucaz\Box\Admin\Finance\Notas Fiscais Emitidas\Assinaturas\Batch - 2020-08-26\Rename").OrderBy<string, int>(x =>
             {
                 var filename = Path.GetFileName(x);
                 var matches = r.Matches(filename);
                 return Convert.ToInt32(matches.First().Value);
             });


            foreach (var file in files)
            {
                var dest = Path.GetDirectoryName(file);
                File.Move(file, Path.Join(dest, $"NotaFiscal_{c.ToString().PadLeft(4, '0')}.pdf"));
                c++;
            }

            return;


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
                var transactionsToFilter = File.ReadAllLines(@"C:\Users\tucaz\Box\Admin\Finance\Notas Fiscais Emitidas\Assinaturas\Batch - 2020-08-26\Transactions - NOVO Inglês Assinatura - 2020-08-26.txt");
                //var transactionsToFilter = File.ReadAllLines(@"C:\temp\vlc.txt");

                //await process.CreateNotasFiscaisFromHotmartTransactions(1576, transactionsToFilter);
                //await process.SendNotasFiscaisToValidation();
                //await process.ProcessValidationResults();
                //await process.TransmitNotasFiscais();
                //await process.ProcessTransmissionResults();
                await process.ProcessNotaFiscalDetails();
                //All the processes above can be executed one after the other given some delay between them for the UniNFE to be able to move stuff around
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
                        .AddFile(config =>
                        {
                            config.LogDirectory = @"C:\temp\";
                        })
                )
                .AddSingleton<NotasFiscais>()
                .AddTransient<ProcessNotasFiscais>();

            ServiceProvider = collection.BuildServiceProvider();

            var logger = ServiceProvider.GetService<ILogger<Program>>();
            SqLiteBaseRepository.Init(logger);
        }
    }
}