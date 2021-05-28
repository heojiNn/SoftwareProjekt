using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Team14.Data
{
    public class DatabaseUtils
    {
        private static string ConnectionString = "MyLocalConnection";

        public static void CheckAndCreate(IConfiguration config)
        {
            var con = new SqlConnection(config.GetConnectionString(ConnectionString));
            using (con)
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    int i = con.Execute(@"IF OBJECT_ID('dbo.Skill', 'U') IS NULL CREATE TABLE dbo.Skill ( Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY, Name VARCHAR(50), Skilltype bit NOT NULL);");
                }
                catch (SqlException e)
                {
                    Console.WriteLine($"Database error: {e}");
                }
            }
        }
    }
}
