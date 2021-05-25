using System.Collections.Generic;


namespace Team14.Data
{
    public interface ISkillService
    {
        //Gibt einen Skill basierend auf der gewünschten ID zurück
        //oder null, wenn kein Skill existiert
        public Skill GetSkill(int skillId);

        //Gibt alle verfügbaren Skills zurück
        public List<Skill> GetAllSkills();

        //Akutalisiert den gegebenen Skill bei der Datenquelle
        //(Identifikation über die ID) oder fügt Namen ein,
        //wenn die ID noch nicht vorhanden ist
        public bool UpdateSkill(Skill skill);

        //Löscht einen Skill basierend auf der ID
        public bool DeleteSkill(int skillId);
    }
}
