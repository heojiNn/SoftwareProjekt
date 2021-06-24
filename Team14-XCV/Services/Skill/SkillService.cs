using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    public class SkillService : ISkillService
    {
        private readonly string connectionString;
        private readonly ILogger<SkillService> log;
        public SkillService(IConfiguration config, ILogger<SkillService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MyLocalConnection");
        }

        //---------------------------------Buissines Logic-----------------------------------------
        //-----------------------------------------------------------------------------------------
        public IEnumerable<Skill> GetSkillsStarWith(string name)
        {
            var skills = GetAllSkills().Where(x => x.Name.ToLower().StartsWith(name.ToLower())).OrderBy(x => x.Name);
            return skills;
        }
        public Skill GetSkill(string cat, string name, string lvl = "")
        {
            var s = GetAllSkills().FirstOrDefault(x => x.Name == name && x.Category.Name == cat);
            if (s == null || s.Type == SkillGroup.Softskill)
                return s;

            s.Level = lvl;
            return s;
        }

        public IEnumerable<Skill> HangThemOnATree(IEnumerable<Skill> skills)
        {
            var cats = GetAllCategories().ToList();
            skills = skills.ToList();
            foreach (var skill in skills)
            {
                var cat = cats.FirstOrDefault(x => x.Name == skill.Category.Name);
                skill.Category = cat;
                cat.Children.Add(skill);
            }
            foreach (var cat in cats)
            {
                var highCat = cats.FirstOrDefault(x => x.Name == cat.Parent.Name);
                cat.Parent = highCat;
                if (highCat != null)
                    highCat.Children.Add(cat);
            }
            var root = new SkillCategory();
            var hard = cats.FirstOrDefault(x => x.Name == "HardSkills");
            var soft = cats.FirstOrDefault(x => x.Name == "SoftSkills");
            if (hard == null || soft == null)
                return new List<Skill>();
            hard.Parent = root;
            soft.Parent = root;
            root.Children.Add(hard);
            root.Children.Add(soft);
            MarkDouble(root);

            return skills;
        }







        public (int[] added, int[] removed) UpdateAllSkills(SkillCategory tree)
        {
            (var cats, var skills) = MarkDouble(tree);
            var add = new int[2];
            var remo = new int[2];
            (add[0], remo[0]) = UpdateCategories(cats);
            (add[1], remo[1]) = UpdateSkills(skills);

            return (add, remo);
        }
        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Skill> GetAllSkills()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var skillsTemp = con.Query<(string Name, string Category)>("Select * From Skill");
            con.Close();
            var skills = skillsTemp.Select(x => new Skill() { Name = x.Name, Category = new SkillCategory() { Name = x.Category } });
            skills = skills.ToList();
            skills = HangThemOnATree(skills);
            return skills;
        }
        public string[] GetAllLevel()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var lvl = con.Query<string>("Select Name From Skill_Level Where level <> 0 Order By Level").ToArray();
            con.Close();
            return lvl;
        }


        private IEnumerable<SkillCategory> GetAllCategories()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();
            var catsTemp = con.Query<(string Name, string Parent)>("Select * From SkillCategory");
            con.Close();
            var cats = catsTemp.Select(x => new SkillCategory() { Name = x.Name, Parent = new SkillCategory() { Name = x.Parent } });
            return cats;
        }

        //---write --------------------------------------------------------------------------------
        public (int added, int removed) UpdateAllLevels(string[] levels)
        {
            int addedRows = 0;
            int removedRows = 0;
            if (GetAllLevel().SequenceEqual(levels))
                return (0, 0);
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                for (int i = 0; i < levels.Length; i++)
                    addedRows += con.Execute($"Update Skill_Level  Set Name='{levels[i]}'  Where Level={i + 1}");
            }
            catch (SqlException e)
            {
                log.LogError($"Error updating Skill Levels in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
            return (addedRows, removedRows);
        }





        private (int added, int removed) UpdateSkills(IEnumerable<Skill> skills)
        {
            int addedRows = 0;
            int removedRows = 0;
            var newSkills = skills.ToImmutableSortedSet();
            var oldSkills = GetAllSkills().ToImmutableSortedSet();
            var toAdd = newSkills.Except(oldSkills);
            var toRemove = oldSkills.Except(newSkills);

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                foreach (var skill in toAdd)
                    addedRows += con.Execute($"Insert Into Skill (Name, Category) Values ('{skill.Name}', '{skill.Category.Name}')");
                foreach (var skill in toRemove)
                    removedRows += con.Execute($"Delete From Skill Where Name = '{skill.Name}' And  Category = '{skill.Category.Name}'");
            }
            catch (SqlException e)
            {
                log.LogError($"Error updating Skills in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
            return (addedRows, removedRows);
        }
        private (int added, int removed) UpdateCategories(IEnumerable<SkillCategory> cats)
        {
            int addedRows = 0;
            int removedRows = 0;
            var newCats = cats.ToImmutableSortedSet();
            var oldCats = GetAllCategories().ToImmutableSortedSet();
            var toAdd = newCats.Except(oldCats);
            var toRemove = oldCats.Except(newCats);

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                foreach (var cat in toAdd)
                    addedRows += con.Execute($"Insert Into SkillCategory (Name, Parent) Values ('{cat.Name}', '{cat.Parent.Name}')");
                foreach (var cat in toRemove)
                    removedRows += con.Execute($"Delete From SkillCategory  Where Name = '{cat.Name}' ");
            }
            catch (SqlException e)
            {
                log.LogError($"Error updating SkillCategory in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
            return (addedRows, removedRows);
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        private (IEnumerable<SkillCategory>, IEnumerable<Skill>) MarkDouble(SkillCategory treeRoot)
        {
            infoMessages = new();
            errorMessages = new();
            (var allCats, var allSkills) = List(treeRoot);
            if (!allSkills.Any())
                return (allCats, allSkills);

            infoMessages.Add($"{allSkills.Count} Fähigkeiten in {allCats.Count} Kategorien.");
            allSkills = allSkills.OrderBy(x => x.Name).ThenBy(x => x.Category.Name).ToList();  //double check through sorting
            var previous = allSkills.Last();
            foreach (var skill in allSkills)
            {
                var previousName = previous.Name;
                var currentName = skill.Name;
                var previousCategory = previous.Category.Name;
                var currentCategory = skill.Category.Name;
                if (skill.Category.Name.Length > 40)
                    log.LogError($"Für Kategorien nicht mehr als 40 Zeichen: {currentCategory}");
                if (currentName.Length > 50)
                    log.LogError($"Das Design beschränkt Skills auf 50 Zeichen: {currentName}");

                if (previousName.ToLower().Equals(currentName.ToLower()))
                    if (currentCategory.ToLower().Equals(previousCategory.ToLower()))
                        errorMessages.Add($"Inakzeptabels Dupllicat:  \"{currentName}\" und \"{previousName}\"   \n" +
                                           $"            in:   \"{currentCategory}\" und \"{previousCategory}\" ");
                    else
                    {
                        infoMessages.Add($"Akzeptabel Dupllicat:  \"{currentName}\" und \"{previousName}\"   \n" +
                                         $"           in:   \"{currentCategory}\" und \"{previousCategory}\"  ");
                        previous.HasDouble = true;
                        skill.HasDouble = true;
                    }
                previous = skill;
            }


            OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
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
        //----------------------------------------------------------------------------------------




        private List<string> errorMessages = new();
        private List<string> infoMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
}
