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
    public class ProjectService : IProjectService
    {
        private readonly string connectionString;
        private readonly ILogger<ProjectService> log;


        public ProjectService(IConfiguration config, ILogger<ProjectService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MS_SQL_Connection");
        }


        //-----------------------------------------------------------------------------------------
        //---------------------------------Buissines Logic-----------------------------------------
        public Project ShowProject(int id)
        {
            return ShowAllProjects().FirstOrDefault(x => x.Id == id);
        }



        public void ValidateUpdate(Project newVersion)
        {
            errorMessages = new();
            infoMessages = new();
            Project oldVersion = ShowProject(newVersion.Id);
            if (newVersion == null || oldVersion == null)
                return;

            if (!oldVersion.Title.Equals(newVersion.Title))
                infoMessages.Add("Der Projekttitel wird geändert.");
            if (!oldVersion.Description.Equals(newVersion.Description))
                infoMessages.Add("Die Beschreibung wird geändert.");
            if (!oldVersion.Start.Equals(newVersion.Start) || !oldVersion.End.Equals(newVersion.End))
                infoMessages.Add("Die Laufzeit wird geändert.");
            if (oldVersion.Field != null && !oldVersion.Field.Equals(newVersion.Field) || oldVersion.Field == null && newVersion.Field != null)
                infoMessages.Add("Die Branche wird geändert.");
            if (oldVersion.Purpose.Count == newVersion.Purpose.Count)
            {
                foreach (var purp in oldVersion.Purpose)
                {
                    if (!newVersion.Purpose.Contains(purp))
                    {
                        infoMessages.Add("Die Projektzwecke werden geändert.");
                        break;
                    }
                }
            }
            else
            {
                infoMessages.Add("Die Projektzwecke werden geändert.");

            }
            if (oldVersion.Activities.Count == newVersion.Activities.Count)
            {
                bool employeeChanged = false;
                bool skillChanged = false;
                foreach (var kvp in oldVersion.Activities)
                {
                    if (!newVersion.Activities.ContainsKey(kvp.Key) || employeeChanged || skillChanged)
                    {
                        infoMessages.Add("Die Projekttätigkeiten und/oder zugehörige MitarbeiterInnen bzw. Skills werden geändert.");
                        break;
                    }
                    if (kvp.Value.persNr.Count != newVersion.Activities[kvp.Key].persNr.Count || kvp.Value.requirements.Count() != newVersion.Activities[kvp.Key].requirements.Count())
                    {
                        infoMessages.Add("Die Projekttätigkeiten und/oder zugehörige MitarbeiterInnen bzw. Skills werden geändert.");
                        break;
                    }

                    foreach (var emp in kvp.Value.persNr)
                    {
                        if (!newVersion.Activities[kvp.Key].persNr.Contains(emp))
                        {
                                employeeChanged = true;
                                break;
                        }
                        
                    }
                    if (!employeeChanged)
                    {
                        foreach (var skill in kvp.Value.requirements)
                        {
                                if (!newVersion.Activities[kvp.Key].requirements.Contains(skill))
                                {
                                    skillChanged = true;
                                    break;
                                }

                        }
                    }

                }
            }
            else
            {
                infoMessages.Add("Die Projekttätigkeiten und/oder zugehörige MitarbeiterInnen bzw. Skills werden geändert");
            }

            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();
            if (ShowAllProjects().Where(p => p.Id != newVersion.Id).Any(x => x.Title.ToLower() == newVersion.Title.ToLower()))
                errorMessages.Add("Es existiert schon ein Projekt mit diesem Titel.");
            if (newVersion.Start < new DateTime(2000, 1, 1))
                errorMessages.Add("Der Projektanfang kann nicht vor dem Jahr 2000 liegen.");
            if (newVersion.Start > newVersion.End.AddSeconds(1))
                errorMessages.Add("Das Enddatum muss hinter den Beginn liegen.");
            if (newVersion.End > new DateTime(2100, 1, 1))
                errorMessages.Add("Das Projekt muss vor dem Jahr 2100 beendet werden.");
            newVersion.Purpose.ForEach(x => x = x.Trim());
            foreach (var pur in newVersion.Purpose)
            {
                if (pur.Length < 2)
                    errorMessages.Add($"Die Beschreibung des Zwecks:({pur}) ist zu kurz.");
                if (pur.Length > 200)
                    errorMessages.Add($"Die Beschreibung des Zwecks:({pur}) ist zu lang.");
            }
            foreach (var key in newVersion.Activities.Keys)
            {
                key.Trim();
                if (key.Length < 2)
                    errorMessages.Add($"Die Beschreibung der Tätigkeit:({key}) ist zu kurz");
                if (key.Length > 50)
                    errorMessages.Add($"Die Beschreibung der Tätigkeit:({key}) ist zu lang");
            }
            OnChange(new()
            {
                InfoMessages = infoMessages,
                ErrorMessages = errorMessages
            });
        }

        private void Validate(Project newVersion)
        {
            errorMessages = new();

            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();
            if (ShowAllProjects().Any(x => x.Title.ToLower() == newVersion.Title.ToLower()))
                errorMessages.Add("Es existiert schon ein Projekt mit diesem Titel.");
            if (newVersion.Start < new DateTime(2011, 1, 1))
                errorMessages.Add("Der Projektanfang kann nicht vor dem Jahr 2011 liegen.");
            if (newVersion.Start > newVersion.End.AddSeconds(1))
                errorMessages.Add("Das Enddatum muss hinter dem Beginn liegen.");
            if (newVersion.End > new DateTime(2100, 1, 1))
                errorMessages.Add("Das Projekt muss vor dem Jahr 2100 beendet werden.");
            newVersion.Purpose.ForEach(x => x = x.Trim());
            foreach (var pur in newVersion.Purpose)
            {
                if (pur.Length < 2)
                    errorMessages.Add($"Die Beschreibung des Zwecks:({pur}) ist zu kurz.");
                if (pur.Length > 200)
                    errorMessages.Add($"Die Beschreibung des Zwecks:({pur}) ist zu lang.");
            }
            foreach (var key in newVersion.Activities.Keys)
            {
                key.Trim();
                if (key.Length < 2)
                    errorMessages.Add($"Die Beschreibung der Tätigkeit:({key}) ist zu kurz.");
                if (key.Length > 50)
                    errorMessages.Add($"Die Beschreibung der Tätigkeit:({key}) ist zu lang.");
            }
            OnChange(new()
            {
                ErrorMessages = errorMessages
            });
        }



        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Project> ShowAllProjects()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var projects = con.Query<Project>("Select * From [Project]");
            con.Close();
            foreach (var pro in projects)
            {
                var purposes = con.Query<string>("Select [Name] From [ProjectHasPurpose] Where [Project] = @ID", new { pro.Id });
                foreach (var pur in purposes)
                    pro.Purpose.Add(pur);

                var activityKeys = con.Query<string>($"Select [Name]  From [ProjectHasActivity] Where [Project] =@ID", new { pro.Id });
                foreach (var key in activityKeys)
                {
                    var PersNrs = con.Query<string>($"Select [Employee] From [ActivityHasEmployee] Where [Project] = @ID  And [Activity] = @Key", new { pro.Id, key });
                    var skills = con.Query<(string, string)>("Select ActivityHasSkill.skill_name as Name, ActivityHasSkill.skill_cat as Cat " +
                                                                "From [ActivityHasSkill]  " +
                                                                    $"Where [Project] = @ID And [Activity] = @Key", new { pro.Id, key });

                    List<Skill> skillList = new();
                    foreach (var (Name, Cat) in skills)
                    {
                        skillList.Add(new Skill() { Name = Name, Category = new SkillCategory() { Name = Cat } });
                    }
                    pro.Activities.Add(key, (PersNrs.ToList(), skillList.ToList()));

                }
            }
            return projects;
        }


        //---write --------------------------------------------------------------------------------
        public void Create(string title)
        {
            errorMessages = new();
            var p = new Project() { Title = title };
            Validate(p);
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return;
            }

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute($"Insert Into [Project] Values ( @Title,  @Description,  @Start,  @End,  @Field)", p);
            }
            catch (SqlException e)
            {
                log.LogError($" Create() persistence Error:: \n{e.Message}\n");
            }
            finally { con.Close(); }

            var newID = ShowAllProjects().FirstOrDefault(x => x.Title == title).Id;
            con.Open();
            con.Execute($"Insert Into [ProjectHasActivity]  Values ('ohne Tätigkeit', @newID)", new { newID });
            con.Close();
            OnChange(new() { SuccesMessage = $"Das Projekt:{title} wurde  mit der ID:{newID} erstellt." });
        }

        public void Create(Project p)
        {
            errorMessages = new();
            Validate(p);
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return;
            }
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute("Insert Into [Project] Values (@Title, @Description, @Start, @End, @Field)", p);

            }
            catch (SqlException e)
            {
                log.LogError($" Add(Project, Field) persitence Error:: \n{e.Message}\n");

            }
            finally
            {
                con.Close();
            }
            var newId = ShowAllProjects().FirstOrDefault(x => x.Title == p.Title).Id;
            try
            {
                con.Open();

                foreach (var purp in p.Purpose)
                {
                    con.Execute($"Insert Into [ProjectHasPurpose] Values (@Purp, @newId) ", new { purp, newId });
                }
                foreach (var kvp in p.Activities)
                {
                    if (Add(newId, kvp.Key))
                    {
                        foreach (var emp in kvp.Value.persNr)
                        {
                            Add(newId, emp, kvp.Key);
                        }
                        foreach (var skill in kvp.Value.requirements)
                        {
                            Add(newId, skill, kvp.Key);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                log.LogError($" Add(Project, Activity) persitence Error:: \n{e.Message}\n");

            }
            finally
            {
                con.Close();
            }
            OnChange(new() { SuccesMessage = $"Das Projekt:{p.Title} wurde mit der Id: {newId} erstellt." });
        }

        public void Update(Project newP)
        {
            errorMessages = new();
            infoMessages = new();
            Project oldVersion = ShowProject(newP.Id);
            ValidateUpdate(newP);
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return;
            }

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open(); ;
                con.Execute($"Update [Project] Set  [Title]=@Title,  [Description]=@Description, [Start]= @Start, [End]=@End, [Field]=@Field  Where [Id]= @ID", newP);
                con.Execute($"Delete From [ProjectHasPurpose]  Where Project={newP.Id} ");
                foreach (var purp in newP.Purpose)
                    con.Execute($"Insert Into [ProjectHasPurpose] Values (@Purp, @Id) ", new { purp, newP.Id });
                con.Execute($"Delete From [ProjectHasActivity]  Where Project={newP.Id} ");
                foreach (var kvp in newP.Activities)
                {
                    if (Add(newP.Id, kvp.Key))
                    {
                        foreach (var emp in kvp.Value.persNr)
                            Add(newP.Id, emp, kvp.Key);
                        foreach (var skill in kvp.Value.requirements)
                            Add(newP.Id, skill, kvp.Key);
                    }
                }
                OnChange(new() { SuccesMessage = "Die Änderungen am Project wurden übernommen" });
            }
            catch (SqlException e)
            {
                log.LogError($" Update() persistence Error:: \n{e.Message}\n");
            }
            finally { con.Close(); }
        }

        public void Delete(Project toDelete)
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute($"Delete From project  Where Id='{toDelete.Id}' ");

            }
            catch (SqlException) { }
            finally { con.Close(); }
        }

        private bool Add(int pId, string activity)
        {
            if (activity.Length > 50)
            {
                OnChange(new() { ErrorMessages = new string[] { "Eine Tätigkeit darf nicht länger als 50 Zeichen sein." } });
                return false;
            }
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                if (con.Query($"SELECT [Project] From [ProjectHasActivity] Where [Project] = {pId} And [Name] = '{activity}' ").Any())
                    OnChange(new() { ErrorMessages = new[] { "Eine Tätigkeit mit diesen Namen ist schon vohanden" } });
                else
                {
                    con.Execute($"Insert Into [ProjectHasActivity]  Values ('{activity}', {pId})");
                    OnChange(new() { SuccesMessage = "Tätigkeit hinzugefügt" });
                }
            }
            catch (SqlException) { }
            finally { con.Close(); }
            return true;
        }
        public void Add(Project p, string activity)
        {
            if (activity.Length > 50)
            {
                OnChange(new() { ErrorMessages = new string[] { "Nicht mehr als 50 Zeichen" } });
                return;
            }
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                if (con.Query($"SELECT [Project] From [ProjectHasActivity] Where [Project] = {p.Id} And [Name] = '{activity}' ").Any())
                    OnChange(new() { ErrorMessages = new[] { "Eine Activität mit diesen Namen ist schon vohanden" } });
                else
                {
                    con.Execute($"Insert Into [ProjectHasActivity]  Values ('{activity}', {p.Id})");
                    OnChange(new() { SuccesMessage = "Aktivität hinzugefügt" });
                }
            }
            catch (SqlException) { }
            finally { con.Close(); }
        }


        public void Remove(Project p, string activity)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From [ProjectHasActivity]  Where [Project]={p.Id} And [Name]='{activity}' ");
            con.Close();
            OnChange(new() { SuccesMessage = "Aktivität entfernt" });
        }

        public void Add(Project p, Employee doneBy, string activity = "ohne Tätigkeit")
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute($"Insert Into [activityHasEmployee] Values('{activity}', {p.Id}, '{doneBy.PersoNumber}' )");

                con.Execute($"IF EXISTS ( SELECT * FROM [ActivityHasEmployee] " +
                                                $"Where  [Activity]<>'ohne Tätigkeit' And [Project]={p.Id} And [Employee]='{doneBy.PersoNumber}' )" +
            $"Delete From [ActivityHasEmployee]  Where  [Activity]='ohne Tätigkeit'  And  [Project]={p.Id} And [Employee]='{doneBy.PersoNumber}' "
                );
            }
            catch (SqlException) { }
            finally { con.Close(); }
        }
        private void Add(int pId, string doneBy, string activity = "ohne Tätigkeit")
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute($"Insert Into [activityHasEmployee] Values('{activity}', {pId}, '{doneBy}' )");

                con.Execute($"IF EXISTS ( SELECT * FROM [ActivityHasEmployee] " +
                                                $"Where  [Activity]<>'ohne Tätigkeit' And [Project]={pId} And [Employee]='{doneBy}' )" +
            $"Delete From [ActivityHasEmployee]  Where  [Activity]='ohne Tätigkeit'  And  [Project]={pId} And [Employee]='{doneBy}' "
                );
            }
            catch (SqlException) { }
            finally { con.Close(); }
        }

        private void Remove(int pId, string doneBy, string activity = "ohne Tätigkeit")
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From [ActivityHasEmployee]  Where  [Activity]='{activity}' And [Project]={pId} And [Employee]='{doneBy}' ");
            con.Close();
        }
        public void Remove(Project p, Employee doneBy, string activity = "ohne Tätigkeit")
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From [ActivityHasEmployee]  Where  [Activity]='{activity}' And [Project]={p.Id} And [Employee]='{doneBy.PersoNumber}' ");
            con.Close();
        }

        private void Add(int pId, Skill skill, string activity = "ohne Tätigkeit")
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute($"Insert Into [activityHasSkill] Values('{activity}', {pId}, '{skill.Name}', '{skill.Category.Name}' )");

                con.Execute($"IF EXISTS ( SELECT * FROM [ActivityHasSkill] " +
                                                $"Where  [Activity]<>'ohne Tätigkeit' And [Project]={pId} And [skill_name]='{skill.Name}' And [skill_cat]='{skill.Category.Name}' )" +
            $"Delete From [ActivityHasSkill]  Where  [Activity]='ohne Tätigkeit'  And  [Project]={pId} And [skill_name]='{skill.Name}' And [skill_cat]='{skill.Category.Name}' "
                );
            }
            catch (SqlException e)
            {
                log.LogError($" Add(Project, Act, Skill) persitence Error:: \n{e.Message}\n");

            }
            finally { con.Close(); }
        }

        public void Remove(int pId, Skill skill, string activity = "ohne Tätigkeit")
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From [ActivityHasSkill]  Where  [Activity]='{activity}' And [Project]={pId}  And [Skill_name]='{skill.Name}' And [Skill_cat]='{skill.Category.Name}' ");
            con.Close();
        }


        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------





        //-----------------------------------------------------------------------------------------
        private List<string> errorMessages = new();
        private List<string> infoMessages = new();

        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e)
        {
            if (e.InfoMessages.Any() || e.ErrorMessages.Any() || e.SuccesMessage != "")
                ChangeEventHandel?.Invoke(this, e);
        }
    }
}
