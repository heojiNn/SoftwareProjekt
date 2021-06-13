using System;
using System.Collections.Generic;



namespace Team14.Data
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
        public IEnumerable<Skill> GetAllSkills();


        // Summary:
        //     UpdateAllSkills in Persistece.
        // Parameters:
        //   skills:
        //     The skills to replace the old ones.
        //
        // Loges:
        //   LogInformation:
        //     All Skill updated  Persitence  {fileName}
        public void UpdateAllSkills(IEnumerable<Skill> skills);
    }
}
