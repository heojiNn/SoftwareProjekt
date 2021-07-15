using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferCreate
    {
        private Modal modal { get; set; }
        private string fieldSearch = "";
        private string skillSearch = "";
        private bool[] showB = new bool[100];
        private int sBi = 0;
        private ChangeResult changeInfo = new();
        private bool eventFromAreaCameBefore = true;


        // Intermediate storage for Data which may or may not be added to an offer.
        private string title { get; set; } = "";
        private string description { get; set; } = "";
        private DateTime SelectedStart { get; set; }
        private DateTime SelectedEnd { get; set; }
        private Offer toCreate { get; set; }
        private ISet<Skill> SelectedHardskills = new SortedSet<Skill>();
        private ISet<Skill> SelectedSoftskills = new SortedSet<Skill>();
        private ISet<Skill> SelectedSkills = new SortedSet<Skill>();
        private ISet<Field> SelectedFields = new SortedSet<Field>();
        private IList<Employee> SelectedParticipants = new List<Employee>();
        private ISet<(Employee, Role)> SelectedRoles = new HashSet<(Employee, Role)>();



        // All Skills and Fields for the search
        private List<Skill> skills;
        private List<Field> fields;
        protected override void OnInitialized()
        {
            skills = skillService.GetAllSkills().ToList();
            fields = fieldService.GetAllFields().ToList();
            offerService.ChangeEventHandel += OnChangeReturn;

            SelectedStart = DateTime.Now;
            SelectedEnd = DateTime.Now;


            if (offerData.offerStore == null) // Create empty storage
            {
                offerData.offerStore = new Offer();
            }
            else if (offerData.offerStore != null) //Load in Data after searching employee
            {
                toCreate = offerData.offerStore;
                title = offerData.offerStore.Title;
                description = offerData.offerStore.Description;
                SelectedFields = offerData.offerStore.Fields;
                SelectedSoftskills = offerData.offerStore.Requirements.Where(s => s.Type == SkillGroup.Softskill).ToHashSet();
                SelectedHardskills = offerData.offerStore.Requirements.Where(s => s.Type == SkillGroup.Hardskill).ToHashSet();
                SelectedParticipants = offerData.offerStore.participants;
                SelectedStart = offerData.offerStore.Start;
                SelectedEnd = offerData.offerStore.End;
            }
        }

        /// <summary>
        /// Stores Data before searching employee, so it remains on the Create page after leaving it to search.
        /// </summary>
        private void Store()
        {
            SelectedSkills = SelectedSoftskills;
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

        private void RemoveAllEmployees()
        {
            SelectedParticipants = new List<Employee>();
        }

        /// <summary>
        /// Called when clicking the "Abbrechen"-Button, discards the stored content so after returning to the Create page the fields are empty.
        /// </summary>
        private void Discard()
        {
            offerData.offerStore = null;
        }

        private async void CreateOffer()
        {
            if (!changeInfo.ErrorMessages.Any())
            {
                offerService.Create(title, description, SelectedStart, SelectedEnd); //First Creates the offer
                toCreate = new Offer { Id = offerService.GetLastId(), Title = title, Description = description, Start = SelectedStart, End = SelectedEnd };

                if (SelectedFields != null)
                {
                    foreach (Field f in SelectedFields)
                    {
                        offerService.Add(toCreate, f);
                    }
                }
                if (SelectedSoftskills != null)
                {
                    foreach (Skill s in SelectedSoftskills)
                    {
                        offerService.Add(toCreate, s);
                    }
                }
                if (SelectedHardskills != null)
                {
                    foreach (Skill s in SelectedHardskills)
                    {
                        offerService.Add(toCreate, s);
                    }
                }
                if (SelectedParticipants != null)
                {
                    foreach (Employee e in SelectedParticipants)
                    {
                        offerService.Add(toCreate, e);
                        //offerService.Add(toCreate, e, new Role() { Name = e.tempOfferRole, RCL = e.RCL} );
                    }
                }

                offerData.offerStore = null;
                modal.Close();
                await JS.InvokeVoidAsync("scrollTop");
            }
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

        public void KeyHandler(KeyboardEventArgs e, bool isTextArea)
        {
            Close();
            if ((e.Code == "Enter" || e.Code == "NumpadEnter") && !(eventFromAreaCameBefore || isTextArea))
                Validate();
            eventFromAreaCameBefore = isTextArea;
        }


        private void Validate()
        {
            SelectedSkills = SelectedSoftskills;
            SelectedSkills.UnionWith(SelectedHardskills);

            Offer validate = new Offer
            {
                Title = title,
                Description = description,
                Start = SelectedStart,
                End = SelectedEnd,
                Requirements = SelectedSkills,
                Fields = SelectedFields,
                participants = SelectedParticipants,
            };


            offerService.ValidateCreate(validate);
            if (changeInfo.InfoMessages.Any() || changeInfo.ErrorMessages.Any())
                modal.Open();
        }

        private void Close()
        {
            modal.Close();
            changeInfo = new();
        }

        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
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

        public bool MitarbeiterCollapsed { get; set; }
        void MitarbeiterToggle()
        {
            MitarbeiterCollapsed = !MitarbeiterCollapsed;
        }
    }
}
