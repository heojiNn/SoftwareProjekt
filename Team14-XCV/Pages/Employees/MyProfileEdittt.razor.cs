using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team14.Data;

namespace Team14.Pages.Employees
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
    }
}
