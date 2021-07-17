using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.ProjectFolder
{
    public partial class ProjectOverview
    {
        

        private string searchTitle ="";
        private Modal modal;
        private IEnumerable<Project> allProjects;
        protected override void OnInitialized()
        {
            allProjects = projectService.ShowAllProjects();
        }
        private void DeleteProject(Project p)
        {
            projectService.Delete(p);
        }
        private void DeleteAll()
        {
            foreach (var p in allProjects)
            {
                projectService.Delete(p);
            }
            navManager.NavigateTo("/projects", true);
        }

        private void Confirm()
        {
            modal.Open();
        }
        private void Close()
        {
            modal.Close();
        }
    }
}
