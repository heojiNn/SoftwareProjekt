using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Team14.Interfaces;

namespace Team14.Data
{
    public class SkillService : ISkillService 
    {
        private readonly string ConnectionString = "MyLocalConnection";
        private readonly IConfiguration _config;

        public SkillService(IConfiguration config)
        {
            _config = config;
        }

        //Datenbankverbindung herstellen
        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString(ConnectionString));
        }

        //Skill mit übergebener iD zurück geben
        public Skill GetSkill (int Id)
        {
            string query = @"SELECT * FROM dbo.Skill WHERE ID=@ID";
            using (var con = GetConnection())
            {
                return con.QueryFirstOrDefault<Skill>(query, new { ID = Id });
            }
        }

        //übergebenen Skill updaten oder neuen erstellen
        public bool UpdateSkill (Skill skill)
        {
            string query = null;
            if (GetSkill(skill.iD) == null)
            {
                query = @"INSERT INTO dbo.Skill (Name, Skilltype) VALUES (@NAME, @STYPE);";
            }
            else
            {
                query = @"UPDATE dbo.Skill SET Name = @NAME, Skilltype = @STYPE WHERE ID = @ID;";
            }
            using ( var con = GetConnection())
            {
                int i = con.Execute(query, new { NAME = skill.Name, STYPE = skill.Kategorisierung, ID = skill.iD });
                return true;
            }
        }

        //löscht existierenden Skill, wenn dieser nicht existiert wird nichts gemacht
        public bool DeleteSkill(int skillId)
        {
            GetConnection().Execute($"DELETE dbo.Skill WHERE Id = {skillId}");
            return false;
        }

        //gibt alle existierenden Skills zurück
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
                    Console.WriteLine($"Database error: {e}");
                }
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return skills;
        }
    }
}
