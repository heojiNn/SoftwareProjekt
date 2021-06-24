using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    public class RoleService : IRoleService
    {
        private readonly string connectionString;
        private readonly ILogger<RoleService> log;
        public RoleService(IConfiguration config, ILogger<RoleService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MyLocalConnection");
        }

        //---------------------------------Buissines Logic-----------------------------------------
        // empty








        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Role> GetAllRoles(int rcl = 8)
        {
            IEnumerable<Role> roles = new List<Role>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                if (rcl == 8)
                    roles = con.Query<Role>("Select * From Role Where Rcl <> 0");
                else
                    roles = con.Query<Role>($"Select * From Role Where Rcl = {rcl}");
                con.Close();
            }
            catch (SqlException e)
            {
                log.LogError($" retrieving Roles from database: {e.Message} \n");
            }
            return roles;
        }

        //---write --------------------------------------------------------------------------------
        public (int added, int removed) UpdateAllRoles(IEnumerable<dataSetrole> roles)
        {
            int addedRows = 0;
            int removedRows = 0;

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                var oldRoles = con.Query<Role>("Select * From Role");
                var anyChanged = false;
                foreach (var role in roles)
                    foreach (var wage in role.wages)
                        if (!oldRoles.Where(x => x.Name == role.name && x.Wage == wage).Any())
                            anyChanged = true;
                if (anyChanged)
                {
                    removedRows = con.Execute($"Delete From Role");
                    foreach (var role in roles)
                    {
                        for (int i = 0; i < role.wages.Length; i++)
                            addedRows += con.Execute($"Insert Into Role  Values ('{role.name}', {i + 1}, {role.wages[i].ToString("F", CultureInfo.InvariantCulture)})");
                        con.Execute($"Insert Into Role  Values ('{role.name}', 0, 0.0)");
                    }
                }
            }
            catch (SqlException e)
            {
                log.LogError($"updating Roles in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
            return (addedRows, removedRows);
        }


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------

    }
}
