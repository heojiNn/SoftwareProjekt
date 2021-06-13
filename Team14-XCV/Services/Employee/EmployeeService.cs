using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
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


        //---------------------------------IProfileService-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //---------------------------------Buissines Logic-----------------------------------------
        //-----------------------------------------------------------------------------------------
        public Employee ShowProfile(string persNum)
        {
            if (persNum == null || persNum.Equals(""))
                return new Employee();
            var employee = ShowAllProfiles().Where(x => x.PersoNumber.Equals(persNum)).FirstOrDefault();
            if (employee == null)
                log.LogWarning($"Request with non Existing: Emplyee-Key:{persNum}");   // correct Frontend  shouldm't do that
            return employee;
        }
        public IEnumerable<Employee> SearchProfiles(string firstName, string lastName)
        {
            var all = ShowAllProfiles();
            bool predicateF(Employee x) => x.FirstName.ToLower().StartsWith(firstName.ToLower());
            bool predicateL(Employee x) => x.LastName.ToLower().StartsWith(lastName.ToLower());
            if (firstName.Equals(""))                                              // first name empty only search for last name
                all = all.Where(predicateL);
            else if (lastName.Equals(""))                                          // last name empty  only search for first name
                all = all.Where(predicateF);
            else
                all = all.Where(x => predicateF(x) && predicateL(x));

            if (!all.Any())
                OnEmptyResult(new() { Message = $"Keine Ergebnis für \"{firstName}\" \"{lastName}\"." });  // Empty Serarch
            return all;
        }
        //-----------------------------------------------------------------------------------------


        //------------------------------------------------------------------------------------------
        // Checkes against buissines Constrains  and updates Employye-profile
        public void ValidateUpdate(Employee newVersion)
        {
            errorMessages = new();
            infoMessages = new();
            internalMessages = new();

            Employee oldVersion = ShowProfile(newVersion.PersoNumber);
            if (newVersion == null || oldVersion == null)
                return;

            //---------------------------------------------------------------------------------------------------------------------Infos
            if (!oldVersion.FirstName.Equals(newVersion.FirstName) || !oldVersion.LastName.Equals(newVersion.LastName))
                infoMessages.Add("Dei Vor oder Nachname würde geändert werden.");
            if (!oldVersion.RCL.Equals(newVersion.RCL))
                infoMessages.Add("Dein Rate-Card-Level würde geändert werden.");
            if (!oldVersion.Description.Equals(newVersion.Description))
                infoMessages.Add("Deine Beschreibung würde geändert werden.");

            var hardSetOld = oldVersion.Abilities.Where(x => x.Type == SkillGroup.Hardskill).ToHashSet();
            var softSetOld = oldVersion.Abilities.Where(x => x.Type == SkillGroup.Softskill).ToHashSet();
            var hardSetNew = newVersion.Abilities.Where(x => x.Type == SkillGroup.Hardskill).ToHashSet();
            var softSetNew = newVersion.Abilities.Where(x => x.Type == SkillGroup.Softskill).ToHashSet();
            if (!softSetOld.SetEquals(softSetNew)) internalMessages.Add("Soft Skils");
            if (!hardSetOld.SetEquals(hardSetNew)) internalMessages.Add("Hard Skills");
            if (!oldVersion.Roles.SetEquals(newVersion.Roles)) internalMessages.Add("Rollen");
            if (!oldVersion.Fields.SetEquals(newVersion.Fields)) internalMessages.Add("Tätigeitfelder");
            if (!oldVersion.Languages.SetEquals(newVersion.Languages)) internalMessages.Add("Sprachfähigkeiten");
            if (internalMessages.Any())
                infoMessages.Add($"Deine {String.Join(", ", internalMessages)} würden geändert werden.");
            //-----------------------------------------------------------------------------------------------------------------Errors
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = errorMessages.Concat(results.Select(e => e.ErrorMessage)).ToList();

            if (!newVersion.Languages.Where(x => x.Level == "Primärsprache").Any())
                errorMessages.Add("Bei der Profile-Erstellung muss mindestens eine Primärsprache auswählen werden.");
            if (newVersion.Languages.Where(x => x.Level == "").Any())
                errorMessages.Add("Mindesetens ein Sprache hat keine Level Angabe.");
            if (newVersion.Abilities.Where(x => x.Level == "" && x.Type == SkillGroup.Hardskill).Any())
                errorMessages.Add("Mindesetens ein Skill hat keine Level Angabe.");

            //---------------------------------------------------------------------------------------------------------------
            OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
        }
        //-----------------------------------------------------------------------------------------
        public void Update(Employee toStore)
        {
            ValidateUpdate(toStore);

            bool isNew = toStore.PersoNumber != ShowProfile(toStore.PersoNumber).PersoNumber;
            if (errorMessages.Any() || isNew)
            {
                log.LogWarning($" The upper Layer tried to update a invalid Profile ");
                return;
            }
            if (!infoMessages.Any())
            {
                OnChange(new() { SuccesMessage = $"Keine Profil-Änderungen zu speichern." });
                return;
            }

            try
            {
                toStore.MadeFirstChangesOnProfile = true;
                UpdatePersistence(toStore);              //the persistence can work with exceptions but the UI wont care
            }
            catch (Exception e)
            {
                OnChange(new() { ErrorMessages = new[] { $"Es trat ein Fehler in der Persistenz auf {e.Message}" } });
                log.LogError($" Persistenz Error {e.Message}");            // shouldnt happen   we might got a Problem
            }

            OnChange(new() { SuccesMessage = $" {toStore.FirstName},  deine Daten wurden gespeichert." });
            log.LogInformation($" \tEmply:{toStore.PersoNumber}  updated himself");
        }






        //----IAccountService implementation ------------------------------------------------------
        public void CreateAccount(Employee newAccount)
        {
            errorMessages = new();
            infoMessages = new();

            if (ShowAllProfiles().Any(x => x.PersoNumber.Equals(newAccount.PersoNumber)))
                errorMessages.Add("Sie könne keinen Account überschreiben.");
            if (newAccount.FirstName.Equals("") || newAccount.LastName.Equals("") || newAccount.PersoNumber.Equals(""))
                errorMessages.Add("Geben sie Vor-, Nachnamen und PersNr ein.");
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages }); ;
            }
            else
            {
                if (newAccount.PersoNumber != newAccount.Password)
                    infoMessages.Add("Passwort und Personal-Number sollten gleich sein. Dies wurde koregiert.");
                CreatePersistence(newAccount);
                OnChange(new() { SuccesMessage = $"Ein neuer Account wurde erstellt {newAccount.PersoNumber}.", InfoMessages = infoMessages });
            }
        }
        //-----------------------------------------------------------------------------------------







        //-------------------------------------Persistence-----------------------------------------with json
        //-----------------------------------------------------------------------------------------
        private readonly string subPath = Path.Combine(new string[] { "jsonPersistierung", "Employees" });
        private readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
        //--read   --------------------------------------------------------------------------------

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
        private string Serialize(Employee employee) => JsonSerializer.Serialize(employee, options);

        //---write --------------------------------------------------------------------------------
        private void CreatePersistence(Employee toBeCreated)
        {
            UpdatePersistence(toBeCreated);
        }
        private void UpdatePersistence(Employee toBeUpdated)
        {
            toBeUpdated.Abilities.ToList().ForEach(x => x.PossibleLevels = null);
            toBeUpdated.Languages.ToList().ForEach(x => x.PossibleLevels = null);

            var json = Serialize(toBeUpdated);

            var path = Path.Combine(env.ContentRootPath, subPath, $"employee{toBeUpdated.PersoNumber}.json");
            File.WriteAllText(path, json, Encoding.UTF8);

            log.LogInformation($"Emplyee wurde persitiert  {toBeUpdated.PersoNumber}");
        }
        private Employee Deserialize(string json) => JsonSerializer.Deserialize<Employee>(json, options);

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------










        // User Messaging Handling
        //----------------keeps delegates to send Events to the calling Layer above----------------
        //-----------------------------------------------------------------------------------------
        public event EventHandler<NoResult> SearchEventHandel;
        protected virtual void OnEmptyResult(NoResult e) => SearchEventHandel?.Invoke(this, e);
        //-----------------------------------------------------------------------------------------


        private List<string> errorMessages = new();
        private List<string> infoMessages = new();
        private List<string> internalMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e)
        {
            if (e.InfoMessages.Any() || e.ErrorMessages.Any() || e.SuccesMessage != "")
                ChangeEventHandel?.Invoke(this, e);
        }
    }

}
