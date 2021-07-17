using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.Employees
{
    public  partial class EditProfile
    {
    private Modal modal { get; set; }
    private Employee myProfile = new Employee();
    private string skillSearch = "";
    private string proSearch = "";

    private DateTime startyear = DateTime.Now;
    private float reduceY1;
    private float reduceY2;
    private bool onlySele = false;
    private ChangeResult changeInfo = new();


readonly Dictionary<string, object> browseAttributes = new Dictionary<string, object> {
{ "accept", ".jpeg,.png" },   // filter pattern
{ "style", "display:none" }, // for custon label
{ "id", "browse-files" }     // for custom label
};
    protected override async Task OnInitializedAsync()
    {
        var authstate = await authentiProvider.GetAuthenticationStateAsync();
        myProfile = profileService.ShowProfile(authstate.User.Identity.Name) ?? new Employee();
        profileService.ChangeEventHandel += OnChangeReturnEvent;
    }
    private void ChaExpirence()
    {
        var f = startyear.AddYears((int)(reduceY1 / 2)).AddYears((int)(reduceY2 * 2 / 3));
        if (f > DateTime.Now.AddDays(-10))
            myProfile.Experience = null;
        else
            myProfile.Experience = f;
    }

    private bool eventFromAreaCameBefore = true;
    public void KeyHandler(KeyboardEventArgs e, bool isTextArea)
    {
        Close();
        if ((e.Code == "Enter" || e.Code == "NumpadEnter") && !(eventFromAreaCameBefore || isTextArea))
            Validate();
        eventFromAreaCameBefore = isTextArea;
    }
    private void Validate()
    {
        profileService.ValidateUpdate(myProfile);
        if (changeInfo.InfoMessages.Any() || changeInfo.ErrorMessages.Any())
            modal.Open();
    }

    private void Discard()
    {
        // Asks if User want to discard changes and return
    }
    private void Close()
    {
        modal.Close();
        changeInfo = new();
    }
    private async void UpdateProfile()
    {
        if (!changeInfo.ErrorMessages.Any())
        {
            profileService.UpdateProfile(myProfile);
            modal.Close();
            await JS.InvokeVoidAsync("scrollTop");
        }
    }
    private async Task JustUpload(InputFileChangeEventArgs eventArgs)
    {
        var TempPath = await profileService.UploadImage(myProfile.PersoNumber, eventArgs.File);
        myProfile.Image = TempPath;
    }


    private void AddMe(Project p, string act)
    {
        proService.Add(p.Id, myProfile.PersoNumber, act);

    }
    private void RemoveMe(Project p, string act)
    {
        proService.Remove(p.Id, myProfile.PersoNumber, act);
    }


    private void OnChangeReturnEvent(object sender, ChangeResult e) => changeInfo = e;
    }
}
