#pragma checksum "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1f2f66076cee634a61ba0d0b9b2da7ac86b0ad8d"
// <auto-generated/>
#pragma warning disable 1591
namespace XCV.Pages.Admin.ChangeDataSet
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
#nullable restore
#line 1 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
using Microsoft.AspNetCore.Components;

#line default
#line hidden
#nullable disable
    public partial class FieldWidg : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "col-12" + " mb-3" + "  " + (
#nullable restore
#line 4 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                           SWidth ? "col-xl-12" : "col-xl-6"

#line default
#line hidden
#nullable disable
            ) + " ");
            __builder.AddAttribute(2, "style", "min-height: 100px;");
            __builder.AddAttribute(3, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 4 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                                                       e => changeInfo = new()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(4, "b-0jwhacc0nv");
            __builder.OpenElement(5, "div");
            __builder.AddAttribute(6, "class", "card mr-0 my-0");
            __builder.AddAttribute(7, "b-0jwhacc0nv");
            __builder.OpenElement(8, "div");
            __builder.AddAttribute(9, "class", "card-header");
            __builder.AddAttribute(10, "style", "padding: 10px;");
            __builder.AddAttribute(11, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 6 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                   Toggle

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "b-0jwhacc0nv");
            __builder.AddMarkupContent(13, "<span style=\"color: #7A212E; font-size: 1.5rem;\" b-0jwhacc0nv> Branchen</span>\r\n            ");
            __builder.OpenElement(14, "span");
            __builder.AddAttribute(15, "class", 
#nullable restore
#line 8 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                          BtnClass

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(16, "b-0jwhacc0nv");
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 11 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
         if (ShowCardBody.Value)
        {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(17, "div");
            __builder.AddAttribute(18, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 13 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                       WidthToggle

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(19, "b-0jwhacc0nv");
            __builder.OpenElement(20, "span");
            __builder.AddAttribute(21, "class", 
#nullable restore
#line 14 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                          WidthClass

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(22, "style", "font-size: 1.5rem;");
            __builder.AddAttribute(23, "b-0jwhacc0nv");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(24, "\r\n        ");
            __builder.OpenElement(25, "div");
            __builder.AddAttribute(26, "class", "card-body p-0");
            __builder.AddAttribute(27, "b-0jwhacc0nv");
#nullable restore
#line 17 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
             foreach (var field in fiService.GetAllFields())
            {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(28, "span");
            __builder.AddAttribute(29, "style", "white-space:nowrap; margin-right: 15px;  line-height: 2.3rem;");
            __builder.AddAttribute(30, "b-0jwhacc0nv");
            __builder.OpenElement(31, "button");
            __builder.AddAttribute(32, "class", "btn btn-outline-primary btn-sm");
            __builder.AddAttribute(33, "disabled");
            __builder.AddAttribute(34, "b-0jwhacc0nv");
            __builder.AddContent(35, 
#nullable restore
#line 20 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                             field

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n                    ");
            __builder.OpenElement(37, "button");
            __builder.AddAttribute(38, "type", "button");
            __builder.AddAttribute(39, "class", "btn btn-light btn-sm");
            __builder.AddAttribute(40, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 21 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                                   e => RemoveField(field) 

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(41, "b-0jwhacc0nv");
            __builder.AddMarkupContent(42, "<span class=\"oi oi-minus\" b-0jwhacc0nv></span>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(43, " <span style=\"white-space:normal;\" b-0jwhacc0nv></span>");
#nullable restore
#line 25 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(44, "<br b-0jwhacc0nv>\r\n\r\n            ");
            __builder.OpenElement(45, "div");
            __builder.AddAttribute(46, "class", "row");
            __builder.AddAttribute(47, "b-0jwhacc0nv");
            __builder.OpenElement(48, "div");
            __builder.AddAttribute(49, "class", "wrap-collabsible4 ml-3");
            __builder.AddAttribute(50, "style", "width:95%");
            __builder.AddAttribute(51, "b-0jwhacc0nv");
            __builder.AddMarkupContent(52, "<input id=\"collapsible4\" class=\"toggle4\" type=\"checkbox\" b-0jwhacc0nv>\r\n                    ");
            __builder.AddMarkupContent(53, "<label for=\"collapsible4\" class=\"lbl-toggle4\" style=\"margin-bottom:0\" b-0jwhacc0nv><tag b-0jwhacc0nv>Neue Branche hinzufügen</tag></label>\r\n\r\n                    ");
            __builder.OpenElement(54, "div");
            __builder.AddAttribute(55, "class", "collapsible-content");
            __builder.AddAttribute(56, "b-0jwhacc0nv");
            __builder.OpenElement(57, "div");
            __builder.AddAttribute(58, "class", "form-row");
            __builder.AddAttribute(59, "b-0jwhacc0nv");
            __builder.OpenElement(60, "input");
            __builder.AddAttribute(61, "id", "newField");
            __builder.AddAttribute(62, "class", "mx-2 my-4");
            __builder.AddAttribute(63, "placeholder", "Name");
            __builder.AddAttribute(64, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 40 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                                              newField.Name

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(65, "oninput", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => newField.Name = __value, newField.Name));
            __builder.SetUpdatesAttributeName("value");
            __builder.AddAttribute(66, "b-0jwhacc0nv");
            __builder.CloseElement();
            __builder.AddMarkupContent(67, "\r\n                            ");
            __builder.OpenElement(68, "button");
            __builder.AddAttribute(69, "class", "btn btn-sm btn-secondary ");
            __builder.AddAttribute(70, "style", " margin-left:5px; margin-top:1.5rem; height:20%");
            __builder.AddAttribute(71, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 41 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                                                                                       CreateField

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(72, "onmouseover", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 41 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                                                                                                                  VCreateField

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(73, "onmouseout", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 41 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                                                                                                                                                                                             VCreateField

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(74, "b-0jwhacc0nv");
            __builder.AddMarkupContent(75, "neue Branche hinzufügen");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(76, "\r\n\r\n\r\n\r\n\r\n            \r\n            ");
            __builder.OpenElement(77, "ul");
            __builder.AddAttribute(78, "class", "alert alert-danger");
            __builder.AddAttribute(79, "role", "alert");
            __builder.AddAttribute(80, "b-0jwhacc0nv");
#nullable restore
#line 53 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                 foreach (string line in changeInfo.ErrorMessages)
                {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(81, "li");
            __builder.AddAttribute(82, "b-0jwhacc0nv");
            __builder.OpenElement(83, "pre");
            __builder.AddAttribute(84, "b-0jwhacc0nv");
            __builder.AddContent(85, 
#nullable restore
#line 56 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                              line

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 58 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
                }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 61 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 65 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Code\Team14-XCV\Pages\Admin\ChangeDataSet\FieldWidg.razor"
       
    [Parameter] public IFieldService fiService { get; set; }
    [Parameter] public ValueWrapper<bool> ShowCardBody { get; set; } = new ValueWrapper<bool>(false);


    private Field newField = new();

    protected override void OnInitialized()
    {
        fiService.ChangeEventHandel += OnChange;
    }


    private void CreateField() => fiService.CreateField(newField);
    private void VCreateField() => fiService.CreateField(newField, justValidate:true);
    private void RemoveField(Field fi) => fiService.DeleteField(fi);


    private ChangeResult changeInfo = new();
    private void OnChange(object sender, ChangeResult e) => changeInfo = e;




    public string BtnClass => ShowCardBody.Value ? "oi oi-caret-top" : "oi oi- oi-caret-bottom";
    public void Toggle() => ShowCardBody.Value = !ShowCardBody.Value;
    private bool SWidth = false;
    public string WidthClass => SWidth ? "oi  oi-resize-height" : "oi oi- oi-resize-width";
    public void WidthToggle() => SWidth = !SWidth;

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
