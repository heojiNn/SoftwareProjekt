using System;
using System.Collections.Generic;

namespace Team14.Data
{
    public class SkillServiceSimple : ISkillService
    {
        public SkillServiceSimple()
        {
        }

        public List<Skill> SkillList;

        public bool DeleteSkill(int skillId)
        {
            if (SkillList.Find(skill => skill.SkillId == skillId) != null)
            {
                SkillList.Remove(GetSkill(skillId));
                return true;
            }
            return false;
        }

        public List<Skill> GetAllSkills()
        {
            return SkillList;
        }

        public Skill GetSkill(int skillId)
        {
            return SkillList.Find(skill => skill.SkillId == skillId);
        }

        public bool UpdateSkill(Skill skill)
        {

            if ((GetSkill(skill.SkillId)) != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }      
    }
}
