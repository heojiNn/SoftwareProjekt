#pragma checksum "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "96992bbf8c5cc13a372bc732b919855c820cf75b"
// <auto-generated/>
#pragma warning disable 1591
namespace XCV.Pages.Employees
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
#nullable restore
#line 2 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
           [Authorize(Roles = "sales, admin")]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/employee-detail-view/{PersoNumber}")]
    public partial class EmployeeDetailView : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "card my-1");
            __builder.OpenElement(2, "div");
            __builder.AddAttribute(3, "class", "card-header text-center");
            __builder.OpenElement(4, "h3");
            __builder.AddContent(5, "Mitarbeiterdetailansicht von ");
            __builder.AddContent(6, 
#nullable restore
#line 18 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                               new String(employee.FirstName + " " + employee.LastName.Substring(0, 1) + ".")

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(7, "\r\n        ");
            __builder.AddMarkupContent(8, "<div width=\"40\" height=\"40\"><style>\r\n                .btn-secondary {\r\n                    border-color: transparent;\r\n                }\r\n            </style></div>");
            __builder.CloseElement();
            __builder.AddMarkupContent(9, "\r\n\r\n    ");
            __builder.OpenElement(10, "div");
            __builder.AddAttribute(11, "class", "card mb-3");
            __builder.AddMarkupContent(12, "<div class=\"card-header\"><h4> Beschreibung </h4></div>\r\n\r\n        ");
            __builder.OpenElement(13, "div");
            __builder.AddAttribute(14, "class", "row g-0");
            __builder.AddMarkupContent(15, "<div class=\"col-md-4\"></div>\r\n            ");
            __builder.OpenElement(16, "div");
            __builder.AddAttribute(17, "class", "col-md-8");
            __builder.OpenElement(18, "div");
            __builder.AddAttribute(19, "class", "card-body");
            __builder.OpenElement(20, "pre");
            __builder.AddContent(21, 
#nullable restore
#line 39 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                              employee.Description

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n                    \r\n                    ");
            __builder.OpenElement(23, "p");
            __builder.AddContent(24, "Rate Card Level: ");
            __builder.AddContent(25, 
#nullable restore
#line 41 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                         employee.RCL

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 42 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                      var yearsE = employee.Experience != null ? (float)(DateTime.Now.Subtract(employee.Experience.Value).Days) / 356 : 0;

#line default
#line hidden
#nullable disable
            __builder.OpenElement(26, "p");
            __builder.AddContent(27, "Berufserfahrung: ");
            __builder.AddContent(28, 
#nullable restore
#line 43 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                          yearsE == 0 ? "noch nicht gesetzt" :  $"{yearsE.ToString("N2") }Jahre" 

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n                    ");
            __builder.OpenElement(30, "p");
            __builder.AddMarkupContent(31, "<b>Rolle:</b> ");
            __builder.AddContent(32, 
#nullable restore
#line 44 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                      employee.Roles.FirstOrDefault()

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(33, " mit RCL (");
            __builder.AddContent(34, 
#nullable restore
#line 44 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                employee.Roles.FirstOrDefault().RCL

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(35, ")");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(36, "\r\n\r\n    ");
            __builder.OpenElement(37, "div");
            __builder.AddAttribute(38, "class", "row gutters-sm");
            __builder.OpenElement(39, "div");
            __builder.AddAttribute(40, "class", "col-sm-6 mb-3");
            __builder.AddAttribute(41, "style", "min-height: 200px;");
            __builder.OpenComponent<XCV.Shared.Misc.CollapsibleCard>(42);
            __builder.AddAttribute(43, "CardHeaderTitle", "Branchenwissen");
            __builder.AddAttribute(44, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 53 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                           true

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(45, "CardBody", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenElement(46, "div");
                __builder2.AddAttribute(47, "class", "scroll");
#nullable restore
#line 56 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                         foreach (Field field in employee.Fields)
                        {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(48, "p");
                __builder2.AddAttribute(49, "class", "ml-2");
                __builder2.AddContent(50, 
#nullable restore
#line 58 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                         field

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(51, " <br>");
                __builder2.CloseElement();
#nullable restore
#line 58 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                       }

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.AddMarkupContent(52, "\r\n\r\n\r\n        ");
            __builder.OpenElement(53, "div");
            __builder.AddAttribute(54, "class", "col-sm-6 mb-3");
            __builder.AddAttribute(55, "style", "min-height: 200px;");
            __builder.OpenComponent<XCV.Shared.Misc.CollapsibleCard>(56);
            __builder.AddAttribute(57, "CardHeaderTitle", "SoftSkills");
            __builder.AddAttribute(58, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 66 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                       true

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(59, "CardBody", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenElement(60, "div");
                __builder2.AddAttribute(61, "class", "scroll");
#nullable restore
#line 69 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                         foreach (Skill sSkill in employee.Abilities.Where(x => x.Type == SkillGroup.Softskill))
                        {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(62, "p");
                __builder2.AddAttribute(63, "class", "ml-2");
                __builder2.AddContent(64, 
#nullable restore
#line 71 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                          sSkill

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(65, " <br>");
                __builder2.CloseElement();
#nullable restore
#line 71 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                         }

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.AddMarkupContent(66, "\r\n\r\n        ");
            __builder.OpenElement(67, "div");
            __builder.AddAttribute(68, "class", "col-sm-6 mb-3 ");
            __builder.AddAttribute(69, "style", "min-height: 200px;");
            __builder.OpenComponent<XCV.Shared.Misc.CollapsibleCard>(70);
            __builder.AddAttribute(71, "CardHeaderTitle", "Hardskills");
            __builder.AddAttribute(72, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 78 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                       true

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(73, "CardBody", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenElement(74, "div");
                __builder2.AddAttribute(75, "class", "scroll2");
#nullable restore
#line 81 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                           var hSkills = employee.Abilities.Where(x => x.Type == SkillGroup.Hardskill);
                            if (hSkills.Any())
                            {
                                var root = hSkills.First().Category.GetRoot();
                                var hRoot = (SkillCategory)(root.Children.First(x => x.Name == "HardSkills"));

                                foreach (SkillCategory topLevel in hRoot.Children)
                                {
                                    if (topLevel is SkillCategory toptCast && toptCast.Children.Any())
                                    {

#line default
#line hidden
#nullable disable
                __builder2.OpenComponent<XCV.Shared.Misc.CollapsibleCard>(76);
                __builder2.AddAttribute(77, "CardHeaderTitle", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String>(
#nullable restore
#line 91 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                               topLevel.Name

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(78, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 91 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                           true

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(79, "SmallInner", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 91 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                            true

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(80, "CardBody", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
#nullable restore
#line 93 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                     foreach (var cat in topLevel.Children)
                    {
                        if (cat is Skill skill)
                        {
                            int lvlIndex = Array.FindIndex(skillService.GetAllLevel(), (x => x == skill.Level));
                            if (lvlIndex == 0)
                            {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(81, "button");
                    __builder3.AddAttribute(82, "class", "btn btn-outline-secondary btn-sm");
                    __builder3.AddAttribute(83, "disabled");
                    __builder3.AddContent(84, 
#nullable restore
#line 100 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                           skill

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 100 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                           }
                                if (lvlIndex == 1)
                                {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(85, "button");
                    __builder3.AddAttribute(86, "class", "btn btn-outline-dark btn-sm");
                    __builder3.AddAttribute(87, "disabled");
                    __builder3.AddContent(88, 
#nullable restore
#line 103 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                      skill

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 103 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                      }
                                if (lvlIndex == 2)
                                {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(89, "button");
                    __builder3.AddAttribute(90, "class", "btn btn-outline-info btn-sm");
                    __builder3.AddAttribute(91, "disabled");
                    __builder3.AddContent(92, 
#nullable restore
#line 106 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                      skill

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 106 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                      }
                                if (lvlIndex == 3)
                                {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(93, "button");
                    __builder3.AddAttribute(94, "class", "btn btn-outline-success btn-sm");
                    __builder3.AddAttribute(95, "disabled");
                    __builder3.AddContent(96, 
#nullable restore
#line 109 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                         skill

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 109 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                         }
                                }
                                if (cat is SkillCategory castCat && castCat.Children.Any())
                                {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(97, "b");
                    __builder3.AddContent(98, 
#nullable restore
#line 113 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                    castCat.Name

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
                    __builder3.AddMarkupContent(99, "\r\n                                ");
                    __builder3.OpenElement(100, "div");
#nullable restore
#line 115 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                     foreach (Skill skill2 in castCat.Children)
                                    {
                                        int lvlIndex = Array.FindIndex(skillService.GetAllLevel(), (x => x == skill2.Level));
                                        if (lvlIndex == 0)
                                        {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(101, "button");
                    __builder3.AddAttribute(102, "class", "btn btn-outline-secondary btn-sm");
                    __builder3.AddAttribute(103, "disabled");
                    __builder3.AddContent(104, 
#nullable restore
#line 120 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                   skill2

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 120 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                                   }
                                        if (lvlIndex == 1)
                                        {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(105, "button");
                    __builder3.AddAttribute(106, "class", "btn btn-outline-dark btn-sm");
                    __builder3.AddAttribute(107, "disabled");
                    __builder3.AddContent(108, 
#nullable restore
#line 123 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                              skill2

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 123 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                              }
                                        if (lvlIndex == 2)
                                        {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(109, "button");
                    __builder3.AddAttribute(110, "class", "btn btn-outline-info btn-sm");
                    __builder3.AddAttribute(111, "disabled");
                    __builder3.AddContent(112, 
#nullable restore
#line 126 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                              skill2

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 126 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                              }
                                        if (lvlIndex == 3)
                                        {

#line default
#line hidden
#nullable disable
                    __builder3.OpenElement(113, "button");
                    __builder3.AddAttribute(114, "class", "btn btn-outline-success btn-sm");
                    __builder3.AddAttribute(115, "disabled");
                    __builder3.AddContent(116, 
#nullable restore
#line 129 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                 skill2

#line default
#line hidden
#nullable disable
                    );
                    __builder3.CloseElement();
#nullable restore
#line 129 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                                }
                                        }

#line default
#line hidden
#nullable disable
                    __builder3.CloseElement();
#nullable restore
#line 131 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                      }
                                        }

#line default
#line hidden
#nullable disable
                }
                ));
                __builder2.CloseComponent();
#nullable restore
#line 134 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                               }
                                }
                            } 

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.AddMarkupContent(117, "\r\n\r\n        ");
            __builder.OpenElement(118, "div");
            __builder.AddAttribute(119, "class", "col-sm-6 mb-3");
            __builder.AddAttribute(120, "style", "min-height: 200px;");
            __builder.OpenComponent<XCV.Shared.Misc.CollapsibleCard>(121);
            __builder.AddAttribute(122, "CardHeaderTitle", "Projekte ");
            __builder.AddAttribute(123, "ShowCardBody", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 143 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                      true

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(124, "CardBody", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenElement(125, "div");
                __builder2.AddAttribute(126, "class", "scroll2");
                __builder2.OpenElement(127, "table");
                __builder2.AddAttribute(128, "class", "table table-bordered table-striped text-center");
                __builder2.AddMarkupContent(129, "<thead><tr><th class=\"text-center\">beteiligte Projekte</th>\r\n                                    <th class=\"text-center\">Tätigkeiten im Projekt</th></tr></thead>");
#nullable restore
#line 153 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                             foreach (Project pro in proService.ShowAllProjects())
                            {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(130, "tr");
                __builder2.OpenElement(131, "td");
                __builder2.AddAttribute(132, "class", "pt-3-half");
                __builder2.AddContent(133, 
#nullable restore
#line 157 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                     pro.Title

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(134, "\r\n                ");
                __builder2.OpenElement(135, "td");
                __builder2.AddAttribute(136, "class", "pt-3-half");
#nullable restore
#line 160 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                     foreach (var keyValue in pro.Activities.Where(x => x.Value.persNr.Contains(employee.PersoNumber)))
                    {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(137, 
#nullable restore
#line 162 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
 keyValue.Key

#line default
#line hidden
#nullable disable
                );
                __builder2.AddMarkupContent(138, "<br>");
#nullable restore
#line 163 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                            }

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
                __builder2.CloseElement();
#nullable restore
#line 165 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                 }

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
                __builder2.CloseElement();
            }
            ));
            __builder.CloseComponent();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(139, "\r\n\r\n\r\n");
            __builder.OpenElement(140, "button");
            __builder.AddAttribute(141, "type", "button");
            __builder.AddAttribute(142, "class", "btn btn-secondary");
            __builder.AddAttribute(143, "style", "float:right; margin: 3em 1em");
            __builder.AddAttribute(144, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 174 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
                                                                                                async () => await BlazorDownloadFileService.DownloadFile("Mitarbeiter.doc",  generateService.GenerateSingleProfile(employee),"application/octet-stream")

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(145, "\r\n    Download des Einzelprofils\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(146, "\r\n");
            __builder.AddMarkupContent(147, "<a href=\"employee-search\" class=\"btn btn-secondary\" style=\"float:right; margin: 3em 1em\"> Zurück zur Mitarbeitersuche </a>");
        }
        #pragma warning restore 1998
#nullable restore
#line 182 "C:\Users\Jonat\Desktop\Git Repositories\SoPro\CICD_DIr\tutorium-g-team-14\Team14-XCV\Pages\Employees\EmployeeDetailView.razor"
 
    // Einzelprofil downloaden
    [Inject] public IBlazorDownloadFileService BlazorDownloadFileService { get; set; }

    [Parameter]
    public string PersoNumber { get; set; } // Of the parenting employee

    private ChangeResult changeInfo = new();
    private Employee employee;


    protected override void OnInitialized()
    {
        employee = profileService.ShowProfile(PersoNumber) ?? new Employee();
    }

    private void OnChangeReturn(object sender, ChangeResult e)
    {
        changeInfo = e;
    }

    public bool MitarbeiterCollapsed { get; set; }
    public bool HardskillsCollapsed { get; set; }
    public bool SoftskillsCollapsed { get; set; }
    public bool RolleCollapsed { get; set; }
    public bool BranchenwissenCollapsed { get; set; }
    public bool SprachenCollapsed { get; set; }
    public bool ProjektCollapsed { get; set; }

    void RolleToggle()
    {
        RolleCollapsed = !RolleCollapsed;
    }
    void HardskillsToggle()
    {
        HardskillsCollapsed = !HardskillsCollapsed;
    }
    void SoftskillsToggle()
    {
        SoftskillsCollapsed = !SoftskillsCollapsed;
    }
    void BranchenwissenToggle()
    {
        BranchenwissenCollapsed = !BranchenwissenCollapsed;
    }
    void SprachenToggle()
    {
        SprachenCollapsed = !SprachenCollapsed;
    }
    void ProjektToggle()
    {
        ProjektCollapsed = !ProjektCollapsed;
    }
    void MitarbeiterToggle()
    {
        MitarbeiterCollapsed = !MitarbeiterCollapsed;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ISkillService skillService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IProjectService proService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IProfileService profileService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IGenerateService generateService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IConfigService configService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IOfferService offerService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private CustomAuthentiProvider authentiProvider { get; set; }
    }
}
#pragma warning restore 1591
