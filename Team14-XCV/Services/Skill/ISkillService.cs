using System;
using System.Collections.Generic;



namespace Team14.Data
{
    public interface ISkillService
    {
        public IEnumerable<Skill> GetSkillsStarWith(string name);

        public IEnumerable<Skill> GetAllSkills();


        public void UpdateSkill(IEnumerable<Skill> skills);


        public event EventHandler<NoResult> SearchEventHandel;
    }

}
