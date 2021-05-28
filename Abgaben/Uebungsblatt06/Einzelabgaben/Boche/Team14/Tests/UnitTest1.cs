using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using Team14.Data;

namespace Tests
{
    public class UnitTest1
    {
        [SetUp]
        public void Setup()
        {
        }


        [TestCase("1")]
        [TestCase("\\")]
        [TestCase("__")]
        public void TestF(string name)
        {
            var targetModel = new Skill() { Skilltype = Skill.SkillCatgeory.Softskill, Name = name };
            var results = new List<ValidationResult>();
            var result = Validator.TryValidateObject(targetModel, new ValidationContext(targetModel), results, true);

            Assert.False(result, string.Join(", ", results.Select(e => e.ErrorMessage)));
        }

        [TestCase("Äa")]
        [TestCase("A  b")]
        [TestCase("abcdef")]
        public void TestG(string name)
        {
            var targetModel = new Skill() { Skilltype = Skill.SkillCatgeory.Softskill, Name = name };
            var results = new List<ValidationResult>();
            var result = Validator.TryValidateObject(targetModel, new ValidationContext(targetModel), results, true);

            Assert.True(result);
        }
    }
}
