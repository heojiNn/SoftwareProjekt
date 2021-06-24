using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace XCV.Data
{
    public class EmployeeService : IAccountService, IProfileService
    {
        private readonly string connectionString;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<EmployeeService> log;
        private readonly ISkillService skillser;

        public EmployeeService(IWebHostEnvironment environment, IConfiguration config, ILogger<EmployeeService> logger, ISkillService skillService)
        {
            env = environment;
            log = logger;
            skillser = skillService;
            connectionString = config.GetConnectionString("MyLocalConnection");
        }

        //-----------------------------------------------------------------------------------------
        //---------------------------------Buissines Logic-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //---------------------------------IProfileService-----------------------------------------
        public Employee ShowProfile(string persNum)
        {
            if (persNum == null)
                return new Employee();
            var employee = ShowAllProfiles().FirstOrDefault(x => x.PersoNumber.Equals(persNum));
            if (employee == null)
                log.LogWarning($"Request with non Existing: Emplyee-Key:{persNum}");   //LogWarning good Frontends won't do that
            return employee;
        }
        public IEnumerable<Employee> SearchProfiles(string firstName, string lastName)
        {
            var all = ShowAllProfiles();
            bool predicateF(Employee x) => x.FirstName.ToLower().StartsWith(firstName.ToLower());
            bool predicateL(Employee x) => x.LastName.ToLower().StartsWith(lastName.ToLower());
            if (firstName.Equals(""))                                       // first name empty only search for last name
                all = all.Where(predicateL);
            else if (lastName.Equals(""))                                   // last name empty only search for first name
                all = all.Where(predicateF);
            else
                all = all.Where(x => predicateF(x) && predicateL(x));

            if (!all.Any())
                OnEmptyResult(new() { Message = $"Keine Ergebnis für \"{firstName}\" \"{lastName}\"." });
            return all;
        }
        //-----------------------------------------------------------------------------------------


        //-----------------------------------------------------------------------------------------
        // Checkes against buissines Constrains  and updates Employye-profile
        public void ValidateUpdate(Employee newVersion)
        {
            errorMessages = new();
            infoMessages = new();
            inlineWords = new();

            Employee oldVersion = ShowProfile(newVersion.PersoNumber);
            if (newVersion == null || oldVersion == null)
                return;

            //-------------------------------------------------------------------------------------infoMessages
            if (!oldVersion.FirstName.Equals(newVersion.FirstName) || !oldVersion.LastName.Equals(newVersion.LastName))
                infoMessages.Add("Dein Vor- und/oder Nachname würde geändert werden.");
            if (!oldVersion.Description.Equals(newVersion.Description))
                infoMessages.Add("Deine Beschreibung würde geändert werden.");
            if (oldVersion.RCL != newVersion.RCL)
                infoMessages.Add("Dein Rate-Card-Level würde geändert werden.");
            if (oldVersion.Expirience != newVersion.Expirience)
                infoMessages.Add("Dein Berufserfahrung würde geändert werden.");
            if (!oldVersion.Languages.SetEquals(newVersion.Languages)) inlineWords.Add("Sprachen");
            else
            {
                var langSetOld = oldVersion.Languages.ToHashSet(new SecondLangComparer());
                var langSetNew = newVersion.Languages.ToHashSet(new SecondLangComparer());
                if (!langSetOld.SetEquals(langSetNew)) inlineWords.Add("Sprach-Level");
            }
            if (!oldVersion.Abilities.SetEquals(newVersion.Abilities)) inlineWords.Add("Skills");
            else
            {
                static bool predicate1(Skill x) => x.Type == SkillGroup.Hardskill;
                var hardSetOld = oldVersion.Abilities.Where(predicate1).ToHashSet(new SecondSkillComparer());
                var hardSetNew = newVersion.Abilities.Where(predicate1).ToHashSet(new SecondSkillComparer());
                if (!hardSetOld.SetEquals(hardSetNew)) inlineWords.Add("Skill-Level");
            }
            static bool predicate2(Skill x) => x.Type == SkillGroup.Softskill;
            var softSetOld = oldVersion.Abilities.Where(predicate2).ToHashSet();
            var softSetNew = newVersion.Abilities.Where(predicate2).ToHashSet();
            if (!softSetOld.SetEquals(softSetNew)) inlineWords.Add("Soft-Skills");
            if (!oldVersion.Fields.SetEquals(newVersion.Fields)) inlineWords.Add("Tätigkeitfelder");
            if (!oldVersion.Roles.SetEquals(newVersion.Roles)) inlineWords.Add("Rollen");
            if (inlineWords.Any())
                infoMessages.Add($"Deine {String.Join(", ", inlineWords)} würden geändert werden.");

            //-------------------------------------------------------------------------------------errorMessages
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (newVersion.Languages.Where(x => x.Level == "").Any())
                errorMessages.Add("Mindesetens ein Sprache hat keine Level Angabe.");
            if (newVersion.Abilities.Where(x => !skillser.GetAllLevel().Contains(x.Level) && x.Type == SkillGroup.Hardskill).Any())
                errorMessages.Add("Mindesetens ein Skill hat keine Level Angabe.");
            //-------------------------------------------------------------------------------------
            OnChange(new()
            {
                InfoMessages = infoMessages,
                ErrorMessages = errorMessages
            });
        }
        //------------------------------------------------------------------------------------------
        public void Update(Employee e)
        {
            ValidateUpdate(e);
            bool isNew = e.PersoNumber != ShowProfile(e.PersoNumber)?.PersoNumber;

            if (errorMessages.Any() || isNew)
                return;
            if (!infoMessages.Any())
            {
                OnChange(new() { SuccesMessage = $"Keine Profil-Änderungen zu speichern." });
                return;                                               //means saving only possible if infoMessages.Any()
            }

            try
            {
                UpdatePersistence(e);
                OnChange(new() { SuccesMessage = $" {e.FirstName},  deine Daten wurden gespeichert." });
            }
            catch (Exception ex)
            {
                OnChange(new() { ErrorMessages = new[] { $"Es trat ein Fehler in der Persistenz auf {ex.Message}" } });
                log.LogError($" persitence Error {ex.Message}");
            }
        }
        public async Task Uploade(Employee toGetNum, IBrowserFile browserFile)
        {
            // TODO Validation
            //  for size/ContentType ...
            //
            var nameInWWW = $"empPic{toGetNum.PersoNumber}.{browserFile.ContentType.Split('/')[1]}";
            var path = Path.Combine(env.WebRootPath, nameInWWW);
            await using FileStream fs = new(path, FileMode.Create);
            await browserFile.OpenReadStream().CopyToAsync(fs);

            toGetNum.Image = nameInWWW;
            UpdatePersistence(toGetNum);
        }





        //----IAccountService implementation ------------------------------------------------------
        public void CreateAccount(Employee e)
        {
            errorMessages = new();
            infoMessages = new();

            if (ShowAllProfiles().Any(x => x.PersoNumber.Equals(e.PersoNumber)))
                errorMessages.Add("Sie könne keinen Account überschreiben.");
            if (e.FirstName.Equals("") || e.LastName.Equals("") || e.PersoNumber.Equals(""))
                errorMessages.Add("Geben sie Vor-, Nachnamen und PersNr ein.");

            if (errorMessages.Any())
            {
                OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages }); //Fail
                return;
            }
            if (e.PersoNumber != e.Password)
            {
                infoMessages.Add("Passwort und Personal-Number sollten gleich sein, dies wurde korregiert.");
                e.Password = e.PersoNumber;
            }
            CreatePersistence(e);
            OnChange(new() { SuccesMessage = $"Ein neuer Account wurde erstellt:{e.PersoNumber}.", InfoMessages = infoMessages });
        }
        //-----------------------------------------------------------------------------------------

        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Employee> ShowAllProfiles()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var employees = con.Query<Employee>("Select * From employee");
            con.Close();
            foreach (var emp in employees)
            {
                var acRoles = con.Query<AccessRole>($"Select AcRole From employee_accessrole Where Employee = '{emp.PersoNumber}'");
                foreach (var aRole in acRoles)
                    emp.AcRoles.Add(aRole);
                var fields = con.Query<Field>($"Select field as Name From employee_field Where Employee = '{emp.PersoNumber}'");
                foreach (var field in fields)
                    emp.Fields.Add(field);
                var roles = con.Query<(string role, float wage)>("Select role.Name, role.Wage From employee_role Join role On " +
                                                                "employee_role.role=role.name  And  employee_role.role_rcl=role.rcl " +
                                                                $" Where employee = '{emp.PersoNumber}'");
                foreach (var role in roles)
                    emp.Roles.Add(new Role() { Name = role.role, Wage = role.wage });
                var languages = con.Query<Language>("Select language as Name, language_level as Level From employee_language " +
                                                                $" Where employee = '{emp.PersoNumber}'");
                foreach (var language in languages)
                    emp.Languages.Add(language);
                var skills = con.Query<(string Name, string Lvl, string Cat)>("Select employee_skill.skill_name as Name, skill_level.Name as Lvl, skill_cat as Cat " +
                                                                    "From employee_skill  Join skill_level On  employee_skill.skill_level=skill_level.level " +
                                                                    $"Where employee = '{emp.PersoNumber}'");
                foreach (var (Name, Lvl, Cat) in skills)
                {
                    emp.Abilities.Add(new Skill() { Name = Name, Level = Lvl, Category = new SkillCategory() { Name = Cat } });
                    skillser.HangThemOnATree(emp.Abilities);
                }
            }
            return employees;
        }

        //---write --------------------------------------------------------------------------------
        private void CreatePersistence(Employee e)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            string rcl = e.RCL == null ? "null" : e.RCL.ToString();
            con.Execute($"Insert Into employee Values ('{e.PersoNumber}', '{e.Password}', '{e.FirstName}', '{e.LastName}', " +
                                                         $" '{e.Description}', '{e.Image}', {rcl}, 0, '{e.WorkingSince:yyyy-MM-dd}', 0)");
            foreach (var acR in e.AcRoles)
                con.Execute($"Insert Into employee_accessrole Values ('{e.PersoNumber}', {(int)acR})");
            con.Close();
        }
        private void UpdatePersistence(Employee e)
        {
            var allLvl = skillser.GetAllLevel();
            using var con = new SqlConnection(connectionString);
            con.Open();
            string rcl = e.RCL == null ? "null" : e.RCL.ToString();
            con.Execute($"Update employee Set  FirstName='{e.FirstName}', LastName='{e.LastName}', Description='{e.Description}', " +
                                                         $" Image='{e.Image}', RCL={rcl}, Expirience={e.Expirience},  MadeFirstChangesOnProfile=1  Where PersoNumber='{e.PersoNumber}' ");

            con.Execute($"Delete From employee_field Where employee='{e.PersoNumber}'");
            foreach (var field in e.Fields)
                con.Execute($"Insert Into employee_field Values ('{e.PersoNumber}', '{field.Name}')");

            con.Execute($"Delete From employee_role Where employee='{e.PersoNumber}'");
            if (rcl == "null")
                rcl = "0";
            foreach (var role in e.Roles)
                con.Execute($"Insert Into employee_role Values ('{e.PersoNumber}', '{role.Name}', '{rcl}')");

            con.Execute($"Delete From employee_language Where employee='{e.PersoNumber}'");
            foreach (var lang in e.Languages)
                con.Execute($"Insert Into employee_language Values ('{e.PersoNumber}', '{lang.Name}', '{lang.Level}')");

            con.Execute($"Delete From employee_skill Where employee='{e.PersoNumber}'");
            foreach (var skill in e.Abilities)
            {
                var lvlIndex = Array.FindIndex(allLvl, x => x == skill.Level);
                con.Execute($"Insert Into employee_skill Values ('{e.PersoNumber}', '{skill.Name}', '{skill.Category.Name}', {lvlIndex + 1})");
            }
            con.Close();
        }
        public void Delete(Employee e)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From employee  Where PersoNumber='{e.PersoNumber}' ");
            con.Close();
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------




        // Upper Layer  Messaging Handling
        //-----------------------------------------------------------------------------------------
        public event EventHandler<NoResult> SearchEventHandel;
        protected virtual void OnEmptyResult(NoResult e) => SearchEventHandel?.Invoke(this, e);
        //-----------------------------------------------------------------------------------------
        private List<string> errorMessages = new();
        private List<string> infoMessages = new();
        private List<string> inlineWords = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e)
        {
            if (e.InfoMessages.Any() || e.ErrorMessages.Any() || e.SuccesMessage != "")
                ChangeEventHandel?.Invoke(this, e);
        }
    }






    class SecondSkillComparer : IEqualityComparer<Skill>
    {
        public bool Equals(Skill s1, Skill s2)
        {
            if (s1 == null && s2 == null)
                return true;
            else if (s1 == null || s2 == null)
                return false;
            return s1.Category.Name == s2.Category.Name && s1.Name == s2.Name && s1.Level == s2.Level;
        }
        public int GetHashCode(Skill s) => HashCode.Combine(s.Category.Name, s.Name, s.Level);
    }
    class SecondLangComparer : IEqualityComparer<Language>
    {
        public bool Equals(Language l1, Language l2)
        {
            if (l1 == null && l2 == null)
                return true;
            else if (l1 == null || l2 == null)
                return false;
            return l1.Name == l2.Name && l1.Level == l2.Level;
        }
        public int GetHashCode(Language l) => HashCode.Combine(l.Name, l.Level);
    }

}
