#pragma checksum "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "817742327aabaa3cc10a94f84bcd376705137c18"
// <auto-generated/>
#pragma warning disable 1591
namespace XCV.Pages.Login
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
    [Microsoft.AspNetCore.Components.RouteAttribute("/login")]
    public partial class Login : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "container");
            __builder.AddAttribute(2, "b-z742qpv15b");
            __builder.AddMarkupContent(3, "<div class=\"sidenav\" b-z742qpv15b><div class=\"login-main-text\" b-z742qpv15b><img src=\"\\css\\XITASO_Logo_hoch_weiss.png\" b-z742qpv15b></div></div>\r\n    ");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "logMain");
            __builder.AddAttribute(6, "b-z742qpv15b");
            __builder.OpenElement(7, "div");
            __builder.AddAttribute(8, "class", "jumbotron container");
            __builder.AddAttribute(9, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 15 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                   LoginA

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(10, "b-z742qpv15b");
            __builder.OpenElement(11, "div");
            __builder.AddAttribute(12, "class", "form login-form");
            __builder.AddAttribute(13, "b-z742qpv15b");
            __builder.OpenElement(14, "div");
            __builder.AddAttribute(15, "class", "form-group");
            __builder.AddAttribute(16, "b-z742qpv15b");
            __builder.AddMarkupContent(17, "<label for=\"PersoNumber\" b-z742qpv15b>PersNr</label>\r\n                    ");
            __builder.OpenElement(18, "input");
            __builder.AddAttribute(19, "class", "form-control");
            __builder.AddAttribute(20, "id", "PersoNumber");
            __builder.AddAttribute(21, "onkeyup", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 20 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                                                         LoginA

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(22, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 20 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                   pNum

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(23, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => pNum = __value, pNum));
            __builder.SetUpdatesAttributeName("value");
            __builder.AddAttribute(24, "b-z742qpv15b");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(25, "\r\n\r\n                ");
            __builder.OpenElement(26, "div");
            __builder.AddAttribute(27, "class", "form-group");
            __builder.AddAttribute(28, "b-z742qpv15b");
            __builder.AddMarkupContent(29, "<label for=\"PersoNumber\" b-z742qpv15b>Password</label>\r\n                    ");
            __builder.OpenElement(30, "input");
            __builder.AddAttribute(31, "class", "form-control");
            __builder.AddAttribute(32, "id", "Password");
            __builder.AddAttribute(33, "onkeyup", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.KeyboardEventArgs>(this, 
#nullable restore
#line 25 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                                                          LoginA

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(34, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 25 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                   password

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(35, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => password = __value, password));
            __builder.SetUpdatesAttributeName("value");
            __builder.AddAttribute(36, "b-z742qpv15b");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(37, "\r\n\r\n                ");
            __builder.OpenElement(38, "div");
            __builder.AddAttribute(39, "class", "alert alert-info");
            __builder.AddAttribute(40, "role", "alert");
            __builder.AddAttribute(41, "b-z742qpv15b");
            __builder.AddContent(42, 
#nullable restore
#line 29 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                     validationInfo?.ErrorMessage

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(43, "\r\n\r\n                ");
            __builder.OpenElement(44, "button");
            __builder.AddAttribute(45, "class", "btn btn-secondary");
            __builder.AddAttribute(46, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 32 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                            LoginA

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(47, "b-z742qpv15b");
            __builder.AddContent(48, "Login");
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n                ");
            __builder.OpenElement(50, "button");
            __builder.AddAttribute(51, "class", "btn btn-secondary");
            __builder.AddAttribute(52, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 33 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                            Register

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(53, "b-z742qpv15b");
            __builder.AddContent(54, "Registrieren");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(55, "\r\n        ");
            __builder.OpenElement(56, "table");
            __builder.AddAttribute(57, "class", "table table-sm");
            __builder.AddAttribute(58, "b-z742qpv15b");
            __builder.AddMarkupContent(59, @"<tr b-z742qpv15b><th b-z742qpv15b>Personal Number</th>
                <th b-z742qpv15b>Password</th>
                <th b-z742qpv15b>Vorname</th>
                <th b-z742qpv15b>Nachname</th>
                <th b-z742qpv15b>Zugriffs Rollen</th></tr>");
#nullable restore
#line 45 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
             foreach (Employee ac in accountService.ShowAllProfiles())
            {
                var roles = String.Join(", ", ac.AcRoles.Where(role => role != AccessRole.Employee));

#line default
#line hidden
#nullable disable
            __builder.OpenElement(60, "tr");
            __builder.AddAttribute(61, "b-z742qpv15b");
            __builder.OpenElement(62, "td");
            __builder.AddAttribute(63, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 49 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                    e => pNum = ac.PersoNumber

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(64, "b-z742qpv15b");
            __builder.AddContent(65, 
#nullable restore
#line 49 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                                   ac.PersoNumber

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(66, "\r\n                    ");
            __builder.OpenElement(67, "td");
            __builder.AddAttribute(68, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 50 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                    e => password = ac.Password

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(69, "b-z742qpv15b");
            __builder.AddContent(70, 
#nullable restore
#line 50 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                                    ac.Password

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(71, "\r\n                    ");
            __builder.OpenElement(72, "td");
            __builder.AddAttribute(73, "b-z742qpv15b");
            __builder.AddContent(74, 
#nullable restore
#line 51 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                          ac.FirstName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n                    ");
            __builder.OpenElement(76, "td");
            __builder.AddAttribute(77, "b-z742qpv15b");
            __builder.AddContent(78, 
#nullable restore
#line 52 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                          ac.LastName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(79, "\r\n                    ");
            __builder.OpenElement(80, "td");
            __builder.AddAttribute(81, "b-z742qpv15b");
            __builder.AddContent(82, 
#nullable restore
#line 53 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                          roles

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 55 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.AddMarkupContent(83, "\r\n        ");
            __builder.OpenElement(84, "div");
            __builder.AddAttribute(85, "style", "font-size: 0.7rem; text-align: left; padding: 0; line-height: 1em;");
            __builder.AddAttribute(86, "b-z742qpv15b");
            __builder.AddMarkupContent(87, "<b b-z742qpv15b><u b-z742qpv15b> Employee.ToString()</u></b>\r\n            ");
            __builder.OpenElement(88, "select");
            __builder.AddAttribute(89, "class", "form-control");
            __builder.AddAttribute(90, "style", "width: 120px;");
            __builder.AddAttribute(91, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 59 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                 ePersToShow

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(92, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => ePersToShow = __value, ePersToShow));
            __builder.SetUpdatesAttributeName("value");
            __builder.AddAttribute(93, "b-z742qpv15b");
#nullable restore
#line 60 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                 foreach (var ePers in accountService.ShowAllProfiles().Select(x => x.PersoNumber))
                {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(94, "option");
            __builder.AddAttribute(95, "value", 
#nullable restore
#line 62 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                    ePers

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(96, "b-z742qpv15b");
            __builder.AddContent(97, 
#nullable restore
#line 62 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                             ePers

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 63 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.AddMarkupContent(98, "\r\n            ");
            __builder.OpenElement(99, "div");
            __builder.AddAttribute(100, "b-z742qpv15b");
            __builder.OpenElement(101, "pre");
            __builder.AddAttribute(102, "b-z742qpv15b");
#nullable restore
#line 67 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                     if (ePersToShow != "")
                    {
                        

#line default
#line hidden
#nullable disable
            __builder.AddContent(103, 
#nullable restore
#line 69 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                         accountService.ShowProfile(ePersToShow)

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 69 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Login\Login.razor"
                                                                

                    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(104, @"
            <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b> <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b>
            <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b> <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b>
            <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b> <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b>
            <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b> <br b-z742qpv15b><br b-z742qpv15b><br b-z742qpv15b>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IProfileService accountService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomAuthentiProvider authentiProvider { get; set; }
    }
}
#pragma warning restore 1591
