// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace XCV.Pages.Admin.ChangeDataSet
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Shared.Misc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using BlazorDownloadFile;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
           [Authorize(Roles = "admin, sales")]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/change-data-set")]
    public partial class ChangeDataSet : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 40 "C:\Users\Silke\Desktop\MAAASTEEEER\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
       
    public ValueWrapper<bool> ColabseStateField = new ValueWrapper<bool>(true);
    public ValueWrapper<bool> ColabseStateRole = new ValueWrapper<bool>(true);
    public ValueWrapper<bool> ColabseStateLang= new ValueWrapper<bool>(true);
    public ValueWrapper<bool> ColabseStateSKill = new ValueWrapper<bool>(true);

    protected override void OnInitialized()
    {
        fiService.ChangeEventHandel += OnChange;
        roService.ChangeEventHandel += OnChange;
        langService.ChangeEventHandel += OnChange;
        skillService.ChangeEventHandel += OnChange;
    }



    private ChangeResult changeInfo = new();
    private void OnChange(object sender, ChangeResult e) 
    {
        if(e.SuccesMessage != "")
            changeInfo.SuccesMessage= e.SuccesMessage;
       StateHasChanged();
    }
    

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ISkillService skillService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ILanguageService langService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IRoleService roService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFieldService fiService { get; set; }
    }
}
#pragma warning restore 1591
