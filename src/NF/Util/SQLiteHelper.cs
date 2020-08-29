using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

public static class SQLiteHelper
{
    public static async Task Execute(this SQLiteConnection connection, Action<SQLiteConnection> action,
        bool enableExtensions = false)
    {
        using (connection)
        {
            await connection.OpenAsync();
            try
            {
                if (enableExtensions)
                {
                    connection.EnableExtensions(true);
                    connection.LoadExtension(@"SQLite.Interop.dll", "sqlite3_json_init");
                }

                using (var transaction = connection.BeginTransaction())
                {
                    action(connection);
                    transaction.Commit();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static async Task<List<T>> Query<T>(this SQLiteConnection connection, string sql, object param=null, bool enableExtensions = false)
    {
        using (connection)
        {
            await connection.OpenAsync();
            try
            {
                if (enableExtensions)
                {
                    connection.EnableExtensions(true);
                    connection.LoadExtension(@"SQLite.Interop.dll", "sqlite3_json_init");
                }

                using (var transaction = connection.BeginTransaction())
                {
                    var results = await connection.QueryAsync<T>(sql, param);
                    transaction.Commit();
                    return results.ToList();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}