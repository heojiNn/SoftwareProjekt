using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;


namespace XCV.Data
{
    /// <inheritdoc/>
    public class BasicDataSetService : IBasicDataSetService
    {
        private readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new SCategoryConverter(), new SkillConverter() } };

        private readonly ISkillService _skillService;
        private readonly ILanguageService _languageService;
        private readonly IFieldService _fieldService;
        private readonly IRoleService _roleService;

        public BasicDataSetService(IFieldService fieldService, IRoleService roleService, ILanguageService languageService, ISkillService skillService)
        {
            _fieldService = fieldService;
            _roleService = roleService;
            _skillService = skillService;
            _languageService = languageService;
        }


        //-------------------------------------Business Logic--------------------------------------
        //-----------------------------------------------------------------------------------------
        // for definition see   IBasicDataSetService
        public string[] ShowCurrentDataSet()
        {
            var result = new string[5];
            dataSet databaseReturns = new();

            IEnumerable<string> fields = _fieldService.GetAllFields().Select(x => x.Name);  // Field names
            databaseReturns.fields = fields;

            var multiRoles = _roleService.GetAllRoles().Select(x => new dataSetrole() { name = x.Name, wages = new[] { new dataSetWage() { RCL = x.RCL, PerHour = x.Wage } } });
            IEnumerable<dataSetrole> roles = multiRoles.GroupBy(x => x.name).Select(g =>
                                            {
                                                var groupedFirst = g.First();
                                                groupedFirst.wages = g.Select(x => x.wages.First()).ToArray();
                                                return groupedFirst;
                                            });
            databaseReturns.roles = roles;

            var languagesNames = _languageService.GetAllLanguages().Select(x => x.Name);
            var langLvls = _languageService.GetAllLevel();
            dataSetLanguages languages = new() { elements = languagesNames, levels = langLvls };
            databaseReturns.languages = languages;


            result[1] = Format(JsonSerializer.Serialize(fields, options));      // "fields": ["Archite,  "Automobil", .....
            result[2] = Format(JsonSerializer.Serialize(roles, options));       // "roles":[{"name":"agileC", "wages": [{"RCL": 0, "PerHour": 0}, {"RC...
            result[3] = Format(JsonSerializer.Serialize(languages, options));   // "languages":{"levels": ["spricht",  "ok",  "gut" ],
                                                                                //              "elements": ["Arabis...


            dataSetSkills skillsDset = new();
            skillsDset.hardSkillLevels = _skillService.GetAllLevel();
            var skills = _skillService.GetAllSkills();
            if (!skills.Any())
                return result;

            var root = skills.First().Category.GetRoot();
            result[4] = Format(JsonSerializer.Serialize(root, options));

            var softCat = skills.FirstOrDefault(x => x.Category.Name == "SoftSkills").Category;
            root.Children.Remove(softCat);
            var hardCat = (SkillCategory)root.Children.First();
            hardCat.Name = "";
            skillsDset.HardSkills = hardCat;
            skillsDset.SoftSkills = softCat.Children.Select(x => ((Skill)x).Name).ToArray();
            databaseReturns.skills = skillsDset;
            result[0] = Format(JsonSerializer.Serialize(databaseReturns, options));// the whole file

            return result;
        }
        public async Task<string[]> ShowBrowserFile(IBrowserFile browserFile)
        {
            var fileContent = await new StreamReader(browserFile.OpenReadStream())
                                        .ReadToEndAsync();
            var result = ShowCurrentDataSet();
            result[0] = fileContent;
            return result;
        }



        public void JsonUpdate(string json, bool dryRun = true)
        {
            errorMessages = new();
            infoMessages = new();
            dataSet wholeJson;
            try
            {
                wholeJson = JsonSerializer.Deserialize<dataSet>(json, options);
            }
            catch (Exception e)
            {
                OnChange(new() { ErrorMessages = new[] { e.Message } });
                return;
            }

            var newFields = wholeJson.fields.Select(x => new Field() { Name = x.Trim() });              // for ValidateFields() + UpdateAllFields()

            var newRoles = new List<Role>();                                                            // for ValidateRoles() + UpdateAllRoles()
            foreach (var roleKind in wholeJson.roles)
                foreach (var wage in roleKind.wages)
                    newRoles.Add(new Role() { Name = roleKind.name, RCL = wage.RCL, Wage = wage.PerHour });

            var languagesDataSet = wholeJson.languages;
            var newLanguages = languagesDataSet.elements.Select(x => new Language() { Name = x.Trim() });// for ValidateLanguages() + UpdateAllLanguages()
            var newLangLvl = languagesDataSet.levels;                                                    // for                       UpdateAllLevels()

            var newSkillTree = new SkillCategory();                // a tree with two children            for ValidateSkill() + UpdateAllSkills()
            var dataSkillTree = wholeJson.skills;
            dataSkillTree.HardSkills.Name = "HardSkills";
            dataSkillTree.HardSkills.Parent = newSkillTree;
            newSkillTree.Children.Add(dataSkillTree.HardSkills);   // HardSkills

            var softSkills = new SkillCategory() { Name = "SoftSkills", Parent = newSkillTree };
            IEnumerable<SkilTreeNode> softChildren = dataSkillTree.SoftSkills.Select(x => new Skill() { Name = x, Category = softSkills });
            softSkills.Children = softChildren.ToList();
            newSkillTree.Children.Add(softSkills);                  // and SoftSkills


            errorMessages = errorMessages.Concat(_fieldService.ValidateFields(newFields)).ToList();
            errorMessages = errorMessages.Concat(_roleService.ValidateRoles(newRoles)).ToList();
            errorMessages = errorMessages.Concat(_languageService.ValidateLanguages(newLanguages)).ToList();
            (var cats, var skills) = _skillService.ValidateSkill(newSkillTree);
            errorMessages = errorMessages.Concat(_skillService.ValidateSkillCategory(cats)).ToList();
            errorMessages = errorMessages.Concat(_skillService.ValidateSkill(skills)).ToList();
            if (dryRun || errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return;
            }


            int addedRows, removedRows, changed;
            (addedRows, removedRows) = _fieldService.UpdateAllFields(newFields);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} Brachen hinzugefügt/entfernt.");

            (addedRows, changed, removedRows) = _roleService.UpdateAllRoles(newRoles);
            infoMessages.Add($"Es wurden: {addedRows}/{changed}/{removedRows} (Rollen mit Lohn) hinzugefügt/geändert/entfernt.");

            (addedRows, removedRows) = _languageService.UpdateAllLanguages(newLanguages);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} Sprachen hinzugefügt/entfernt.");
            (addedRows, removedRows) = _languageService.UpdateAllLevels(newLangLvl);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} SprachenLevel geändert.");

            (var added, var removed) = _skillService.UpdateAllSkills(newSkillTree);
            infoMessages.Add($"Es wurden: {added[0]}/{removed[0]} SkillsKategorien hinzugefügt/entfernt.");
            infoMessages.Add($"Es wurden: {added[1]}/{removed[1]} Skills hinzugefügt/entfernt.");
            changed = _skillService.UpdateAllLevels(dataSkillTree.hardSkillLevels);
            infoMessages.Add($"Es wurden: {changed} Skill Level geändert.");

            OnChange(new() { SuccesMessage = "Änderungen in die Datenbank übernommen.", InfoMessages = infoMessages });
        }





        // an alternative json intendation Style
        public static string Format(string json)   // removes newlines in arrayes
        {
            string cr = Environment.NewLine;
            var result = json;
            var pattern = @"(\[)\s*([^" + cr + @"\]\{}]*)\s*";
            var replacement = @"$1$2  ";
            for (int i = 0; i < 100; i++)
                result = Regex.Replace(result, pattern, replacement);
            result = Regex.Replace(result, @"\s*\]", " ]");
            return result;
        }
        //-------------------------------------User Messaging--------------------------------------
        private List<string> errorMessages = new();
        private List<string> infoMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
    //----------------------------------Helper Classes-----------------------------------------
    //-----------------------------------------------------------------------------------------
    /// <summary>
    ///         the Class/Struct that represents the datenbasis.json
    /// </summary>
    struct dataSet
    {
        public IEnumerable<string> fields;  //nur elements
        public dataSetLanguages languages;  //elements und wages
        public dataSetSkills skills;  //elements und wages
        public IEnumerable<dataSetrole> roles;  //elements und wages
    }

    /// <summary>
    ///         a internal strutcure of the datenbasis.json
    /// </summary>
    public struct dataSetrole
    {
        public string name;
        public dataSetWage[] wages;
    }
    public struct dataSetWage
    {
        public int RCL;
        public float PerHour;
    }
    /// <summary>
    ///         a internal strutcure of the datenbasis.json
    /// </summary>
    struct dataSetLanguages
    {
        public string[] levels;
        public IEnumerable<string> elements;
    }

    /// <summary>
    ///         a internal strutcure of the datenbasis.json
    /// </summary>
    struct dataSetSkills
    {
        public string[] hardSkillLevels;
        public SkillCategory HardSkills;
        public string[] SoftSkills;
    }
}
