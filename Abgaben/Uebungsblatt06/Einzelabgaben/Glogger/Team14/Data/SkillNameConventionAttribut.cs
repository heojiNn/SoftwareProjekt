using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;




namespace Team14.Data
{
    public class SkillNameConventionAttribut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var skill = (Skill)validationContext.ObjectInstance;
            string name = (string) value;

            Regex regex = new Regex(@"^[A-Za-z\sÄäÖöÜü]+$");

            if(skill.Skilltype == Skill.SkillCatgeory.Softskill && !regex.IsMatch(name))
                {
                return new ValidationResult("Es sind keine Sonderzeichen erlaubt.");
                }
            return ValidationResult.Success;
        }
    }
}
