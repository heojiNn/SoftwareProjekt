using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using XCV.Data;

namespace XCV.Pages.OfferNamespace
{
    public partial class OfferSearchAdd
        {

        // Edit or Create Offer?
        public bool toBeCreated; 

        [Parameter] 
        public int? Id { get; set; }
        private ChangeResult changeInfo = new();
        private Offer myOffer;

        //=====================================


        // Searchresult: a Dictionary sorted in EmployeeSearch.razor, key = PersoNumber, value = a tuple of the score and a list of all fulfilled properties.
        private Dictionary<string, Tuple<int, List<String>>> results = new();

        // Lists of all Employees, Roles etc.
        private List<Employee> employees;
        private List<Role> roles;
        private List<Skill> skills;
        private List<Field> fields;
        private List<Language> languages;

        // Lists of all selected Employees, Roles etc.
        private Employee selectedEmployee;
        private IList<Role> selectedRoles;
        private IList<Skill> selectedHardskills;
        private IList<Skill> selectedSoftskills;
        private IList<Field> selectedFields;
        private IList<Language> selectedLanguages;
        private IList<Role> advancedSelectedRoles;
        private IList<Skill> advancedSelectedHardskills;
        private IList<Skill> advancedSelectedSoftskills;
        private IList<Field> advancedSelectedFields;
        private IList<Language> advancedSelectedLanguages;

        private bool advancedSearch = false;
        private bool showSearch = false;

        // To assess whether any employee meets all criteria.
        private int maxScore = 0;
        private int numberOfSelectedAttributes = 0;

        // Criteria can be weighted.
        private int weight;

        // Show numberOfColumns Employees as default.
        private int numberOfColumns = 10;
        // Show numberOfAttributes fulfilled properties.
        private readonly int numberOfAttributes = 15;

        protected override void OnInitialized()
        {
            employees = profileService.ShowAllProfiles().ToList();
            roles = roleService.GetAllRoles().Where(x => x.RCL == 0).ToList();
            skills = skillService.GetAllSkills().ToList();
            fields = fieldService.GetAllFields().ToList();
            languages = languageService.GetAllLanguages().ToList();
            if (Id.HasValue)
            {
                try { myOffer = offerService.ShowOffer(Id.Value); }
                catch (Exception e) { Console.WriteLine(e.Message + e.StackTrace); }
                myOffer ??= new Offer();
                offerService.ChangeEventHandel += OnChangeReturn;
                toBeCreated = false;
            } else 
            {
                toBeCreated = true; 
            }
        }

