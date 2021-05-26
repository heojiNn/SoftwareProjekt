using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Team14.Data
{
    public class Skill
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Der Name darf nur aus maximal 50 Zeichen bestehen.")]
        public string Name { get; set; }

        public SkillCategory Skilltype { get; set; }
        public enum SkillCategory
        {
            Hardskill,
            Softskill
        }
    }
}
