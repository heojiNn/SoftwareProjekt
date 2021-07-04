using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;


namespace XCV.Pages.OfferNamespace
{
    public partial class OfferEdit
    {
        private List<Skill> skills;
        private List<Field> fields;

        private IList<Skill> SelectedHardskills;
        private IList<Skill> SelectedSoftskills;
        private IList<Field> SelectedFields;

        [Parameter]
        public string Id { get; set; }
        private ChangeResult changeInfo = new();
        private Offer myOffer;

        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
        }
        protected override void OnInitialized()
        {
            myOffer = offerService.ShowOffer(int.Parse(Id));
            myOffer ??= new Offer();
            offerService.ChangeEventHandel += OnChangeReturn;
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
