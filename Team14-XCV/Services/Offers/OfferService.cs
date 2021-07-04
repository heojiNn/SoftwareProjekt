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
    public class OfferService : IOfferService
    {
        private readonly string connectionString;
        private readonly ILogger<ProjectService> log;
        private readonly ISkillService ofSkillService;
         
        public OfferService(IConfiguration config, ILogger<ProjectService> logger, ISkillService skillService)
        {
            log = logger;
            ofSkillService = skillService;
            connectionString = config.GetConnectionString("MyLocalConnection");
        }

        //-----------------------------------------------------------------------------------------
        //---------------------------------Business Logic------------------------------------------

        public Offer ShowOffer(int id)
        {
            return ShowAllOffers().FirstOrDefault(x => x.Id == id);
        }




        private void ValidateUpdate(Offer newVersion)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();
            // ...

        }


        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //--read   --------------------------------------------------------------------------------

        public IEnumerable<Offer> ShowAllOffers()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var offers = con.Query<Offer>("Select * From offer");
            con.Close();
            foreach (var of in offers)
            {
                var fields = con.Query<Field>("Select Field as Name " +
                                                $"From offer_field Where Offer = '{of.Id}'");
                foreach (var field in fields)
                    of.Fields.Add(field);

                var skills = con.Query<(string Name, string Lvl, string Cat)>(  "Select offer_skill.skill_name as Name, skill_level.Name as Lvl, offer_skill.skill_cat as Cat " +
                                                                                "From offer_skill  " +
                                                                                    "Join skill_level On  offer_skill.skill_level=skill_level.level " +
                                                                                    $"Where Offer = '{of.Id}';");
                foreach (var (Name, Lvl, Cat) in skills)
                {
                    of.Requirements.Add(new Skill() { Name = Name, Level = Lvl, Category = new SkillCategory() { Name = Cat } });
                    ofSkillService.HangThemOnATree(of.Requirements);
                }


                // TODO
                //offer_employee's RCL may vary in the specific offer compared to his profile. ("angebotsspezifisch angepasstes RC Level")
                //Other then that the employee within the offer should be equal to the employee in table "employee" (PersoNumber references that attribute)

                var employees = con.Query<Employee>(" Select employee.PersoNumber as PersoNumber, employee.FirstName as FirstName, employee.LastName as LastName," +
                                                    " employee.Description as Description, employee.Image as Image, employee.RCL as RCL, employee.Expirience as Expirience, employee.WorkingSince as WorkingSince, employee.MadeFirstChangesOnProfile as MadeFirstChangesOnProfile" +
                                                    " From offer_employee Join employee On offer_employee.PersoNumber=employee.PersoNumber" +
                                                   $" Where Offer = '{of.Id}';");
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
            return offers;
        }

        //---write --------------------------------------------------------------------------------

        public void Create(string title, string description, Skill skill, Field field, ISet<Employee> participants)
        {
            throw new NotImplementedException();
        }

        public void Create(string title, string description)
        {
            errorMessages = new();
            var o = new Offer() { Title = title, Description = description };
            ValidateUpdate(o);
            if (!errorMessages.Any())
            {
                using var con = new SqlConnection(connectionString);
                try
                {
                    con.Open();
                    con.Execute($"Insert Into offer Values ('{o.Title}', '{o.Description}')");
                    con.Close();
                }
                catch (SqlException e)
                {
                    log.LogError($" creating Offer on database: {e.Message} \n");
                }    
                OnChange(new() { SuccesMessage = $"Es wurde ein Objekt erstellt." });
            }
            else
                OnChange(new() { ErrorMessages = errorMessages });
        }

        public void Delete(Offer o)
        {
            if (o != null)
            {
                using var con = new SqlConnection(connectionString);
                con.Open();
                con.Execute("IF EXISTS (SELECT Id " +
                                        "From offer " +
                                        $"Where Id='{o.Id}')" +
                            $"Delete From offer Where Id='{o.Id}';");
                con.Close();
            }
        }

        public void Update(Offer o)
        {
            throw new NotImplementedException();
        }

        public void Add(Offer o, Employee e)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            if (con.Query($"SELECT Offer From offer_employee " +
                          $"Where Offer='{o.Id}' And PersoNumber='{e.PersoNumber}'").Any())
                OnChange(new() { ErrorMessages = new string[] { "EinE MitarbeiterIn mit dieser PersNr existiert bereits in dem Angebot" } });
            else
            {
                con.Execute($"Insert Into offer_employee" +
                            $" Values ('{o.Id}', '{e.PersoNumber}')");
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
            if (con.Query($"SELECT Offer From offer_field Where Offer='{o.Id}' And Field='{f.Name}'").Any())
                OnChange(new() { ErrorMessages = new string[] { "Das Angebot enthält bereits diese Branche" } });
            else
            {
                con.Execute($"Insert Into offer_field  Values ('{o.Id}', '{f.Name}')");
                OnChange(new() { InfoMessages = new string[] {$"Branche zum Angebot {o.Title} (Id: {o.Id}) hinzugefügt" } });
            }
            con.Close();
        }

        public void Add(Offer o, Skill s)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            if (con.Query($"SELECT Offer From offer_skill Where Offer='{o.Id}' And skill_name='{s.Name}' And skill_cat='{s.Category}' And skill_lvl='{s.Level}'").Any())
                OnChange(new() { ErrorMessages = new string[] { "Das Angebot enthält diesen Skill bereits" } });
            else
            {
                con.Execute($"Insert Into offer_field  Values ('{o.Id}', '{s.Name}', '{s.Category}', '{s.Level}')");
                OnChange(new() { InfoMessages = new string[] { $"Skill zum Angebot {o.Title} (Id: {o.Id}) hinzugefügt" } });
            }
            con.Close();
        }

        public void Remove(Offer o, Skill s)
        {
            throw new NotImplementedException();
        }

        public void Remove(Offer o, Field f)
        {
            throw new NotImplementedException();
        }

        public void Remove(Offer o, Employee offerEmp)
        {
            throw new NotImplementedException();
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
                    con.Open();
                    con.Execute($"Insert Into offer Values ('{o.Title}', '{o.Description}')");
                    //Create references to offer_employee, offer_field, offer_skill

                    



                    con.Close();
                }
                catch (SqlException e)
                {
                    log.LogError($" creating Offer on database: {e.Message} \n");
                }
                OnChange(new() { SuccesMessage = $"Die Kopie wurde erstellt." });
            }
            else
                OnChange(new() { ErrorMessages = errorMessages });
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
