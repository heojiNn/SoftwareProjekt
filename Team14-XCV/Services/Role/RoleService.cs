using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    /// <inheritdoc/>
    public class RoleService : IRoleService
    {
        private readonly string connectionString;
        private readonly ILogger<RoleService> log;

        public RoleService(IConfiguration config, ILogger<RoleService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MS_SQL_Connection");
        }

        //-------------------------------------Business Logic--------------------------------------
        //-----------------------------------------------------------------------------------------
        // for definition see   IRoleService
        public IEnumerable<string> ValidateRoles(IEnumerable<Role> roles)
        {
            errorMessages = new();
            foreach (var role in roles)
            {
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(role, new ValidationContext(role), results, true))
                    foreach (var result in results)
                        errorMessages.Add($"{result.ErrorMessage} {role}");
            }
            return errorMessages;
        }




        //-------------------------------------Persistence-----------------------------------------
        //--read   --------------------------------------------------------------------------------
        // for definition see   IRoleService
        public IEnumerable<Role> GetAllRoles()
        {
            IEnumerable<Role> roles = new List<Role>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                roles = con.Query<Role>("Select * From [Role]");
            }
            catch (SqlException e)
            {
                log.LogError($"GetAllRoles() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            return roles;
        }
        //---write --------------------------------------------------------------------------------
        public (int added, int changed, int removed) UpdateAllRoles(IEnumerable<Role> roles)
        {
            int addedRows = 0;
            int changedRows = 0;
            int removedRows = 0;
            var oldRoles = GetAllRoles();
            var toAdd = roles.Except(oldRoles, new RoleComparerAnything());
            var toRemove = oldRoles.Except(roles, new RoleComparerAnything());
            ValidateRoles(toAdd);
            if (errorMessages.Any())
            {
                return (0, 0, 0);    // if a new one(name) is invalid, nothing changes
            }
            var toAddWage = roles.Except(oldRoles, new RoleComparer_NameOrRCL());
            var toRemoveWage = oldRoles.Except(roles, new RoleComparer_NameOrRCL());

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                if (!toAddWage.Any() && !toRemoveWage.Any()) // rcl and names didnt change
                    changedRows += con.Execute("Update [Role] Set [Wage] = @Wage   Where [Name] = @Name And [Rcl] = @Rcl", toAdd);
                else
                {
                    removedRows += con.Execute("Delete From [Role] Where [Name] = @Name And [Rcl] = @Rcl", toRemove);
                    addedRows += con.Execute("Insert Into [Role] Values (@Name, @Rcl, @Wage)", toAdd);
                }
            }
            catch (SqlException e)
            {
                log.LogError($"UpdateAllRoles() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            return (addedRows, changedRows, removedRows);
        }

        //-------------------------------------User Messaging--------------------------------------
        private List<string> errorMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }


    //-------------------------------------Helper Classes--------------------------------------
    public class RoleComparerAnything : IEqualityComparer<Role>
    {
        public bool Equals(Role r1, Role r2)
        {
            if (r1 == null && r2 == null)
                return true;
            else if (r1 == null || r2 == null)
                return false;
            return r1.Name == r2.Name && r1.RCL == r2.RCL && r1.Wage == r2.Wage;
        }
        public int GetHashCode(Role r) => HashCode.Combine(r.Name, r.RCL, r.Wage);
    }
    class RoleComparer_NameOrRCL : IEqualityComparer<Role>
    {
        public bool Equals(Role r1, Role r2)
        {
            if (r1 == null && r2 == null)
                return true;
            else if (r1 == null || r2 == null)
                return false;
            return r1.Name == r2.Name && r1.RCL == r2.RCL;
        }
        public int GetHashCode(Role r) => HashCode.Combine(r.Name, r.RCL);
    }
}
