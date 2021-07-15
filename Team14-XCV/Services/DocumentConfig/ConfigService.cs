using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;



namespace XCV.Data
{
    public class ConfigService : IConfigService
    {
        private readonly string connectionString;
        private readonly ILogger<ConfigService> log;
        private readonly IProfileService cProfileService;
        private readonly IProjectService cProjectService;
        private readonly ISkillService cSkillService;

        public ConfigService(IConfiguration config, ILogger<ConfigService> logger, ISkillService skillService, IProfileService profileService, IProjectService projectService)
        {
            connectionString = config.GetConnectionString("MS_SQL_Connection");
            log = logger;
            cProfileService = profileService;
            cSkillService = skillService;
            cProjectService = projectService;

        }

        //-----------------------------------------------------------------------------------------
        //---------------------------------Business Logic------------------------------------------

        public void ValidateUpdate(Offer o, DocumentConfig newVersion, EmployeeConfig opt)
        {
            errorMessages = new();
            infoMessages = new();
            inlineWords = new();

            //-------------------------------------------------------------------------------------errorMessages
            // adds the Model-Validation to the List of Errors
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (errorMessages.Count == 0)
                infoMessages.Add("Die Änderung war erfolgreich.");
            //-------------------------------------------------------------------------------------

            OnChange(new()
            {
                InfoMessages = infoMessages,
                ErrorMessages = errorMessages
            });
        }

        public void ValidateCreate(Offer o, DocumentConfig newVersion)
        {
            errorMessages = new();
            infoMessages = new();
            inlineWords = new();

            //-------------------------------------------------------------------------------------errorMessages
            // adds the Model-Validation to the List of Errors
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(newVersion, new ValidationContext(newVersion), results, true))
                errorMessages = results.Select(e => e.ErrorMessage).ToList();

            if (newVersion == null)
                errorMessages.Add("Es wurde keine Referenz erzeugt.");
            if (GetAllDocumentConfigs(o).Where(x => x.Name == newVersion.Name).Any())
                errorMessages.Add("Der Name der Dokumentenkonfiguration muss einzigartig sein - keine Duplikate.");


            if (errorMessages.Count == 0)
                infoMessages.Add("Die Erzeugung war erfolgreich.");
            //-------------------------------------------------------------------------------------

            OnChange(new()
            {
                InfoMessages = infoMessages,
                ErrorMessages = errorMessages
            });
        }

        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //--read   -------------------------------------------------------------------------------

        public DocumentConfig GetDocumentConfig(Offer o, string name)
        {
            return GetAllDocumentConfigs(o).FirstOrDefault(x => x.Name == name);
        }

