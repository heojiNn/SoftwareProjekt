using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using XCV.Data;

namespace XCV.Pages.ProjectFolder
{
    public partial class ProjectPage
    {
        [Parameter]
        public string Id { get; set; }

        private Project project;
        private IEnumerable<Employee> allEmployees;

        protected override void OnInitialized()
        {
            allEmployees = profilService.ShowAllProfiles();
            project = projectService.ShowProject(int.Parse(Id));
        }

        private void DeleteProject(Project p)
        {
            projectService.Delete(p);
        }

        public bool BeschreibungCollapsed { get; set; }
        public bool ZweckCollapsed { get; set; }
        public bool MitarbeiterCollapsed { get; set; }
        public bool TaetigkeitCollapsed { get; set; }

        void BeschreibungToggle()
        {
            BeschreibungCollapsed = !BeschreibungCollapsed;
        }
        void ZweckToggle()
        {
            ZweckCollapsed = !ZweckCollapsed;
        }
        void MitarbeiterToggle()
        {
            MitarbeiterCollapsed = !MitarbeiterCollapsed;
        }
        void TaetigkeitToggle()
        {
            TaetigkeitCollapsed = !TaetigkeitCollapsed;
        }

    }
}
