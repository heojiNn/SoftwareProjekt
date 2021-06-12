using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace Team14.Pages.Login
{
    public partial class Login
    {
        // Used as the EditForm Input Model.
        private Team14.Data.Employee employee = new Team14.Data.Employee();

        private string errorMessage;

        // Calls CustomAuthentiProvider.
        private void LoginMe()
        {
            errorMessage = "";
            try
            {
                CustomAuthentiProvider.Login(employee.PersoNumber, employee.Password);
                NavigationManager.NavigateTo("/my-profile", forceLoad: true);
            }
            catch (ArgumentException e) { errorMessage = e.Message; }
        }
    }
}
