#pragma checksum "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "324dd33e96be626a55a766593ec9542d9347110b"
// <auto-generated/>
#pragma warning disable 1591
namespace XCV.Pages.Admin.ChangeDataSet
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
#line 2 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
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
            __builder.AddMarkupContent(0, "<div class=\"card my-1\"><div class=\"card-header text-center\" style=\"position: relative\"><h3 style=\"text-align: center\">Datenbasis bearbeiten</h3></div></div>\r\n\r\n");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "class", "alert alert-success");
            __builder.AddAttribute(3, "role", "alert");
            __builder.AddContent(4, 
#nullable restore
#line 18 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
     changeInfo?.SuccesMessage

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(5, "\r\n\r\n          \r\n");
            __builder.OpenElement(6, "div");
            __builder.AddAttribute(7, "class", "row");
            __builder.OpenComponent<XCV.Pages.Admin.ChangeDataSet.FieldWidg>(8);
            __builder.AddAttribute(9, "fiService", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.IFieldService>(
#nullable restore
#line 23 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                           fiService

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(10, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.ValueWrapper<System.Boolean>>(
#nullable restore
#line 23 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                                                     ColabseStateField

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(11, "\r\n\r\n    ");
            __builder.OpenComponent<XCV.Pages.Admin.ChangeDataSet.RoleWidg>(12);
            __builder.AddAttribute(13, "roleService", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.IRoleService>(
#nullable restore
#line 26 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                            roService

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(14, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.ValueWrapper<System.Boolean>>(
#nullable restore
#line 26 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                                                      ColabseStateRole

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(15, "\r\n\r\n    ");
            __builder.OpenComponent<XCV.Pages.Admin.ChangeDataSet.LangWidg>(16);
            __builder.AddAttribute(17, "laService", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.ILanguageService>(
#nullable restore
#line 29 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                          langService

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(18, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.ValueWrapper<System.Boolean>>(
#nullable restore
#line 29 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                                                      ColabseStateLang

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(19, "\r\n\r\n    ");
            __builder.OpenComponent<XCV.Pages.Admin.ChangeDataSet.SkillWidg>(20);
            __builder.AddAttribute(21, "skService", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.ISkillService>(
#nullable restore
#line 32 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                           skillService

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(22, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<XCV.Data.ValueWrapper<System.Boolean>>(
#nullable restore
#line 32 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
                                                        ColabseStateSKill

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.AddMarkupContent(23, "\r\n<br>\r\n<br>\r\n<br>");
        }
        #pragma warning restore 1998
#nullable restore
#line 40 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\ChangeDataSet.razor"
       
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
