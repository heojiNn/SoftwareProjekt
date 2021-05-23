using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Team14.Data
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [SkillNameConventionAttribut]
        public string Name { get; set; }
        public SkillCatgeory Skilltype { get; set; }

        public enum SkillCatgeory
        {
            Hardskill,
            Softskill,
        }

        public class SkillNameConventionAttribut : ValidationAttribute
        {

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var skill = (Skill)validationContext.ObjectInstance;
                var name = value as string;

                string pattern = @"^[A-Za-zÄäÖöÜü ]+$";
                if (skill.Skilltype == SkillCatgeory.Softskill && !Regex.IsMatch(name, pattern))
                {
                    return new ValidationResult("nur deutsch");
                }

                return ValidationResult.Success;
            }
        }

    }
}
