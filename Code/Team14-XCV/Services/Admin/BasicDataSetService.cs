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
            _fieldService.ChangeEventHandel += OnInternalMess;
            _roleService = roleService;
            _roleService.ChangeEventHandel += OnInternalMess;
            _languageService = languageService;
            _languageService.ChangeEventHandel += OnInternalMess;
            _skillService = skillService;
            _skillService.ChangeEventHandel += OnInternalMess;
        }


        //-------------------------------------Business Logic--------------------------------------
        //-----------------------------------------------------------------------------------------
        // for definition see   IBasicDataSetService
        public string ShowCurrentDataSet()
        {
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
            //result[1] = Format(JsonSerializer.Serialize(fields, options));
            //result[2] = Format(JsonSerializer.Serialize(roles, options));
            //result[3] = Format(JsonSerializer.Serialize(languages, options));
            dataSetSkills skillsDset = new();
            skillsDset.hardSkillLevels = _skillService.GetAllLevel();
            var skills = _skillService.GetAllSkills();
            if (!skills.Any())
                return "err";

            var root = skills.First().Category.GetRoot();
            //result[4] = Format(JsonSerializer.Serialize(root, options));

            var softCat = skills.FirstOrDefault(x => x.Category.Name == "SoftSkills").Category;
            root.Children.Remove(softCat);
            var hardCat = (SkillCategory)root.Children.First();
            hardCat.Name = "";
            skillsDset.HardSkills = hardCat;
            skillsDset.SoftSkills = softCat.Children.Select(x => ((Skill)x).Name).ToArray();
            databaseReturns.skills = skillsDset;

            return Format(JsonSerializer.Serialize(databaseReturns, options));
        }
        public async Task<string> ShowBrowserFile(IBrowserFile browserFile)
        {
            var fileContent = await new StreamReader(browserFile.OpenReadStream())
                                        .ReadToEndAsync();
            return fileContent;
        }



        public void JsonUpdate(string json, bool dryRun = true)
        {
            errorMessages = new();
            infoMessages = new();
            dataSet wholeJson;

            //---------------------------------------------------------------------------Validation
            try
            {
                wholeJson = JsonSerializer.Deserialize<dataSet>(json, options);
            }
            catch (Exception e)
            {
                OnChange(new() { ErrorMessages = new[] { e.Message } });
                return;
            }

            var newFields = wholeJson.fields.Select(x => new Field() { Name = x.Trim() });

            var newRoles = new List<Role>();
            foreach (var roleKind in wholeJson.roles)
                foreach (var wage in roleKind.wages)
                    newRoles.Add(new Role() { Name = roleKind.name, RCL = wage.RCL, Wage = wage.PerHour });

            var languagesDataSet = wholeJson.languages;
            var newLanguages = languagesDataSet.elements.Select(x => new Language() { Name = x.Trim() });
            var newLangLvl = languagesDataSet.levels;

            var newSkillTree = new SkillCategory();
            var dataSkillTree = wholeJson.skills;
            dataSkillTree.HardSkills.Name = "HardSkills";
            dataSkillTree.HardSkills.Parent = newSkillTree;
            newSkillTree.Children.Add(dataSkillTree.HardSkills);   // HardSkills

            var softSkills = new SkillCategory() { Name = "SoftSkills", Parent = newSkillTree };
            IEnumerable<SkilTreeNode> softChildren = dataSkillTree.SoftSkills.Select(x => new Skill() { Name = x, Category = softSkills });
            softSkills.Children = softChildren.ToList();
            newSkillTree.Children.Add(softSkills);                  // and SoftSkills
            var newSkillLvl = dataSkillTree.hardSkillLevels;



            _fieldService.UpdateAllFields(newFields, justValidate: true);
            errorMessages = errorMessages.Concat(changeInfo.ErrorMessages).ToList();
            _roleService.UpdateAllRoles(newRoles, justValidate: true);
            errorMessages = errorMessages.Concat(changeInfo.ErrorMessages).ToList();

            _languageService.UpdateAllLanguages(newLanguages, justValidate: true);
            errorMessages = errorMessages.Concat(changeInfo.ErrorMessages).ToList();
            _languageService.UpdateAllLevels(newLangLvl, justValidate: true);
            errorMessages = errorMessages.Concat(changeInfo.ErrorMessages).ToList();

            (var cats, var skills) = _skillService.ValidateSkill(newSkillTree);
            errorMessages = errorMessages.Concat(_skillService.ValidateSkillCategory(cats)).ToList();
            errorMessages = errorMessages.Concat(_skillService.ValidateSkill(skills)).ToList();
            changeInfo = new();
            _skillService.UpdateAllLevels(newSkillLvl, justValidate: true);
            errorMessages = errorMessages.Concat(changeInfo.ErrorMessages).ToList();
            if (dryRun || errorMessages.Any())                              //if dryrun(justValidate)
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return;
            }
            //-------------------------------------------------------------------------------------

            //--------------------------------------------------------------------------Persistence
            int addedRows, removedRows, changed;
            (addedRows, removedRows) = _fieldService.UpdateAllFields(newFields);
            if (changeInfo.SuccesMessage != "")
                infoMessages.Add(changeInfo.SuccesMessage);

            (addedRows, changed, removedRows) = _roleService.UpdateAllRoles(newRoles);
            if (changed != 0)
                infoMessages.Add($"Es wurden: {changed} Löhne geändert.");
            if (addedRows != 0 || removedRows != 0)
                infoMessages.Add($"Es wurden: {addedRows}/{removedRows} Rollen hinzugefügt/entfernt.");

            (addedRows, removedRows) = _languageService.UpdateAllLanguages(newLanguages);
            if (changeInfo.SuccesMessage != "")
                infoMessages.Add(changeInfo.SuccesMessage); ;
            changed = _languageService.UpdateAllLevels(newLangLvl);
            if (changed != 0)
                infoMessages.Add($"Es wurden: {changed} SprachenLevel geändert.");


            (var added, var removed) = _skillService.UpdateAllSkills(newSkillTree);
            if (added[0] != 0 || removed[0] != 0)
                infoMessages.Add($"Es wurden: {added[0]}/{removed[0]} SkillsKategorien hinzugefügt/entfernt.");
            if (added[1] != 0 || removed[1] != 0)
                infoMessages.Add($"Es wurden: {added[1]}/{removed[1]} Skills hinzugefügt/entfernt.");
            changed = _skillService.UpdateAllLevels(newSkillLvl);
            if (changed != 0)
                infoMessages.Add($"Es wurden: {changed} Skill Level geändert.");
            //-------------------------------------------------------------------------------------
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



        private ChangeResult changeInfo = new();
        private void OnInternalMess(object sender, ChangeResult e) => changeInfo = e;

    }
}
