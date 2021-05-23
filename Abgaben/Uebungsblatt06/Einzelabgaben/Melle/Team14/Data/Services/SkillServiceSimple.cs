using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Team14.Data
{
    // mit lustiger koplzierter Stack implmntierung
    //
    public class SkillServiceSimple : ISkillService
    {
        private readonly Stack<Skill> theSigelton;
        public SkillServiceSimple(Stack<Skill> allSkillsforevaSigelton)
        {
            theSigelton = allSkillsforevaSigelton;
        }

        //Gibt einen Skill basierend auf der gewuenschten ID zurueck
        //oder null,wenn kein Skill existiert.
        public Skill GetSkill(int Id)
        {
            return theSigelton.ToList().Find(x => x.Id == Id);
        }
        //Gibt alle verfuegbaren Skills zurueck.





        public IEnumerable<Skill> GetAllSkills()
        {
            if (theSigelton.Count == 0)
            {


                var bd = JsonSerializer.Deserialize<BaseData>(File.ReadAllText("datenbasis.json"));
                for (int i = 0; i < bd.skills["Sprachen"].Count; i++)
                    UpdateSkill(new Skill() { Id = i, Name = bd.skills["Sprachen"][i] });
            }
            return theSigelton;
        }




        //Aktualisiert den gegeben Skill bei der Datenquelle
        //oder fueg teinen Neuen ein,wenn die ID noch nicht
        public bool UpdateSkill(Skill skill)
        {
            var isUpdating = null != GetSkill(skill.Id);

            DeleteSkill(skill.Id);
            theSigelton.Push(skill);
            //Console.WriteLine($"Update {isUpdating} nowhaving size\t{theSigelton.Count}");
            return isUpdating;
        }
        //Loescht einen Skill basierend auf der ID.
        public bool DeleteSkill(int skillId)
        {
            if (null != GetSkill(skillId))
            {
                var copy = theSigelton.ToList();
                theSigelton.Clear();
                foreach (Skill s in copy)
                {
                    if (s.Id != skillId)
                        theSigelton.Push(s);
                }
                return true;
            }
            else
                return false;
        }
    }
}
