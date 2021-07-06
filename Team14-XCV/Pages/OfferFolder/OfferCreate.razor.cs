using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferCreate
    {
        private List<Skill> skills;
        private List<Field> fields;

        private IList<Skill> SelectedHardskills = new List<Skill>();
        private IList<Skill> SelectedSoftskills = new List<Skill>();
        private IList<Field> SelectedFields = new List<Field>();

        private IList<Employee> SelectedParticipants = new List<Employee>();

        protected override void OnInitialized()
        {
            skills = skillService.GetAllSkills().ToList();
            fields = fieldService.GetAllFields().ToList();

            if (offerData.creatingOffer != null) //Load in Data after searching employee
            {
                toCreate = offerData.creatingOffer;
                title = offerData.creatingOffer.Title;
                description = offerData.creatingOffer.Description;
                foreach (Field f in offerData.creatingOffer.Fields)
                {
                    SelectedFields.Add(f);
                }
                SelectedSoftskills = offerData.creatingOffer.Requirements.Where(s => s.Type == SkillGroup.Softskill).ToList();
                SelectedHardskills = offerData.creatingOffer.Requirements.Where(s => s.Type == SkillGroup.Hardskill).ToList();
                SelectedParticipants = offerData.creatingOffer.participants;
            } else if (offerData.creatingOffer == null) // Create another offer.
            {
                offerData.creatingOffer = new Offer();
            }
        }

        private string title { get; set; } = "";
        private string description { get; set; } = "";
        private Offer toCreate { get; set; }

        private void CreateOffer()
        {
            offerService.Create(title, description); //First Creates the offer
            toCreate = new Offer { Id = offerService.GetLastId(), Title = title, Description = description };

            if (SelectedFields != null)
            {
                foreach (Field f in SelectedFields)
                {
                    offerService.Add(toCreate, f);
                }
            }

            if (SelectedParticipants != null)
            {
                foreach (Employee e in SelectedParticipants)
                {
                    offerService.Add(toCreate, e);
                }
            }

            
        /*if (SelectedSoftskills != null)
            {
                foreach (Skill s in SelectedSoftskills)
                {
                    offerService.Add(toCreate, s);
                }
            }
            */

            offerData.creatingOffer = null;
        }

        private void Store() //Store Data before searching employee
        {
            offerData.creatingOffer.Title = title;
            offerData.creatingOffer.Description = description;
            offerData.creatingOffer.Fields = SelectedFields;
            offerData.creatingOffer.Requirements = SelectedSoftskills.Concat(SelectedHardskills).ToList();
            offerData.creatingOffer.participants = SelectedParticipants;
        }

        private void Discard()
        {
            offerData.creatingOffer = null;
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
