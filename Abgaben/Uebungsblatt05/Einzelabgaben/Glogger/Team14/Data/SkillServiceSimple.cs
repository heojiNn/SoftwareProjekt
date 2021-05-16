using System.Collections.Generic;

namespace Team14.Data
{
    public class SkillServiceSimple : ISkillService
    {
        private readonly List<Skill> skills;
        public SkillServiceSimple(List<Skill> _skills)
        {
            skills = _skills;
        }
        //Gibt einen Skill basierend auf der gewünschten ID zurück oder null, wenn kein Skill existiert
        public Skill GetSkill(int skillId)
        {
            return skills.Find(skill => skill.SkillId == skillId);
        }

        //Gibt alle verfügbaren Skills zurück
        public List<Skill> GetAllSkills()
        {
            return skills;
        }

        //Akutalisiert den gegebenen Skill bei der Datenquelle (Identifikation über die ID) oder fügt Namen ein,
        //wenn die ID noch nicht vorhanden ist
        public bool UpdateSkill(Skill updateSkill)
        {
            if(updateSkill != null) {
                foreach(Skill skill in skills)
                {
                    if(skill.SkillId == updateSkill.SkillId)
                    {
                        skills.Insert(skills.IndexOf(skill), updateSkill);
                        return true;
                    }
                }
                skills.Add(updateSkill);
                return true;
            }
            return false;
        }

        //Löscht einen Skill basierend auf der ID
        public bool DeleteSkill(int skillId)
        {
            if(GetSkill(skillId) != null)
            {
                foreach (Skill skill in skills)
                {
                    if (skill.SkillId != skillId)
                    {
                        continue;
                    }
                    skills.Remove(skill);
                }
                return true;
            }

            return false;
        }
    }
}
