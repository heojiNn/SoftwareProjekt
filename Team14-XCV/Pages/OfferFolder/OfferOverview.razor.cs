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
        private IEnumerable<Offer> offers;

        /// <summary>
        /// false: show all offers <para></para>
        /// true: show offers matching search
        /// </summary>
        private bool showSearchResults = false;
        //Search:
        private List<Skill> skills = new List<Skill>();  
        private List<Field> fields = new List<Field>();  
        //Search-selections:
        private IList<Offer> SelectedOffers = new List<Offer>();
        private IList<Skill> SelectedSoftskills = new List<Skill>();
        private IList<Skill> SelectedHardskills = new List<Skill>();
        private IList<Field> SelectedFields = new List<Field>();
        private ChangeResult changeInfo = new();
        //Search-results:
        private IList<Offer> resultingOffers = new List<Offer>();

        protected override void OnInitialized()
        {
            offerService.ChangeEventHandel += OnChangeReturn;
            offers = offerService.ShowAllOffers();
            foreach (Offer o in offers)
            {
                foreach (Skill s in o.Requirements) skills.Add(s);
                foreach (Field f in o.Fields) fields.Add(f);
            }
            //Currently not working, better as squared 
            //skills = offerService.ShowAllOfferSkills();
            //fields = offerService.ShowAllOfferFields();
        }

        private void Validate() { modal.Open(); }
            
        private void Close() { modal.Close(); }

        private void OnSelected(string selection)
        {
            Console.WriteLine(selection);
        }


        /// <summary>
        /// Starts a search if a search "exists", else OfferOverview displays all offers like it does for default.
        /// </summary>
        private void ShowResults()
        {
            if (SelectedOffers.Count == 0 && SelectedSoftskills.Count == 0 && SelectedHardskills.Count == 0 && SelectedFields.Count == 0) showSearchResults = false;
            else
            {
                
                if (SelectedOffers != null && SelectedOffers.Count > 0)
                {
                    resultingOffers = SelectedOffers;
                    showSearchResults = true;
                }
                if (SelectedSoftskills != null && SelectedSoftskills.Count > 0)
                {
                    foreach (Offer o in offers)
                    {
                        foreach (Skill s in SelectedSoftskills) {
                            if (o.Requirements.Where(o => o.Name.Equals(s.Name)).Any())
                            {
                                if (!resultingOffers.Contains(o))
                                {
                                    resultingOffers.Add(o);
                                    showSearchResults = true;
                                }
                            }
                        }
                    }
                }
                if (SelectedHardskills != null && SelectedHardskills.Count > 0)
                {
                    foreach (Offer o in offers)
                    {
                        foreach (Skill s in SelectedHardskills)
                        {
                            if (o.Requirements.Where(o => o.Name.Equals(s.Name)).Any())
                            {
                                if (!resultingOffers.Contains(o))
                                {
                                    resultingOffers.Add(o);
                                    showSearchResults = true;
                                }
                            }
                        }
                    }
                }
                if (SelectedFields != null && SelectedFields.Count > 0)
                {
                    foreach (Offer o in offers)
                    {
                        foreach (Field f in SelectedFields)
                        {
                            if (o.Fields.Where(o => o.Name.Equals(f.Name)).Any())
                            {
                                if (!resultingOffers.Contains(o))
                                {
                                    resultingOffers.Add(o);
                                    showSearchResults = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes all current offer, reseeds Id
        /// </summary>
        private void DeleteAll()
        {
            foreach (var o in offers)
                offerService.Delete(o);
            offerService.ResetId();
            Close();
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






        public bool AngebotCCollapsed { get; set; }

        void AngebotCToggle()
        {
            AngebotCCollapsed = !AngebotCCollapsed;
        }
    }
}
