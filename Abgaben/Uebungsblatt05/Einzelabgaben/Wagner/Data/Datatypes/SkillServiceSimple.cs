using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Team14.Data.Interfaces;


namespace Team14.Data.Datatypes
{
    public class SkillServiceSimple : ISkillService
    {
        public List<Skill> SkillList;

        public bool DeleteSkill(int skillId)
        {
            if (SkillList.Contains(new Skill { Id = skillId }))
            {
                SkillList.Remove(SkillList.Find(s => s.Id == skillId));
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
            try
            {
                if (SkillList.Contains(new Skill { Id = skillId }))
                {
                    return SkillList.Find(s => s.Id == skillId);
                }
                return null;
            }
            catch (NullReferenceException)
            {
                Console.Write("Skill not found");
                return null;
            }

        }

        public bool UpdateSkill(Skill skill)
        {
            if (!SkillList.Contains(skill))
                return false;

            SkillList.Remove(SkillList.Find(s => s.Id == skill.Id));
            SkillList.Add(skill);
            return true;
        }

        private readonly HttpClient httpClient;

    }
}
