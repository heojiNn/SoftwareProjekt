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



        private void ValidateUpdate(Project newVersion)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (newVersion.Start > newVersion.End.AddSeconds(1))
                errorMessages.Add("Das Enddatum muss hinter den Beginn liegen");
            newVersion.Purpose.ForEach(x => x = x.Trim());
            foreach (var pur in newVersion.Purpose)
            {
                if (pur.Length < 2)
                    errorMessages.Add("Die Beschreibung des Zwecks:({pur}) ist zu kurz");
                if (pur.Length > 200)
                    errorMessages.Add("Die Beschreibung des Zwecks:({pur}) ist zu lang");
            }
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
                    pro.Activities.Add(key, (PersNrs.ToList(), new List<Skill>()));
                }
            }
            return projects;
        }


        //---write --------------------------------------------------------------------------------
        public void Create(string title)
        {
            errorMessages = new();
            var p = new Project() { Title = title };
            ValidateUpdate(p);
            if (ShowAllProjects().Any(x => x.Title.ToLower() == title.ToLower()))
                errorMessages.Add("Es existiert schon ein Projekt mit diesem Titel");
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
                log.LogError($" Create() persitence Error:: \n{e.Message}\n");
            }
            finally { con.Close(); }

            var newID = ShowAllProjects().FirstOrDefault(x => x.Title == title).Id;
            con.Open();
            con.Execute($"Insert Into [ProjectHasActivity]  Values ('ohne sepz. Aktivität', @newID)", new { newID });
            con.Close();
            OnChange(new() { SuccesMessage = $"Das Projekt:{title} wurde  mit der ID:{newID} erstellt." });

        }
        public void Update(Project newP)
        {
            errorMessages = new();
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

                OnChange(new() { SuccesMessage = "Die Änderungen am Project wurden übernommen" });
            }
            catch (SqlException e)
            {
                log.LogError($" Update() persitence Error:: \n{e.Message}\n");
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

        public void Add(Project p, Employee doneBy, string activity = "ohne sepz. Aktivität")
        {
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Execute($"Insert Into [activityHasEmployee] Values('{activity}', {p.Id}, '{doneBy.PersoNumber}' )");

                con.Execute($"IF EXISTS ( SELECT * FROM [ActivityHasEmployee] " +
                                                $"Where  [Activity]<>'ohne sepz. Aktivität' And [Project]={p.Id} And [Employee]='{doneBy.PersoNumber}' )" +
            $"Delete From [ActivityHasEmployee]  Where  [Activity]='ohne sepz. Aktivität'  And  [Project]={p.Id} And [Employee]='{doneBy.PersoNumber}' "
                );
            }
            catch (SqlException) { }
            finally { con.Close(); }
        }
        public void Remove(Project p, Employee doneBy, string activity = "ohne sepz. Aktivität")
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From [ActivityHasEmployee]  Where  [Activity]='{activity}' And [Project]={p.Id} And [Employee]='{doneBy.PersoNumber}' ");
            con.Close();
        }

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------





        //-----------------------------------------------------------------------------------------
        private List<string> errorMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
}
