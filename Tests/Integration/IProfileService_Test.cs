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

        private Employee toBeUpdated;

        [OneTimeSetUp]
        public void GetSut()
        {
            skillService = GetSkillService();

            sut = GetEmployeeService();
            toBeUpdated = sut.ShowAllProfiles().First();
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
            var skills = skillService.GetAllSkills();
            var levels = skillService.GetAllLevel();

            // PreCondition
            var skill1 = skills.First();
            skill1.Level = levels[2];
            var skill2 = skills.Last();
            skill2.Level = levels[3];

            // Act
            toBeUpdated.Abilities.Add(skill1);
            toBeUpdated.Abilities.Add(skill2);
            sut.UpdateProfile(toBeUpdated);

            // Assert
            var requestAgain = sut.ShowProfile(toBeUpdated.PersoNumber);

            Assert.NotNull(requestAgain);
            Assert.Contains(skill1, requestAgain.Abilities.ToList(), "Skill1 wasn't updated ");
            Assert.Contains(skill2, requestAgain.Abilities.ToList(), "Skill2 wasn't updated");
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

