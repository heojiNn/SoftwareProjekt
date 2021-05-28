using System.Collections.Generic;
using Team14.Data;

namespace Team14.Interfaces
{
    public interface ISkillService
    {
        //Gibt einen Skill basierend auf der gewuenschten ID zurureck
        //oder null, wenn kein Skill existiert
        public Skill GetSkill(int skillId);

        //Gibt alle verfügbaren Skills zurueck
        public IEnumerable<Skill> GetAllSkills();

        //Aktualisiert den gegeben Skill bei der Datenquelle
        //(Identifikation ueber die ID) oder fuegt einen Neuen ein,
        //wenn die ID noch nicht vorhanden ist
        public bool UpdateSkill(Skill skill);

        //Loescht einen Skill basierend auf der ID
        public bool DeleteSkill(int skillId);
    }
}
