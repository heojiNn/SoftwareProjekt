#pragma checksum "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\NavMenu.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "faaae58c66aa84cecffbc9cbd1a6a805057e4315"
// <auto-generated/>
#pragma warning disable 1591
namespace XCV.Shared
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
#line 6 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

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
    public partial class NavMenu : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<style b-dkmez7ftnw>\r\n\r\n    .nav-item a {\r\n        height: 2rem !important;\r\n    }\r\n</style>\r\n\r\n");
            __builder.AddMarkupContent(1, "<nav class=\"navbar navbar-expand-lg\" b-dkmez7ftnw><a class=\"navbar-brand\" b-dkmez7ftnw><img src=\"\\css\\XITASO_Logo_quer_weiss.png\" width=\"159\" height=\"40\" b-dkmez7ftnw></a></nav>\r\n\r\n");
            __builder.OpenElement(2, "div");
            __builder.AddAttribute(3, "class", "collapse navbar-collapse");
            __builder.AddAttribute(4, "id", "navbarSupportedContent");
            __builder.AddAttribute(5, "b-dkmez7ftnw");
            __builder.OpenElement(6, "ul");
            __builder.AddAttribute(7, "class", "nav flex-column");
            __builder.AddAttribute(8, "b-dkmez7ftnw");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(9);
            __builder.AddAttribute(10, "Roles", "employee");
            __builder.AddAttribute(11, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder2) => {
                __builder2.OpenElement(12, "li");
                __builder2.AddAttribute(13, "class", "nav-item px-3 pb-4");
                __builder2.AddAttribute(14, "b-dkmez7ftnw");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(15);
                __builder2.AddAttribute(16, "class", "nav-link");
                __builder2.AddAttribute(17, "href", "");
                __builder2.AddAttribute(18, "Match", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.Routing.NavLinkMatch>(
#nullable restore
#line 24 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\NavMenu.razor"
                                                         NavLinkMatch.All

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(19, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(20, "<span class=\"oi oi-home\" b-dkmez7ftnw></span> Profil\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(21, "\r\n            ");
                __builder2.OpenElement(22, "li");
                __builder2.AddAttribute(23, "class", "nav-item px-3 pb-4");
                __builder2.AddAttribute(24, "b-dkmez7ftnw");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(25);
                __builder2.AddAttribute(26, "class", "nav-link");
                __builder2.AddAttribute(27, "href", "my-projects");
                __builder2.AddAttribute(28, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(29, "<span class=\"oi oi-box\" aria-hidden=\"true\" b-dkmez7ftnw></span> Meine Projekte\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(30, "\r\n\r\n        ");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(31);
            __builder.AddAttribute(32, "Roles", "sales, admin");
            __builder.AddAttribute(33, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder2) => {
                __builder2.OpenElement(34, "li");
                __builder2.AddAttribute(35, "class", "nav-item px-3 pb-4");
                __builder2.AddAttribute(36, "b-dkmez7ftnw");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(37);
                __builder2.AddAttribute(38, "class", "nav-link");
                __builder2.AddAttribute(39, "href", "projects");
                __builder2.AddAttribute(40, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(41, "<span class=\"oi oi-project\" aria-hidden=\"true\" b-dkmez7ftnw></span> Projektübersicht\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(42, "\r\n            ");
                __builder2.OpenElement(43, "li");
                __builder2.AddAttribute(44, "class", "nav-item px-3 pb-4");
                __builder2.AddAttribute(45, "b-dkmez7ftnw");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(46);
                __builder2.AddAttribute(47, "class", "nav-link");
                __builder2.AddAttribute(48, "href", "employee-search");
                __builder2.AddAttribute(49, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(50, "<span class=\"oi oi-magnifying-glass\" aria-hidden=\"true\" b-dkmez7ftnw></span> Mitarbeitersuche\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(51, "\r\n            ");
                __builder2.OpenElement(52, "li");
                __builder2.AddAttribute(53, "class", "nav-item px-3 pb-4");
                __builder2.AddAttribute(54, "b-dkmez7ftnw");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(55);
                __builder2.AddAttribute(56, "class", "nav-link");
                __builder2.AddAttribute(57, "href", "offers");
                __builder2.AddAttribute(58, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(59, "<span class=\"oi oi-document\" aria-hidden=\"true\" b-dkmez7ftnw></span> Angebotsübersicht\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(60, "\r\n            ");
                __builder2.OpenElement(61, "li");
                __builder2.AddAttribute(62, "class", "nav-item px-3 pb-1");
                __builder2.AddAttribute(63, "b-dkmez7ftnw");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(64);
                __builder2.AddAttribute(65, "class", "nav-link");
                __builder2.AddAttribute(66, "href", "change-data-set");
                __builder2.AddAttribute(67, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(68, "<span class=\"oi oi-wrench\" aria-hidden=\"true\" b-dkmez7ftnw></span> Datenbasis ändern\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(69, "\r\n                ");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(70);
                __builder2.AddAttribute(71, "class", "nav-link");
                __builder2.AddAttribute(72, "href", "change-json");
                __builder2.AddAttribute(73, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(74, "<span class=\"oi oi-wrench\" aria-hidden=\"true\" b-dkmez7ftnw></span> Datenbasis .json\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(75, "\r\n\r\n        ");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Authorization.AuthorizeView>(76);
            __builder.AddAttribute(77, "Roles", "admin");
            __builder.AddAttribute(78, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Authorization.AuthenticationState>)((context) => (__builder2) => {
                __builder2.OpenElement(79, "li");
                __builder2.AddAttribute(80, "class", "nav-item px-3 pb-1");
                __builder2.AddAttribute(81, "b-dkmez7ftnw");
                __builder2.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(82);
                __builder2.AddAttribute(83, "class", "nav-link");
                __builder2.AddAttribute(84, "href", "create-employee");
                __builder2.AddAttribute(85, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(86, "<span class=\"oi oi-wrench\" aria-hidden=\"true\" b-dkmez7ftnw></span> Mitarbeiter hinzu\r\n                ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(87, "\r\n\r\n            ");
            __builder.OpenElement(88, "li");
            __builder.AddAttribute(89, "class", "nav-item px-3");
            __builder.AddAttribute(90, "b-dkmez7ftnw");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Routing.NavLink>(91);
            __builder.AddAttribute(92, "class", "nav-link");
            __builder.AddAttribute(93, "href", "#");
            __builder.AddAttribute(94, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 75 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\NavMenu.razor"
                                                             Logout

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(95, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(96, "<span class=\"oi oi-account-logout\" b-dkmez7ftnw></span>Logout\r\n\r\n                ");
            }
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 87 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Shared\NavMenu.razor"
       

    private string persoNumber;
    protected override async Task OnInitializedAsync()
    {
        var authstate = await CustomAuthentiProvider.GetAuthenticationStateAsync();
        persoNumber = authstate.User.Identity.Name;
    }


    public async Task Logout()
    {
        await CustomAuthentiProvider.Logout();
        NavigationManager.NavigateTo("/", forceLoad: true);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomAuthentiProvider CustomAuthentiProvider { get; set; }
    }
}
#pragma warning restore 1591
