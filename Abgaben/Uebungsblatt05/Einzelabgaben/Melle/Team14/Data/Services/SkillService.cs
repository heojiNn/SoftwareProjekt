using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Team14.Data
{
    // 
    //
    public class SkillService : ISkillService
    {

        private readonly IConfiguration _config;
        private readonly string SettingsConectString = "the jSOn" ";
        public SkillService(IConfiguration config)
        {
            _config = config;
        }


        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString(SettingsConectString));
        }
        public Skill GetSkill(int skillId)
        {
            return null;
        }

        public IEnumerable<Skill> GetAllSkills()
        {
            IEnumerable<Skill> skills = null;
            const string query = @"select * from Schemaa.Skill";

            using (var conn = GetConnection())
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    skills = conn.Query<Skill>(query);
                }
                catch (SqlException e)
                {
                    Console.WriteLine($"Database Problem OHNO {e}");
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            return skills;
        }


        public bool UpdateSkill(Skill skill)
        {
            return false;
        }

        public bool DeleteSkill(int skillId)
        {
            return false;
        }
    }
}
