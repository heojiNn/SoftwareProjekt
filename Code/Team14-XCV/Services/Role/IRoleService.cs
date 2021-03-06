using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface IRoleService
    {
        /// <summary>
        ///         GetAllRoles from persistence.
        /// </summary>
        /// <remarks>
        ///         Catches all SqlExceptions, logs an Error: {e.Message}
        ///         and will just return an empty collection
        /// </remarks>
        ///
        /// <returns>
        ///          A collection of Roles, that might be empty
        /// </returns>
        public IEnumerable<Role> GetAllRoles();


        /// <summary>
        ///         Updates roles in the persistence.
        ///         via delet/insert normally  and  update if only wage changes
        /// </summary>
        /// <remarks>
        ///         uses ValidateRoles(roles),
        ///         catches SqlExceptions and logs them
        /// </remarks>
        ///
        /// <param name="roles">
        ///         The roles to replace the old ones.
        /// </param>
        public (int added, int changed, int removed) UpdateAllRoles(IEnumerable<Role> roles, bool justValidate = false);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
