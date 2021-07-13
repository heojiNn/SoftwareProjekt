using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;
using XCV.Shared.Misc;

namespace XCV.Pages.ProjectFolder
{
    public partial class ProjectEdit
    {
        [Parameter]
        public string Id { get; set; }

        private ChangeResult changeInfo = new();
        private Modal activityModal { get; set; }
        private Modal validationModal { get; set; }

        private Project updateProject;

        private List<string> purposes = new();
        private List<string> activities = new();
        private Dictionary<string, (List<Employee> employees, List<Skill> requirements)> activityInfo = new();

        private IList<Employee> SelectedEmployees = new List<Employee>();
        private IList<Skill> SelectedHardskills = new List<Skill>();
        private IList<Skill> SelectedSoftskills = new List<Skill>();

        private IEnumerable<Employee> allEmployees;
        private IEnumerable<Field> allFields;
        private IEnumerable<Skill> allSkills;


        private string withoutActivity = "ohne Tätigkeit";
        private string emptyField = "keine Branche ausgewählt";
        private string selectedActivity = "";

        private bool BeschreibungCollapsed { get; set; }
        private bool ZweckCollapsed { get; set; }
        private bool MitarbeiterCollapsed { get; set; }
        private bool TaetigkeitCollapsed { get; set; }

        protected override void OnInitialized()
        {
            updateProject = projectService.ShowProject(int.Parse(Id));
            updateProject ??= new Project();
            allEmployees = profileService.ShowAllProfiles();
            allFields = fieldService.GetAllFields();
            allSkills = skillService.GetAllSkills();
            purposes = updateProject.Purpose;
            foreach(var kvp in updateProject.Activities)
            {
                if (!kvp.Key.Equals(withoutActivity))
                    activities.Add(kvp.Key);
                activityInfo.Add(kvp.Key, (new List<Employee>(), kvp.Value.requirements.ToList()));
                foreach (var emp in kvp.Value.persNr)
                {
                    activityInfo[kvp.Key].employees.Add(allEmployees.First(x => x.PersoNumber == emp));
                }
            }
            projectService.ChangeEventHandel += OnChangeReturn;

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

        private void Validate()
        {

            if (purposes != null) updateProject.Purpose = purposes.Distinct().Where(x => x != "").ToList();
            if (updateProject.Field != null && updateProject.Field.Equals(emptyField)) updateProject.Field = null;

            updateProject.Activities = new();
            foreach (var kvp in activityInfo.Where(x => activities.Contains(x.Key) || x.Key.Equals(withoutActivity)))
            {
                if (!updateProject.Activities.ContainsKey(kvp.Key)) updateProject.Activities.Add(kvp.Key, ((kvp.Value.employees.Select(x => x.PersoNumber).ToList(), kvp.Value.requirements.ToList())));
            }
            if (!updateProject.Activities.ContainsKey(withoutActivity)) updateProject.Activities.Add(withoutActivity, (activityInfo[withoutActivity].employees.Select(x => x.PersoNumber).ToList(), activityInfo[withoutActivity].requirements));

            projectService.ValidateUpdate(updateProject);



            if (changeInfo.ErrorMessages.Any() || changeInfo.InfoMessages.Any() )
            {
                validationModal.Open();
            }

        }

        private void UpdateProject()
        {
            if (!changeInfo.ErrorMessages.Any())
            {
                projectService.Update(updateProject);
                validationModal.Close();
                navManager.NavigateTo("/project/" + updateProject.Id);


            }
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
