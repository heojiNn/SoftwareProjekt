using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;


namespace Team14.Data
{
    public class EmployeeService : IAccountService, IProfileService
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<EmployeeService> log;

        public EmployeeService(IWebHostEnvironment environment, ILogger<EmployeeService> logger)
        {
            env = environment;
            log = logger;
        }


        //  IProfileService  implementation
        //-----------------------------------------------------------------------------------------
        public Employee ShowProfile(string persNum)
        {
            if (persNum == null || persNum.Equals(""))
                return new Employee();
            var employee = ShowAllProfiles().Where(x => x.PersoNumber.Equals(persNum)).FirstOrDefault();
            if (employee == null)
                log.LogWarning($"Request with non Existing: Emplyee-Key:{persNum}");   //   Frontend did somthing stupid
            return employee;
        }
        public IEnumerable<Employee> SearchProfiles(string firstName, string lastName)
        {
            var all = ShowAllProfiles();
            bool predicateF(Employee x) => x.FirstName.ToLower().StartsWith(firstName.ToLower());
            bool predicateL(Employee x) => x.LastName.ToLower().StartsWith(lastName.ToLower());
            if (firstName.Equals(""))                                              // first name empty  only search for last name
                all = all.Where(predicateL);
            else if (lastName.Equals(""))                                          // last name empty  only search for first name
                all = all.Where(predicateF);
            else
                all = all.Where(x => predicateF(x) && predicateL(x));

            if (!all.Any())
                OnEmptyResult(new() { Message = $"Keine Ergebnis für \"{firstName}\" \"{lastName}\"" });  // Empty Serarch
            return all;
        }
        //-----------------------------------------------------------------------------------------



        //------------------------------------------------------------------------------------------
        // Checkes updated Employye-profile   against buissines Constrains
        public void CheckProfileUpdate(Employee newVersion)
        {
            changeMessages = new();
            changeMini = new();
            errorMessage = "";

            Employee oldVersion = ShowProfile(newVersion.PersoNumber);
            if (newVersion == null || oldVersion == null)
                return;

            if (!oldVersion.FirstName.Equals(newVersion.FirstName) || !oldVersion.LastName.Equals(newVersion.LastName))
                changeMessages.Add("Dein Name würden geändert werden");
            if (!oldVersion.RCL.Equals(newVersion.RCL))
                changeMessages.Add("Dein RCL würden geändert werden");

            var hardSetOld = oldVersion.Abilities.Where(x => x.Type == SkillGroup.Hardskill).ToHashSet();
            var softSetOld = oldVersion.Abilities.Where(x => x.Type == SkillGroup.Softskill).ToHashSet();
            var hardSetNew = newVersion.Abilities.Where(x => x.Type == SkillGroup.Hardskill).ToHashSet();
            var softSetNew = newVersion.Abilities.Where(x => x.Type == SkillGroup.Softskill).ToHashSet();


            if (!softSetOld.SetEquals(softSetNew))
                changeMini.Add("SoftSkils ");
            if (!hardSetOld.SetEquals(hardSetNew))
                changeMini.Add("HardSkills ");

            if (!oldVersion.Roles.SetEquals(newVersion.Roles))
                changeMini.Add("Rollen ");
            if (!oldVersion.Fields.SetEquals(newVersion.Fields))
                changeMini.Add("Tätigeitfelder ");
            if (!oldVersion.Languages.SetEquals(newVersion.Languages))
                changeMini.Add("Sprachen ");

            if (changeMini.Any())
                changeMessages.Add($"Deine {String.Join(", ", changeMini)} würden geändert werden");
            if (newVersion.Abilities.Where(x => x.Category == "Frameworks").Any() && !newVersion.Roles.Where(x => x.Name == "Software Developer").Any())
                errorMessage = "nur Devoloper können sich Framworks hinzufüfgen";

            OnChange(new() { InfoMessages = changeMessages, ErrorMessage = errorMessage });
        }
        //-----------------------------------------------------------------------------------------
        public void CommitProfileUpdate(Employee toCommit)
        {
            errorMessage = "";
            bool isNew = toCommit.PersoNumber != ShowProfile(toCommit.PersoNumber).PersoNumber;
            CheckProfileUpdate(toCommit);

            if (!errorMessage.Equals("") || isNew)
            {
                log.LogWarning($" The upper Layer tried to  update invalid Profile {errorMessage}");  // Frontend did something stupid
                return;
            }
            try
            {
                CreateOnPersistence(toCommit);              //the persistence can work with exceptions but the UI wont care
            }
            catch (Exception e)
            {
                OnChange(new() { ErrorMessage = $"Es trat ein Fehler in der Persistenz auf {e.Message}" });
                log.LogError($" Persistenz Error {e.Message}");            // shouldnt happen   we might got a Problem
            }

            OnChange(new() { SuccesMessage = $" \"{toCommit.FirstName}\"    deine Daten wurden gespeichert" });
            log.LogInformation($" \tEmply:{toCommit.PersoNumber}  updated himself");
        }






