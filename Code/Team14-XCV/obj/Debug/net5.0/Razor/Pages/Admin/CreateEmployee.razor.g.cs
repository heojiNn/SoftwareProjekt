#pragma checksum "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a5f96702433d6b9dcd13e9effffaf51ebfcc3de0"
// <auto-generated/>
#pragma warning disable 1591
namespace XCV.Pages.Admin
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
#line 2 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
           [Authorize(Roles = "admin")]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/create-employee")]
    public partial class CreateEmployee : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "container-fluid");
            __builder.AddMarkupContent(2, "<div class=\"card my-1\"><div class=\"card-header text-center\"><h3>Neuen Mitarbeiter erstellen</h3></div></div>\r\n\r\n    ");
            __builder.OpenElement(3, "table");
            __builder.AddAttribute(4, "class", "table table-striped");
            __builder.AddMarkupContent(5, "<tr><th>Personalnummer</th>\r\n            <th>Passwort</th>\r\n            <th>Zugriffsrollen</th>\r\n            <th>Vorname</th>\r\n            <th>Nachname</th>\r\n            <th>Angestellt seit  </th>\r\n            <th></th></tr>\r\n        ");
            __builder.OpenElement(6, "tr");
            __builder.OpenElement(7, "td");
            __builder.AddAttribute(8, "class", "form-group");
            __builder.OpenElement(9, "input");
            __builder.AddAttribute(10, "class", "form-control");
            __builder.AddAttribute(11, "id", "ps");
            __builder.AddAttribute(12, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 29 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                               newOne.PersoNumber

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(13, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => newOne.PersoNumber = __value, newOne.PersoNumber));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(14, "\r\n            ");
            __builder.OpenElement(15, "td");
            __builder.AddAttribute(16, "class", "form-group");
            __builder.OpenElement(17, "input");
            __builder.AddAttribute(18, "class", "form-control");
            __builder.AddAttribute(19, "id", "pw");
            __builder.AddAttribute(20, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 33 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                               newOne.Password

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(21, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => newOne.Password = __value, newOne.Password));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n            ");
            __builder.OpenElement(23, "td");
            __builder.AddAttribute(24, "class", "form-group");
            __builder.AddAttribute(25, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 35 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                                             StateHasChanged

#line default
#line hidden
#nullable disable
            ));
#nullable restore
#line 37 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                  
                    List<AccessRole> allRoles = new() { AccessRole.Sales, AccessRole.Admin };
                

