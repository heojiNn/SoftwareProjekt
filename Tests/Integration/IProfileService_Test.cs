using System.Linq;
using NUnit.Framework;
using XCV.Data;
using System;

namespace Tests.Integration
{
    //----------------------Integration Tests for    IProfileService
    [TestFixture]
    public class IProfileService_Test : Initializer
    {
        private IProfileService sut;
        private ISkillService skillService;
        private IRoleService roleService;
        private ChangeResult changeRes;
        private void OnEventReturn(object sender, ChangeResult e) => changeRes = e;
        private IProfileService profileService;
        private IAccountService accountService;
        private EmployeeService employeeService;
        private Employee toBeUpdated;

        [OneTimeSetUp]
        public void GetSut()
        {
            skillService = GetSkillService();
            roleService = GetRoleService();
            employeeService = GetEmployeeService();
            sut = GetEmployeeService();
            toBeUpdated = sut.ShowAllProfiles().First();
            sut.ChangeEventHandel += OnEventReturn;
        }



        //--------------------------UpdateProfile(fields)--------------------------
        //
        //   Vorbedingungen: .PersoNumber==(noch frei)   .PersoNumber==(2-6 Zeichen   [a-zA-Z0-9_\-,.]*)
        //                   .FirstName==(1-20 Zeichen)   .LastName==(1-20 Zeichen)
        //
        //   Nachbedingungen: .PersoNumber .Password .FirstName .LastName  .AcRoles
        //                      1-zu-1 gespeichert
        [Test]
        [Order(4)]
        public void UpdateAddTest()
        {
            //employeeService.CreateAccount(new Employee() { PersoNumber = "001", FirstName="Test", LastName="nach", Password="001" });
            Employee toBeUpdated2 = sut.ShowAllProfiles().ToArray()[0];
            var skills = skillService.GetAllSkills();
            var levels = skillService.GetAllLevel();
            var roles = roleService.GetAllRoles();
            
            // PreCondition
            var skill = skills.Take(1).ToArray()[0];
            skill.Level = levels[2];
            var role = roles.Take(1).ToArray()[0];
            string firstname = "FirstName";
            string lastname = "lastname";

            // Act
            toBeUpdated2.Abilities.Add(skill);
            toBeUpdated2.Roles.Add(role);
            toBeUpdated2.FirstName = firstname;
            toBeUpdated2.LastName = lastname;
            
            sut.UpdateProfile(toBeUpdated2);
            //Assert.True(1 == 0, changeRes.ErrorMessages.First());
            // Assert
            var requestAgain = sut.ShowProfile(toBeUpdated2.PersoNumber);

            Assert.NotNull(requestAgain);
            //Assert.True(1 == 0,requestAgain.FirstName);
            Assert.Contains(skill, requestAgain.Abilities.ToList(), "Skill wasn't updated");
            Assert.Contains(role, requestAgain.Roles.ToList(), "Role wasn't updated");
            Assert.True(firstname == requestAgain.FirstName, "FirstName wasn't updated");
            Assert.True(lastname == requestAgain.LastName, "LastName wasn't updated");
        }


        [Test]
        [Order(3)]
        public void InvalidUpdateAddTest()
        {
            var employee = sut.ShowProfile("000");  //should have been created by Initializer.cs

            var level = skillService.GetAllLevel()[3];
            var skill = new Skill() { Name = "C", Level = level, Category = new SkillCategory() { Name = "Sprach" } };
            employee.Abilities.Add(skill);

            // Act
            sut.UpdateProfile(employee);

            // Assert
            var newEmployee = sut.ShowProfile("000");
            Assert.False(newEmployee.Abilities.Contains(skill));

            string firstname = new string('a', 41);
            employee.FirstName = firstname;
            sut.UpdateProfile(employee);
            var newEmployee1 = sut.ShowProfile("000");
            Assert.AreNotEqual(newEmployee1.FirstName, firstname, "FirstName hat Constrains nicht eingehalten");

            string lastname = new string('a',41);
            employee.LastName = lastname;
            sut.UpdateProfile(employee);
            var newEmployee2 = sut.ShowProfile("000");
            Assert.AreNotEqual(newEmployee2.LastName, lastname, "LastName hat Constrains nicht eingehalten");

            var role = new Role() { Name = new string('a', 51) };
            employee.Roles.Add(role);
            sut.UpdateProfile(employee);
            var newEmployee3 = sut.ShowProfile("000");
            Assert.IsEmpty(newEmployee3.Roles.Where(r => r.Name == role.Name), "Rollen hat Constrains nicht eingehalten");

        }

        [Test]
        [Order(1)]
        public void CreateEmployeeTest()
        {
            var employed = DateTime.Now;
            employeeService.CreateAccount(new Employee() { PersoNumber = "005", FirstName = "Nummer", LastName = "Fuenf", EmployedSince = employed });

            Employee employee = sut.ShowProfile("005");

            Assert.IsNotNull(employee, "Employee wurde nicht erstellt");
            Assert.AreEqual(employee.FirstName, "Nummer", "Employee.FirstName wurde nicht erstellt");
            Assert.AreEqual(employee.LastName, "Fuenf", "Employee.LastName wurde nicht erstellt");
            Assert.AreEqual(employee.EmployedSince.ToString("dd.MM.yyyy"), employed.ToString("dd.MM.yyyy"), "Employee.EmployedSince wurde nicht erstellt");
        }

        [Test]
        [Order(5)]
        public void InvalidCreateEmployeeTest()
        {
            var employed = DateTime.Now;
            employeeService.CreateAccount(new Employee() { PersoNumber = "0", FirstName = "Nummer", LastName = "Fuenf", EmployedSince = employed });
            employeeService.CreateAccount(new Employee() { PersoNumber = "0000005", FirstName = "Nummer", LastName = "Fuenf", EmployedSince = employed });
            employeeService.CreateAccount(new Employee() { PersoNumber = "005?", FirstName = "Nummer", LastName = "Fuenf", EmployedSince = employed });
            employeeService.CreateAccount(new Employee() { PersoNumber = "005", FirstName = new string('c',41), LastName = "Fuenf", EmployedSince = employed });
            employeeService.CreateAccount(new Employee() { PersoNumber = "006", FirstName = "Nummer", LastName = new string('c',41), EmployedSince = employed });

            Employee employee1 = sut.ShowProfile("00");
            Employee employee2 = sut.ShowProfile("0000005");
            Employee employee3 = sut.ShowProfile("005?");
            Employee employee4 = sut.ShowProfile("005");
            Employee employee5 = sut.ShowProfile("006");

            Assert.Null(employee1, "PersoNummer > 1 chr. constraint fehlgeschlagen");
            Assert.Null(employee2, "PersoNummer < 7 chr. constraint fehlgeschlagen");
            Assert.Null(employee3, "PersoNummer aus [a-zA-Z0-9_\\-,.]* constraint fehlgeschlagen");
            Assert.Null(employee4, "FirstName < 40 chr. constraint fehlgeschlagen");
            Assert.Null(employee5, "LastName < 40 chr. constraint fehlgeschlagen");
        }

        [Test]
        [Order(2)]
        public void DeleteEmployeeTest()
        {
            employeeService.DeleteAccount("005");

            Employee employee = sut.ShowProfile("005");

            Assert.IsNull(employee, "Employee wurde nicht geloescht");
        }
    }
}

