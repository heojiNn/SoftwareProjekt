// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace XCV.Pages.OfferNamespace
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Shared.Misc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using BlazorDownloadFile;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\OfferFolder\OfferEmployeeDetailView.razor"
           [Authorize(Roles = "sales, admin")]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/offer-employee-detail-view/{Id:int}/{Config}/{PersoNumber}")]
    [Microsoft.AspNetCore.Components.RouteAttribute("/offer-employee-detail-view/{Id:int}/{PersoNumber}")]
    public partial class OfferEmployeeDetailView : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 209 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\OfferFolder\OfferEmployeeDetailView.razor"
 
    // Einzelprofil downloaden
    [Inject] public IBlazorDownloadFileService BlazorDownloadFileService { get; set; }

    private Modal modal { get; set; }
    private void Notify() { modal.Open(); }
    private void Close() { modal.Close(); }

    [Parameter]
    public int Id { get; set; } // Of the parenting offer (identifies the parenting config in combination with PersoNumber below)
    [Parameter]
    public string Config { get; set; } // Used to identify the config which can be edited in the edit page with routing
    [Parameter]
    public string PersoNumber { get; set; }

    private ChangeResult changeInfo = new();
    private Offer offer;
    private Employee offerEmployee;


    protected override void OnInitialized()
    {
        offer = offerService.ShowOffer(Id);
        offerService.ChangeEventHandel += OnChangeReturn;
        offerEmployee = profileService.ShowProfile(PersoNumber) ?? new Employee();
        Employee temp = offerService.ShowOfferEmployee(Id, PersoNumber);
        offerEmployee.offerRole = temp.offerRole;
        offerEmployee.offerRCL = temp.offerRCL;
        offerEmployee.offerWage = temp.offerWage;
        offerEmployee.hoursPerDay = temp.hoursPerDay;
        offerEmployee.daysPerRun = temp.daysPerRun;
        offerEmployee.discount = temp.discount;
    }

    private void OnSelected(string selection)
    {
        Console.WriteLine(selection);
    }


    private void OnChangeReturn(object sender, ChangeResult e)
    {
        changeInfo = e;
    }

    public bool MitarbeiterCollapsed { get; set; }
    public bool HardskillsCollapsed { get; set; }
    public bool SoftskillsCollapsed { get; set; }
    public bool RolleCollapsed { get; set; }
    public bool BranchenwissenCollapsed { get; set; }
    public bool SprachenCollapsed { get; set; }
    public bool ProjektCollapsed { get; set; }

    void RolleToggle()
    {
        RolleCollapsed = !RolleCollapsed;
    }
    void HardskillsToggle()
    {
        HardskillsCollapsed = !HardskillsCollapsed;
    }
    void SoftskillsToggle()
    {
        SoftskillsCollapsed = !SoftskillsCollapsed;
    }
    void BranchenwissenToggle()
    {
        BranchenwissenCollapsed = !BranchenwissenCollapsed;
    }
    void SprachenToggle()
    {
        SprachenCollapsed = !SprachenCollapsed;
    }
    void ProjektToggle()
    {
        ProjektCollapsed = !ProjektCollapsed;
    }
    void MitarbeiterToggle()
    {
        MitarbeiterCollapsed = !MitarbeiterCollapsed;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ISkillService skillService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IProjectService proService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IProfileService profileService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGenerateService generateService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IConfigService configService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IOfferService offerService { get; set; }
    }
}
#pragma warning restore 1591
