using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferOverview
    {

        /// <summary>
        /// false: show all offers <para></para>
        /// true: show searchresults
        /// </summary>
        private bool showSearchResults = false;

        private IEnumerable<Offer> offers;
        private ChangeResult changeInfo = new();
        private IList<Offer> SelectedOffers = new List<Offer>();

        protected override void OnInitialized()
        {
            offerService.ChangeEventHandel += OnChangeReturn;
            offers = offerService.ShowAllOffers();

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

        private void ShowResults()
        {
            if (SelectedOffers != null)
            {
                if (SelectedOffers.Count > 0)
                    showSearchResults = true;
                else
                {
                    showSearchResults = false;
                }
            }
            
        }

        private void DeleteOffer(Offer o)
        {
            offerService.Delete(o);
        }

        private void DeleteAll()
        {
            foreach (var o in offers)
                DeleteOffer(o);
        }

        public bool AngebotACollapsed { get; set; }
        public bool AngebotBCollapsed { get; set; }
        public bool AngebotCCollapsed { get; set; }

        void AngebotAToggle()
        {
            AngebotACollapsed = !AngebotACollapsed;
        }
        void AngebotBToggle()
        {
            AngebotBCollapsed = !AngebotBCollapsed;
        }
        void AngebotCToggle()
        {
            AngebotCCollapsed = !AngebotCCollapsed;
        }
    }
}
