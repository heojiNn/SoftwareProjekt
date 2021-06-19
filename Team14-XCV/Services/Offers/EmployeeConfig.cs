using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team14.Data;

namespace Team14.Services.Offers
{
    public class EmployeeConfig
    {
        // Should be inherited by overlying "EmployeeDetailView"-employee.

        // More reference: OfferEmployeeConfigEdit.razor.cs

        public Employee configEmployee;

        private bool _default = true;

        public EmployeeConfig()
        {
            CreateDefaultConfig();
        }

        public ISet<Field> selectedFields { get; set; }
        public ISet<Skill> selectedSoftSkills { get; set; }
        public ISet<Skill> selectedHardSkills { get; set; }
        public ISet<Project> selectedProjects { get; set; }
        // TODO: Employee's activity within his projects
        public string[] selectedProjectroles { get; set; }

        public void CreateDefaultConfig()
        {
            if (_default)
            {
                selectedFields = configEmployee.Fields;
                selectedSoftSkills = (ISet<Skill>)configEmployee.Abilities.Where(s => s.Type == SkillGroup.Softskill);
                selectedHardSkills = (ISet<Skill>)configEmployee.Abilities.Where(s => s.Type == SkillGroup.Hardskill);
                selectedProjects = configEmployee.Projects;
                //selectedProjectroles = ...
            }
        }

        public void UpdateConfig()
        {
            // Interaction with page
            // when "confirm-button is pressed"
        }


        // Ability to reset Config.
        // Usecase: add a small amount of items quicker or reset in general.
        public void ClearConfig()
        {
            selectedFields.Clear();
            selectedSoftSkills.Clear();
            selectedHardSkills.Clear();
            selectedProjects.Clear();
            selectedProjectroles = null;
        }


        public void Discard()
        {

        }

        public void Reset()
        {

        }


        // etc.





    }

}
