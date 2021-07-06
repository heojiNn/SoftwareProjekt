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

        public OfferService(IConfiguration config, ILogger<OfferService> logger, ISkillService skillService)
        {
            log = logger;
            ofSkillService = skillService;
            connectionString = config.GetConnectionString("MS_SQL_Connection");
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
            var offers = con.Query<Offer>("Select * From [offer]");
            con.Close();
            foreach (var of in offers)
            {
                var fields = con.Query<Field>("Select [Field] as Name " +
                                                $"From [offerHasField] Where [Offer] = {of.Id}");
                foreach (var field in fields)
                    of.Fields.Add(field);

                var skills = con.Query<(string Name, string Lvl, string Cat)>("Select offerHasSkill.skill_name as Name, skill_level.Name as Lvl, offerHasSkill.skill_cat as Cat " +
                                                                                "From [offerHasSkill]  " +
                                                                                    "Join [skill_level] On  offerHasSkill.skill_level=skill_level.level " +
                                                                                    $"Where [Offer] = {of.Id};");
                foreach (var (Name, Lvl, Cat) in skills)
                {
                    of.Requirements.Add(new Skill() { Name = Name, Level = Lvl, Category = new SkillCategory() { Name = Cat } });
                    ofSkillService.HangThemOnATree(of.Requirements);
                }


                // TODO
                //offerHasEmployee's RCL may vary in the specific offer compared to his profile. ("angebotsspezifisch angepasstes RC Level")
                //Other then that the employee within the offer should be equal to the employee in table "employee" (PersoNumber references that attribute)

                var employees = con.Query<Employee>(" Select employee.PersoNumber as PersoNumber, employee.FirstName as FirstName, employee.LastName as LastName," +
                                                    " employee.Description as Description, employee.Image as Image, employee.RCL as RCL, employee.Expirience as Expirience, employee.EmployedSince as WorkingSince, employee.MadeFirstChangesOnProfile as MadeFirstChangesOnProfile" +
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
            return offers;
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
                    con.Execute($"Insert Into [offer] Values ('{o.Title}', '{o.Description}')");
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
                con.Execute($"Update [offer] Set  [Title]=@Title,  [Description]=@Description Where [Id]= @ID", o);
                //Update Fields
                con.Execute($"Delete From [offerHasField] Where Offer={o.Id}");
                foreach (var fld in o.Fields) 
                    con.Execute($"Insert Into [offerHasField] Values (@Id, @Name);", new { o.Id, fld.Name});

                    //con.Execute($"Insert Into [offerHasField] Values (@Id, @fld) ", new { o.Id, fld.Name });
                //Update Employees
                //con.Execute($"Delete From [offerHasEmployee] Where Offer={o.Id}");
                //foreach (var emp in o.participants) 
                    //con.Execute($"Insert Into [offerHasEmployee] Values ({o.Id}, {emp.PersoNumber});");
                    //con.Execute($"Insert Into [offerHasEmployee] Values (@Id, @fld) ", new { o.Id, emp });

                OnChange(new() { SuccesMessage = "Die Änderungen am Angebot wurden übernommen" });
            }
            catch (SqlException e)
            {
                log.LogError($" Update() persistence Error:: \n{e.Message}\n");
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
                            $" Values ({o.Id}, '{e.PersoNumber}', NULL, NULL)");
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
            con.Open();
            if (con.Query($"SELECT [Offer] From [offerHasSkill] Where [Offer]={o.Id} And [skill_name]='{s.Name}' And [skill_cat]='{s.Category}' And [skill_level]={s.Level}").Any())
                OnChange(new() { ErrorMessages = new string[] { "Das Angebot enthält diesen Skill bereits" } });
            else
            {
                con.Execute($"Insert Into [offerHasSkill]  Values ({o.Id}, '{s.Name}', '{s.Category}', {s.Level})");
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
                    con.Execute($"Insert Into [offer] Values ('{o.Title}', '{o.Description}')");
                    id = GetLastId();
                    //Create references to offerHasEmployee - TODO Roles and wages
                    if (o.participants.Count() != 0)
                    {
                        foreach (Employee e in o.participants)
                            con.Execute($"Insert Into [offerHasEmployee] Values ({id}, '{e.PersoNumber}', NULL, NULL)");
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
                            con.Execute("Insert Into [offerHasSkill] Values (@Offer, @Skill, @Cat, @Level, @E)", new { id, Skill = skill.Name, Cat = skill.Category.Name, level});
                        }
                    }
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
//DBCC CHECKIDENT('[TestTable]', RESEED, 0);
//GO
