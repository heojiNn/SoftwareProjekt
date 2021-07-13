using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;
using XCV.Shared.Misc;


namespace XCV.Pages.ProjectFolder
{
    public partial class ProjectCreate
    {
        private ChangeResult changeInfo = new();
        private Modal activityModal { get; set; }
        private Modal validationModal { get; set; }


        private Project newProject = new();

        private List<string> purposes { get; set; } = new();
        private List<string> activities { get; set; } = new();
        private Dictionary<string, (List<Employee> employees, List<Skill> requirements)> activityInfo = new();

        private IList<Employee> SelectedEmployees = new List<Employee>();
        private IList<Skill> SelectedHardskills = new List<Skill>();
        private IList<Skill> SelectedSoftskills = new List<Skill>();

        private IEnumerable<Employee> allEmployees;
        private IEnumerable<Field> allFields;
        private IEnumerable<Skill> allSkills;

        private string emptyField = "Keine Branche ausgewählt";
        private string withoutActivity = "ohne Tätigkeit";
        private string selectedActivity = "";

        public bool BeschreibungCollapsed { get; set; }
        public bool ZweckCollapsed { get; set; }
        public bool MitarbeiterCollapsed { get; set; }
        public bool TaetigkeitCollapsed { get; set; }
        protected override void OnInitialized()
        {
            purposes.Add("");
            activities.Add("");
            allFields = fieldService.GetAllFields();
            allEmployees = profilService.ShowAllProfiles();
            allSkills = skillService.GetAllSkills();
            projectService.ChangeEventHandel += OnChangeReturn;
            activityInfo.Add(withoutActivity, (new List<Employee>(), new List<Skill>()));
        }
        private void NewPurpose() => purposes.Add("");
        private void RemovePurpose(int i) => purposes.RemoveAt(i);

        private void NewActivity() => activities.Add("");
        private void RemoveActivity(int index)
        {
            string activity = activities[index];
            activities.RemoveAll(name => name.Equals(activity));
            if (activityInfo.ContainsKey(activity)) activityInfo.Remove(activity);

        }

        private void CreateProject()
        {
            if (purposes != null) newProject.Purpose = purposes.Distinct().Where(x => x != "").ToList();
            if (newProject.Field != null && newProject.Field.Equals(emptyField)) newProject.Field = null;
            foreach(var act in activities.Distinct().Where(x => x != ""))
            {
                newProject.Activities.Add(act, (activityInfo[act].employees.Select(x => x.PersoNumber).ToList(), activityInfo[act].requirements));
            }
            if (!newProject.Activities.ContainsKey(withoutActivity)) newProject.Activities.Add(withoutActivity, (activityInfo[withoutActivity].employees.Select(x => x.PersoNumber).ToList(), activityInfo[withoutActivity].requirements));
            projectService.Create(newProject);
            if (changeInfo.ErrorMessages.Any())
            {
                validationModal.Open();
                newProject.Activities = new();
            }
            else
            {
                int id = projectService.ShowAllProjects().First(x => x.Title == newProject.Title).Id;
                navManager.NavigateTo("/project/" + id);
            }
        }

        private void Change(string act)
        {
            selectedActivity = act;
            if (activityInfo.ContainsKey(act))
            {
                SelectedHardskills = activityInfo[act].requirements.Where(x => x.Type == SkillGroup.Hardskill).ToList();
                SelectedSoftskills = activityInfo[act].requirements.Where(x => x.Type == SkillGroup.Softskill).ToList();
                SelectedEmployees = activityInfo[act].employees.ToList();

            }
            activityModal.Open();
        }
        private void Close()
        {
            SelectedEmployees = new List<Employee>();
            SelectedHardskills = new List<Skill>();
            SelectedSoftskills = new List<Skill>();
            activityModal.Close();
        }
        private void ValidationClose()
        {
            validationModal.Close();
            changeInfo = new();
        }
        private void Save()
        {
            if (!activityInfo.ContainsKey(selectedActivity)) activityInfo.Add(selectedActivity, (new List<Employee>(SelectedEmployees), new List<Skill>(SelectedHardskills.Concat(SelectedSoftskills))));
            else
            {
                activityInfo.Remove(selectedActivity);
                activityInfo.Add(selectedActivity, (new List<Employee>(SelectedEmployees), new List<Skill>(SelectedHardskills.Concat(SelectedSoftskills))));
            }
            Close();
        }

        private async Task<IEnumerable<Employee>> SearchEmployees(string searchText)
        {

            return await Task.FromResult(allEmployees.Where(
                (x => x.LastName.ToLower().StartsWith(searchText.ToLower()) ||
                string.Concat(x.FirstName.ToLower(), " ", x.LastName.ToLower()).ToString().StartsWith(searchText.ToLower())
               || x.FirstName.ToLower().StartsWith(searchText.ToLower()))).ToList());
        }
        private async Task<IEnumerable<Skill>> SearchHardskills(string searchText)
        {

            return await Task.FromResult(allSkills.Where(
                (x => x.Type == SkillGroup.Hardskill && x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
        }

        private async Task<IEnumerable<Skill>> SearchSoftskills(string searchText)
        {

            return await Task.FromResult(allSkills.Where(
                (x => x.Type == SkillGroup.Softskill && x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
        }

        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
        }


        void BeschreibungToggle()
        {
            BeschreibungCollapsed = !BeschreibungCollapsed;
        }
        void ZweckToggle()
        {
            ZweckCollapsed = !ZweckCollapsed;
        }
        void MitarbeiterToggle()
        {
            MitarbeiterCollapsed = !MitarbeiterCollapsed;
        }
        void TaetigkeitToggle()
        {
            TaetigkeitCollapsed = !TaetigkeitCollapsed;

        }
    }
}
