
using System.Collections.Generic;
using Team14.Data;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace Test
{
    class TestSkillNameConventionAttribut
    {
        [TestCase("Selbstorganisation!")]
        [TestCase("Team-Fähigkeit")]
        public void TestInvalidInput(string name)
        {
            var skill = new Skill() { Name = name, Skilltype = Skill.SkillCatgeory.Softskill };
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(skill, new ValidationContext(skill), results, true);
            Assert.False(isValid);

        }
        [TestCase("Selbstorganisation")]
        [TestCase("Teamfähigkeit")]
        [TestCase("Team und Mitarbeiterkoordination")]
        public void TestValidInput(string name)
        {
            var skill = new Skill() { Name = name, Skilltype = Skill.SkillCatgeory.Softskill };
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(skill, new ValidationContext(skill), results, true);
            Assert.True(isValid);
        }

        //[TestCase("Teamfähigkeit")]
        //[TestCase("Viel zu langer Name:.................................................................................................................................")]
        //public void TestLength(string name)
        //{
        //    Assert.Greater(41, name.Length);
        //}
    }
}