        public IEnumerable<DocumentConfig> GetAllDocumentConfigs(Offer o)
        {
            errorMessages = new();
            using var con = new SqlConnection(connectionString);
            con.Open();
            IEnumerable<DocumentConfig> configs = con.Query<DocumentConfig>($"Select [Offer] as offerId, [Config] as Name From [offerHasConfig] Where [Offer] = {o.Id}");
            con.Close();
            if (!errorMessages.Any())
            {
                con.Open();
                try
                {
                    foreach (DocumentConfig cfg in configs)
                    {
                        cfg.employeeConfigs = new List<EmployeeConfig>();
                        var persNrs = con.Query<string>($"Select [Employee] From [config] Where [Offer] = {o.Id} And [Name] = '{cfg.Name}'"); // All PersNrs in the current Config
                        foreach (string pnr in persNrs) //Reconstruct the EmployeeConfigs from Database
                        {
                            #nullable enable
                            //config
                            string? FirstName = con.QuerySingleOrDefault<string>($"Select [FirstName] From [config] Where [Offer] = {o.Id} And [Name] = '{cfg.Name}' And [Employee] = '{pnr}'");
                            string? LastName = con.QuerySingleOrDefault<string>($"Select [LastName] From [config] Where [Offer] = {o.Id} And [Name] = '{cfg.Name}' And [Employee] = '{pnr}'");
                            string? Description = con.QuerySingleOrDefault<string>($"Select [Description] From [config] Where [Offer] = {o.Id} And [Name] = '{cfg.Name}' And [Employee] = '{pnr}'");
                            string? Image = con.QuerySingleOrDefault<string>($"Select [Image] From [config] Where [Offer] = {o.Id} And [Name] = '{cfg.Name}' And [Employee] = '{pnr}'");
                            DateTime? Experience = con.QuerySingleOrDefault<DateTime?>($"Select [Experience] From [config] Where [Offer] = {o.Id} And [Name] = '{cfg.Name}' And [Employee] = '{pnr}'");
                            DateTime? EmployedSince = con.QuerySingleOrDefault<DateTime?>($"Select [EmployedSince] From [config] Where [Offer] = {o.Id} And [Name] = '{cfg.Name}' And [Employee] = '{pnr}'");

                            //configHasField
                            IEnumerable<string>? fieldsAsString = con.Query<string>($"Select [Field] From [configHasField] Where [Offer] = {o.Id} And [Config] = '{cfg.Name}' And [cfgEmployee] = '{pnr}'");
                            ISet<Field> fields = new HashSet<Field>();
                            foreach (string f in fieldsAsString)
                                fields.Add(new Field() { Name = f });
                            #nullable disable

                            //configHasSkill
                            var skills = con.Query<Skill, string, Skill>(@$"Select s.Skill as Name,  sl.Name as Level,  s.Skill_Cat as Category
                                                                        From [configHasSkill] s  Left Join [Skill_Level] sl   ON s.Skill_Level = sl.Level
                                                                    Where [Offer] = {o.Id} And [Config] = '{cfg.Name}' And [cfgEmployee] = '{pnr}'",
                                                                            (skill, category) =>
                                                                            {
                                                                                if (skill != null)
                                                                                    skill.Category = new SkillCategory() { Name = category };
                                                                                return skill;
                                                                            }, splitOn: "Category").ToHashSet();
                            //configHasActivity
                            var proAndAct = con.Query<(int project, string activity)>($"Select [Project], [Activity] From [configHasActivity] Where [Offer] = {o.Id} And [Config] = '{cfg.Name}' And [cfgEmployee] = '{pnr}'");

                            IList<(int project, string activity)> projects = new List<(int project, string activity)>();
                            foreach (var paA in proAndAct)
                            {
                                Project temp = cProjectService.ShowProject(paA.project);
                                projects.Add((temp.Id, temp.Title));
                            }

                            //if (fieldsAsString.Count() == 0) Console.Write($"COnfig: KEine Felder {cfg.Name} {pnr}");
                            //if (skills.Where(s => s.Type == SkillGroup.Softskill).ToHashSet().Count() == 0) Console.Write("COnfig: KEine Softskills" );
                            //if (skills.Where(s => s.Type == SkillGroup.Hardskill).ToHashSet().Count() == 0) Console.Write($"COnfig: KEine Hardskills {cfg.Name} {pnr}");
                            //if (proAndAct.Count() == 0 || proAndAct == null) Console.Write($"COnfig: KEine Projekte! {cfg.Name} {pnr}");

                            EmployeeConfig employeeConfig = new EmployeeConfig
                            {
                                PersNr = pnr,
                                FirstName = FirstName ?? null,
                                LastName = LastName ?? null,
                                Description = Description ?? null,
                                Image = Image ?? null,
                                Experience = Experience ?? null,
                                EmployedSince = EmployedSince ?? null,
                                selectedFields = fields,
                                selectedSoftSkills = skills.Where(s => s.Type == SkillGroup.Softskill).ToHashSet(),
                                selectedHardSkills = skills.Where(s => s.Type == SkillGroup.Hardskill).ToHashSet(),
                                selectedProjects = projects,
                            };


                            cfg.employeeConfigs.Add(employeeConfig);
                        }
                    }
                } catch (Exception e)
                {
                    log.LogError($"Error getting Config from database: {e.Message} \n");
                }
                con.Close();
            }
            return configs;
        }

        public DocumentConfig GetSelectedConfig(Offer o)
        {
            errorMessages = new();
            using var con = new SqlConnection(connectionString);
            con.Open();
            var configs = con.Query<(int, string)>("Select * From [offerHasActiveConfig]");
            con.Close();
            try
            {
                con.Open();
                foreach (var item in configs)
                {
                    if (item.Item1 == o.Id)
                        return GetDocumentConfig(o, item.Item2);
                }
                con.Close();

                return null;
            } catch (Exception e)
            {
                log.LogError($"Error returning selected Config from database: {e.Message} \n");
                return null;
            }
        }

        
        //---write --------------------------------------------------------------------------------


