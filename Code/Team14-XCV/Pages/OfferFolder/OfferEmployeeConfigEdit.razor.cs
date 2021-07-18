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
        private Modal modal2 { get; set; }
        private string fieldSearch = "";
        private string hskillSearch = "";
        private string sskillSearch = "";
        private string error = "";

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
        public ISet<(int project, string activity)>? selectedProjects { get; set; }

        #nullable disable

        public int[] order { get; set; }

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
            //load all profile data
            offerEmployee = profileService.ShowProfile(PersoNumber); // Persistence of the employee independent from a config -> used to show all possible features to include in a config
            profileService.ChangeEventHandel += OnChangeReturn;

            FirstName = offerEmployee.FirstName;
            LastName = offerEmployee.LastName;
            Description = offerEmployee.Description;
            Image = offerEmployee.Image;
            Experience = offerEmployee.Experience;
            EmployedSince = offerEmployee.EmployedSince;

            //load config
            EmployeeConfig ecfg = configService.GetDocumentConfig(offer, Config).employeeConfigs.Where(x => x.PersNr.Equals(PersoNumber)).Single(); // Persistence of the employeeconfig in db
            configService.ChangeEventHandel += OnChangeReturn;

            selectedFields = ecfg.selectedFields;
            selectedSoftSkills = ecfg.selectedSoftSkills;
            selectedHardSkills = ecfg.selectedHardSkills;
            selectedProjects = ecfg.selectedProjects;
            order = ecfg.order;

            try//If unchecked all will be removed and vice verca
            {
                if (ecfg.FirstName != null) showFirstName = true;

                if (ecfg.LastName != null)  showLastName = true;

                if (ecfg.Description != null && !ecfg.Description.Equals("")) showDescription = true;

                if (ecfg.Image != null)  showImage = true;

                if (ecfg.Experience.HasValue) showExperience = true;

                if (ecfg.EmployedSince.HasValue)  showEmployedSince = true;

                if (showFirstName || showLastName || showDescription || showImage || showExperience || showEmployedSince) showpersonalData = true;

                if (ecfg.selectedFields != null && ecfg.selectedFields.Count != 0) showfieldData = true;

                if (ecfg.selectedSoftSkills != null && ecfg.selectedSoftSkills.Count != 0) showSSkill = true;

                if (ecfg.selectedHardSkills != null && ecfg.selectedHardSkills.Count != 0) showHSkill = true;

                if (ecfg.selectedProjects != null && ecfg.selectedProjects.Count != 0) showProjects = true;

            }
            catch (Exception e)
            {
                Console.WriteLine("Creation failed" + e.Message);

            }
        }

        private void Validate() {
            if (order.Length != order.Distinct().Count())
            {
                error = "Die Angabe der Reihenfolge enthält Duplikate";
                modal2.Open();
            } else
            {
                modal.Open();
            }
        }
        private void Close()
        {
            modal.Close();
            modal2.Close();
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
            } else
            {
                toUpdate.FirstName = showFirstName == true ? FirstName : null;
                toUpdate.LastName = showLastName == true ? LastName : null;
                toUpdate.Description = showDescription == true ? Description : null;
                toUpdate.Image = showImage == true ? Image : null;
                toUpdate.Experience = showExperience == true ? Experience : null;
                toUpdate.EmployedSince = showEmployedSince == true ? EmployedSince : null;
            }
            if (!showfieldData)
            {
                toUpdate.selectedFields = null;
            } else 
                toUpdate.selectedFields = selectedFields ?? null;
            if (!showSSkill)
            {
                toUpdate.selectedSoftSkills = null;
            } else
                toUpdate.selectedSoftSkills = selectedSoftSkills ?? null;

            if (!showHSkill)
            {
                toUpdate.selectedHardSkills = null;
            }  else
                toUpdate.selectedHardSkills = selectedHardSkills ?? null;

            if (!showProjects)
            {
                toUpdate.selectedProjects = null;
            } else
                toUpdate.selectedProjects = selectedProjects ?? null;

            toUpdate.order = order;
            configService.UpdateEmployeeConfig(offer, configService.GetDocumentConfig(offer, Config), offerEmployee.PersoNumber, toUpdate);
            Close();
        }


        // 4 states: Project checked/unchecked, Projectactivity checked/unchecked

        /// <summary>
        /// Adds or removes all projects with all activities
        /// </summary>
        /// <param name="marked"></param>
        /// <param name="pro"></param>
        /// <param name="act"></param>
        public void ProjectsClicked(object marked, int pro, string[] act)
        {
            if ((bool)marked)
            {
                foreach (string activity in act)
                {
                    selectedProjects.Add((pro, activity));
                }
            } else
            {
                foreach (string activity in act)
                {
                    selectedProjects.Remove((pro, activity));
                }
            }
        }

        public void ProjectActivitiesClicked(object marked, int pro, string[] act)
        {
            if ((bool)marked)
            {
                if (!selectedProjects.Contains((pro, act[0])))
                    selectedProjects.Add((pro, act[0]));
            } else
            {
                if (selectedProjects.Contains((pro, act[0])))
                    selectedProjects.Remove((pro, act[0]));
                if (!selectedProjects.Contains((pro, ""))) //i.e. one project with two acts: checked project, unchecked both activites -> project only has "" activities -> display only project.
                    selectedProjects.Add((pro, ""));
            } 
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
    }
}
