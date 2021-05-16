using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team14.Data
{
    public class SkillServiceSimple : ISkillService
    {
        public List<Skill> SkillListe;
        public bool DeleteSkill(int skillId)
        {
            if(SkillListe.Find(skill => skill.skillId == skillId) != null)
            {
                SkillListe.Remove(GetSkill(skillId));
                return true;
            }
            return false;
        }


        public List<Skill> GetAllSkills()
        {
            return SkillListe;
        }

        public Skill GetSkill(int skillId)
        {
            return SkillListe.Find(skill => skill.skillId == skillId);
        }

        public bool UpdateSkill(Skill skill)
        {
            
            
            if((GetSkill(skill.skillId)) != null) {
                return true;
            }else
            {
                return false;
            }

        }
    }
}