        // Search functions for typeahead.
        private async Task<IEnumerable<Role>> SearchRoles(string searchText)
        {
            return await Task.FromResult(roles.Where(role => role.Name.ToLower().StartsWith(searchText.ToLower())).ToList());
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

        private async Task<IEnumerable<Language>> SearchLanguages(string searchText)
        {
            return await Task.FromResult(languages.Where(language => language.Name.ToLower().StartsWith(searchText.ToLower())).ToList());
        }

        private async Task<IEnumerable<Employee>> SearchName(string searchText)
        {

            return await Task.FromResult(employees.Where(
               (x => x.LastName.ToLower().StartsWith(searchText.ToLower()) ||
                string.Concat(x.FirstName.ToLower(), " ", x.LastName.ToLower()).ToString().StartsWith(searchText.ToLower())
               || x.FirstName.ToLower().StartsWith(searchText.ToLower()))

           ).ToList());
        }

        // Calculation of the searchresult.
        private void SearchEmployee()
        {
            if (showSearch) NewSearch();
            showSearch = !showSearch;

            //If a skill is searched for several times, it is only assigned to the weighted criteria.
            if (selectedHardskills != null)
            {
                numberOfSelectedAttributes += (advancedSelectedHardskills != null) ? selectedHardskills.Except(advancedSelectedHardskills).Count() : selectedHardskills.Count;
            }
            if (selectedSoftskills != null)
            {
                numberOfSelectedAttributes += (advancedSelectedSoftskills != null) ? selectedSoftskills.Except(advancedSelectedSoftskills).Count() : selectedSoftskills.Count;
            }
            if (selectedRoles != null)
            {
                numberOfSelectedAttributes += (advancedSelectedRoles != null) ? selectedRoles.Except(advancedSelectedRoles).Count() : selectedRoles.Count;
            }
            if (selectedFields != null)
            {
                numberOfSelectedAttributes += (advancedSelectedFields != null) ? selectedFields.Except(advancedSelectedFields).Count() : selectedFields.Count;
            }
            if (selectedLanguages != null)
            {
                numberOfSelectedAttributes += (advancedSelectedLanguages != null) ? selectedLanguages.Except(advancedSelectedLanguages).Count() : selectedLanguages.Count;
            }
            weight = (numberOfSelectedAttributes > 1) ? numberOfSelectedAttributes + 1 : 2;
            if (advancedSelectedHardskills != null) numberOfSelectedAttributes += advancedSelectedHardskills.Count * weight;
            if (advancedSelectedSoftskills != null) numberOfSelectedAttributes += advancedSelectedSoftskills.Count * weight;
            if (advancedSelectedRoles != null) numberOfSelectedAttributes += advancedSelectedRoles.Count * weight;
            if (advancedSelectedFields != null) numberOfSelectedAttributes += advancedSelectedFields.Count * weight;
            if (advancedSelectedLanguages != null) numberOfSelectedAttributes += advancedSelectedLanguages.Count * weight;

            results = ScoreEvaluator();
        }

        public Dictionary<string, Tuple<int, List<string>>> ScoreEvaluator()
        {
            // Dictonary with (key, value) represented by (currentEmployee, (score, list of fulfilled properties))
            Dictionary<string, Tuple<int, List<string>>> keyValuePairs = new();
            List<string> attributes = new();

            int score = 0;

            foreach (Employee employee in employees)
            {
                if (selectedEmployee != null && selectedEmployee.PersoNumber.Equals(employee.PersoNumber))
                {
                    score = numberOfSelectedAttributes + 1;
                    maxScore = score;
                    keyValuePairs.Clear();
                    attributes.Add("RCL: " + employee.RCL.ToString());
                    keyValuePairs.Add(employee.PersoNumber, new Tuple<int, List<string>>(score, attributes));

                    return keyValuePairs;
                }

                //If a role is searched for several times, it is only assigned to the weighted criteria.
                if (advancedSelectedRoles != null)
                {
                    foreach (Role role in advancedSelectedRoles)
                    {
                        if (employee.Roles.Contains(role))
                        {
                            score += weight;
                            attributes.Add(role.Name);
                        }
                    }
                    if (selectedRoles != null)
                    {
                        var realSelectedRoles = selectedRoles.Except(advancedSelectedRoles);
                        if (realSelectedRoles != null)
                        {
                            foreach (Role role in realSelectedRoles)
                            {
                                if (employee.Roles.Contains(role))
                                {
                                    score++;
                                    attributes.Add(role.Name);
                                }
                            }
                        }
                    }
                }
                else if (selectedRoles != null)
                {
                    foreach (Role role in selectedRoles)
                    {
                        if (employee.Roles.Contains(role))
                        {
                            score++;
                            attributes.Add(role.Name);
                        }
                    }
                }

                if (advancedSelectedHardskills != null)
                {
                    foreach (Skill skill in advancedSelectedHardskills)
                    {
                        if (employee.Abilities.Where(x => x.Type == SkillGroup.Hardskill).Contains(skill))
                        {
                            score += weight;
                            attributes.Add(skill.Name);
                        }
                    }
                    if (selectedHardskills != null)
                    {
                        var realSelectedHardskills = selectedHardskills.Except(advancedSelectedHardskills);
                        if (realSelectedHardskills != null)
                        {
                            foreach (Skill skill in realSelectedHardskills)
                            {
                                if (employee.Abilities.Where(x => x.Type == SkillGroup.Hardskill).Contains(skill))
                                {
                                    score++;
                                    attributes.Add(skill.Name);
                                }
                            }
                        }
                    }
                }
                else if (selectedHardskills != null)
                {
                    foreach (Skill skill in selectedHardskills)
                    {
                        if (employee.Abilities.Where(x => x.Type == SkillGroup.Hardskill).Contains(skill))
                        {
                            score++;
                            attributes.Add(skill.Name);
                        }
                    }
                }

                if (advancedSelectedFields != null)
                {
                    foreach (Field field in advancedSelectedFields)
                    {
                        if (employee.Fields.Contains(field))
                        {
                            score += weight;
                            attributes.Add(field.Name);
                        }
                    }
                    if (selectedFields != null)
                    {
                        var realSelectedFields = selectedFields.Except(advancedSelectedFields);
                        if (realSelectedFields != null)
                        {
                            foreach (Field field in realSelectedFields)
                            {
                                if (employee.Fields.Contains(field))
                                {
                                    score++;
                                    attributes.Add(field.Name);
                                }
                            }
                        }
                    }
                }
                else if (selectedFields != null)
                {
                    foreach (Field field in selectedFields)
                    {
                        if (employee.Fields.Contains(field))
                        {
                            score++;
                            attributes.Add(field.Name);
                        }
                    }
                }


                if (advancedSelectedLanguages != null)
                {
                    foreach (Language language in advancedSelectedLanguages)
                    {
                        if (employee.Languages.Contains(language))
                        {
                            score += weight;
                            attributes.Add(language.Name);
                        }
                    }
                    if (selectedLanguages != null)
                    {
                        var realSelectedLanguages = selectedLanguages.Except(advancedSelectedLanguages);
                        if (realSelectedLanguages != null)
                        {
                            foreach (Language language in realSelectedLanguages)
                            {
                                if (employee.Languages.Contains(language))
                                {
                                    score++;
                                    attributes.Add(language.Name);
                                }
                            }
                        }
                    }
                }
                else if (selectedLanguages != null)
                {
                    foreach (Language language in selectedLanguages)
                    {
                        if (employee.Languages.Contains(language))
                        {
                            score++;
                            attributes.Add(language.Name);
                        }
                    }
                }

                if (advancedSelectedSoftskills != null)
                {
                    foreach (Skill skill in advancedSelectedSoftskills)
                    {
                        if (employee.Abilities.Where(x => x.Type == SkillGroup.Softskill).Contains(skill))
                        {
                            score += weight;
                            attributes.Add(skill.Name);
                        }
                    }
                    if (selectedSoftskills != null)
                    {
                        var realSelectedSoftskills = selectedSoftskills.Except(advancedSelectedSoftskills);
                        if (realSelectedSoftskills != null)
                        {
                            foreach (Skill skill in realSelectedSoftskills)
                            {
                                if (employee.Abilities.Where(x => x.Type == SkillGroup.Softskill).Contains(skill))
                                {
                                    score++;
                                    attributes.Add(skill.Name);
                                }
                            }
                        }
                    }
                }
                else if (selectedSoftskills != null)
                {
                    foreach (Skill skill in selectedSoftskills)
                    {
                        if (employee.Abilities.Where(x => x.Type == SkillGroup.Softskill).Contains(skill))
                        {
                            score++;
                            attributes.Add(skill.Name);
                        }
                    }
                }

                if (score > maxScore) maxScore = score;

                if (score != 0)
                {
                    if (!keyValuePairs.ContainsKey(employee.PersoNumber))
                        keyValuePairs.Add(employee.PersoNumber, new Tuple<int, List<string>>(score, new List<string>(attributes)));
                    score = 0;
                }
                attributes.Clear();
            }

            return keyValuePairs;
        }

        private void HandleChange(ChangeEventArgs e)
        {
            numberOfColumns = Convert.ToInt32(e.Value);
        }

        private void NewSearch()
        {
            navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
        }

        void SearchToggle()
        {
            advancedSearch = !advancedSearch;
        }

        // Offer =============================================================================

        private void OnChangeReturn(object sender, ChangeResult e)
        {
            changeInfo = e;
        }
        
        private void AddEmp(Employee toAdd)
        {
            try
            {
                foreach (Employee e in offerData.offerStore.participants)
                {
                    if (e.PersoNumber.Equals(toAdd.PersoNumber))
                    {
                        Console.WriteLine($"Mitarbeiter - {toAdd.FirstName} wurde schon hinzugefügt. ");
                        return;
                    }
                }
                // Assign with a role to start with, change possible and intended
                toAdd.offerRole ??= toAdd.Roles.First().Name ?? roleService.GetAllRoles().First().Name;
                // On init: takes RCL of employee, on change: keeps RCL in offer
                toAdd.offerRCL = toAdd.offerRCL != 0 ? toAdd.offerRCL : ((0 <= toAdd.RCL && toAdd.RCL <= 8) ? toAdd.RCL : 1);
                // Get normal wages, via database, or the old value after a previous search, or 1 if both don't exist
                Role temp = roleService.GetAllRoles().Where(x => x.Name.Equals(toAdd.offerRole) && x.RCL == toAdd.offerRCL).FirstOrDefault();
                if (toAdd.offerWage == 0) //default
                    toAdd.offerWage = temp.Wage == 0 ? 1 : temp.Wage;
                // Default as per excel-example
                toAdd.hoursPerDay = 8;
                // Total runtime defaults to Project runtime
                toAdd.daysPerRun = (offerData.offerStore.End - offerData.offerStore.Start).Days;
                // No Discount to begin with
                toAdd.discount = 0;
                offerData.offerStore.participants.Add(toAdd);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Illegal Input - {toAdd.FirstName} konnte nicht ins Angebot hinzugefügt werden! " + e.Message);
            }
        }

        //====================================================================================
    }
}

