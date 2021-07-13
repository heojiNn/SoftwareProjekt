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
                    new Employee() { PersoNumber = "007", FirstName = "Sean", LastName = "Zardoz", EmployedSince = DateTime.Now }
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
                _projectService.Create("Projekt1");                 // 2 projects
                _projectService.Create("Projekt2");
                _projectService.Create("Projekt3");
                _projectService.Create("Projekt4");
                _projectService.Create("Projekt5");
                _projectService.Create("Projekt6");
                var pros = _projectService.ShowAllProjects();
                foreach (var p in pros)
                {
                    _projectService.Add(p, "Tätigkeit 1");
                    _projectService.Add(p, "Tätigkeit 2");
                }
                Thread.Sleep(1000);
                pros = _projectService.ShowAllProjects();
                var rand = new Random();
                foreach (var p in pros)
                    foreach (var pActi in p.Activities.Keys)
                    {
                        _projectService.Add(p, employyes6[rand.Next(6)], pActi);    // adds 1-3 random employyes to each activity
                        _projectService.Add(p, employyes6[rand.Next(6)], pActi);
                        _projectService.Add(p, employyes6[rand.Next(6)], pActi);
                    }


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
