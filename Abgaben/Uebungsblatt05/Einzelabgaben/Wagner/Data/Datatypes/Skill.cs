using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team14.Data.Datatypes
{
    public class Skill
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public enum SkillTyp {
            Hardskill,
            Softskill,
        }
    }
}






