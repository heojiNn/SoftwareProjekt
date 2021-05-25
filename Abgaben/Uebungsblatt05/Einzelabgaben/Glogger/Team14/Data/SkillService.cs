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
