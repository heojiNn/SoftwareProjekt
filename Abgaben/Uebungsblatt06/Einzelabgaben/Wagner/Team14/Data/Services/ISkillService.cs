using Team14.Data;
using System.Collections.Generic;

namespace Team14.Data
{
    public interface ISkillService
    {
        //Gibt einen Skill basierend auf der gewuenschten ID zurueck
        //oder null,wenn kein Skill existiert.
        public Skill GetSkill(int skillId);
        //Gibt alle verfuegbaren Skills zurueck.
        public IEnumerable<Skill> GetAllSkills();
        //Aktualisiert den gegeben Skill bei der Datenquelle
        //oder fueg teinen Neuen ein,wenn die ID noch nicht
        public bool UpdateSkill(Skill skill);
        //Loescht einen Skill basierend auf der ID.
        public bool DeleteSkill(int skillId);
    }
}
