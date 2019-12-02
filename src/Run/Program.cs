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

            await NotasFiscais.GenerateNotasFiscais();
            var all = await NotasFiscais.LoadAllNotasFiscais();
            var s = "";
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
        }
    }
}