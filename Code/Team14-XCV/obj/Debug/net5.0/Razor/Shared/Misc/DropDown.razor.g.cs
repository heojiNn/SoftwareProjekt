#pragma checksum "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "32cb240f7132d5b734451f98242bf7c6ac858877"
// <auto-generated/>
#pragma warning disable 1591
namespace XCV.Shared.Misc
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using XCV.Shared.Misc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using BlazorDownloadFile;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
    public partial class DropDown<TItem> : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "dropdown");
            __builder.AddAttribute(2, "style", "float:right;");
            __builder.AddAttribute(3, "width", "40");
            __builder.AddAttribute(4, "height", "40");
            __builder.AddAttribute(5, "b-imps4eiydx");
            __builder.OpenElement(6, "button");
            __builder.AddAttribute(7, "class", "btn btn-secondary dropdown-toggle mr-4 oi oi-cog");
            __builder.AddAttribute(8, "data-toggle", "dropdown");
            __builder.AddAttribute(9, "type", "button");
            __builder.AddAttribute(10, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 6 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor"
                                                                                                                    e => this.show=!this.show 

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(11, "aria-haspopup", "true");
            __builder.AddAttribute(12, "aria-expanded", "false");
            __builder.AddAttribute(13, "width", "40");
            __builder.AddAttribute(14, "height", "40");
            __builder.AddAttribute(15, "b-imps4eiydx");
            __builder.AddContent(16, 
#nullable restore
#line 8 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor"
         Tip

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n    ");
            __Blazor.XCV.Shared.Misc.DropDown.TypeInference.CreateCascadingValue_0(__builder, 18, 19, "Dropdown", 20, 
#nullable restore
#line 10 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor"
                                            this

#line default
#line hidden
#nullable disable
            , 21, (__builder2) => {
                __builder2.OpenElement(22, "div");
                __builder2.AddAttribute(23, "class", "dropdown-menu" + " " + (
#nullable restore
#line 11 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor"
                                    show? "show":""

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(24, "b-imps4eiydx");
                __builder2.AddContent(25, 
#nullable restore
#line 12 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor"
             ChildContent

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
            }
            );
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 17 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\Misc\DropDown.razor"
       
    [Parameter]
    public RenderFragment InitialTip { get; set; }
    [Parameter]
    public RenderFragment ChildContent { get; set; }
    [Parameter]
    public EventCallback<TItem> OnSelected { get; set; }

    private bool show = false;
    private RenderFragment Tip;

    protected override void OnInitialized() { this.Tip = InitialTip; }
    public async Task HandleSelect(TItem item, RenderFragment<TItem> contentFragment)
    {
        this.Tip = contentFragment.Invoke(item);
        this.show = false;
        StateHasChanged();
        await this.OnSelected.InvokeAsync(item);
    }

#line default
#line hidden
#nullable disable
    }
}
namespace __Blazor.XCV.Shared.Misc.DropDown
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateCascadingValue_0<TValue>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, System.Object __arg0, int __seq1, TValue __arg1, int __seq2, global::Microsoft.AspNetCore.Components.RenderFragment __arg2)
        {
        __builder.OpenComponent<global::Microsoft.AspNetCore.Components.CascadingValue<TValue>>(seq);
        __builder.AddAttribute(__seq0, "name", __arg0);
        __builder.AddAttribute(__seq1, "Value", __arg1);
        __builder.AddAttribute(__seq2, "ChildContent", __arg2);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591