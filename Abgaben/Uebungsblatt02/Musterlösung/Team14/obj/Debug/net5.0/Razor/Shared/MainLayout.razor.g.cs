#pragma checksum "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\Shared\MainLayout.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "05ef48643664999a848c7483706a5f2017bcecd8"
// <auto-generated/>
#pragma warning disable 1591
namespace Team14.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Team14;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\_Imports.razor"
using Team14.Shared;

#line default
#line hidden
#nullable disable
    public partial class MainLayout : LayoutComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "page");
            __builder.AddAttribute(2, "b-9136oml59s");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "sidebar");
            __builder.AddAttribute(5, "b-9136oml59s");
            __builder.OpenComponent<Team14.Shared.NavMenu>(6);
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.AddMarkupContent(7, "\r\n\r\n    ");
            __builder.OpenElement(8, "div");
            __builder.AddAttribute(9, "class", "main");
            __builder.AddAttribute(10, "b-9136oml59s");
            __builder.AddMarkupContent(11, "<div class=\"top-row px-4\" b-9136oml59s><a href=\"https://gitlab.isse.de/lehre/sopro/2021-sose/groups/tutorium-g-team-14/-/tree/master\" target=\"_blank\" b-9136oml59s>Gitlab Repo</a></div>\r\n\r\n        ");
            __builder.OpenElement(12, "div");
            __builder.AddAttribute(13, "class", "content px-4");
            __builder.AddAttribute(14, "b-9136oml59s");
            __builder.AddContent(15, 
#nullable restore
#line 14 "C:\$Allgemein\Git\tutorium-g-team-14\Abgaben\Uebungsblatt02\Einzelabgabe\Wagner\Team14\Shared\MainLayout.razor"
             Body

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
