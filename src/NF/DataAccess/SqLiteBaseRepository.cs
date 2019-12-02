using System.Data.SQLite;
using System.IO;
using Dapper;
using NF.Hotmart;
using NF.NotaFiscal;

namespace NF.DataAccess
{
    public static class SqLiteBaseRepository
    {
        private static string DbFile => Path.Combine(@"C:\temp","NF.sqlite");

        public static void Init()
        {
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<Transaction>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<Member>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<EnviarLoteRpsEnvio>());
	        SqlMapper.AddTypeHandler(new JsonTypeHandler<EnviarLoteRpsResposta>());
	        
	        if (File.Exists(DbFile)) return;
            
            using (var conn = DbConnection())
            {
                conn.CreateTables();
            }
        }

        private static void CreateTables(this SQLiteConnection conn)
        {
            var create = @"CREATE TABLE nota_fiscal (
	id INTEGER PRIMARY KEY AUTOINCREMENT,
	member Text NOT NULL,
	purchase Text NOT NULL,
	nf_number INTEGER NOT NULL,
	nf_request Text NOT NULL,
	nf_request_xml Text NOT NULL,
	nf_response Text NULL,
	nf_response_xml Text NULL,
	valid BIT NULL,
	sent BIT NOT NULL,
	is_foreigner BIT NOT NULL,
	invalid_address BIT NOT NULL,
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