#line default
#line hidden
#nullable disable
            __Blazor.XCV.Pages.Admin.CreateEmployee.TypeInference.CreateCheckBoxList_0(__builder, 26, 27, 
#nullable restore
#line 40 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                                     allRoles

#line default
#line hidden
#nullable disable
            , 28, 
#nullable restore
#line 40 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                                                             item => item.ToString() 

#line default
#line hidden
#nullable disable
            , 29, 
#nullable restore
#line 40 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                                                                                                        newOne.AcRoles

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n            ");
            __builder.OpenElement(31, "td");
            __builder.AddAttribute(32, "class", "form-group");
            __builder.OpenElement(33, "input");
            __builder.AddAttribute(34, "class", "form-control");
            __builder.AddAttribute(35, "id", "fName");
            __builder.AddAttribute(36, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 44 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                               newOne.FirstName

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(37, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => newOne.FirstName = __value, newOne.FirstName));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(38, "\r\n\r\n            ");
            __builder.OpenElement(39, "td");
            __builder.AddAttribute(40, "class", "form-group");
            __builder.OpenElement(41, "input");
            __builder.AddAttribute(42, "class", "form-control");
            __builder.AddAttribute(43, "id", "name");
            __builder.AddAttribute(44, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 49 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                               newOne.LastName

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(45, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => newOne.LastName = __value, newOne.LastName));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(46, "\r\n            ");
            __builder.OpenElement(47, "td");
            __builder.AddAttribute(48, "class", "form-group");
            __builder.OpenElement(49, "input");
            __builder.AddAttribute(50, "class", "form-control");
            __builder.AddAttribute(51, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 53 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                               startDate

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(52, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => startDate = __value, startDate));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(53, "\r\n            ");
            __builder.OpenElement(54, "td");
            __builder.OpenElement(55, "input");
            __builder.AddAttribute(56, "type", "submit");
            __builder.AddAttribute(57, "class", "btn btn-primary");
            __builder.AddAttribute(58, "value", "Hinzufügen");
            __builder.AddAttribute(59, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 57 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                                                                                          Create

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 61 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
         foreach (Employee account in accountService.ShowAllProfiles())
        {
            var roles = String.Join(", ", account.AcRoles);

#line default
#line hidden
#nullable disable
            __builder.OpenElement(60, "tr");
            __builder.OpenElement(61, "td");
            __builder.AddContent(62, 
#nullable restore
#line 65 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                     account.PersoNumber

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(63, "\r\n                ");
            __builder.OpenElement(64, "td");
            __builder.AddContent(65, 
#nullable restore
#line 66 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                     account.Password

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(66, "\r\n                ");
            __builder.OpenElement(67, "td");
            __builder.AddContent(68, 
#nullable restore
#line 67 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                     roles

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(69, "\r\n                ");
            __builder.OpenElement(70, "td");
            __builder.AddContent(71, 
#nullable restore
#line 68 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                     account.FirstName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(72, "\r\n                ");
            __builder.OpenElement(73, "td");
            __builder.AddContent(74, 
#nullable restore
#line 69 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                     account.LastName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n                ");
            __builder.OpenElement(76, "td");
            __builder.AddContent(77, 
#nullable restore
#line 70 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                     account.EmployedSince.ToString("dd.MM.yyyy")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(78, "\r\n                ");
            __builder.OpenElement(79, "td");
            __builder.OpenElement(80, "button");
            __builder.AddAttribute(81, "class", "btn btn-secondary");
            __builder.AddAttribute(82, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 71 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                                                                   e => Delete(account)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(83, "Entfernen ");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 73 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.AddMarkupContent(84, "\r\n\r\n\r\n    ");
            __builder.OpenElement(85, "ul");
            __builder.AddAttribute(86, "class", "alert alert-danger");
            __builder.AddAttribute(87, "role", "alert");
#nullable restore
#line 79 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
         foreach (string line in changeInfo.ErrorMessages)
        {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(88, "li");
            __builder.OpenElement(89, "pre");
            __builder.AddContent(90, 
#nullable restore
#line 82 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                      line

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 84 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.AddMarkupContent(91, "\r\n    ");
            __builder.OpenElement(92, "div");
            __builder.AddAttribute(93, "class", "alert-container");
            __builder.OpenElement(94, "div");
            __builder.AddAttribute(95, "class", "alert alert-success");
            __builder.AddAttribute(96, "role", "alert");
            __builder.AddContent(97, 
#nullable restore
#line 88 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
             changeInfo?.SuccesMessage

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(98, "\r\n    ");
            __builder.OpenElement(99, "ul");
            __builder.AddAttribute(100, "class", "alert alert-info");
            __builder.AddAttribute(101, "role", "alert");
#nullable restore
#line 92 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
         foreach (string line in changeInfo.InfoMessages)
        {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(102, "li");
            __builder.OpenElement(103, "pre");
            __builder.AddContent(104, 
#nullable restore
#line 95 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
                      line

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 97 "C:\Users\Silke\Desktop\Master2\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\CreateEmployee.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IAccountService accountService { get; set; }
    }
}
namespace __Blazor.XCV.Pages.Admin.CreateEmployee
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateCheckBoxList_0<TItem>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.Collections.Generic.IEnumerable<TItem> __arg0, int __seq1, global::System.Func<TItem, global::System.String> __arg1, int __seq2, global::System.Collections.Generic.ISet<TItem> __arg2)
        {
        __builder.OpenComponent<global::XCV.Shared.Misc.CheckBoxList<TItem>>(seq);
        __builder.AddAttribute(__seq0, "Data", __arg0);
        __builder.AddAttribute(__seq1, "TextField", __arg1);
        __builder.AddAttribute(__seq2, "SelectedValues", __arg2);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
