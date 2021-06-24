using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;


namespace XCV.Pages.Offers
{
    public partial class OfferEdit
    {
        private List<Skill> skills;
        private List<Field> fields;

        private IList<Skill> SelectedHardskills;
        private IList<Skill> SelectedSoftskills;
        private IList<Field> SelectedFields;


        protected override void OnInitialized()
        {
            skills = skillService.GetAllSkills().ToList();
            fields = fieldService.GetAllFields().ToList();

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







        public bool HardskillsCollapsed { get; set; }
        public bool SoftskillsCollapsed { get; set; }
        public bool MitarbeiterCollapsed { get; set; }
        public bool BrancheCollapsed { get; set; }

        void HardskillsToggle()
        {
            HardskillsCollapsed = !HardskillsCollapsed;
        }
        void SoftskillsToggle()
        {
            SoftskillsCollapsed = !SoftskillsCollapsed;
        }
        void MitarbeiterToggle()
        {
            MitarbeiterCollapsed = !MitarbeiterCollapsed;
        }
        void BrancheToggle()
        {
            BrancheCollapsed = !BrancheCollapsed;
        }
    }
}
