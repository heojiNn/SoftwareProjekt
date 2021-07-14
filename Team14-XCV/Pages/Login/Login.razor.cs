using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;

namespace XCV.Pages.Login
{
    public partial class Login
    {

        private string pNum;
        private string password;
        private LoginResult validationInfo;

        private string ePersToShow = "";

        protected override async Task OnInitializedAsync()
        {
            var authstate = await authentiProvider.GetAuthenticationStateAsync(); // insert PersoNumber if allready logdIN
            pNum = authstate.User.Identity.Name;
            authentiProvider.ValidationEventHandel += OnValidationReturn;
        }


        private async void LoginA()
        {
            if (await authentiProvider.Login(pNum, password))
                NavigationManager.NavigateTo("/my-profile", forceLoad: true);
        }

        private async void Register()
        {
            if (await authentiProvider.Register())
                NavigationManager.NavigateTo("/register", forceLoad: true);
        }


        private void OnValidationReturn(object sender, LoginResult e) => validationInfo = e;

    }
}
