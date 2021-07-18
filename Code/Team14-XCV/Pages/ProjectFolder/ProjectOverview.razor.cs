using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.ProjectFolder
{
    public partial class ProjectOverview
    {
        //Parameters and References:
        private string searchTitle ="";
        private Modal modal;
        private IEnumerable<Project> allProjects;
        protected override void OnInitialized()
        {
            allProjects = projectService.ShowAllProjects();
        }

        /// <summary>
        /// Deletes a project p irreversibly.
        /// </summary>
        /// <param name="p"></param>
        private void DeleteProject(Project p)
        {
            projectService.Delete(p);
        }
        /// <summary>
        /// Deletes all projects irreversibly after confirmation.
        /// </summary>
        private void DeleteAll()
        {
            foreach (var p in allProjects)
            {
                projectService.Delete(p);
            }
            navManager.NavigateTo("/projects", true);
        }
        /// <summary>
        /// Opens a modal dialogue to confirm that all projects should be deleted. 
        /// </summary>
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
