using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Team14.Data
{
    // 
    //
    public class SkillService : ISkillService
    {
        private readonly string ConnectionString = "MyLocalConnection";
        private readonly IConfiguration __config;

        public SkillService(IConfiguration config)
        {
            __config = config;
        }



        public DbConnection GetConnection()
        {
            return new SqlConnection(__config.GetConnectionString(ConnectionString));
        }
        public Skill GetSkill(int Id)
        {
            string query = @"select * from dbo.Skill WHERE ID=@ID";

            using (var con = GetConnection())
            {
                return con.QueryFirstOrDefault<Skill>(query, new { ID = Id });
            }
        }

        

        // can update existing skill and create a new skill
        public bool UpdateSkill(Skill skill)
        {
            string query = null;
            if (GetSkill(skill.Id) == null){
                query = @"INSERT INTO dbo.Skill (Name, Skilltype)  VALUES(@NAME, @STYPE);";
            }
            else{
                query = @"UPDATE dbo.Skill SET Name = @NAME, Skilltype = @STYPE  WHERE ID = @ID;";
            }
            
            using (var conn = GetConnection())
            {
                int i = conn.Execute(query, new { NAME = skill.Name, STYPE = skill.Skilltype, ID = skill.Id });
                return true;
            }
        }


        // deletes existing skill, if skill not existing then it does nothing
        public bool DeleteSkill(int skillId)
        {
            GetConnection().Execute($"DELETE dbo.Skill WHERE Id = {skillId}");

            return false;
        }


        // returns all existing skills
        public IEnumerable<Skill> GetAllSkills()
        {
            IEnumerable<Skill> skills = null;
            string query = @"SELECT * from dbo.Skill";

            using (var con = GetConnection())
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    skills = con.Query<Skill>(query);
                }
                catch (SqlException e)
                {
                    Console.WriteLine($"Database-Error occured: {e}");
                }

                if (con.State == ConnectionState.Open)
                    con.Close();

            }
            return skills;
        }
    }
}
