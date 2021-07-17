using System.Linq;
using NUnit.Framework;
using XCV.Data;


namespace Tests.Integration
{
    //----------------------Integration Tests for    ISkillService
    [TestFixture]
    public class ISkillService_Test : Initializer
    {
        private SkillService sut;

        [OneTimeSetUp]
        public void GetSut()
        {
            sut = GetSkillService();
        }


        //--------------------------UpdateAllSkills(tree)--------------------------
        //
        // Vorbeding.: keine einziger (Category.Name.Length>50) (Skill.Name.Length>50)
        // Nachbedin.:
        //
        [Test]
        public void UpdateAllTest()
        {
            var newSkillTree = new SkillCategory() { Name = "" };
            var hardSkills = new SkillCategory() { Name = "HardSkills", Parent = newSkillTree };
            newSkillTree.Children.Add(hardSkills);
            var lang = new SkillCategory() { Name = "Sprachen", Parent = hardSkills };
            hardSkills.Children.Add(lang);
            var skill1 = new Skill() { Name = "C", Category = lang };
            var skill2 = new Skill() { Name = "D", Category = lang };
            lang.Children.Add(skill1);
            lang.Children.Add(skill2);

            var softSkills = new SkillCategory() { Name = "SoftSkills", Parent = newSkillTree };
            newSkillTree.Children.Add(softSkills);
            var skill3 = new Skill() { Name = "E", Category = softSkills };
            softSkills.Children.Add(skill3);


            // Act
            (var addedRows, _) = sut.UpdateAllSkills(newSkillTree);

            // Assert
            var requestAgain = sut.GetAllSkills();

            Assert.AreEqual(0, addedRows[0]);
            Assert.AreEqual(1, addedRows[1]);
            Assert.AreEqual(3, requestAgain.Count());

        }
        [TestCase("")]
        [TestCase("123456789-123456789-123456789-123456789-123456789-skill")]
        public void Invalid_Skill(string name)
        {
            // Act
            var skill = new Skill() { Name = name };
            var error = sut.ValidateSkill(new[] { skill });

            // Assert
            Assert.True(error.Any());
        }
        [TestCase("123456789-123456789-123456789-123456789-cat")]
        public void Invalid_Category(string name)
        {
            // Act
            var cat = new SkillCategory() { Name = name };
            var error = sut.ValidateSkillCategory(new[] { cat });

            // Assert
            Assert.True(error.Any());
        }



        //--------------------------UpdateAllLevels(levels)--------------------------
        [Test]
        public void UpdateAllLevels()
        {
            var newLvl = new[] { "ho", "pro", "reg", "ex" };

            // Act
            int changed = GetSkillService().UpdateAllLevels(newLvl);

            // Assert
            var requestAgain = GetSkillService().GetAllLevel();
            Assert.AreEqual(4, changed);
            Assert.AreEqual(newLvl, requestAgain);

            // Act
            changed = GetSkillService().UpdateAllLevels(newLvl);

            // Assert
            Assert.AreEqual(0, changed);
        }


    }
}
