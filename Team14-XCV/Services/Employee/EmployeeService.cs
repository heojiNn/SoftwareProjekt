using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    /// <inheritdoc/>
    public class EmployeeService : IAccountService, IProfileService
    {
        private readonly IWebHostEnvironment env;
        private readonly string connectionString;
        private readonly ILogger<EmployeeService> log;
        private readonly ISkillService _skillService;

        public EmployeeService(IConfiguration config, ILogger<EmployeeService> logger, ISkillService skillService, IWebHostEnvironment environment)
        {
            connectionString = config.GetConnectionString("MS_SQL_Connection");
            log = logger;
            env = environment;          // for image directiory
            _skillService = skillService;
        }


        //-------------------------------------Business Logic--------------------------------------
        //-----------------------------------------------------------------------------------------
        // for definition see   IProfileService
        public Employee ShowProfile(string persNum)
        {
            var employee = ShowAllProfiles().FirstOrDefault(x => x.PersoNumber == persNum);
            if (employee == null)
                log.LogWarning($"Request with non existing: Emplyee-Key:{persNum}");   //Warning  cause good Frontends won't do that
            return employee;
        }

        // for definition see   IProfileService
        public IEnumerable<Employee> ShowAllProfiles()
        {
            var all = GetAllEmployees().OrderBy(x => x.PersoNumber);
            foreach (var employee in all)
                _skillService.HangThemOnATree(employee.Abilities);
            return all;
        }


        // for definition see   IProfileService
        public IEnumerable<Employee> SearchProfiles(string firstName, string lastName)
        {
            var all = ShowAllProfiles();
            bool predicateF(Employee x) => x.FirstName.ToLower().StartsWith(firstName.ToLower());
            bool predicateL(Employee x) => x.LastName.ToLower().StartsWith(lastName.ToLower());
            if (firstName.Equals(""))                                       // first name empty, only search for last name
                all = all.Where(predicateL);
            else if (lastName.Equals(""))                                   // last name empty,  only search for first name
                all = all.Where(predicateF);
            else
                all = all.Where(x => predicateF(x) && predicateL(x));

            if (!all.Any())
                OnEmptyResult(new() { Message = $"Keine Ergebnis für \"{firstName}\" \"{lastName}\"." });
            return all;
        }
        //-------------------------------------------------------------------------------------------------------------


        // for definition see   IProfileService
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
                infoMessages.Add("Dein Rate Card Level würde geändert werden.");
            if (oldVersion.Expirience != newVersion.Expirience)
                infoMessages.Add("Dein Berufserfahrung würde geändert werden.");


            if (!oldVersion.Languages.SetEquals(newVersion.Languages)) inlineWords.Add("Sprachen");
            else
            {
                var langSetOld = oldVersion.Languages.ToHashSet(new LangComparerWithLevel());
                var langSetNew = newVersion.Languages.ToHashSet(new LangComparerWithLevel());
                if (!langSetOld.SetEquals(langSetNew)) inlineWords.Add("Sprach-Level");
            }

            if (!oldVersion.Abilities.SetEquals(newVersion.Abilities)) inlineWords.Add("Skills");
            else
            {
                static bool predicate1(Skill x) => x.Type == SkillGroup.Hardskill;
                var hardSetOld = oldVersion.Abilities.Where(predicate1).ToHashSet(new SkillComparerWithLevel());
                var hardSetNew = newVersion.Abilities.Where(predicate1).ToHashSet(new SkillComparerWithLevel());
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
            // adds the Model-Validation to the List of Errors
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (newVersion.Roles.Where(x => x.Name == "Consultant").Any() && newVersion.RCL < 4)
                errorMessages.Add("Consultant wird erst ab RCL:4 freigeschaltet");
            if (newVersion.Languages.Where(x => x.Level == "").Any())
                errorMessages.Add("Mindesetens ein Sprache hat keine Level Angabe.");
            if (newVersion.Abilities.Where(x => !_skillService.GetAllLevel().Contains(x.Level) && x.Type == SkillGroup.Hardskill).Any())
                errorMessages.Add("Mindesetens ein Skill hat keine Level Angabe.");
            //-------------------------------------------------------------------------------------

            OnChange(new()
            {
                InfoMessages = infoMessages,
                ErrorMessages = errorMessages
            });
        }

        // for definition see   IProfileService
        public void UpdateProfile(Employee newVersion)
        {
            ValidateUpdate(newVersion);
            bool isNew = ShowProfile(newVersion.PersoNumber) == null;

            if (errorMessages.Any() || isNew)
                return;

            if (!infoMessages.Any())
            {                                                       // means saving only possible if infoMessages.Any()
                OnChange(new() { SuccesMessage = $"Keine Profil-Änderungen zu speichern." });
                return;
            }
            try
            {
                UpdateEmployee(newVersion);
                OnChange(new() { SuccesMessage = $" {newVersion.FirstName}, deine Daten wurden gespeichert." });
            }
            catch (Exception ex)
            {                                                    // it is risky to show the user unknown, possible sensitive.infos
                OnChange(new() { ErrorMessages = new[] { $"Es trat ein Fehler in der Persistenz auf\n {ex.Message}" } });
                log.LogError($"UpdateProfile() persitence Error: \n{ex.Message}\n");
            }


        }

        // for definition see   IProfileService
        public async Task UploadeImage(string persoNumber, IBrowserFile image)
        {
            // TODO Validation
            //  for size/ContentType ...
            //
            var nameInWWW = $"empPic{persoNumber}.{image.ContentType.Split('/')[1]}";
            var path = Path.Combine(env.WebRootPath, nameInWWW);

            try
            {
                await using FileStream fs = new(path, FileMode.Create);
                await image.OpenReadStream().CopyToAsync(fs);

                var profile = ShowProfile(persoNumber);
                profile.Image = nameInWWW;
                UpdateEmployee(profile);
            }
            catch (Exception ex)
            {                                                    // it is risky to show the user unknown, possible sensitive.infos
                OnChange(new() { ErrorMessages = new[] { $"Es trat ein Fehler in der Persistenz auf\n {ex.Message}" } });
                log.LogError($"UploadeImage() persitence Error: \n{ex.Message}\n");
            }

        }





        // for definition see   IAccountService
        public void CreateAccount(Employee newAccount)
        {
            errorMessages = new();
            infoMessages = new();

            // adds the Entity-DataAnnotations to the list of Errors
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newAccount, new ValidationContext(newAccount), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (ShowAllProfiles().Any(x => x.PersoNumber == newAccount.PersoNumber))
                errorMessages.Add("Sie können keinen Account überschreiben.");

            if (errorMessages.Any())                            // return ------Fail
            {
                OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
                return;
            }

            if (newAccount.PersoNumber != newAccount.Password)
                infoMessages.Add("Passwort und Personal-Nummer sollten gleich sein, dies wurde korrigiert.");
            newAccount.Password = newAccount.PersoNumber;

            try
            {
                InsertEmployee(newAccount);
                var message = $"Ein neuer Account wurde erstellt:{newAccount.PersoNumber}.";    // ------Succes
                OnChange(new() { SuccesMessage = message, InfoMessages = infoMessages });
            }
            catch (Exception ex)                                      // return ------Fail
            {                                   // it is risky to show the user unknown, possible sensitive info...
                OnChange(new() { ErrorMessages = new[] { $"Es trat ein Fehler in der Persistenz auf \n{ex.Message}" } });
                log.LogError($"CreateAccount() persitence Error: \n{ex.Message}\n");
            }
        }

        // for definition see   IAccountService
        public void DeleteAccount(string IdToDelete)
        {
            try
            {
                DeleteEmployee(IdToDelete);
                OnChange(new() { SuccesMessage = $"Account:{IdToDelete} wurde gelöscht." });    // ------Succes
            }
            catch (Exception ex)
            {                                   // it is risky to show the user unknown, possible sensitive info...
                OnChange(new() { ErrorMessages = new[] { $"Es trat ein Fehler in der Persistenz auf\n {ex.Message}" } });
                log.LogError($"DeleteAccount() persitence Error: \n{ex.Message}\n");
            }
        }
        //-------------------------------------------------------------------------------------------------------------




        //-------------------------------------------------------------------------------------------------------------
        //-------------------------------------Persistence-----------------------------------------
        //--read   --------------------------------------------------------------------------------
        /// <summary>
        ///         Returns all Employees, workes with SQL Server
        /// </summary>
        private IEnumerable<Employee> GetAllEmployees()
        {
            IEnumerable<Employee> employees = new List<Employee>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                var query = @"SELECT  e.*,  a.EnumerationInt,   f.Field,  r.Role as Name, r.Role_Rcl as Rcl, rw.Wage,   l.Language, l.Language_Level,  ac.Project, ac.Activity
                            FROM Employee e
                            LEFT JOIN EmployeeHasAcrole a   ON e.PersoNumber = a.Employee
                            LEFT JOIN EmployeeHasField  f   ON e.PersoNumber = f.Employee
                            LEFT JOIN EmployeeHasRole   r   ON e.PersoNumber = r.Employee
                            LEFT JOIN Role rw    ON r.Role = rw.Name AND r.Role_Rcl = rw.Rcl
                            LEFT JOIN EmployeeHasLanguage l   ON e.PersoNumber = l.Employee
                            LEFT JOIN ActivityHasEmployee ac   ON e.PersoNumber = ac.Employee";

                var multiEmployees = con.Query<Employee, AccessRole?, string, Role, (string language, string language_level), (int project, string activity), Employee>(query,
                    (employee, acRole, field, role, language, pros) =>
                    {
                        if (acRole.HasValue)                                // AcRoles
                            employee.AcRoles.Add(acRole.Value);
                        if (field != null)                                  // Fields
                            employee.Fields.Add(new Field() { Name = field });
                        if (role != null)                                   // Roles
                            employee.Roles.Add(role);
                        if (language.language != null)                      // Languages
                            employee.Languages.Add(new Language() { Name = language.language, Level = language.language_level });
                        if (pros.activity != null)                          //  Projects
                            employee.Projects.Add(pros);
                        return employee;
                    }, splitOn: "EnumerationInt, Field, Name, Language, Project");

                employees = multiEmployees.GroupBy(e => e.PersoNumber).Select(g =>
                    {
                        var groupedFirst = g.First();
                        if (groupedFirst.AcRoles.Any())
                            groupedFirst.AcRoles = g.Select(e => e.AcRoles.First()).ToHashSet();
                        if (groupedFirst.Fields.Any())
                            groupedFirst.Fields = g.Select(e => e.Fields.First()).ToHashSet();
                        if (groupedFirst.Roles.Any())
                            groupedFirst.Roles = g.Select(e => e.Roles.First()).ToHashSet();
                        if (groupedFirst.Languages.Any())
                            groupedFirst.Languages = g.Select(e => e.Languages.First()).ToHashSet();
                        if (groupedFirst.Abilities.Any())
                            groupedFirst.Abilities = g.Select(e => e.Abilities.First()).ToHashSet();
                        if (groupedFirst.Projects.Any())
                            groupedFirst.Projects = g.Select(e => e.Projects.First()).Distinct().ToList();
                        return groupedFirst;
                    }).ToList();    //--------------------------------ToDebuk

                foreach (var employee in employees)// an extra query,  otherwise the result above could get quite huge(50*10*2*3*30*50)
                {
                    var skills = con.Query<Skill, string, Skill>(@$"SELECT  s.Skill as Name,  sl.Name as Level,  s.Skill_Cat as Category
                                                                        From [EmployeeHasSkill] s  Left Join [Skill_Level] sl   ON s.Skill_Level = sl.Level
                                                                    Where [Employee] = '{employee.PersoNumber}'",
                                                                    (skill, category) =>
                                                                    {
                                                                        if (skill != null)
                                                                            skill.Category = new SkillCategory() { Name = category };
                                                                        return skill;
                                                                    }, splitOn: "Category").ToHashSet();
                    employee.Abilities = skills;
                }
            }
            finally
            {
                con.Close();
            }
            return employees;
        }
        //---write --------------------------------------------------------------------------------
        /// <summary>
        ///         Workes with SQL Server
        /// </summary>
        private void InsertEmployee(Employee e)
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute(@"Insert Into [Employee] Values (@PersoNumber, @Password, @FirstName, @LastName,
                                                         @Description, @Image, null, null, @EmployyedSince, 0)", e);
                foreach (int acR in e.AcRoles)
                    con.Execute("Insert Into [EmployeeHasAcrole] Values (@EnumInt, @E)", new { EnumInt = acR, E = e.PersoNumber });
            }
            finally
            {
                con.Close();
            }
        }
        /// <summary>
        ///         Workes with SQL Server
        /// </summary>
        ///
        /// <remarks>
        ///         Updates [Employee] except(Password, EmployyedSince)
        ///         and [EmployeeHasField] [EmployeeHasRole] [EmployeeHasLanguage] [EmployeeHasSkill]  through DELETE all and INSERT
        /// </remarks>
        private void UpdateEmployee(Employee e)
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute(@"Update [Employee] Set [FirstName]=@FirstName, [LastName]=@LastName, [Description]=@Description, [Image]=@Image, [Rcl]=@Rcl,
                                                    [Expirience]=@Expirience, [MadeFirstChangesOnProfile]=1   Where [PersoNumber]=@PersoNumber ", e);

                con.Execute($"Delete From [EmployeeHasField] Where employee='{e.PersoNumber}'");
                foreach (var field in e.Fields)
                    con.Execute("Insert Into [EmployeeHasField] Values (@Field, @E)", new { Field = field.Name, E = e.PersoNumber });

                con.Execute($"Delete From [EmployeeHasRole] Where employee='{e.PersoNumber}'");
                foreach (var role in e.Roles)
                {
                    int rcl = (e.RCL > 7 && role.Name != "Consultant") ? 7 : e.RCL;
                    con.Execute("Insert Into [EmployeeHasRole] Values (@Role, @Rcl, @E)", new { Role = role.Name, rcl, E = e.PersoNumber });
                }
                con.Execute($"Delete From [EmployeeHasLanguage] Where employee='{e.PersoNumber}'");
                foreach (var lang in e.Languages)
                    con.Execute("Insert Into [EmployeeHasLanguage] Values (@Language, @Level, @E)", new { Language = lang.Name, lang.Level, E = e.PersoNumber });

                con.Execute($"Delete From [EmployeeHasSkill] Where employee='{e.PersoNumber}'");
                foreach (var skill in e.Abilities)
                {
                    int? level = Array.FindIndex(_skillService.GetAllLevel(), x => x == skill.Level) + 1;
                    if (skill.Type == SkillGroup.Softskill)
                        level = null;
                    con.Execute("Insert Into [EmployeeHasSkill] Values (@Skill, @Cat, @Level, @E)", new { Skill = skill.Name, Cat = skill.Category.Name, level, E = e.PersoNumber });
                }
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        ///         Workes with SQL Server
        /// </summary>
        private void DeleteEmployee(string id)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute("Delete From employee  Where PersoNumber = @PersoNumber", new { PersoNumber = id });
            con.Close();
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------




        //-------------------------------------User Messaging--------------------------------------
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



    //----------------------------------Helper Classes-----------------------------------------
    //-----------------------------------------------------------------------------------------

    /// <summary>
    ///         to create Sets in which the level is consinderd too
    /// </summary>
    class LangComparerWithLevel : IEqualityComparer<Language>
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

    /// <summary>
    ///         to create Sets in which the level is consinderd too
    /// </summary>
    class SkillComparerWithLevel : IEqualityComparer<Skill>
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
}
