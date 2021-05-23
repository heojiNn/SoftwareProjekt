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
        private readonly string SettingsConectString = "MyLocalConnection";

        public SkillService(IConfiguration config)
        {
            _config = config;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString(SettingsConectString));
        }

        public Skill GetSkill(int Id)
        {
            string query = @"select * from dbo.Skill WHERE ID=@ID";

            using (var conn = GetConnection())
            {
                return conn.QueryFirstOrDefault<Skill>(query, new { ID = Id });
            }
        }

        public IEnumerable<Skill> GetAllSkills()
        {
            IEnumerable<Skill> skills = null;
            string query = @"select * from dbo.Skill";

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
                    Console.WriteLine($"Database Problem oNo {e}");
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
            string query;
            if (GetSkill(skill.Id) != null)
                query = @"UPDATE dbo.Skill SET Name = @NAME, Skilltype = @STYPE  WHERE ID = @ID;";
            else
                query = @"INSERT INTO dbo.Skill (Name, Skilltype)  VALUES(@NAME, @STYPE);";

            using (var conn = GetConnection())
            {
                int i = conn.Execute(query, new { NAME = skill.Name, STYPE = skill.Skilltype, ID = skill.Id });

                Console.WriteLine($"safdas {i}");
                return true;
            }
        }

        public bool DeleteSkill(int skillId)
        {
            GetConnection().Execute($"DELETE dbo.Skill WHERE Id = {skillId}");

            return false;
        }
    }
}
