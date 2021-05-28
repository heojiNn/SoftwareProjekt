using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static Team14.Data.Skill;

namespace Team14.Data
{
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var skill = (Skill)value;
            if (skill.Kategorisierung.Equals(Category.Hardskill)) return true;
            return NameFormatMatch(skill.Name); //if SkillCatgeory is Softskill check the Name
        }

        private bool NameFormatMatch(string str)
        {
            return new Regex(@"^[A-Z\u00c4\u00e4\u00d6\u00f6\u00dc\u00fc\u00df\s\u0021]+$").IsMatch(str);
        }
    }
}
