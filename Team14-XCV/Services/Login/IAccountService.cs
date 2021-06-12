using System;
using System.Collections.Generic;




namespace Team14.Data
{
    public interface IAccountService
    {
        public IEnumerable<Employee> ShowAllProfiles();
        public void CreateAccount(Employee newAccount);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
