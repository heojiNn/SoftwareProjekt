using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;



namespace Team14.Data
{
    public class SkillService : ISkillService
    {
        private readonly IConfiguration _config;
        public string ConnectionStringName { get; set; } = "Default";
        public SkillService(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString(ConnectionStringName));
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            string connectionString = _config.GetConnectionString(ConnectionStringName);

            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            string connectionString = _config.GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }


        public bool DeleteSkill(int skillId)
        {
            throw new NotImplementedException();
        }

        public List<Skill> GetAllSkills()
        {
            throw new NotImplementedException();
            
        }

        public Skill GetSkill(int skillId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSkill(Skill skill)
        {
            throw new NotImplementedException();
        }
    }
}
