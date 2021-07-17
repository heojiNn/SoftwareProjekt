using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace XCV.Data
{
    public class InsertRandomData
    {
        private readonly ISkillService _skillService;
        private readonly IBasicDataSetService _bDataSetService;
        private readonly IAccountService _accountService;
        private readonly IProfileService _profileService;
        private readonly ILanguageService _languageService;
        private readonly IFieldService _fieldService;
        private readonly IProjectService _projectService;


        public InsertRandomData(IFieldService fieldService,
                                ILanguageService languageService,
                                ISkillService skillService,
                                IBasicDataSetService bDataSetService,
                                IAccountService accountService,
                                IProfileService profileService,
                                IProjectService projectService)
        {
            _fieldService = fieldService;
            _languageService = languageService;
            _skillService = skillService;
            _bDataSetService = bDataSetService;
            _accountService = accountService;
            _profileService = profileService;
            _projectService = projectService;
        }



        public void InsertJson()
        {
            if (!_skillService.GetAllSkills().Any())
            {
                //var currentParent = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
                var content = File.ReadAllText(/*Path.Combine(currentParent, */"datenbasis.json"/*)*/);
                _bDataSetService.JsonUpdate(content, false);
            }
        }
        public void Insert6Employyes()
        {
            if (!_accountService.ShowAllProfiles().Any())
            {
                //hier wird der Registrierungsaccount erstellt
                Employee register = new Employee() { PersoNumber = "999-R", FirstName="Register", LastName="NewAccount", EmployedSince=DateTime.Now };
                register.AcRoles.Add(AccessRole.Register);
                _accountService.CreateAccount(register);
                List<Employee> employyes6 = new()
                {
                    // Wie Christian damals meinte im Allgemeinen sollte das Anstellungsdatum schon vor Profilanlegung geschehen koennen.
                    // Sind aber nur Testdaten
                    new Employee() { PersoNumber = "000", FirstName = "admin", LastName = "admin", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "001", FirstName = "arnold", LastName = "schwarzenegger", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "002", FirstName = "brad", LastName = "pitt", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "003", FirstName = "daniel", LastName = "craig", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "004", FirstName = "linus", LastName = "torvalds", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "005-0", FirstName = "Anaïs", LastName = "Boucher", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "006_0", FirstName = "Aimée", LastName = "Bisset", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "007", FirstName = "Sean", LastName = "Zardoz", EmployedSince = DateTime.Now }, 
                    new Employee() { PersoNumber = "008", FirstName = "Rózsa", LastName = "Péter", EmployedSince = DateTime.Now }, 
                    new Employee() { PersoNumber = "009", FirstName = "Alan", LastName = "Turing", EmployedSince = DateTime.Now }
                };
                foreach (var e in employyes6)
                    e.AcRoles.Add(AccessRole.Employee);
                employyes6[0].AcRoles.Add(AccessRole.Admin);
                employyes6[1].AcRoles.Add(AccessRole.Admin);
                employyes6[1].AcRoles.Add(AccessRole.Sales);
                employyes6[2].AcRoles.Add(AccessRole.Sales);
                employyes6[3].AcRoles.Add(AccessRole.Sales);
                
                
                foreach (var e in employyes6)
                    _accountService.CreateAccount(e);               //creates 6 accounts
                Thread.Sleep(1000);
                foreach (var e in employyes6)
                    UpdateWithRandom(e);               // adds radom rolles fields skills to the 6
                _projectService.Create(new Project
                {
                    Title = "XITASO CV (XCV)",
                    Description = "Es soll ein Tool zur automatisierten Generierung von Mitarbeiterprofilen ausgearbeitet und implementiert werden.",
                    Start = new DateTime(2021, 04, 01), 
                    End = new DateTime(2021, 07, 18),
                    Purpose = new List<string> { "Angebotserstellung", "Kundengewinnung", "Dynamische Teambildung, welche genau den Anforderungen der Kundenprojekte gerecht wird"},
                    Field = _fieldService.GetAllFields().First(x => x.Name == "IT").Name,
                    Activities = new Dictionary<string, (List<string> persNr, IEnumerable<Skill> requirements)>()
                    {
                        { "Anforderungsanalyse",
                            (
                            new List<string>(){
                                employyes6[0].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Analytische Fähigkeiten"),
                                _skillService.GetAllSkills().First(x => x.Name == "Organisationsfähigkeit")
                            } )
                        },
                        { "GUI-Design",
                            (
                            new List<string>(){
                                employyes6[1].PersoNumber, employyes6[2].PersoNumber
                                    },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name ==  "C#"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "CSS"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "HTML 5"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "Innovationsfreudigkeit")
                            } )
                        },
                        { "Persistenz",
                            (
                            new List<string>(){
                                employyes6[1].PersoNumber, employyes6[3].PersoNumber,employyes6[4].PersoNumber }
                            ,
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "C#"),
                                _skillService.GetAllSkills().First(x => x.Name == "SQL"),
                                _skillService.GetAllSkills().First(x => x.Name == "MySQL"),
                                _skillService.GetAllSkills().First(x => x.Name == "Oracle"),
                                _skillService.GetAllSkills().First(x => x.Name == "MS SQL Server"),
                                _skillService.GetAllSkills().First(x => x.Name == "SQLite")
                            } )
                        },
                        { "Review und Testen",
                            (
                            new List<string>(){
                                employyes6[0].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Methodische und strukturierte Vorgehensweise"),
                                _skillService.GetAllSkills().First(x => x.Name == ".NET Core"),
                                _skillService.GetAllSkills().First(x => x.Name == ".NET Framework"),
                                _skillService.GetAllSkills().First(x => x.Name == "C#"),
                                _skillService.GetAllSkills().First(x => x.Name == "SQL")
                            } )
                        },
                        { "ohne Tätigkeit",
                            (
                            new List<string>(){
                                employyes6[0].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Methodische und strukturierte Vorgehensweise")
                            } )
                        },
                        { "Einsatz und Wartung",
                            (
                            new List<string>(){
                                employyes6[0].PersoNumber, employyes6[7].PersoNumber },
                            
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Kommunikationsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "Organisationsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "C#")
                            } )
                        }

                    }
                });

                _projectService.Create(new Project
                {
                    Title = "Universität Augsburg - Redesign",
                    Description = "Die Webseite der Universität Augsburg soll optisch und strukturell erneuert werden. Es wird eine Open-Source-Lösung angestrebt mit einer Vielzahl neuer Funktionen, u. a. mit einem responsiven Design, das sich auch an mobile Endgeräte anpasst. Bei allen Entwicklungen stehen die Bedürfnisse der Besucherinnen und Besucher im Mittelpunkt.",
                    Start = new DateTime(2018, 01, 01),
                    End = new DateTime(2020, 01, 13),
                    Purpose = new List<string> { "Übersichtlichkeit", "Barrierefreiheit", "einfache Informationssuche" , "Anpassung an verschiedene Endgeräte"},
                    Field = _fieldService.GetAllFields().First(x => x.Name == "IT").Name,
                    Activities = new Dictionary<string, (List<string> persNr, IEnumerable<Skill> requirements)>()
                    {
                        { "Anforderungsanalyse",
                            (
                            new List<string>(){
                                employyes6[1].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Beratungsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "Verhandlungsgeschick")
                            } )
                        },
                        { "statische Analyse",
                            (
                            new List<string>(){
                                employyes6[1].PersoNumber, employyes6[2].PersoNumber
                                    },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name ==  "Organisationsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "Ganzheitliches Denken"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "Innovationsfreudigkeit")
                            } )
                        },
                        { "dynamische Analyse",
                            (
                            new List<string>(){
                                employyes6[1].PersoNumber, employyes6[3].PersoNumber,employyes6[4].PersoNumber }
                            ,
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Impulsgeben"),
                                _skillService.GetAllSkills().First(x => x.Name == "Innovationsfreudigkeit")
                            } )
                        },
                        { "Prototyping",
                            (
                            new List<string>(){
                                employyes6[3].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Methodische und strukturierte Vorgehensweise"),
                                _skillService.GetAllSkills().First(x => x.Name == "CSS"),
                                _skillService.GetAllSkills().First(x => x.Name == "HTML 5"),
                                _skillService.GetAllSkills().First(x => x.Name == "Design Management")
                            } )
                        },
                        { "ohne Tätigkeit",
                            (
                            new List<string>(){
                                employyes6[8].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Zielorientiertes Führen")
                            } )
                        },
                        { "Erstellung der Website",
                            (
                            new List<string>(){
                                employyes6[5].PersoNumber, employyes6[6].PersoNumber },

                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Kommunikationsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "Organisationsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "CSS"),
                                _skillService.GetAllSkills().First(x => x.Name == "HTML 5")
                            } )
                        }

                    }
                });

                _projectService.Create(new Project
                {
                    Title = "Universitätsklinikum Augsburg - KIS",
                    Description = "Überarbeitung des Krankenhausinformationssystems.",
                    Start = new DateTime(2019, 01, 01),
                    End = new DateTime(2025, 01, 13),
                    Purpose = new List<string> { "Effiziente Abwicklung der Dokumentation", "Qualitätssicherung der Patientenversorgung", "Zugriff auf Fachwissen"},
                    Field = _fieldService.GetAllFields().First(x => x.Name == "Gesundheit/Soziales/Pflege").Name,
                    Activities = new Dictionary<string, (List<string> persNr, IEnumerable<Skill> requirements)>()
                    {
                        { "Kundenbetreuung und Analyse",
                            (
                            new List<string>(){
                                employyes6[5].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Beratungsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "Verhandlungsgeschick"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "Rhetorik"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "Ganzheitliches Denken"),
                                _skillService.GetAllSkills().First(x => x.Name ==  "Innovationsfreudigkeit")
                            } )
                        },
                        { "GUI-Design",
                            (
                            new List<string>(){
                                employyes6[1].PersoNumber, employyes6[2].PersoNumber
                                    },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name ==  "Angular")
                            } )
                        },
                        { "Persistenz",
                            (
                            new List<string>(){
                                employyes6[5].PersoNumber, employyes6[6].PersoNumber,employyes6[8].PersoNumber }
                            ,
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "MySQL"),
                                _skillService.GetAllSkills().First(x => x.Name == "MS SQL Server"),
                                _skillService.GetAllSkills().First(x => x.Name == "Oracle"),
                                _skillService.GetAllSkills().First(x => x.Name == "AWS Aurora")
                            } )
                        },
                        { "Backend-Entwicklung",
                            (
                            new List<string>(){
                                employyes6[3].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Methodische und strukturierte Vorgehensweise"),
                                _skillService.GetAllSkills().First(x => x.Name == "Problemlösungsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "Mitarbeiterförderung"),
                                _skillService.GetAllSkills().First(x => x.Name == "Java"),
                                _skillService.GetAllSkills().First(x => x.Name == "JavaScript"),
                                _skillService.GetAllSkills().First(x => x.Name == "JQuery")
                            } )
                        },
                        { "ohne Tätigkeit",
                            (
                            new List<string>(){
                                employyes6[6].PersoNumber },
                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Saltstack")
                            } )
                        },
                        { "Einsatz und Wartung",
                            (
                            new List<string>(){
                                employyes6[5].PersoNumber, employyes6[6].PersoNumber },

                            new List<Skill>() {
                                _skillService.GetAllSkills().First(x => x.Name == "Kommunikationsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "Organisationsfähigkeit"),
                                _skillService.GetAllSkills().First(x => x.Name == "MySQL"),
                                _skillService.GetAllSkills().First(x => x.Name == "Java"),
                                _skillService.GetAllSkills().First(x => x.Name == "Beratungsfähigkeit")
                            } )
                        }

                    }
                });

            }
        }

        public void UpdateWithRandom(Employee e)
        {
            var rand = new Random();

            e.RCL = rand.Next(8) + 1;
            if (e.RCL < 4)
                e.Roles.Add(new Role() { Name = "EntwicklerIn" });
            else
                e.Roles.Add(new Role() { Name = "ProjektmanagerIn" });
            if (e.RCL > 4)
                e.Roles.Add(new Role() { Name = "Consultant" });

            var fields = _fieldService.GetAllFields();
            foreach (var field in fields)
                if (rand.Next(6) == 0)
                    e.Fields.Add(field);

            var langs = _languageService.GetAllLanguages();
            var lLvel = _languageService.GetAllLevel();
            foreach (var lang in langs)
                if (rand.Next(5) == 0)
                {
                    var lvl = lLvel[rand.Next(lLvel.Length)];
                    lang.Level = lvl;
                    e.Languages.Add(lang);
                }

            var skills = _skillService.GetAllSkills().GroupBy(x => x.Category.Name);
            var sLvel = _skillService.GetAllLevel();
            foreach (var cat in skills)
                if (rand.Next(3) == 0)      //for a third of the cats
                    foreach (var skill in cat)
                        if (rand.Next(4) == 0)      // adds every 4 skill
                        {
                            var lvl = sLvel[rand.Next(4)];
                            skill.Level = lvl;
                            e.Abilities.Add(skill);
                        }
            var sSkills = _skillService.GetAllSkills().Where(x => x.Type == SkillGroup.Softskill);
            foreach (var s in sSkills)
                if (rand.Next(3) == 0)
                    e.Abilities.Add(s);


            _profileService.UpdateProfile(e);
        }

    }
}
