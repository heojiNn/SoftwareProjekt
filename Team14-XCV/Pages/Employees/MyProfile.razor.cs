using System;
using System.Threading.Tasks;
using XCV.Data;

namespace XCV.Pages.Employees
{
    public partial class MyProfile
    {
    private Employee myProfile = new Employee();
    private ValueWrapper<bool>[] ColabseState = new ValueWrapper<bool>[100];
    private int ColabseNum =0;


    protected override async Task OnInitializedAsync()
    {
        for (int i = 0; i < 100; i++ )
            ColabseState[i] = new ValueWrapper<bool>(true);

        var authstate = await authentiProvider.GetAuthenticationStateAsync();
        myProfile = profileService.ShowProfile(authstate.User.Identity.Name) ?? new Employee();
    }


    }
}
