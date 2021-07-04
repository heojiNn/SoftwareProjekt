using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    /// <inheritdoc/>
    public class SkillService : ISkillService
    {
        private readonly string connectionString;
        private readonly ILogger<SkillService> log;
        private IEnumerable<Skill> cache = new List<Skill>();
        private string[] lvlCache = Array.Empty<string>();

        public SkillService(IConfiguration config, ILogger<SkillService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MS_SQL_Connection");
        }

        //-------------------------------------Business Logic--------------------------------------
        //-----------------------------------------------------------------------------------------
        // for definition see   ISkillService
        public (IEnumerable<SkillCategory>, IEnumerable<Skill>) ValidateSkill(SkillCategory tree)
        {
            infoMessages = new();
            errorMessages = new();
            (var cats, var skills) = MarkDouble(tree);
            ValidateSkillCategory(cats);
            ValidateSkill(skills);
            OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
            return (cats, skills);
        }
        public IEnumerable<string> ValidateSkill(IEnumerable<Skill> skills)
        {
            foreach (var skill in skills)
            {
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(skill, new ValidationContext(skill), results, true))
                    foreach (var result in results)
                        errorMessages.Add($"{skill}: {result.ErrorMessage}");
            }
            return errorMessages;
        }
        public IEnumerable<string> ValidateSkillCategory(IEnumerable<SkillCategory> cats)
        {
            foreach (var cat in cats)
            {
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(cat, new ValidationContext(cat), results, true))
                    foreach (var result in results)
                        errorMessages.Add($"{cat.Name}: {result.ErrorMessage}");
            }
            return errorMessages;
        }
        //-----------------------------------------------------------------------------------------





        // for definition see   ISkillService
        public (int[] added, int[] removed) UpdateAllSkills(SkillCategory tree)
        {
            var add = new int[2];
            var remo = new int[2];
            (var cats, var skills) = ValidateSkill(tree);

            if (!errorMessages.Any())
            {
                (add[0], remo[0]) = UpdateCategories(cats);
                (add[1], remo[1]) = UpdateSkills(skills);
            }

            OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
            return (add, remo);
        }
        //-------------------------------------Persistence-----------------------------------------
        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Skill> GetAllSkills()
        {
            IEnumerable<Skill> skills = new List<Skill>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                int i = con.ExecuteScalar<int>("Select Count(*) From [Skill]");
                if (i == cache.Count()) return cache;

                skills = con.Query<Skill, string, Skill>("Select * From [Skill]",
                    (skill, category) =>
                    {
                        skill.Category = new SkillCategory() { Name = category };
                        return skill;
                    }, splitOn: "Category");
            }
            catch (SqlException e)
            {
                log.LogError($"GetAllSkills() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            HangThemOnATree(skills);
            cache = skills;
            return skills;
        }

        public string[] GetAllLevel()
        {
            if (lvlCache.Any())
                return lvlCache;
            using var con = new SqlConnection(connectionString);
            con.Open();
            var lvl = con.Query<string>("Select [Name] From [Skill_Level] Order By [Level]").ToArray();
            con.Close();
            lvlCache = lvl;
            return lvl;
        }
        public IEnumerable<SkillCategory> GetAllCategories()
        {
            IEnumerable<SkillCategory> cats = new List<SkillCategory>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                cats = con.Query<SkillCategory, string, SkillCategory>("Select * From [SkillCategory]",
                    (cat, parent) =>
                    {
                        cat.Parent = new SkillCategory() { Name = parent };
                        return cat;
                    }, splitOn: "Parent");
            }
            catch (SqlException) { }
            finally { con.Close(); }
            return cats;
        }


        //---write --------------------------------------------------------------------------------
        private (int added, int removed) UpdateSkills(IEnumerable<Skill> skills)
        {
            int addedRows = 0;
            int removedRows = 0;

            var oldSkills = GetAllSkills();
            var toAdd = skills.Except(oldSkills);
            var toRemove = oldSkills.Except(skills);
            var allCats = GetAllCategories();

            Parallel.ForEach(toAdd, skill =>        // reduces runtime significant  on 460 skills databasis load
            {
                addedRows += InsertSkill(skill);
            });
            Parallel.ForEach(toRemove, skill =>
            {
                if (allCats.Select(x => x.Name).Contains(skill.Category.Name))
                    removedRows += DeleteSkill(skill);
            });

            return (addedRows, removedRows);
        }
        public int InsertSkill(Skill skill)
        {
            int i = 0;
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                i = con.Execute("Insert Into [Skill] Values (@Name, @Category)", new { skill.Name, Category = skill.Category.Name });
            }
            catch (SqlException e) { log.LogError($"InsertSkill() persitence Error: \n{e.Message}\n"); }
            finally { con.Close(); }
            return i;
        }
        public int DeleteSkill(Skill skill)
        {
            int i = 0;
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                i = con.Execute("Delete From [Skill] Where [Name] = @Name AND [Category] = @Category", new { skill.Name, Category = skill.Category.Name });
            }
            catch (SqlException e) { log.LogError($"DeleteSkill() persitence Error: \n{e.Message}\n"); }
            finally { con.Close(); }
            return i;
        }
        public int UpdateAllLevels(string[] levels)
        {
            int changed = 0;
            if (levels.Length != 4 || GetAllLevel().SequenceEqual(levels))
                return 0;

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                for (int i = 0; i < levels.Length; i++)
                    con.Execute($"Update [Skill_Level]  Set [Name]={i}  Where Level={i + 1}"); // if order changes cause unique
                for (int i = 0; i < levels.Length; i++)
                    changed += con.Execute($"Update [Skill_Level]  Set [Name]=@Name  Where Level={i + 1}", new { Name = levels[i] });
            }
            catch (SqlException e)
            {
                log.LogError($"UpdateAllLevels() persitence Error: \n{e.Message}\n");
                OnChange(new() { ErrorMessages = new[] { e.Message } });
            }
            finally { con.Close(); }
            cache = new List<Skill>();
            return changed;
        }

        private (int added, int removed) UpdateCategories(IEnumerable<SkillCategory> cats)
        {
            int addedRows = 0;
            int removedRows = 0;

            var oldCats = GetAllCategories();
            var toAdd = cats.Except(oldCats).Select(x => new { x.Name, Parent = x.Parent.Name });
            var toRemove = oldCats.Except(cats);

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                removedRows += con.Execute($"Delete From [SkillCategory]  Where [Name] = @Name ", toRemove);
                addedRows += con.Execute($"Insert Into [SkillCategory] Values (@Name, @Parent)", toAdd);
            }
            catch (SqlException e) { log.LogError($"UpdateCategories() persitence Error: \n{e.Message}\n"); }
            finally { con.Close(); }
            return (addedRows, removedRows);
        }
        //--End Persistence------------------------------------------------------------------------



        //---------------------------------------Helper--------------------------------------------
        //-----------------------------------------------------------------------------------------
        public void HangThemOnATree(IEnumerable<Skill> skills)
        {
            var cats = GetAllCategories().ToList();
            cats.Add(new SkillCategory() { Name = "" });
            foreach (var skill in skills)   //replace Category
            {
                var cat = cats.FirstOrDefault(x => x.Name == skill.Category.Name);
                skill.Category = cat;
                cat.Children.Add(skill);
            }
            foreach (var cat in cats)       // connect parents
            {
                var highCat = cats.FirstOrDefault(x => x.Name == cat.Parent?.Name);
                cat.Parent = highCat;
                if (highCat != null)        // Hard and Soft have a Parent(name="") itself got null as parrent
                    highCat.Children.Add(cat);
            }
            var root = cats.FirstOrDefault().GetRoot();
            if (root != null)
                MarkDouble(root);
        }
        //-----------------------------------------------------------------------------------------
        private (IEnumerable<SkillCategory>, IEnumerable<Skill>) MarkDouble(SkillCategory treeRoot)
        {
            (var allCats, var allSkills) = List(treeRoot);
            if (!allSkills.Any())
                return (allCats, allSkills);

            allSkills = allSkills.OrderBy(x => x.Name).ThenBy(x => x.Category.Name).ToList();  //double check through sorting
            var previous = allSkills.Last();
            foreach (var skill in allSkills)
            {
                var previousName = previous.Name;
                var currentName = skill.Name;
                var previousCategory = previous.Category.Name;
                var currentCategory = skill.Category.Name;

                if (previousName.ToLower().Equals(currentName.ToLower()))
                    if (currentCategory.ToLower().Equals(previousCategory.ToLower()))
                        errorMessages.Add($"Inakzeptabels Duplikat:  \"{currentName}\" und \"{previousName}\"   \n" +
                                           $"             in:  \"{currentCategory}\" und \"{previousCategory}\" ");
                    else
                    {
                        infoMessages.Add($"Akzeptabels Duplikat:  \"{currentName}\" und \"{previousName}\"   \n" +
                                         $"            in:  \"{currentCategory}\" und \"{previousCategory}\"  ");
                        previous.HasDouble = true;
                        skill.HasDouble = true;
                    }
                previous = skill;
            }
            return (allCats, allSkills);
        }
        private (List<SkillCategory>, List<Skill>) List(SkillCategory tree)
        {
            List<Skill> allSkills = new();
            List<SkillCategory> allCats = new();
            if (!tree.Children.Any())
                return (allCats, allSkills);

            if (tree.Children.First() is Skill)
                foreach (Skill s in tree.Children)
                    allSkills.Add(s);
            else
                foreach (SkillCategory node in tree.Children)
                {
                    allCats.Add(node);
                    (var newCats, var newSkills) = List(node);
                    allCats = allCats.Concat(newCats).ToList();
                    allSkills = allSkills.Concat(newSkills).ToList();
                }
            return (allCats, allSkills);
        }



        //-------------------------------------User Messaging--------------------------------------
        private List<string> errorMessages = new();
        private List<string> infoMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
}