        public void DeleteDocumentConfig(Offer parent, DocumentConfig cfg)
        {
            if (parent != null && cfg != null)
            {
                try
                {
                    using var con = new SqlConnection(connectionString);
                    con.Open();
                    con.Execute($"IF EXISTS (SELECT [Config] From [offerHasConfig] Where [Offer]={parent.Id} And [Config]='{cfg.Name}') DELETE FROM [offerHasConfig] Where [Config] = '{cfg.Name}';");
                    con.Close();
                }
                catch (Exception e)
                {
                    log.LogError($"Error deleting Config in database: {e.Message} \n");
                }
            }
        }

        public void UpdateDocumentConfig(DocumentConfig cfg)
        {
            // change name , change employees, change etc.
            throw new NotImplementedException();
        }

        public void UpdateEmployeeConfig(Offer o, DocumentConfig c, string persnr, EmployeeConfig cfg)
        {
            errorMessages = new();
            ValidateUpdate(o, c, cfg);
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return;
            }
            using var con = new SqlConnection(connectionString);
            try
            {   // Null: Unchecked on page || Not Null: Checked on page
                con.Open();

                //Profileinformation
                if (cfg.FirstName == null)
                    con.Execute($"Update [config] Set [FirstName] = NULL Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");
                else
                    con.Execute($"Update [config] Set [FirstName] = '{cfg.FirstName}' Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");

                if (cfg.LastName == null)
                    con.Execute($"Update [config] Set [LastName] = NULL Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");
                else
                    con.Execute($"Update [config] Set [LastName] = '{cfg.LastName}' Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");

                if (cfg.Description == null)
                    con.Execute($"Update [config] Set [Description] = NULL Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");
                else
                    con.Execute($"Update [config] Set [Description] = '{cfg.Description}' Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");

                if (cfg.Image == null)
                    con.Execute($"Update [config] Set [Image] = NULL Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");
                else
                    con.Execute($"Update [config] Set [Image] = '{cfg.Image}' Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");

                if (cfg.Experience == null)
                    con.Execute($"Update [config] Set [Experience] = NULL Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");
                else
                    con.Execute($"Update [config] Set [Experience] = @Experience Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';", new { Experience = cfg.Experience });

                if (cfg.EmployedSince == null)
                    con.Execute($"Update [config] Set [EmployedSince] = NULL Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';");
                else
                    con.Execute($"Update [config] Set [EmployedSince] = @EmployedSince Where [Offer] = {o.Id} And [Name] = '{c.Name}' And [Employee] = '{persnr}';", new { EmployedSince = cfg.EmployedSince });

                //Fields, Softskills, Hardskills, Projects&activities
                if (cfg.selectedFields == null)
                {
                    con.Execute($"Delete From [configHasField] Where [Offer] = {o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}'");
                } else
                {
                    con.Execute($"Delete From [configHasField] Where [Offer] = {o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}'");
                    foreach (var fld in cfg.selectedFields)
                        con.Execute($"Insert Into [configHasField] Values ({o.Id}, '{c.Name}', '{persnr}', '{fld.Name}')");
                }
                
                if (cfg.selectedSoftSkills == null)
                {
                    con.Execute($"Delete From [configHasSkill] Where [Offer]={o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}' And [Skill_Cat] = 'SoftSkills';");
                } else
                {
                    con.Execute($"Delete From [configHasSkill] Where [Offer]={o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}' And [Skill_Cat] = 'SoftSkills';");
                    foreach (var sk in cfg.selectedSoftSkills.Where(x => x.Type == SkillGroup.Softskill))
                        con.Execute($"Insert Into [configHasSkill] Values ({o.Id}, '{c.Name}', '{persnr}', '{sk.Name}', 'SoftSkills', NULL)");
                }
                
                if (cfg.selectedHardSkills == null)
                {
                    con.Execute($"Delete From [configHasSkill] Where [Offer] = {o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}' And [Skill_Cat] <> 'SoftSkills';");
                }
                else
                {
                    con.Execute($"Delete From [configHasSkill] Where [Offer] = {o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}' And [Skill_Cat] <> 'SoftSkills';");
                    foreach (var sk in cfg.selectedHardSkills.Where(x => x.Type == SkillGroup.Hardskill))
                        con.Execute($"Insert Into [configHasSkill] Values ({o.Id}, '{c.Name}', '{persnr}', '{sk.Name}', '{sk.Category.Name}', {sk.Level});");
                }

                if (cfg.selectedProjects == null)
                {
                    con.Execute($"Delete From [configHasActivity] Where [Offer] = {o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}';");
                }
                else
                {
                    con.Execute($"Delete From [configHasActivity] Where [Offer] = {o.Id} And [Config] = '{c.Name}' And [cfgEmployee] = '{persnr}';");
                    foreach (var pro in cfg.selectedProjects)
                        con.Execute($"Insert Into [configHasActivity] Values ({o.Id}, '{c.Name}', '{persnr}', {pro.project}, '{pro.activity}')");
                }

                OnChange(new() { SuccesMessage = "Die Änderungen an der Konfiguration wurden übernommen" });
            }
            catch (SqlException e)
            {
                log.LogError($"Error updating Employeeconfig: \n{e.Message}\n");
            }
            finally { con.Close(); }
        }

        public DocumentConfig CreateDefaultDocumentConfig(Offer parent, string name)
        {
            DocumentConfig defaultcfg = new DocumentConfig() { offerId = parent.Id, Name = name, employeeConfigs = new List<EmployeeConfig>()};
            if (parent.participants.Count == 0)
                return defaultcfg; // Kann in weiterer Fehlerbehandlung abgehandelt werden. -> ValidateUpdate
            errorMessages = new();
            ValidateUpdate(parent, defaultcfg, null);
            if (!errorMessages.Any())
            {
                using var con = new SqlConnection(connectionString);
                try
                {
                    con.Open();
                    con.Execute($"Insert Into [offerHasConfig] Values ({parent.Id}, '{name}')");
                    foreach (Employee e in parent.participants)
                    {
                        Employee emp = cProfileService.ShowProfile(e.PersoNumber);
                        con.Execute($"Insert Into [config] Values (@Offer, @Name, @PersoNumber, @FirstName , @LastName, @Description, @Image, @Experience, @EmployedSince)",
                            new { Offer = parent.Id, Name = name, PersoNumber = emp.PersoNumber, FirstName = emp.FirstName, LastName = emp.LastName, Description = emp.Description, Image = emp.Image,
                            Experience = emp.Experience, EmployedSince = emp.EmployedSince}); 
                        foreach (Field f in emp.Fields)
                            con.Execute($"Insert Into [configHasField] Values ({parent.Id}, '{name}', '{emp.PersoNumber}', '{f.Name}')");
                        foreach (Skill s in emp.Abilities)
                        {
                            int? level = Array.FindIndex(cSkillService.GetAllLevel(), x => x == s.Level) + 1;
                            if (s.Type == SkillGroup.Softskill)
                                level = null;
                            con.Execute($"Insert Into [configHasSkill] Values ({parent.Id}, @Name, @Employee, @Skill, @Cat, @Level)", new { Name = name, Employee = emp.PersoNumber, Skill = s.Name, Cat = s.Category.Name, level });
                        }
                        foreach (var p in emp.Projects)
                        {
                            con.Execute($"Insert Into [configHasActivity] Values ({parent.Id}, @Name, @Employee, @Project, @Activity)", new { Name = name, Employee = emp.PersoNumber, Project = p.project, Activity = p.activity });

                        }
                    }
                    con.Close();
                }
                catch (SqlException e)
                {
                    log.LogError($"Error creating default config on database: {e.Message} \n");
                }
            }
            return defaultcfg;
        }


        public void SaveSelectedConfig(Offer o, DocumentConfig cfg)
        {
            if (o != null && cfg != null)
            {
                using var con = new SqlConnection(connectionString);
                try
                {
                    con.Open();
                    con.Execute($"Delete From [offerHasActiveConfig] Where [Offer] = {o.Id}");
                    con.Execute($"Insert Into [offerHasActiveConfig] Values ({o.Id}, '{cfg.Name}')");
                    OnChange(new() { InfoMessages = new string[] { $"Neue Konfiguration aktiviert." } });
                    con.Close();
                } catch (SqlException e)
                {
                    log.LogError($"Error creating default config on database: {e.Message} \n");
                }
            }
            else
                return;
        }

        public void DeleteSelectedConfig(Offer o, DocumentConfig cfg)
        {
            errorMessages = new();
            using var con = new SqlConnection(connectionString);
            con.Open();
            var configs = con.Query<(int, string)>("Select * From [offerHasActiveConfig]");
            con.Close();
            try
            {
                con.Open();
                con.Execute($"Delete From [offerHasActiveConfig] Where [Offer] = {o.Id}");
                con.Close();
            }
            catch (Exception e)
            {
                log.LogError($"Error deleting selected Config from database: {e.Message} \n");
            }
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
