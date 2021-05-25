using System;
using System.Collections.Generic;
using System.Linq;

namespace Team14.Data
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string Name { get; set; }

        public SkillType Type { get; set; }
        public enum SkillType
        {
            Hardskill,
            Softskill
        }
    }
}
