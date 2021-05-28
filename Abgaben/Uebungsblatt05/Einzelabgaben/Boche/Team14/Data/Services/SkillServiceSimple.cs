using System;
using System.Linq;
using System.Collections.Generic;

using Team14.Interfaces;

namespace Team14.Data
{
    public class SkillServiceSimple : ISkillService
    {
        private readonly Stack<Skill> theSingleton;
        public SkillServiceSimple(Stack<Skill> allSkillsForeverSingleton)
        {
            theSingleton = allSkillsForeverSingleton;
        }

        //Gibt einen Skill basierend auf der gewuenschten ID zurueck
        //oder null,wenn kein Skill existiert
        public Skill GetSkill(int skillId)
        {
            return theSingleton.ToList().Find(x => x.iD == skillId);
        }

        //Gibt alle verfuegbaren Skills zurueck
        public IEnumerable<Skill> GetAllSkills()
        {
            return theSingleton;
        }

        //Aktualisiert den gegeben Skill bei der Datenquelle
        //oder fueg teinen Neuen ein,wenn die ID noch nicht
        public bool UpdateSkill(Skill skill)
        {
            var isUpdating = null != GetSkill(skill.iD);

            DeleteSkill(skill.iD);
            theSingleton.Push(skill);
            //Console.WriteLine($"Update {isUpdating} nowhaving size\t{theSigelton.Count}");
            return isUpdating;
        }

        //Loescht einen Skill basierend auf der ID
        public bool DeleteSkill(int skillId)
        {
            if (null != GetSkill(skillId))
            {
                var copy = theSingleton.ToList();
                theSingleton.Clear();
                foreach (Skill s in copy)
                {
                    if (s.iD != skillId)
                        theSingleton.Push(s);
                }
                return true;
            }
            else
                return false;
        }
    }
}
