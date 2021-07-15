using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferOverview
    {
        private Modal modal { get; set; }

        
        /// <summary>
        /// false: show all offers <para></para>
        /// true: show offers matching search
        /// </summary>
        private bool showSearchResults = false;

        private IEnumerable<Offer> offers;
        private IEnumerable<Skill> skills = new List<Skill>();
        private IEnumerable<Field> fields = new List<Field>();



        private ChangeResult changeInfo = new();
        private IList<Offer> SelectedOffers = new List<Offer>();
        private IList<Skill> SelectedSoftskills = new List<Skill>();
        private IList<Skill> SelectedHardskills = new List<Skill>();
        private IList<Field> SelectedFields = new List<Field>();



        protected override void OnInitialized()
        {
            offerService.ChangeEventHandel += OnChangeReturn;
            offers = offerService.ShowAllOffers();
            foreach (Offer o in offers)
            {
                skills.Union(o.Requirements);
                fields.Union(o.Fields);
                //SelectedSoftskills.Concat(o.Requirements.Where(skill => skill.Type == SkillGroup.Softskill));
                //SelectedHardskills.Concat(o.Requirements.Where(skill => skill.Type == SkillGroup.Hardskill));
                //SelectedFields.Concat(o.Fields);
            }
        }

        private void Validate() { modal.Open(); }
            
        private void Close() { modal.Close(); }

        private void OnSelected(string selection)
        {
            Console.WriteLine(selection);
        }


        private void OnSelected(string selection)
        {
            Console.WriteLine(selection);
        }

        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
            if (changeInfo.InfoMessages.Any())
                navManager.NavigateTo("/offers", forceLoad: true);
        }


        private async Task<IEnumerable<Offer>> SearchTitle(string searchText)
        {
            return await Task.FromResult(offers.Where((x => x.Title.ToLower().Contains(searchText.ToLower()))).ToList());
        }

        private async Task<IEnumerable<Skill>> SearchHardskills(string searchText)
        {
            return await Task.FromResult(skills.Where(skill => skill.Type == SkillGroup.Hardskill && skill.Name.ToLower().StartsWith(searchText.ToLower())).ToList());
        }

        private async Task<IEnumerable<Skill>> SearchSoftskills(string searchText)
        {
            return await Task.FromResult(skills.Where(skill => skill.Type == SkillGroup.Softskill && skill.Name.ToLower().StartsWith(searchText.ToLower())).ToList());
        }

        private async Task<IEnumerable<Field>> SearchFields(string searchText)
        {
            return await Task.FromResult(fields.Where(field => field.Name.ToLower().StartsWith(searchText.ToLower())).ToList());
        }

        private void ShowResults()
        {
            if (SelectedOffers != null || SelectedSoftskills != null || SelectedHardskills != null || SelectedFields != null)
            {
                if (SelectedOffers.Count > 0 || SelectedSoftskills.Count > 0 || SelectedHardskills.Count > 0 || SelectedFields.Count > 0)
                {
                    showSearchResults = true;
                }
                    
                else
                {
                    showSearchResults = false;
                }
            }
            
        }

        private void DeleteAll()
        {
            foreach (var o in offers)
                offerService.Delete(o);
            offerService.ResetId();
            Close();
        }

        public bool AngebotCCollapsed { get; set; }

        void AngebotCToggle()
        {
            AngebotCCollapsed = !AngebotCCollapsed;
        }
    }
}
