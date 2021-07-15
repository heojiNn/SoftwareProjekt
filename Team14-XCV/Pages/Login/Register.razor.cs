using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;

namespace XCV.Pages.Login
{
    public partial class Register
    {
        

        private Employee newOne = new();
        private DateTime startDate = DateTime.Now.Date;

        private ChangeResult changeInfo = new();
        protected override void OnInitialized()
        {

            
            accountService.ChangeEventHandel += OnChangeReturn;
        }


        private async void Create()
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
                await authentiProvider.Logout();
                if (await authentiProvider.Login(newOne.PersoNumber, newOne.Password))
                    NavigationManager.NavigateTo("/my-profile", forceLoad: true);
            
        }



        private void OnChangeReturn(object sender, ChangeResult e) => changeInfo = e;


    }
}
