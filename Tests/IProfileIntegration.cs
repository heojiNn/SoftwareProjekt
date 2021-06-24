
using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using XCV.Data;


namespace Tests
{
    //----------------------Integration Tests for the   IProfileService / EmployeeService
    [TestFixture]
    public class IProfileIntegration : Initializer
    {
        private EmployeeService sut;
        private ISkillService skillService;


        [OneTimeSetUp]
        public void GetSut()
        {
            var sLogger = Mock.Of<ILogger<SkillService>>();
            var eLogger = Mock.Of<ILogger<EmployeeService>>();

            skillService = new SkillService(GetConfigMoq(), sLogger);

            sut = new EmployeeService(GetEnvMoq(), GetConfigMoq(), eLogger, skillService);
        }



        //aus Kontrakt:
        //   Vorbedingunge: .........
        //   Nachbedingunge:
        //
        [Test]
        public void HardSkillUpdateTest()
        {
            var skills = skillService.GetAllSkills();
            foreach (var skilli in skills)
                Console.WriteLine($"s    {skilli}-{skilli.Category.Name}"); // seen if test fails

            var employee = sut.ShowProfile("000");
            var level = skillService.GetAllLevel()[3];
            var skill = new Skill() { Name = "C", Level = level, Category = new SkillCategory() { Name = "Sprachen" } };

            Assert.False(employee.Abilities.Contains(skill));
            employee.Abilities.Add(skill);
            sut.Update(employee);
            Thread.Sleep(200);
            var newEmployee = sut.ShowProfile("000");
            Assert.True(newEmployee.Abilities.Contains(skill));
        }
        [Test]
        public void HardSkillUpdateFail()
        {
            var employee = sut.ShowProfile("000");
            var level = skillService.GetAllLevel()[3];
            var skill = new Skill() { Name = "C", Level = level, Category = new SkillCategory() { Name = "Sprach" } };

            employee.Abilities.Add(skill);
            sut.Update(employee);
            var newEmployee = sut.ShowProfile("000");
            Assert.False(newEmployee.Abilities.Contains(skill));
        }



        //aus Kontrakt:
        //   Vorbedingunge: .........
        //   Nachbedingunge:
        //
        [Test]
        public void CreateTest()
        {
            sut.CreateAccount(new Employee() { PersoNumber = "11", FirstName = "Test", LastName = "Tester" });

            Assert.True(sut.ShowProfile("11").LastName == "Tester");
        }

    }

}

