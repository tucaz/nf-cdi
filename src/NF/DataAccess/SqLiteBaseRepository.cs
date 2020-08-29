using System.Data.SQLite;
using System.IO;
using Dapper;
using Microsoft.Extensions.Logging;
using NF.Hotmart;
using NF.Hotmart.DataContract;
using NF.NotaFiscal;
using NF.NotaFiscal.DataContract;

namespace NF.DataAccess
{
    public static class SqLiteBaseRepository
    {
        private static string DbFile => Path.Combine(@"C:\temp","NF.sqlite");

        public static void Init(ILogger logger)
        {
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<Transaction>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<Member>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<EnviarLoteRpsEnvio>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<EnviarLoteRpsResposta>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<ConsultarNfseRpsResposta>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<Validacao>());

	        if (File.Exists(DbFile))
	        {
		        logger.LogInformation($"{DbFile} exists. Moving on.");
		        return;
		        //File.Delete(DbFile);
	        }

            using (var conn = DbConnection())
            {
                logger.LogInformation($"{DbFile} does not exist. Let's create the file and add some tables.");
	            conn.CreateTables();
	            logger.LogInformation("Tables created. Moving on.");
            }
        }

        private static void CreateTables(this SQLiteConnection conn)
        {
            var create = @"CREATE TABLE nota_fiscal (
	id INTEGER PRIMARY KEY AUTOINCREMENT,
	member Text NOT NULL,
	purchase Text NOT NULL,
	rps_number INTEGER NOT NULL,
	nf_number INTEGER NULL,
	nf_request Text NOT NULL,
	nf_request_xml Text NOT NULL,	
	valid BIT NULL,
	validation Text NULL,
	validation_xml Text NULL,
	sent BIT NOT NULL,
	sucessfully_transmitted BIT NULL,
	nf_response Text NULL,
	nf_response_xml Text NULL,
	nf_data Text NULL,
	nf_data_xml Text NULL,
	is_foreigner BIT NOT NULL,
	invalid_address BIT NOT NULL,
	us_dolar BIT NOT NULL,
	created DATETIME NOT NULL
)";

            conn.Execute(create);
        }

        public static SQLiteConnection DbConnection()
        {
            return new SQLiteConnection ($"Data Source={DbFile};");
        }
    }
}