// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace XCV.Shared.Misc
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using XCV;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using XCV.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using XCV.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using XCV.Shared.Misc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\_Imports.razor"
using BlazorDownloadFile;

#line default
#line hidden
#nullable disable
    public partial class Modal : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 28 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Shared\Misc\Modal.razor"
       
    [Parameter]
    public RenderFragment Title { get; set; }

    [Parameter]
    public RenderFragment Body { get; set; }

    [Parameter]
    public RenderFragment Footer { get; set; }

    private string modalDisplay = "none;";
    private string modalClass = "";
    private bool showBackdrop = false;

    public void Open()
    {
        modalDisplay = "block;";
        modalClass = "show";
        showBackdrop = true;
    }

    public void Close()
    {
        modalDisplay = "none";
        modalClass = "";
        showBackdrop = false;
    }


#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
