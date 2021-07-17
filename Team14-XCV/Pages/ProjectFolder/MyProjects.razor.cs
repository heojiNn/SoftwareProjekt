using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;

namespace XCV.Pages.ProjectFolder
{
    public partial class MyProjects
    {
       

        private List<Project> myProjects = new();
        private string searchTitle = "";

        private void DeleteProject(Project p)
        {
            projectService.Delete(p);
        }

        protected override async Task OnInitializedAsync()
        {
            var authstate = await authentiProvider.GetAuthenticationStateAsync();

            foreach (var pro in projectService.ShowAllProjects())
            {
                foreach (var kvp in pro.Activities)
                {
                    if (kvp.Value.persNr.Contains(authstate.User.Identity.Name))
                    {
                        myProjects.Add(pro);
                    }
                }
            }
        }
    }
}
