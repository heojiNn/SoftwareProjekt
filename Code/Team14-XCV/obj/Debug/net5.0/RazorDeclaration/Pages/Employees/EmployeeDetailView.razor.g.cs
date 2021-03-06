// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace XCV.Pages.Employees
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
#line 2 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
           [Authorize]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/employee-detail-view/{PersoNumber}")]
    public partial class EmployeeDetailView : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 189 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
 
    // Einzelprofil downloaden
    [Inject] public IBlazorDownloadFileService BlazorDownloadFileService { get; set; }

    [Parameter]
    public string PersoNumber { get; set; } // Of the parenting employee

    private ChangeResult changeInfo = new();
    private Employee employee;


    protected override void OnInitialized()
    {
        employee = profileService.ShowProfile(PersoNumber) ?? new Employee();
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
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomAuthentiProvider authentiProvider { get; set; }
    }
}
#pragma warning restore 1591
