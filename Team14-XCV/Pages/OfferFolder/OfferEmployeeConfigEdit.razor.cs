using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferEmployeeConfigEdit
    {
        //Routing
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        public string Config { get; set; }
        [Parameter]
        public string PersoNumber { get; set; }
        private ChangeResult changeInfo = new();
        private Offer offer;


        private Modal modal { get; set; }
        private string fieldSearch = "";
        private string hskillSearch = "";
        private string sskillSearch = "";

        //======================     Booleans     =========================//

        //==================================================//
        // Topic selections
        //==================================================//
        public bool showpersonalData { get; set; } = false;
        public bool showfieldData { get; set; } = false;
        public bool showSSkill { get; set; } = false;
        public bool showHSkill { get; set; } = false;
        public bool showProjects { get; set; } = false;

        //==================================================//
        // Detailed selections
        //==================================================//

        public bool showFirstName { get; set; } = false;
        public bool showLastName { get; set; } = false;
        public bool showDescription { get; set; } = false;
        public bool showImage { get; set; } = false;
        public bool showExperience { get; set; } = false;
        public bool showEmployedSince { get; set; } = false;

        //=====================    Content     ==========================//

        //==================================================//
        // Selected Data as in Database (checked)
        //==================================================//

        // Personal Data
        #nullable enable
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime? Experience { get; set; }
        public DateTime? EmployedSince { get; set; }


        // Qualifications
        public ISet<Field>? selectedFields { get; set; }
        public ISet<Skill>? selectedSoftSkills { get; set; }
        public ISet<Skill>? selectedHardSkills { get; set; }
        public IList<(int project, string activity)>? selectedProjects { get; set; }

        #nullable disable

        //==================================================//
        // All possible Data to show for the employee (checked and unchecked)
        //==================================================//

        public Employee offerEmployee { get; set; }

        //==================================================//
        //==================================================//
        //==================================================//

        protected override void OnInitialized()
        {
            //load offer
            offer = offerService.ShowOffer(Id);
            offerService.ChangeEventHandel += OnChangeReturn;
            //load profile
            offerEmployee = profileService.ShowProfile(PersoNumber); // Persistence of the employee independent from a config -> used to show all possible features to include in a config
            profileService.ChangeEventHandel += OnChangeReturn;
            //load config
            EmployeeConfig ecfg = configService.GetDocumentConfig(offer, Config).employeeConfigs.Where(x => x.PersNr.Equals(PersoNumber)).First(); // Persistence of the employeeconfig in db
            configService.ChangeEventHandel += OnChangeReturn;

            FirstName = offerEmployee.FirstName;
            LastName = offerEmployee.LastName;
            Description = offerEmployee.Description;
            Image = offerEmployee.Image;
            Experience = offerEmployee.Experience;
            EmployedSince = offerEmployee.EmployedSince;
            selectedFields = offerEmployee.Fields;
            selectedSoftSkills = offerEmployee.Abilities.Where(x => x.Type == SkillGroup.Softskill).ToHashSet();
            selectedHardSkills = offerEmployee.Abilities.Where(x => x.Type == SkillGroup.Hardskill).ToHashSet();
            selectedProjects = offerEmployee.Projects;

            if (ecfg == null)
            {
                Console.WriteLine("Error!");
            } else
            {
                try
                {
                    if (ecfg.FirstName != null) showFirstName = true;
                    if (ecfg.LastName != null)  showLastName = true;
                    if (ecfg.Description != null) showDescription = true;
                    if (ecfg.Image != null)  showImage = true;
                    if (ecfg.Experience.HasValue) showExperience = true;
                    if (ecfg.EmployedSince.HasValue)  showEmployedSince = true;
                    if (showFirstName || showLastName || showDescription || showImage || showExperience || showEmployedSince) //If unchecked all will be removed and vice verca
                        showpersonalData = true;
                    if (ecfg.selectedFields != null && ecfg.selectedFields.Count != 0) showfieldData = true;
                    else Console.WriteLine("Felder nicht vorhanden");
                    if (ecfg.selectedSoftSkills != null && ecfg.selectedSoftSkills.Count != 0) showSSkill = true;
                    else Console.WriteLine("Softskills nicht vorhanden");
                    if (ecfg.selectedHardSkills != null && ecfg.selectedHardSkills.Count != 0) showHSkill = true;
                    else Console.WriteLine("Hardskills nicht vorhanden");
                    if (ecfg.selectedProjects != null && ecfg.selectedProjects.Count != 0) showProjects = true;
                    else Console.WriteLine("Projekte nicht vorhanden");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Creation failed" + e.Message);

                }
            }
        }

        // Update the config
        private void Validate()
        {
            modal.Open();
        }
        private void Close()
        {
            modal.Close();
            changeInfo = new();
        }

        /// <summary>
        /// Nullifies deselected Data when unchecked, signaling the ConfigService to delete the corresponding entry in the database.
        /// </summary>
        private void UpdateConfig()
        {
            EmployeeConfig toUpdate = new EmployeeConfig();
            if (!showpersonalData)
            {
                toUpdate.FirstName = null;
                toUpdate.LastName = null;
                toUpdate.Description = null;
                toUpdate.Image = null;
                toUpdate.Experience = null;
                toUpdate.EmployedSince = null;
            }
            toUpdate.FirstName = showFirstName == true ? FirstName : null;
            toUpdate.LastName = showLastName == true ? LastName : null;
            toUpdate.Description = showDescription == true ? Description : null;
            toUpdate.Image = showImage == true ? Image : null;
            toUpdate.Experience = showExperience == true ? Experience : null;
            toUpdate.EmployedSince = showEmployedSince == true ? EmployedSince : null;
            if (!showfieldData)
            {
                toUpdate.selectedFields = null;
            } else 
                toUpdate.selectedFields = selectedFields == null ? null : selectedFields;
            if (!showSSkill)
            {
                toUpdate.selectedSoftSkills = null;
            } else
                toUpdate.selectedSoftSkills = selectedSoftSkills == null ? null : selectedSoftSkills;

            if (!showHSkill)
            {
                toUpdate.selectedHardSkills = null;
            }  else
                toUpdate.selectedHardSkills = selectedHardSkills == null ? null : selectedHardSkills;

            if (!showProjects)
            {
                toUpdate.selectedProjects = null;
            } else
                toUpdate.selectedProjects = selectedProjects == null ? null : selectedProjects;
            configService.UpdateEmployeeConfig(offer, configService.GetDocumentConfig(offer, Config), offerEmployee.PersoNumber, toUpdate);
            Close();
        }

        private void Discard()
        {

        }
        //


        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
        }

        private void OnChangeReturnEvent(object sender, ChangeResult e) => changeInfo = e;

        public bool MitarbeiterCollapsed { get; set; }
        void MitarbeiterToggle()
        {
            MitarbeiterCollapsed = !MitarbeiterCollapsed;
        }
    }
}
