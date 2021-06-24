using System.Collections.Generic;



namespace XCV.Data
{
    public interface IRoleService
    {
        // Summary:
        //     GetAllRoles from Persistece.
        // Returns:
        //     A List of Roles that might be  empty
        //
        // Exceptions:
        //   Exception:
        //     Could not reach Persistence: {subPath}/{fileName}
        public IEnumerable<Role> GetAllRoles(int Rcl = 8);


        // Summary:
        //     UpdateAllRoles in Persistece.
        // Parameters:
        //   roles:
        //     The roles to replace the old ones.
        //
        // Loges:
        //   LogInformation:
        //     All Role updated  Persitence  {fileName}
        public (int added, int removed) UpdateAllRoles(IEnumerable<dataSetrole> roles);
    }
}
