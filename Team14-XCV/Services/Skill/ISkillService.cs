using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface ISkillService
    {
        //----------------------------------------Logic--------------------------------------------
        /// <summary>
        ///         Instantiates all Category.Parent,  with pointers to navigate  up and down the tree
        ///         modifies the input so that the category structure represents the one from the json
        /// </summary>
        ///
        /// <param name="skills">
        ///         their .Category property will be modified/replaced
        /// </param>
        public void SetCategoryRelationTree(IEnumerable<Skill> skills);

        /// <summary>
        ///         Validates a tree against DataAnotations on the  Skill- + SkillCategory-Entity
        /// </summary>
        ///
        /// <event cref="OnChange">
        ///     Info: Akzeptabels Duplikat: {Skill} und {Skill}    in:   {Cat} und {DiffCat}
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
        ///         Updates(Renames)AllLevels in persistence.
        /// </summary>
        ///
        /// <param name="levels">
        ///         a correctly order array of size 4
        ///         for each element (.lenght &lt 1 && lenght &lt 30)  &amp&amp  uniqe(caseinsensitve)
        /// </param>
        public int UpdateAllLevels(string[] levels, bool justValidate = false);
        public int InsertSkill(Skill skill, bool justValidate = false);
        public int DeleteSkill(Skill skill);

        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
