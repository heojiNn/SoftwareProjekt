using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;

namespace XCV.Pages.ProjectFolder
{
    public partial class MyProjects
    {
        private List<Project> myProjects = new();
        private string searchTitle = "";

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
