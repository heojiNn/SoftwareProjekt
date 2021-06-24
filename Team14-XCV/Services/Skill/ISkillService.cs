using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface ISkillService

    {
        // Summary:
        //     GetSkills with a name  (case insensitive).
        // Returns:
        //     A List of Skills that might be  empty
        public IEnumerable<Skill> GetSkillsStarWith(string name);


        // Summary:
        //     GetAllSkills from Persistece.
        // Returns:
        //     A List of Skills that might be  empty
        //
        // Exceptions:
        //   Exception:
        //     Could not reach Persistence: {subPath}/{fileName}
        public Skill GetSkill(string cat, string name, string lvl);


        public IEnumerable<Skill> GetAllSkills();
        public IEnumerable<Skill> HangThemOnATree(IEnumerable<Skill> skills);

        public string[] GetAllLevel();


        // Summary:
        //     UpdateAllSkills in Persistece.
        // Parameters:
        //   skills:
        //     The skills to replace the old ones.
        //
        // Loges:
        //   LogInformation:
        //     All Skill updated  Persitence  {fileName}
        public (int[] added, int[] removed) UpdateAllSkills(SkillCategory tree);
        public (int added, int removed) UpdateAllLevels(string[] levels);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
