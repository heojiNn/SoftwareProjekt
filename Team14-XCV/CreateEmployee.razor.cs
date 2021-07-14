using System;
using XCV.Data;

namespace XCV.Pages.Admin
{
    public  partial class CreateEmployee
    {
        private Employee newOne = new();
        private DateTime startDate = DateTime.Now.Date;

        private ChangeResult changeInfo = new();
        protected override void OnInitialized()
        {
            accountService.ChangeEventHandel += OnChangeReturn;
        }


        private void Create()
        {
            Employee copy = new()/// copy cause working-since is readonly(init)
            {
                PersoNumber = newOne.PersoNumber,
                Password = newOne.Password,
                FirstName = newOne.FirstName,
                LastName = newOne.LastName,
                AcRoles = newOne.AcRoles,
                EmployedSince = startDate
            };
            accountService.CreateAccount(copy);
        }
        private void Delete(Employee e)
        {
            accountService.DeleteAccount(e.PersoNumber);
        }


        private void OnChangeReturn(object sender, ChangeResult e) => changeInfo = e;
    }
}
