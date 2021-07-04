using System.Collections.Generic;
using System.Linq;

namespace XCV.Data
{
    public class EmployeeConfig
    {
        // Should be inherited by overlying "EmployeeDetailView"-employee.

        // More reference: OfferEmployeeConfigEdit.razor.cs

        public Employee configEmployee;

        private bool _default = true;

        public EmployeeConfig(Employee e)
        {
            CreateDefaultConfig(e);
        }

        public ISet<Field> selectedFields { get; set; }
        public ISet<Skill> selectedSoftSkills { get; set; }
        public ISet<Skill> selectedHardSkills { get; set; }
        public ISet<Project> selectedProjects { get; set; }
        // TODO: Employee's activity within his projects
        public string[] selectedProjectroles { get; set; }

        public void CreateDefaultConfig(Employee e)
        {
            if (_default)
            {
                selectedFields = e.Fields;
                selectedSoftSkills = (ISet<Skill>)e.Abilities.Where(s => s.Type == SkillGroup.Softskill);
                selectedHardSkills = (ISet<Skill>)e.Abilities.Where(s => s.Type == SkillGroup.Hardskill);

                ///   by marrrio
                //----------------by mario
                ////            projectservice Get(  e.Projects.ProNumber  )  use se servi to get the whole project
                selectedProjects = new HashSet<Project>() { new Project() };
                //----------------------------------------------------


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
