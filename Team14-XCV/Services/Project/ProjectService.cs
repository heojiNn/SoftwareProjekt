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
        private readonly IFieldService _fieldService;

        public ProjectService(IConfiguration config, ILogger<ProjectService> logger, IFieldService fieldService)
        {
            _fieldService = fieldService;
            log = logger;
            connectionString = config.GetConnectionString("MyLocalConnection");
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

            if (newVersion.Start > DateTime.Now.AddYears(1))
                errorMessages.Add("Projecte dürfen nicht ein erst in einem Jahr starten");

        }





        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Project> ShowAllProjects()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var projects = con.Query<Project>("Select * From project");
            con.Close();
            foreach (var pro in projects)
            {
                var purposes = con.Query<string>($"Select Purpose From project_purpose Where Project = '{pro.Id}'");
                foreach (var pur in purposes)
                    pro.Purpose.Add(pur);

                var activityKeys = con.Query<string>($"Select activitie  From activitie Where Project = '{pro.Id}'");
                foreach (var key in activityKeys)
                {
                    pro.ActivitiesWithEmployees.Add(key, con.Query<string>($"Select employee From activitie_done_by Where Project = '{pro.Id}' And Activitie='{key}' ").ToList());
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
            if (!errorMessages.Any())
            {
                using var con = new SqlConnection(connectionString);
                try
                {
                    p.Field ??= "";
                    con.Open();
                    con.Execute($"Insert Into project Values (    '{p.Title}', '{p.Description}',  " +
                                                    $"'{p.Start:yyyy-MM-dd}', '{p.Ende:yyyy-MM-dd}', '{p.Field}' )");
                    con.Close();
                }
                catch (SqlException e)
                {
                    log.LogError($" creating Projekt on database: {e.Message} \n");
                }
                var newID = ShowAllProjects().FirstOrDefault(x => x.Title == title).Id;
                con.Open();
                con.Execute($"Insert Into activitie  Values ({newID}, '' )");
                con.Close();
                OnChange(new() { SuccesMessage = $"Es wurde ein Project erstellt mit der ID:{newID}." });
            }
            else
                OnChange(new() { ErrorMessages = errorMessages });
        }
        public void Update(Project newP)
        {
            errorMessages = new();
            ValidateUpdate(newP);
            if (!_fieldService.GetAllFields().Any(x => x.Name == newP.Field))
                OnChange(new() { ErrorMessages = new string[] { $"Dieses Wert:{newP.Field} muss aus der vohanden Brachen kommen" } });

            if (errorMessages.Any())
            {
                OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
                return;
            }
            using var con = new SqlConnection(connectionString);
            try
            {
                newP.Field ??= "";
                con.Open(); ;
                con.Execute($"Update project Set  Title='{newP.Title}', Description='{newP.Description}', Start='{newP.Start:yyyy-MM-dd}', " +
                                                             $" Ende='{newP.Ende:yyyy-MM-dd}', Field='{newP.Field}'  Where Id={newP.Id} ");

                con.Execute($"Delete From project_purpose  Where Project={newP.Id} ");
                foreach (var purp in newP.Purpose)
                    if (con.Query($"SELECT Project From project_purpose Where Project={newP.Id} And Purpose='{purp}' ").Any())
                        OnChange(new() { ErrorMessages = new string[] { "Eine Zweck mit diesen Namen ist schon vohanden" } });
                    else
                        con.Execute($"Insert Into project_purpose  Values ({newP.Id}, '{purp}' )");

                OnChange(new() { SuccesMessage = "Die Änderungen am Project wurden übernommen" });
            }
            catch (SqlException e)
            {
                log.LogError($"updating Projekt in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
        }

        public void Delete(Project toDelete)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From project  Where Id='{toDelete.Id}' ");
            con.Close();
        }


        public void Add(Project p, string activitie)
        {
            if (activitie.Length > 50)
            {
                OnChange(new() { ErrorMessages = new string[] { "Nicht mehr als 50 Zeichen" } });
                return;
            }
            using var con = new SqlConnection(connectionString);
            con.Open();
            if (con.Query($"SELECT Project From activitie Where Project={p.Id} And Activitie='{activitie}' ").Any())
                OnChange(new() { ErrorMessages = new string[] { "Eine Activität mit diesen Namen ist schon vohanden" } });
            else
            {
                con.Execute($"Insert Into activitie  Values ({p.Id}, '{activitie}' )");
                OnChange(new() { InfoMessages = new string[] { "Activität hinzugefügt" } });
            }
            con.Close();
        }
        public void Remove(Project p, string activitie)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From activitie  Where Project={p.Id} And Activitie='{activitie}' ");
            con.Close();
            OnChange(new() { InfoMessages = new string[] { "Activität entfernt" } });
        }

        public void Add(Project p, Employee doneBy, string activitie = "")
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Insert Into activitie_done_by Values({p.Id}, '{activitie}', '{doneBy.PersoNumber}' )");
            con.Close();
        }
        public void Remove(Project p, Employee doneBy, string activitie = "")
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            con.Execute($"Delete From activitie_done_by  Where Project={p.Id} And Activitie='{activitie}' And Employee='{doneBy.PersoNumber}' ");
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
