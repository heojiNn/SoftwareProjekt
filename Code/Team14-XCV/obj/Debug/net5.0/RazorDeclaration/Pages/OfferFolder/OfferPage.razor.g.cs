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
#line 1 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using XCV;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using XCV.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using XCV.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using XCV.Shared.Misc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/_Imports.razor"
using BlazorDownloadFile;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/Pages/OfferFolder/OfferPage.razor"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/moritzboche/Desktop/MAAAASTEEEEER/tutorium-g-team-14/Code/Team14-XCV/Pages/OfferFolder/OfferPage.razor"
           [Authorize(Roles = "sales, admin")]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/offer/{Id:int}")]
    public partial class OfferPage : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JS { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IConfigService configService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGenerateService generateService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IOfferService offerService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IProfileService profileService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomAuthentiProvider authentiProvider { get; set; }
    }
}
#pragma warning restore 1591
