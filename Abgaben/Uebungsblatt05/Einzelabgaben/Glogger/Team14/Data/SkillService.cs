using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Team14.Data
{
    public class SkillService : ISkillService
    {
        private readonly IConfiguration _config;
        public readonly string ConnectionStringName = "MyLocalConnection";
        public SkillService(IConfiguration config)
        {
            _config = config;
        }

        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString(ConnectionStringName));
        }

        public bool DeleteSkill(int skillId)
        {
            using(var conn = GetConnection())
            {
                string query = @"DELETE FROM dbo.Skill WHERE Id = @Id";
                try {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    conn.Execute(query, new { Id = skillId });
                }
                catch (SqlException)
                {
                    Console.WriteLine("Fehler beim Löschen aus DB");
                    
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    }
                }
            return true;
        }

        public List<Skill> GetAllSkills()
        {
            List<Skill> skills = null;
            using (var conn = GetConnection())
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    string query = @"SElECT * FROM dbo.Skill";
                    skills = (conn.Query<Skill>(query)).AsList();
                }
                catch (Exception)
                {
                    Console.WriteLine("Fehler in GetAllSkills");
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return skills;
        }

        public Skill GetSkill(int Id)
        {
            Skill skill = null;
            using (var conn = GetConnection())
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = @"SElECT * FROM dbo.Skill WHERE Id = @Id";
                try
                {
                    skill = conn.QueryFirstOrDefault<Skill>(query, new {Id = Id });
                }
                catch (Exception)
                {
                    Console.WriteLine("Fehler in GetSkill");
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }

            return skill;
        }

        public bool UpdateSkill(Skill skill)
        {
            using(var conn = GetConnection())
            {
                string query;
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    if (GetSkill(skill.Id) != null)
                    {
                        query = @"UPDATE dbo.Skill SET Name=@Name, Skilltype=@Skilltype WHERE Id=@Id";
                    }
                    else
                    {
                        query = @"INSERT INTO dbo.Skill (Name, Skilltype) VALUES (@Name, @Skilltype)";
                    }
                    conn.Execute(query, new { Id = skill.Id, Name = skill.Name, Skilltype = skill.Skilltype });

                }
                catch (Exception)
                {
                    Console.WriteLine("Fehler in UpdateSkill");
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return true;
        }


    }
}