        //----IAccountService implementation ------------------------------------------------------
        public void CreateAccount(Employee newAccount)
        {
            if (ShowAllProfiles().Any(x => x.PersoNumber.Equals(newAccount.PersoNumber)))
            {
                OnChange(new() { ErrorMessage = "Sie könne keine Account überschreiben." });
                return;
            }
            if (newAccount.FirstName.Equals("") || newAccount.LastName.Equals("") || newAccount.PersoNumber.Equals(""))
            {
                OnChange(new() { ErrorMessage = "Geben sie Vor UND Nachnamen  UND PersNr ein." });
                return;
            }
            changeMessages = new();
            if (newAccount.PersoNumber != newAccount.Password)
            {
                changeMessages.Add("Passwort und Perso Nummer sollten gleich sein   wurde vom System koregiert");
                newAccount.Password = newAccount.PersoNumber;
            }

            CreateOnPersistence(newAccount);
            OnChange(new() { SuccesMessage = $"Ein neuer Account wurde erstellt {newAccount.PersoNumber}", InfoMessages = changeMessages });
        }
        //-----------------------------------------------------------------------------------------






        //-----------------------------------------------------------------------------------------
        // public void AddProject(string persNum, Project project)
        // {
        // }
        //public void AddProject(string persNum, Project project, string projectActivities)
        // {  ihen fehlen dir Vorasezunge für diese aufgabe
        // }
        //-----------------------------------------------------------------------------------------













        // Persestierung (json)
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        readonly string subPath = Path.Combine(new string[] { "jsonPersistierung", "Employees" });
        readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, };
        //-------------------------------------------------------------------------------------------------------------

        //  Read Form Persitence
        public IEnumerable<Employee> ShowAllProfiles()
        {
            IEnumerable<IFileInfo> files = new List<IFileInfo>();
            try
            {
                files = env.ContentRootFileProvider.GetDirectoryContents(subPath);
                if (!files.Any())
                    log.LogError($"Not a sigle  Employees file");
                files = files.Where(x => x.Name.StartsWith("employee")).OrderBy(x => x.Name);
            }
            catch (IOException e) { log.LogError($"IO File Exception: \t{e.Message}"); }
            catch (Exception e) { log.LogError($"UNEXPECTED File Exception {e.Message}"); }

            return files.Select((x) => Deserialize(File.ReadAllText(x.PhysicalPath)));
        }


        //  Write To Persitence
        private void CreateOnPersistence(Employee toBeCreated)
        {
            UpdateToPersistence(toBeCreated);
        }

        private void UpdateToPersistence(Employee toBeUpdated)
        {
            toBeUpdated.Abilities.ToList().ForEach(x => x.PossibleLevels = Array.Empty<string>());
            toBeUpdated.Languages.ToList().ForEach(x => x.PossibleLevels = Array.Empty<string>());


            var json = Serialize(toBeUpdated);
            var path = Path.Combine(env.ContentRootPath, subPath, $"employee{toBeUpdated.PersoNumber}.json");
            File.WriteAllText(path, json, Encoding.UTF8);

            log.LogInformation($"Emplyee wurde persitiert  {path}");
        }
        //-------------------------------------------------------------------------------------------------------------
        private Employee Deserialize(string json) => JsonSerializer.Deserialize<Employee>(json, options);
        private string Serialize(Employee employee) => JsonSerializer.Serialize(employee, options);
        //-------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------










        // User Messegig Handling
        //-----------------------------------------------------------------------------------------
        public event EventHandler<NoResult> SearchEventHandel;
        protected virtual void OnEmptyResult(NoResult e) => SearchEventHandel?.Invoke(this, e);


        //-----------------------------------------------------------------------------------------
        private string errorMessage = "";
        private List<string> changeMessages = new();
        private List<string> changeMini = new();

        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }

}
