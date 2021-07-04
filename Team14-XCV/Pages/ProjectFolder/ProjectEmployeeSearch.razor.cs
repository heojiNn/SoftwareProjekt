using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;

namespace XCV.Pages.ProjectNamespace
{
    public partial class ProjectEmployeeSearch
    {


            public string simpleAttributes;
            public int simpleCounter = 0;
            public Boolean showSearch = false;
            private List<Employee> employees;
            private List<Role> roles;
            private List<Skill> skills;
            private List<Field> fields;
            private List<Language> languages;

            private Employee SelectedEmployee;
            private IList<Role> SelectedRoles;
            private IList<Skill> SelectedHardskills;
            private IList<Skill> SelectedSoftskills;
            private IList<Field> SelectedFields;
            private IList<Language> SelectedLanguages;

            public int[] RealKeys;
            public Employee[] RealValues;
            public SortedDictionary<int, Employee> resultList { get; set; }
            protected override void OnInitialized()
            {
                employees = profileService.ShowAllProfiles().ToList();
                roles = roleService.GetAllRoles().ToList();
                skills = skillService.GetAllSkills().ToList();
                fields = fieldService.GetAllFields().ToList();
                languages = languageService.GetAllLanguages().ToList();
            }

            private async Task<IEnumerable<Role>> SearchRoles(string searchText)
            {

                return await Task.FromResult(roles.Where(
                    (x => x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
            }
            private async Task<IEnumerable<Skill>> SearchHardskills(string searchText)
            {

                return await Task.FromResult(skills.Where(
                    (x => x.Type == SkillGroup.Hardskill && x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
            }

            private async Task<IEnumerable<Skill>> SearchSoftskills(string searchText)
            {

                return await Task.FromResult(skills.Where(
                    (x => x.Type == SkillGroup.Softskill && x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
            }
            private async Task<IEnumerable<Field>> SearchFields(string searchText)
            {

                return await Task.FromResult(fields.Where(
                    (x => x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
            }
            private async Task<IEnumerable<Language>> SearchLanguages(string searchText)
            {

                return await Task.FromResult(languages.Where(
                    (x => x.Name.ToLower().Contains(searchText.ToLower()))).ToList());
            }


            private async Task<IEnumerable<Employee>> SearchName(string searchText)
            {

                return await Task.FromResult(employees.Where(
                   (x => x.LastName.ToLower().Contains(searchText.ToLower()) || x.FirstName.ToLower().Contains(searchText.ToLower()))

               ).ToList());
            }




            public void SearchEmployee()
            {

                showSearch = !showSearch;
                Employee test = new Employee { };
                if (SelectedEmployee != null)
                {
                    test.FirstName = SelectedEmployee.FirstName;
                    test.LastName = SelectedEmployee.LastName;
                }
                if (SelectedHardskills != null) foreach (Skill hard in SelectedHardskills) { test.Abilities.Add(hard); }
                if (SelectedSoftskills != null) foreach (Skill soft in SelectedSoftskills) { test.Abilities.Add(soft); }
                if (SelectedRoles != null) foreach (Role role in SelectedRoles) { test.Roles.Add(role); }
                if (SelectedFields != null) foreach (Field field in SelectedFields) { test.Fields.Add(field); }
                if (SelectedLanguages != null) foreach (Language language in SelectedLanguages) { test.Languages.Add(language); }
                SortedDictionary<int, Employee> sortedList = ScoreEvaluator(test);
                resultList = sortedList;
            }
            public SortedDictionary<int, Employee> ScoreEvaluator(Employee model)
            {

                // Sorted List with (key, value) represented by (currentEmployee, score)
                SortedDictionary<int, Employee> sortedList = new SortedDictionary<int, Employee>();

                //score
                System.Int32 score = 0;


                foreach (Employee emp in employees)
                {
                    Console.WriteLine("vor namenscheck" + score);

                    if (model.FirstName.Equals(emp.FirstName) && model.LastName.Equals(emp.LastName))
                    {
                        score = Int32.MaxValue;
                        sortedList.Clear();
                        sortedList.Add(1, emp);
                        RealKeys = new int[sortedList.Count()];
                        RealKeys[0] = 100;
                        RealValues = new Employee[sortedList.Count()];
                        RealValues[0] = emp;



                        return sortedList;
                    }


                    ISet<string> allAttributes = roles.Select((x) => x.Name).ToHashSet();
                    foreach (String s in allAttributes)
                    {
                        if (emp.Roles.Select(x => x.Name).Contains(s)) ++score;
                    }

                    allAttributes = fields.Select((x) => x.Name).ToHashSet();

                    foreach (String s in allAttributes)
                    {
                        if (emp.Fields.Select(x => x.Name).Contains(s)) ++score;
                    }
                    allAttributes = languages.Select((x) => x.Name).ToHashSet();

                    foreach (String s in allAttributes)
                    {
                        if (emp.Languages.Select(x => x.Name).Contains(s)) ++score;
                    }
                    allAttributes = skills.Select((x) => x.Name).ToHashSet();
                    foreach (String s in allAttributes)
                    {
                        if (emp.Abilities.Select(x => x.Name).Contains(s)) ++score;
                    }

                    //Hier kann noch nach Berufserfahrung und RCL sortiert werden.

                    //score += model.experience - emp.experience;

                    //score += model.rcl - emp.rcl;

                    // Spezialfall: Score dieses Mitarbeiters ist schon vergeben.
                    // Durchlaufe solange die keys und inkrementiere score um 1  bis erster "freier" key vorkommt.


                    //Hier ist noch ein Fehler in der Suche, durch die Funktion kann ein Mitarbeiter ohne Treffer als am besten geeigneter Ausgegeben werden.

                    while (sortedList.ContainsKey(score))
                    {
                        score++;
                    }

                    sortedList.Add(score, emp);
                    score = 0;
                }

                // Save the keys and values to be able to iterate backwards through them: Possibility do display score descending or ascending.
                SortedDictionary<int, Employee>.KeyCollection keys = sortedList.Keys;
                SortedDictionary<int, Employee>.ValueCollection values = sortedList.Values;

                RealKeys = new int[sortedList.Count()];
                RealValues = new Employee[sortedList.Count()];

                keys.CopyTo(RealKeys, 0);
                values.CopyTo(RealValues, 0);



                return sortedList;
            }

            public void simpleCount()
            {
                simpleCounter++;
            }

            public void simpleCountClear()
            {
                simpleCounter = 0;
            }

            public void simpleAttributesAppend(string attribut)
            {
                simpleAttributes += attribut + " ";
            }
            public void simpleAttributeClear()
            {
                simpleAttributes = "";
            }
        }


    



}
