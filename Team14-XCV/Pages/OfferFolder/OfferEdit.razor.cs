using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferEdit
    {

        //Storage, Parameters and References

        [Parameter]
        public int Id { get; set; }
        private ChangeResult changeInfo = new();
        private Offer myOffer;

        private Modal modal { get; set; }
        private Modal modal2 { get; set; }
        private string fieldSearch = "";
        private string skillSearch = "";
        private bool[] showB = new bool[100];
        private int sBi = 0;
        private string error = "";

        // Values in/out the page
        private string title { get; set; } = "";
        private string description { get; set; } = "";
        private DateTime SelectedStart { get; set; }
        private DateTime SelectedEnd { get; set; }
        private ISet<Skill> SelectedHardskills = new SortedSet<Skill>();
        private ISet<Skill> SelectedSoftskills = new SortedSet<Skill>();
        private ISet<Skill> SelectedSkills = new SortedSet<Skill>();
        private ISet<Field> SelectedFields = new SortedSet<Field>();
        private IList<Employee> SelectedParticipants = new List<Employee>();

        // All Skills and Fields for the search
        private List<Skill> skills;
        private List<Field> fields;




        protected override void OnInitialized()
        {
            myOffer = offerService.ShowOffer(Id);
            myOffer ??= new Offer();
            offerService.ChangeEventHandel += OnChangeReturn;
            skills = skillService.GetAllSkills().ToList();
            fields = fieldService.GetAllFields().ToList();

        }

        private void UpdateOffer()
        {
            myOffer.Fields = SelectedFields;
            //...
            offerService.Update(myOffer);
        }

            if (offerData.offerStore == null) // Create new storage and add Offervalues in Page
            {
                offerData.offerStore = new Offer();
                title = myOffer.Title;
                description = myOffer.Description;
                SelectedStart = myOffer.Start;
                SelectedEnd = myOffer.End;
                SelectedFields = myOffer.Fields;
                SelectedSoftskills = myOffer.Requirements.Where(s => s.Type == SkillGroup.Softskill).ToHashSet();
                SelectedHardskills = myOffer.Requirements.Where(s => s.Type == SkillGroup.Hardskill).ToHashSet();
                SelectedParticipants = myOffer.participants;
            }
            else if (offerData.offerStore != null) // create empty storage and load previously selected Data after switching to employeesearch page
            {
                title = offerData.offerStore.Title;
                description = offerData.offerStore.Description;
                SelectedStart = offerData.offerStore.Start;
                SelectedEnd = offerData.offerStore.End;
                SelectedFields = offerData.offerStore.Fields;
                SelectedSoftskills = offerData.offerStore.Requirements.Where(s => s.Type == SkillGroup.Softskill).ToHashSet();
                SelectedHardskills = offerData.offerStore.Requirements.Where(s => s.Type == SkillGroup.Hardskill).ToHashSet();
                SelectedParticipants = offerData.offerStore.participants;

            }
        }

        private async void UpdateOffer()
        {
            if (!changeInfo.ErrorMessages.Any())
            {
                myOffer.Title = title;
                myOffer.Description = description;
                myOffer.Start = SelectedStart;
                myOffer.End = SelectedEnd;
                myOffer.Fields = SelectedFields;

                SelectedSkills.UnionWith(SelectedSoftskills);
                SelectedSkills.UnionWith(SelectedHardskills);
                myOffer.Requirements = SelectedSkills;
                myOffer.participants = SelectedParticipants;

                offerService.Update(myOffer);
                offerData.offerStore = null;
                modal.Close();
                await JS.InvokeVoidAsync("scrollTop");

            }
        }

        // Modals
        private void ValidateDelete() { modal2.Open(); }
        private void Validate()
        {
            myOffer.Title = title;
            myOffer.Description = description;
            myOffer.Start = SelectedStart;
            myOffer.End = SelectedEnd;
            myOffer.Fields = SelectedFields;

            SelectedSkills.UnionWith(SelectedSoftskills);
            SelectedSkills.UnionWith(SelectedHardskills);
            myOffer.Requirements = SelectedSkills;
            myOffer.participants = SelectedParticipants;

            offerService.ValidateUpdate(myOffer);
            if (changeInfo.InfoMessages.Any() || changeInfo.ErrorMessages.Any())
                modal.Open();
        }
        private void Close() { modal.Close(); modal2.Close(); changeInfo = new(); }




        private void OnChangeReturnEvent(object sender, ChangeResult e) => changeInfo = e;



        /// <summary>
        /// Stores Data before searching employee, so it remains on the Edit page after leaving it to search.
        /// </summary>
        private void Store()
        {
            SelectedSkills.UnionWith(SelectedSoftskills);
            SelectedSkills.UnionWith(SelectedHardskills);
            offerData.offerStore = new Offer()
            {
                Title = title,
                Description = description,
                Start = SelectedStart,
                End = SelectedEnd,
                Fields = SelectedFields,
                Requirements = SelectedSkills,
                participants = SelectedParticipants
            };
        }

        private void Discard()
        {
            offerData.offerStore = null;
        }

        private void Reset()
        {
            offerData.offerStore = new Offer();
            title = myOffer.Title;
            description = myOffer.Description;
            SelectedStart = myOffer.Start;
            SelectedEnd = myOffer.End;
            SelectedFields = myOffer.Fields;
            SelectedSoftskills = myOffer.Requirements.Where(s => s.Type == SkillGroup.Softskill).ToHashSet();
            SelectedHardskills = myOffer.Requirements.Where(s => s.Type == SkillGroup.Hardskill).ToHashSet();
            SelectedParticipants = myOffer.participants;
        }


        private void RemoveAllEmployees()
        {
            SelectedParticipants = new List<Employee>();
        }

        private void RemoveOneEmployee(Employee e)
        {
            SelectedParticipants.Remove(e);
        }

        /// <summary>
        /// Returns end - start in Days
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int Runtime(DateTime end, DateTime start)
        {
            return (end - start).Days;
        }

        // Parameters and else


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

        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
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
