using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Team14.Data
{
    public class DatabaseUtils
    {
        private static readonly string ConnectionString = "MyLocalConnection";

        public static void CheckOnCreate(IConfiguration config)
        {
            using (var conn = new SqlConnection(config.GetConnectionString(ConnectionString)))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    string query = "CREATE TABLE IF NOT EXISTS SKILL (Id NOT NULL IDENTITY(1,1) PRIMARY KEY, Name VARCHAR(50), Skilltype BIT NOT NULL)";
                    conn.Execute(query);
                }
                catch (Exception)
                {
                    Console.WriteLine("Fehler in DatabaseUtils");
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

    }
}
