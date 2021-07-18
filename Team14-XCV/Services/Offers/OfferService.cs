using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    public class OfferService : IOfferService
    {
        private readonly string connectionString;
        private readonly ILogger<OfferService> log;
        private readonly ISkillService ofSkillService;
        private readonly IProfileService ofProfileService;

        public OfferService(IConfiguration config, ILogger<OfferService> logger, ISkillService skillService, IProfileService profileService)
        {
            log = logger;
            ofSkillService = skillService;
            ofProfileService = profileService;
            connectionString = config.GetConnectionString("MS_SQL_Connection");
        }

        //-----------------------------------------------------------------------------------------
        //---------------------------------Business Logic------------------------------------------

        public void ValidateUpdate(Offer newVersion)
        {
            errorMessages = new();
            infoMessages = new();
            inlineWords = new();

            Offer oldVersion = ShowOffer(newVersion.Id);
            if (newVersion == null || oldVersion == null)
                return;
            //-------------------------------------------------------------------------------------infoMessages
            if (!oldVersion.Title.Equals(newVersion.Title))
                infoMessages.Add("Der Titel wurde geändert.");
            if (!oldVersion.Description.Equals(newVersion.Description))
                infoMessages.Add("Die Beschreibung wurde geändert.");
            if (!oldVersion.Start.Equals(newVersion.Start))
                infoMessages.Add("Das Projekt-startdatum wurde geändert.");
            if (!oldVersion.End.Equals(newVersion.End))
                infoMessages.Add("Das Projekt-enddatum wurde geändert.");
            

            if (!oldVersion.Requirements.SetEquals(newVersion.Requirements)) inlineWords.Add("Skills");
            else
            {
                static bool predicate1(Skill x) => x.Type == SkillGroup.Hardskill;
                var hardSetOld = oldVersion.Requirements.Where(predicate1).ToHashSet(new SkillComparerWithLevel());
                var hardSetNew = newVersion.Requirements.Where(predicate1).ToHashSet(new SkillComparerWithLevel());
                if (!hardSetOld.SetEquals(hardSetNew)) inlineWords.Add("Skill-Level");
            }
            static bool predicate2(Skill x) => x.Type == SkillGroup.Softskill;
            var softSetOld = oldVersion.Requirements.Where(predicate2).ToHashSet();
            var softSetNew = newVersion.Requirements.Where(predicate2).ToHashSet();
            if (!softSetOld.SetEquals(softSetNew)) inlineWords.Add("Soft-Skills");
            if (!oldVersion.Fields.All(newVersion.Fields.Contains))
                inlineWords.Add("Tätigkeitsfeldern");
            if (oldVersion.participants.Count != newVersion.participants.Count)
                inlineWords.Add("MitarbeiterInnen");

            if (inlineWords.Any())
                infoMessages.Add($"Änderungen an {String.Join(", ", inlineWords)} wurden vorgenommen.");
            //-------------------------------------------------------------------------------------errorMessages
            // adds the Model-Validation to the List of Errors
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (newVersion.Requirements.Where(x => !ofSkillService.GetAllLevel().Contains(x.Level) && x.Type == SkillGroup.Hardskill).Any())
                errorMessages.Add("Mindestens ein HardSkill hat keine Level Angabe.");
            if (newVersion.Title.Length == 0)
                errorMessages.Add($"{newVersion.Title}: Der Titel sollte aus mindestens einem Zeichen bestehen.");
            if (newVersion.End.Year < 2000)
                errorMessages.Add($"{newVersion.End.Year}: Das Enddatum sollte in der Zukunft liegen.");
            if (newVersion.End.Year > 3000)
                errorMessages.Add($"{newVersion.End.Year}: Das Enddatum sollte noch in diesem Jahrtausend liegen.");
            if (newVersion.Start.Year > 3000)
                errorMessages.Add($"{newVersion.End.Year}: Das Startdatum sollte noch in diesem Jahrtausend liegen.");
            if (newVersion.Start.Year < 2000)
                errorMessages.Add($"{newVersion.End.Year}: Das Startdatum sollte nicht zu weit in der Vergangenheit liegen.");

            foreach(Employee e in newVersion.participants)
            {
                if (e.offerRole.Equals("Consultant") && e.offerRCL < 4) errorMessages.Add("Consultant hat aktuell mindestens RCL 4.");
                if (0 == e.offerRCL || e.offerRCL > 8 ) errorMessages.Add("RCL sollte im Bereich [1,8] liegen.");
                if (e.offerWage > 9999.99) errorMessages.Add("Der Stundenlohn ist momentan auf 9999.99 begrenzt.");
                if (e.hoursPerDay > 24) errorMessages.Add("Die maximale Anzahl an Arbeitsstunden pro Tag ist überschritten.");
                if (e.daysPerRun > (newVersion.End-newVersion.Start).Days) errorMessages.Add("EinE MitarbeiterIn hat mehr Arbeitstage als die Projektgesamtlaufzeit besitzt.");
                if (e.discount > 100 || e.discount < 0) errorMessages.Add("Die Rabattangabe bitte als ganze Zahl zwischen 0 - 100 (%), ohne das Prozentsymbol");

                foreach (Employee eo in oldVersion.participants)
                {
                    if (e.PersoNumber.Equals(eo.PersoNumber))
                    {
                        if (!e.offerRole.Equals(eo.offerRole)) infoMessages.Add($"Rolle von {e.FirstName} wurde geändert.");
                        if (!e.offerRCL.Equals(eo.offerRCL)) infoMessages.Add($"RCL von {e.FirstName} wurde geändert.");
                        if (!e.offerWage.Equals(eo.offerWage)) infoMessages.Add($"Stundenlohn von {e.FirstName} wurde geändert.");
                        if (!e.hoursPerDay.Equals(eo.hoursPerDay)) infoMessages.Add($"Arbeitsstunden von {e.FirstName} wurde geändert.");
                        if (!e.daysPerRun.Equals(eo.daysPerRun)) infoMessages.Add($"Arbeitstage von {e.FirstName} wurde geändert.");
                        if (!e.discount.Equals(eo.discount)) infoMessages.Add($"Rabattangabe von {e.FirstName} wurde geändert.");
                    }
                }
            }

            //-------------------------------------------------------------------------------------

            if (infoMessages.Count == 0 && errorMessages.Count == 0)
                infoMessages.Add("Es wurden keine Änderungen vorgenommen.");

            OnChange(new()
            {
                InfoMessages = infoMessages,
                ErrorMessages = errorMessages
            });

        }

        public void ValidateCreate(Offer newVersion)
        {
            errorMessages = new();
            infoMessages = new();
            inlineWords = new();

            //-------------------------------------------------------------------------------------errorMessages
            // adds the Model-Validation to the List of Errors
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (newVersion.Requirements.Where(x => !ofSkillService.GetAllLevel().Contains(x.Level) && x.Type == SkillGroup.Hardskill).Any())
                errorMessages.Add("Mindestens ein HardSkill hat keine Level Angabe.");
            if (newVersion.Title.Length == 0)
                errorMessages.Add($" Der Title sollte aus mindestens einem Zeichen bestehen.");
            if (newVersion.End.Year < 2000)
                errorMessages.Add($"{newVersion.End.Year}: Das Enddatum sollte in der Zukunft liegen.");
            if (newVersion.End.Year > 3000)
                errorMessages.Add($"{newVersion.End.Year}: Das Enddatum sollte noch in diesem Jahrtausend liegen.");
            if (newVersion.Start.Year > 3000)
                errorMessages.Add($"{newVersion.Start.Year}: Das Startdatum sollte noch in diesem Jahrtausend liegen.");
            if (newVersion.Start.Year < 2000)
                errorMessages.Add($"{newVersion.Start.Year}: Das Startdatum sollte nicht zu weit in der Vergangenheit liegen.");
            foreach (Employee e in newVersion.participants)
            {
                if (e.offerRole.Equals("Consultant") && e.offerRCL < 4) errorMessages.Add("Consultant hat aktuell mindestens RCL 4.");

                if (0 == e.offerRCL || e.offerRCL > 8) errorMessages.Add("RCL sollte im Bereich [1,8] liegen.");

                if (e.offerWage > 9999.99) errorMessages.Add("Der Stundenlohn ist momentan auf 9999.99 begrenzt.");

                if (e.hoursPerDay > 24) errorMessages.Add("Die maximale Anzahl an Arbeitsstunden pro Tag ist überschritten.");

                if (e.daysPerRun > (newVersion.End - newVersion.Start).Days) errorMessages.Add("EinE MitarbeiterIn hat mehr Arbeitstage als die Projektgesamtlaufzeit besitzt.");

                if (e.discount > 100 || e.discount < 0) errorMessages.Add("Die Rabattangabe bitte als ganze Zahl zwischen 0 - 100 (%), ohne das Prozentsymbol");

            }
            //-------------------------------------------------------------------------------------
            if (errorMessages.Count == 0)
                infoMessages.Add("Das Angebot kann erfolgreich erstellt werden.");


            OnChange(new()
            {
                InfoMessages = infoMessages,
                ErrorMessages = errorMessages
            });
        }

        public int Runtime(DateTime end, DateTime start)
        {
            return (end - start).Days;
        }

        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //--read   --------------------------------------------------------------------------------


        public Employee ShowOfferEmployee(int id, string persnr)
        {
            return ShowOffer(id).participants.Where(e => e.PersoNumber.Equals(persnr)).Single();
        }

        public Offer ShowOffer(int id)
        {
            return ShowAllOffers().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Offer> ShowAllOffers()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var offers = con.Query<Offer>("Select * From [offer]");
            con.Close();
            try {
                con.Open();
                foreach (var of in offers)
                {
                    var fields = con.Query<Field>("Select [Field] as Name " +
                                                    $"From [offerHasField] Where [Offer] = {of.Id}");
                    foreach (var field in fields)
                        of.Fields.Add(field);

                    var skills = con.Query<Skill, string, Skill>(@$"SELECT  s.Skill as Name,  sl.Name as Level,  s.skill_cat as Category
                                                                        From [offerHasSkill] s  Left Join [Skill_Level] sl   ON s.Skill_Level = sl.Level
                                                                    Where [Offer] = '{of.Id}'",
                                                                        (skill, category) =>
                                                                        {
                                                                            if (skill != null)
                                                                                skill.Category = new SkillCategory() { Name = category };
                                                                            return skill;
                                                                        }, splitOn: "Category").ToHashSet();
                    of.Requirements = skills;
                    var employees = con.Query<Employee>(" Select employee.PersoNumber as PersoNumber, employee.FirstName as FirstName, employee.LastName as LastName," +
                                                        " employee.Description as Description, employee.Image as Image, employee.RCL as RCL, employee.Experience as Experience," +
                                                        " employee.EmployedSince as EmployedSince, employee.MadeFirstChangesOnProfile as MadeFirstChangesOnProfile," +
                                                        " offerHasEmployee.Role as offerRole, offerHasEmployee.RCL as offerRCL, offerHasEmployee.Wage as offerWage, offerHasEmployee.HoursPerDay as hoursPerDay, " +
                                                        " offerHasEmployee.DaysPerRuntime as daysPerRun, offerHasEmployee.Discount as discount" +
                                                        " From [offerHasEmployee] Join [employee] On offerHasEmployee.PersoNumber=employee.PersoNumber" +
                                                       $" Where [Offer] = {of.Id};");
                    foreach (var emp in employees)
                    {
                        try
                        {
                            of.participants.Add(emp);
                        }
                        catch (Exception e)
                        {
                            Console.Write($"Failed to add Employee {emp.FirstName} to offer {of.Title}" + e.Message);
                        }
                    }
                }
            } finally 
            {
                con.Close();
            }
            return offers;
        }

        public List<Skill> ShowAllOfferSkills()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var offers = con.Query<Offer>("Select * From [offer]");
            con.Close();
            List<Skill> skills = new List<Skill>();
            try
            {
                con.Open();
                foreach (var of in offers)
                {
                    var offerSkills = con.Query<Skill, string, Skill>(@$"SELECT  s.Skill as Name,  sl.Name as Level,  s.skill_cat as Category
                                                                        From [offerHasSkill] s  Left Join [Skill_Level] sl   ON s.Skill_Level = sl.Level
                                                                    Where [Offer] = {of.Id}",
                                                                        (skill, category) =>
                                                                        {
                                                                            if (skill != null)
                                                                                skill.Category = new SkillCategory() { Name = category };
                                                                            return skill;
                                                                        }, splitOn: "Category").ToList();
                    skills.Union(offerSkills);
                }
            }
            finally
            {
                con.Close();
            }
            return skills;
        }


        public List<Field> ShowAllOfferFields()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var offers = con.Query<Offer>("Select * From [offer]");
            con.Close();
            List<Field> fields = new List<Field>();
            try
            {
                con.Open();
                foreach (var of in offers)
                {
                    var offerFields = con.Query<Field>($"Select [Field] as Name From [offerHasField] Where [Offer] = {of.Id}");
                    fields.Concat(offerFields);
                }
            }
            finally
            {
                con.Close();
            }
            return fields;
        }


        public int GetLastId()
        {
            int id = 1;
            using var con = new SqlConnection(connectionString);
            con.Open();
            id = con.QuerySingle<int>("SELECT IDENT_CURRENT('offer');");
            con.Close();
            return id;
        }

        public int GetNextId()
        {
            int id = 1;
            using var con = new SqlConnection(connectionString);
            con.Open();
            id = con.QuerySingle<int>("SELECT IDENT_CURRENT('offer')+1;");
            con.Close();
            return id;
        }

        public void ResetId()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute("DBCC CHECKIDENT('[offer]', RESEED, 0);");
            con.Close();
        }

        //---write --------------------------------------------------------------------------------

        public void Create(string title, string description, DateTime start, DateTime end, Skill skill, Field field, ISet<Employee> participants)
        {
            throw new NotImplementedException();
        }

        public void Create(string title, string description, DateTime start, DateTime end)
        {
            errorMessages = new();
            var o = new Offer() { Title = title, Description = description, Start = start, End = end };
            ValidateUpdate(o);
            if (!errorMessages.Any())
            {
                using var con = new SqlConnection(connectionString);
                try
                {
                    con.Open();
                    con.Execute($"Insert Into [offer] Values (@Title, @Description, @Start, @End)", o);
                    con.Close();
                }
                catch (SqlException e)
                {
                    log.LogError($" creating Offer on database: {e.Message} \n");
                }
                OnChange(new() { SuccesMessage = $"Es wurde ein Angebot erstellt." });
            }
            else
                OnChange(new() { ErrorMessages = errorMessages });
        }

        public void Delete(Offer o)
        {
            if (o != null)
            {
                try
                {
                    using var con = new SqlConnection(connectionString);
                    con.Open();
                    con.Execute("IF EXISTS (SELECT [Id] " +
                                            "From [offer] " +
                                            $"Where [Id]={o.Id})" +
                                $"Delete From [offer] Where [Id]={o.Id};");
                    con.Close();
                } catch (Exception e)
                {
                    log.LogError($" creating Offer on database: {e.Message} \n");
                }
                
            }
        }

        public void Update(Offer o)
        {
            errorMessages = new();
            ValidateUpdate(o);
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return;
            }
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open(); ;
                con.Execute($"Update [offer] Set  [Title]=@Title,  [Description]=@Description, [Start]=@Start, [End]=@End Where [Id]={o.Id}", o);
                //Update Fields
                con.Execute($"Delete From [offerHasField] Where [Offer]={o.Id}");
                foreach (var fld in o.Fields) 
                    con.Execute($"Insert Into [offerHasField] Values ({o.Id}, '{fld.Name}')");
                //Update Employees
                con.Execute($"Delete From [offerHasEmployee] Where [Offer]={o.Id}");
                foreach (var emp in o.participants) 
                    con.Execute($"Insert Into [offerHasEmployee] Values ({o.Id}, '{emp.PersoNumber}', '{emp.offerRole}', {emp.offerRCL}, {emp.offerWage}, {emp.hoursPerDay}, {emp.daysPerRun}, {emp.discount});");
                //Update Skills
                con.Execute($"Delete From [offerHasSkill] Where [Offer]='{o.Id}'");
                foreach (var skill in o.Requirements)
                {
                    int? level = Array.FindIndex(ofSkillService.GetAllLevel(), x => x == skill.Level) + 1;
                    if (skill.Type == SkillGroup.Softskill)
                        level = null;
                    con.Execute("Insert Into [offerHasSkill] Values (@Id, @Skill, @Cat, @Level)", new { Id = o.Id, Skill = skill.Name, Cat = skill.Category.Name, level});
                }
                OnChange(new() { SuccesMessage = "Die Änderungen am Angebot wurden übernommen" });
            }
            catch (SqlException e)
            {
                log.LogError($"Error updating the offer: \n{e.Message}\n");
            }
            finally { con.Close(); }
        }

        public void Add(Offer o, Employee e)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            if (con.Query($"SELECT [Offer] From [offerHasEmployee] " +
                          $"Where [Offer]={o.Id} And [PersoNumber]='{e.PersoNumber}'").Any())
                OnChange(new() { ErrorMessages = new string[] { "EinE MitarbeiterIn mit dieser PersNr existiert bereits in dem Angebot" } });
            else
            {
                con.Execute($"Insert Into [offerHasEmployee]" +
                            $" Values ({o.Id}, '{e.PersoNumber}', '{e.offerRole}', {e.RCL}, {e.offerWage}, {e.hoursPerDay}, {e.daysPerRun}, {e.discount})");

                OnChange(new() { InfoMessages = new string[] { "MitarbeiterIn hinzugefügt" } });
            }
            con.Close();
        }

        public void Add(Offer o, Field f)
        {
            if (f.Name.Length > 50)
            {
                OnChange(new() { ErrorMessages = new string[] { "Branche darf nicht mehr als 50 Zeichen haben" } });
                return;
            }
            using var con = new SqlConnection(connectionString);
            con.Open();
            if (con.Query($"SELECT [Offer] From [offerHasField] Where [Offer]={o.Id} And [Field]='{f.Name}'").Any())
                OnChange(new() { ErrorMessages = new string[] { "Das Angebot enthält diese Branche bereits" } });
            else
            {
                con.Execute($"Insert Into [offerHasField]  Values ({o.Id}, '{f.Name}')");
                OnChange(new() { InfoMessages = new string[] { $"Branche zum Angebot {o.Title} (Id: {o.Id}) hinzugefügt" } });
            }
            con.Close();
        }

        public void Add(Offer o, Skill s)
        {
            using var con = new SqlConnection(connectionString);
            int? level = Array.FindIndex(ofSkillService.GetAllLevel(), x => x == s.Level) + 1;
            if (s.Type == SkillGroup.Softskill)
                level = null;
            con.Open();
            if (con.Query($"SELECT [Offer] From [offerHasSkill] Where [Offer]=@Id And [Skill]=@Skill And [Skill_Cat]=@Cat And [Skill_level]=@level;", new { Id = o.Id, Skill = s.Name, Cat = s.Category.Name, level = level}).Any())
                OnChange(new() { ErrorMessages = new string[] { "Das Angebot enthält diesen Skill bereits" } });
            else
            {
                con.Execute($"Insert Into [offerHasSkill]  Values (@Id, @Skill, @Cat, @level)", new { Id = o.Id, Skill = s.Name, Cat = s.Category.Name, level});
                OnChange(new() { InfoMessages = new string[] { $"Skill zum Angebot {o.Title} (Id: {o.Id}) hinzugefügt" } });
            }
            con.Close();
        }

        public void Remove(Offer o, Skill s)
        {
            // Currently Implemented Within UpdateOffer()
            throw new NotImplementedException();
        }

        public void Remove(Offer o, Field f)
        {
            // Currently Implemented Within UpdateOffer()
            throw new NotImplementedException();
        }

        public void Remove(Offer o, Employee offerEmp)
        {
            if (o != null)
            {
                try
                {
                    using var con = new SqlConnection(connectionString);
                    con.Open();
                    con.Execute("IF EXISTS (SELECT [PersoNumber] " +
                                            "From [offerHasEmployee] " +
                                            $"Where [Offer] = {o.Id})" +
                                $"Delete From [offerHasEmployee] Where [Offer]={o.Id} And [PersoNumber]='{offerEmp.PersoNumber}';");
                    con.Close();
                }
                catch (SqlException e)
                {
                    log.LogError($" Error deleting employee from offer: {e.Message} \n");
                }
                OnChange(new() { SuccesMessage = $"Es wurde ein Objekt erstellt." });
            }
            else
            {
                OnChange(new() { ErrorMessages = new string[] { $"Das übergebene Angebot ist ungültig." } });
            }
        }

        public void Copy(Offer o)
        {
            errorMessages = new();
            ValidateUpdate(o);
            if (!errorMessages.Any())
            {
                using var con = new SqlConnection(connectionString);
                try
                {
                    int id;
                    con.Open();
                    Create(o.Title, o.Description, o.Start, o.End);
                    id = GetLastId();
                    //offerHasEmployee
                    if (o.participants.Count() != 0)
                    {
                        foreach (Employee e in o.participants)
                            con.Execute($"Insert Into [offerHasEmployee] Values ({id}, '{e.PersoNumber}', '{e.offerRole}', {e.offerRCL}, {e.offerWage}, {e.hoursPerDay}, {e.daysPerRun}, {e.discount});");
                    }
                    //offerHasField,
                    if (o.Fields.Count() != 0)
                    {
                        foreach (Field f in o.Fields)
                            con.Execute($"Insert Into [offerHasField]  Values ({id}, '{f.Name}')");
                    }
                    //offerHasSkill
                    if (o.Requirements.Count() != 0)
                    {
                        foreach (var skill in o.Requirements)
                        {
                            int? level = Array.FindIndex(ofSkillService.GetAllLevel(), x => x == skill.Level) + 1;
                            if (skill.Type == SkillGroup.Softskill)
                                level = null;
                            con.Execute("Insert Into [offerHasSkill] Values (@Offer, @Skill, @Cat, @Level)", new { Offer = id, Skill = skill.Name, Cat = skill.Category.Name, level});
                        }
                    }
                    con.Close();
                }
                catch (SqlException e)
                {
                    log.LogError($"Error copying offer {o.Id}, {o.Title}: {e.Message} \n");
                }
                OnChange(new() { SuccesMessage = $"Die Kopie wurde erstellt." });
            }
            else
                OnChange(new() { ErrorMessages = errorMessages });
        }

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
}

