using System;
using System.Threading.Tasks;
using XCV.Data;

namespace XCV.Pages.Employees
{
    public partial class MyProfile
    {


        private Employee myProfile = new Employee();


        protected override async Task OnInitializedAsync()
        {
            var authstate = await authentiProvider.GetAuthenticationStateAsync();

            myProfile = profileService.ShowProfile(authstate.User.Identity.Name) ?? new Employee();
        }

        private void OnSelected(string selection)
        {
            Console.WriteLine(selection);
        }




    }
}
