using System;
using System.Collections.Generic;



namespace Team14.Data
{
    public interface IRoleService
    {
        public IEnumerable<Role> GetAllRoles();


        public void UpdateRole(IEnumerable<Role> skills);

        public event EventHandler<NoResult> SearchEventHandel;
    }

}
