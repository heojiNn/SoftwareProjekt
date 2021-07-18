using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlazorDownloadFile;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferPage
    {
        // Parameters and Storage

        [Inject] public IBlazorDownloadFileService BlazorDownloadFileService { get; set; }

        [Parameter]
        public int Id { get; set; }
        private ChangeResult changeInfo = new();
        private Offer myOffer;

        public IEnumerable<Skill> softskills;
        public IEnumerable<Skill> hardskills;

        public IList<DocumentConfig> configs = new List<DocumentConfig>();
        public IList<DocumentConfig> selectedConfig { get; set; } = new List<DocumentConfig>();   //Only contains one at a time

        public string error = "";

        public bool changedSelected = false;

        private void toggleSelected()
        {
            changedSelected = !changedSelected;
        }

        public DocumentConfig editingConfig;
        public string configName { get; set; } = ""; // Name for newly created config;

        // Initialization

        protected override void OnInitialized()
        {
            myOffer = offerService.ShowOffer(Id);
            myOffer ??= new Offer();
            offerService.ChangeEventHandel += OnChangeReturn;

            DocumentConfig sel = configService.GetSelectedConfig(myOffer);
            if (sel != null)
            {
                selectedConfig = new List<DocumentConfig>();
                selectedConfig.Add(sel);
            }

            configs = configService.GetAllDocumentConfigs(myOffer).ToList();


            if (myOffer.Requirements.Where(s => s.Type == SkillGroup.Softskill) != null)
                softskills = myOffer.Requirements.Where(s => s.Type == SkillGroup.Softskill).ToList();
            else
                softskills = new List<Skill>();
            if (myOffer.Requirements.Where(s => s.Type == SkillGroup.Hardskill) != null)
                hardskills = myOffer.Requirements.Where(s => s.Type == SkillGroup.Hardskill).ToList();
            else
                hardskills = new List<Skill>();
        }

        // Modals

        private Modal modal { get; set; }
        private Modal modal2 { get; set; }
        private Modal modal3 { get; set; }

        private void NotifyDel(DocumentConfig cfg)
        {
            editingConfig = cfg;
            modal.Open();
        }

        private void NotifySel()
        {
            modal2.Open();
        }

        private void Validate(Offer o, DocumentConfig newVersion)
        {
            if (newVersion.Name.Length == 0) //Name too short
            {
                error = "Bitte geben Sie einen Namen ein der mindestens ein Zeichen enthält.";
                modal3.Open();
            }
            else if (configService.GetAllDocumentConfigs(o).Where(x => x.Name == newVersion.Name).Any()) //Name is duplicate
            {
                error = "Der Name der Dokumentenkonfiguration muss einzigartig sein, bitte keine Duplikate.";
                modal3.Open();
            }
            else if (o.participants.Count == 0) //No Employees
            {
                error = "Das zugrundeliegende Angebot besitzt keine Mitarbeiter, die für eine Konfiguration in Frage kämen.";
                modal3.Open();
            }
            else //No Errors, proceed
            {
                CreateDefault(o, newVersion.Name);
            }
        }
        private void Close() { modal.Close(); modal2.Close(); modal3.Close(); changeInfo = new(); }


        // Configs


        private DocumentConfig GetSelected()
        {
            return selectedConfig.First();
        }

        public void CreateDefault(Offer o, string name)
        {
            configs.Add(configService.CreateDefaultDocumentConfig(offerService.ShowOffer(o.Id), name));
        }

        private void DeleteConfig(Offer o, DocumentConfig cfg)
        {
            configService.DeleteDocumentConfig(o, cfg);
        }

        private void DeleteOffer(Offer o)
        {
            offerService.Delete(o);
        }

        public void CheckboxClicked(object marked, DocumentConfig selected)
        {
            if ((bool)marked)
            {
                selectedConfig.Add(selected);
                configService.SaveSelectedConfig(myOffer, selected);
            }
            else
            {
                selectedConfig.Remove(selected);
                configService.DeleteSelectedConfig(myOffer, selected);
            }
        }

        // ChangeEvents

        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
        }

        private void OnChangeReturnEvent(object sender, ChangeResult e) => changeInfo = e;

        //Booleans


        public bool DescriptionCollapsed { get; set; }
        public bool FieldsCollapsed { get; set; }
        public bool MitarbeiterCollapsed { get; set; }
        public bool ConfigCollapsed { get; set; } = true;

        void BeschreibungToggle()
        {
            DescriptionCollapsed = !DescriptionCollapsed;
        }

        void BrancheToggle()
        {
            FieldsCollapsed = !FieldsCollapsed;
        }
        void MitarbeiterToggle()
        {
            MitarbeiterCollapsed = !MitarbeiterCollapsed;
        }

        void KonfigurationsToggle()
        {
            ConfigCollapsed = !ConfigCollapsed;
        }

    }
}
