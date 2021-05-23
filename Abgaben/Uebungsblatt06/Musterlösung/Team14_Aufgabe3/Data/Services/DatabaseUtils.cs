using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Team14.Data
{
    public class DatabaseUtils
    {

        private static string SettingsConectString = "MyLocalConnection";

        public static void CheckAndCreate(IConfiguration config)
        {
            var conn = new SqlConnection(config.GetConnectionString(SettingsConectString));
            using (conn)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    int i = conn.Execute(@"IF OBJECT_ID('dbo.Skill', 'U') IS NULL
                                    CREATE TABLE dbo.Skill 
                                        (  Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,   Name VARCHAR(50),  Skilltype bit NOT NULL 
                                    );");
                    Console.WriteLine($"is strangly always {i}");
                }
                catch (SqlException e)
                {
                    Console.WriteLine($"oNo{e}");
                }


            }
        }
    }
}
