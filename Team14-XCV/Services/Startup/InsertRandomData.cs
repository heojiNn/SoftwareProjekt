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
        private readonly IOfferService _offerService;
        private readonly IRoleService _roleService;


        public InsertRandomData(IFieldService fieldService,
                                ILanguageService languageService,
                                ISkillService skillService,
                                IBasicDataSetService bDataSetService,
                                IAccountService accountService,
                                IProfileService profileService,
                                IProjectService projectService,
                                IOfferService offerService,
                                IRoleService roleService)
        {
            _fieldService = fieldService;
            _languageService = languageService;
            _skillService = skillService;
            _bDataSetService = bDataSetService;
            _accountService = accountService;
            _profileService = profileService;
            _projectService = projectService;
            _offerService = offerService;
            _roleService = roleService;
        }



        public void InsertJson()
        {
            if (!_skillService.GetAllSkills().Any())
            {
                var content = File.ReadAllText( "datenbasis.json");
                _bDataSetService.JsonUpdate(content, false);
            }
        }
        public void Insert10Employyes()
        {
            if (!_accountService.ShowAllProfiles().Any())
            {
                //hier wird der Registrierungsaccount erstellt
                Employee register = new Employee() { PersoNumber = "999-R", FirstName="Register", LastName="NewAccount", EmployedSince=DateTime.Now };
                register.AcRoles.Add(AccessRole.Register);
                _accountService.CreateAccount(register);
                List<Employee> employyes10 = new()
                {
                    new Employee() { PersoNumber = "000", FirstName = "Admin", LastName = "admin", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "001", FirstName = "Arnold", LastName = "schwarzenegger", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "002", FirstName = "Brad", LastName = "Pitt", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "003", FirstName = "Daniel", LastName = "Craig", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "004", FirstName = "Linus", LastName = "Torvalds", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "005-0", FirstName = "Anaïs", LastName = "Boucher", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "006_0", FirstName = "Aimée", LastName = "Bisset", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "007", FirstName = "Sean", LastName = "Zardoz", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "008", FirstName = "Hubert", LastName = "Wolfe­schlegel­stein", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "009", FirstName = " Uvuvwevwevwe ", LastName = "onyetenyevwe ugwemuhwem osas", EmployedSince = DateTime.Now },
                    new Employee() { PersoNumber = "010", FirstName = "Rainer", LastName = "Wahnsinn", EmployedSince = DateTime.Now },
                };
                foreach (var e in employyes10)
                    e.AcRoles.Add(AccessRole.Employee);
                employyes10[0].AcRoles.Add(AccessRole.Admin);
                employyes10[1].AcRoles.Add(AccessRole.Admin);
                employyes10[1].AcRoles.Add(AccessRole.Sales);
                employyes10[2].AcRoles.Add(AccessRole.Sales);
                employyes10[3].AcRoles.Add(AccessRole.Sales);


                foreach (var e in employyes10)
                    _accountService.CreateAccount(e);               //creates 10 accounts
                Thread.Sleep(1000);
                foreach (var e in employyes10)
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
                        _projectService.Add(p, employyes10[rand.Next(10)], pActi);    // adds 1-3 random employyes to each activity
                        _projectService.Add(p, employyes10[rand.Next(10)], pActi);
                        _projectService.Add(p, employyes10[rand.Next(10)], pActi);
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

        public void Insert4Offers()
        {
            if (!_offerService.ShowAllOffers().Any())
            {
                var rand = new Random();

                List<Offer> list = new()
                {
                    new() { Title = "Angebot-Microsoft", Description = "Windows 12 entwickeln", Start = DateTime.Now, End = DateTime.Now.AddDays(5)},
                    new() { Title = "Angebot-Apple", Description = "iPhone 30 entwickeln", Start = DateTime.Now, End = DateTime.Now.AddDays(4) },
                    new() { Title = "Angebot-Samsung", Description = "", Start = DateTime.Now, End = DateTime.Now.AddDays(12) },
                    new() { Title = "Angebot-Tesla", Description = "Layer2-solution Blockchain", Start = DateTime.Now, End = DateTime.Now.AddDays(100) }
                };

                var fields = _fieldService.GetAllFields();
                var skills = _skillService.GetAllSkills().GroupBy(x => x.Category.Name);
                var sLvel = _skillService.GetAllLevel();
                foreach (Offer o in list)
                {
                    _offerService.Create(o.Title, o.Description, o.Start, o.End);
                    o.Id = _offerService.GetLastId();

                    if (rand.Next(4) == 0)
                    {
                        foreach (Field f in fields) _offerService.Add(o, f);
                    }
                    foreach (var cat in skills)
                        if (rand.Next(3) == 0)      //for a third of the cats
                            foreach (var skill in cat)
                                if (rand.Next(4) == 0)      // adds every 4 skill
                                {
                                    var lvl = sLvel[rand.Next(4)];
                                    skill.Level = lvl;
                                    o.Requirements.Add(skill);
                                }
                    var sSkills = _skillService.GetAllSkills().Where(x => x.Type == SkillGroup.Softskill);
                    foreach (var s in sSkills)
                        if (rand.Next(3) == 0)
                            o.Requirements.Add(s);
                    foreach (var os in o.Requirements)
                        _offerService.Add(o, os);
                }
            }
        }



    }
}
