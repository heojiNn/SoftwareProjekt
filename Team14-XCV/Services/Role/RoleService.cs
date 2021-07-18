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




        // for definition see   IRoleService
        public IEnumerable<Role> GetAllRoles()
        {
            IEnumerable<Role> roles = new List<Role>();
            //-------------------------------------------------------------------------MS-SQL-Query
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                roles = con.Query<Role>("Select * From [Role]");
            }
            catch (SqlException e) { log.LogError($"GetAllRoles() persistence Error: \n{e.Message}\n"); }  //log Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            return roles;
        }

        // for definition see   IRoleService
        public (int added, int changed, int removed) UpdateAllRoles(IEnumerable<Role> roles, bool justValidate = false)
        {
            errorMessages = new();
            int addedRows = 0;
            int changedRows = 0;
            int removedRows = 0;
            var oldRoles = GetAllRoles();
            var toAddAny = roles.Except(oldRoles, new RoleComparerAnything());
            var toRemoveAny = oldRoles.Except(roles, new RoleComparerAnything());
            //---------------------------------------------------------------------------Validation
            foreach (var role in toAddAny)
                ValidateDataAno(role);
            if (errorMessages.Any() || justValidate)
            {
                OnChange(new() { ErrorMessages = errorMessages });                          //Validation-Fail + ErrorMessages
                return (0, 0, 0);    // if one of the (new names) is invalid, nothing changes
            }
            //-------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------MS-SQL-Command
            var toAddWage = roles.Except(oldRoles, new RoleComparer_NameOrRCL());
            var toRemoveWage = oldRoles.Except(roles, new RoleComparer_NameOrRCL());

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                if (!toAddWage.Any() && !toRemoveWage.Any()) // rcl and names didn't change, but wages did
                    changedRows += con.Execute("Update [Role] Set [Wage] = @Wage   Where [Name] = @Name And [Rcl] = @Rcl", toAddAny);
                else
                {
                    removedRows += con.Execute("Delete From [Role] Where [Name] = @Name And [Rcl] = @Rcl", toRemoveAny);
                    addedRows += con.Execute("Insert Into [Role] Values (@Name, @Rcl, @Wage)", toAddAny);
                }
            }
            catch (SqlException e) { log.LogError($"UpdateAllRoles() persistence Error: \n{e.Message}\n"); }    //log SQL-Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            if (changedRows > 0)
                OnChange(new() { SuccesMessage = $"{changedRows}: Löhne aktualisiert." });
            if (addedRows > 0)
                OnChange(new() { SuccesMessage = $"{addedRows}: Einträge hinzugefügt." });
            if (removedRows > 0)
                OnChange(new() { SuccesMessage = $"{addedRows}: Einträge entfernt." });
            return (addedRows, changedRows, removedRows);
        }






        /// <summary>
        ///         Validates against DataAnotation(.Name.Length &lt50 &amp&amp &lt2)
        /// </summary>
        /// <remarks>
        ///         used by Update
        /// </remarks>
        private IEnumerable<string> ValidateDataAno(Role role)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(role, new ValidationContext(role), results, true))
                foreach (var result in results)
                    errorMessages.Add($"{result.ErrorMessage}");
            return errorMessages;
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
