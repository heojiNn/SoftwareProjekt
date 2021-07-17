using System.Linq;
using NUnit.Framework;
using XCV.Data;


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
        }


    }
}

