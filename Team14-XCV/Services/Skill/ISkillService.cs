using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface ISkillService
    {
        //----------------------------------------Logic--------------------------------------------
        /// <summary>
        ///         Instantiates all Category.Parent,  with pointers to navigate  up and down the tree
        /// </summary>
        ///
        /// <param name="skills">
        ///         their .Category property which will be modified/replaced
        /// </param>
        public void HangThemOnATree(IEnumerable<Skill> skills);

        /// <summary>
        ///         Validates a tree against DataAnotations on the  Skill- + SkillCategory-Entity
        /// </summary>
        ///
        /// <event cref="OnChange">
        ///     Info: Akzeptabels Duplikat: {Skill} und {Skill}    in:   {Cat} und {Cat}
        /// or
        ///     Error: Inakzeptabels Duplikat: {Skill} und {Skill}    in:   {Cat} und {Cat}
        ///</event>
        public (IEnumerable<SkillCategory>, IEnumerable<Skill>) ValidateSkill(SkillCategory tree);
        public IEnumerable<string> ValidateSkill(IEnumerable<Skill> skills);
        public IEnumerable<string> ValidateSkillCategory(IEnumerable<SkillCategory> cats);




        //-------------------------------------Persistence-----------------------------------------
        /// <summary>
        ///          GetAllSkills from persistence.
        /// </summary>
        ///
        /// <returns>
        ///          A collection of Skills, that might be empty.
        /// </returns>
        public IEnumerable<Skill> GetAllSkills();

        /// <summary>
        ///         GetAllLevel correctly orderd from persistence.
        /// </summary>
        public string[] GetAllLevel();


        /// <summary>
        ///         UpdateAllSkills in persistence.
        /// </summary>
        ///
        /// <param name="tree">
        ///     the root of a tree, which contains all Categories and Skills  as .Children
        /// </param>
        public (int[] added, int[] removed) UpdateAllSkills(SkillCategory tree);

        /// <summary>
        ///         UpdateAllLevels in persistence.
        /// </summary>
        ///
        /// <param name="levels">
        ///         a correctly order array of size 4
        /// </param>
        public int UpdateAllLevels(string[] levels);
        public int InsertSkill(Skill skill);
        public int DeleteSkill(Skill skill);

        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
