using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;

namespace XCV.Pages.Employees
{
    public partial class MyProfileEdittt
    {
        private List<Role> roles;
        private List<Skill> skills;
        private List<Field> fields;
        private List<Language> languages;

        private IList<Role> SelectedRoles;
        private IList<Skill> SelectedHardskills;
        private IList<Skill> SelectedSoftskills;
        private IList<Field> SelectedFields;
        private IList<Language> SelectedLanguages;

        protected override void OnInitialized()
        {
            roles = roleService.GetAllRoles().ToList();
            skills = skillService.GetAllSkills().ToList();
            fields = fieldService.GetAllFields().ToList();
            languages = languageService.GetAllLanguages().ToList();

        }
        private async Task<IEnumerable<Role>> SearchRoles(string searchText)
        {

            return await Task.FromResult(roles.Where(
                (x => x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
        }
        private async Task<IEnumerable<Skill>> SearchHardskills(string searchText)
        {

            return await Task.FromResult(skills.Where(
                (x => x.Type == SkillGroup.Hardskill && x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
        }

        private async Task<IEnumerable<Skill>> SearchSoftskills(string searchText)
        {

            return await Task.FromResult(skills.Where(
                (x => x.Type == SkillGroup.Softskill && x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
        }
        private async Task<IEnumerable<Field>> SearchFields(string searchText)
        {

            return await Task.FromResult(fields.Where(
                (x => x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
        }
        private async Task<IEnumerable<Language>> SearchLanguages(string searchText)
        {

            return await Task.FromResult(languages.Where(
                (x => x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
        }

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

        Employee employee = new Employee() { FirstName = "Max",Image = "\\kappa2.jpg", LastName = "Mayer", Description = "Geboren 1969 Eintritt in das Unternehmen 2004 Mitglied der Geschäftsführung seit 2016 Verantwortungsbereich: Vertriebsgeschäftsführung", RCL = 5 };
        IEnumerable<Role>
            rollen = new List<Role>
                () { new Role() { Name = "Software Developer" }, new Role() { Name = "Product Owner" }, new Role() { Name = "UI/UX-Designer" }, new Role() { Name = "Agile Coach" } };
        IEnumerable<Skill>
            hardskills = new List<Skill>
                () { new Skill() { Name = "C", Level = "lvl1", LevelNr = "width:25%" }, new Skill() { Name = "C++", Level = "lvl4", LevelNr = "width:100%" }, new Skill() { Name = "Angular", Level = "lvl3", LevelNr = "width:75%" }, new Skill() { Name = "Jasmine", Level = "lvl2", LevelNr = "width:50%" } };
        IEnumerable<Skill>
            softskills = new List<Skill>
                () { new Skill() { Name = "Interdisziplinärer Sachverstand" }, new Skill() { Name = "Kommunikationsfähigkeit" }, new Skill() { Name = "Soziale Kompetenz" }, new Skill() { Name = "Problemlösungsfähigkeit" } };
        IEnumerable<Field>
            branchenwissen = new List<Field>
                () { new Field() { Name = "Architektur/Bau/Immobilien" }, new Field() { Name = "Automobil" }, new Field() { Name = "Banken/Finanzsektor/Versicherung" } };
        IEnumerable<Language>
            sprachen = new List<Language>
                () { new Language() { Name = "Deutsch", Level = "lvl3" }, new Language() { Name = "Englisch", Level = "lvl3" }, new Language() { Name = "Türkisch", Level = "lvl3" }, new Language() { Name = "Italienisch", Level = "lvl3" }, new Language() { Name = "Spanisch", Level = "lvl3" }, new Language() { Name = "Russisch", Level = "lvl3" } };
        IEnumerable<Project>
            projekte = new List<Project>
                () { new Project() { Title = "Siemens", Purpose = new List<string>() { "Projektleiter" } }, new Project() { Title = "Xitaso", Purpose = new List<string>() { "UI-Design" } }, new Project() { Title = "BMW", Purpose = new List<string>() { "SQL-Server" } } };
        // Dictionary Projekt --> Projekttätigkeit
        Dictionary<Project, string> Taetigkeiten = new Dictionary<Project, string> { { new Project() { Title = "Siemens" }, "Projektleiter"} };
    }
}
