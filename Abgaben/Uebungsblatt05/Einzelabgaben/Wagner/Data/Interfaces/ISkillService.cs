using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team14.Data.Datatypes;

namespace Team14.Data.Interfaces
{
    interface ISkillService
    {
        //Gibt einen Skill basierend auf der gewuenschten ID zurueck oder null, wenn kein Skill existiert
        public Skill GetSkill(int skillId);

        //Gibt alle verfuegbaren Skills zurueck
        public List<Skill> GetAllSkills();

        //Aktualisiert den gegebenen Skill bei der Datenquelle (Identifikation ueber die ID oder fuegt einen Neuen ein, wenn die ID noch nicht vorhanden ist
        public bool UpdateSkill(Skill skill);

        //loescht einen Skill basierend auf der ID
        public bool DeleteSkill(int skillId);
    }
}
