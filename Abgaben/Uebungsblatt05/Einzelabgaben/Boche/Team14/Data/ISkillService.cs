using System;
using System.Collections.Generic;

namespace Team14.Data
{
    public interface ISkillService
    {
        public Skill GetSkill(int skillId);
        public List<Skill> GetAllSkills();
        public bool UpdateSkill(Skill skill);
        public bool DeleteSkill(int skillId);
    }
}
