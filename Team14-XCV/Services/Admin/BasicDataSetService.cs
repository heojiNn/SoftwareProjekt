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
    public class BasicDataSetService : IBasicDataSetService
    {
        private readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new SCategoryConverter(), new SkillConverter() } };

        private readonly ISkillService _skillService;
        private readonly ILanguageService _languageService;
        private readonly IFieldService _fieldService;
        private readonly IRoleService _roleService;

        public BasicDataSetService(ISkillService skillService, ILanguageService languageService,
                                   IRoleService roleService, IFieldService fieldService)
        {
            _skillService = skillService;
            _skillService.ChangeEventHandel += OnSkillServ;
            _languageService = languageService;
            _fieldService = fieldService;
            _roleService = roleService;
        }


        //-----------------------------------------------------------------------------------------
        //---------------------------------Buissines Logic-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //---------------------------------IBasicDataSetService------------------------------------
        public string[] ShowCurrentDataSet()      //returns eveytig as a json
        {
            var result = new string[5];
            var allDset = new dataSet();
            var fields = _fieldService.GetAllFields();
            var fieldDset = fields.Select(x => x.Name).ToList();
            allDset.fields = fieldDset;

            var roles = _roleService.GetAllRoles();
            List<dataSetrole> roleDset = new();
            var roleNames = roles.Select(x => x.Name).Distinct();
            foreach (var name in roleNames)
            {
                List<float> wages = new();
                roles.Where(x => x.Name == name).ToList().ForEach(x => wages.Add(x.Wage));
                roleDset.Add(new() { name = name, wages = wages.ToArray() });
            }
            allDset.roles = roleDset;

            var languages = _languageService.GetAllLanguages();
            var langLvls = _languageService.GetAllLevel();
            var languageDset = new dataSetLanguages() { elements = languages.Select(x => x.Name), levels = langLvls };
            allDset.languages = languageDset;

            result[1] = Format(JsonSerializer.Serialize(fieldDset, options));
            result[2] = Format(JsonSerializer.Serialize(roleDset, options));
            result[3] = Format(JsonSerializer.Serialize(languageDset, options));



            dataSetSkills skillsDset = new();
            skillsDset.hardSkillLevels = _skillService.GetAllLevel();
            var skills = _skillService.GetAllSkills();
            if (!skills.Any())
                return result;

            var root = skills.First().Category.Parent;
            if (root.Parent != null)
                root = root.Parent;
            if (root.Parent != null)
                root = root.Parent;
            var softCat = skills.FirstOrDefault(x => x.Category.Name == "SoftSkills").Category;
            result[4] = Format(JsonSerializer.Serialize(root, options));
            root.Children.Remove(softCat);
            var hardCat = root.Children.First();
            hardCat.Name = "";
            skillsDset.HardSkills = hardCat;
            skillsDset.SoftSkills = softCat.Children.Select(x => ((Skill)x).Name).ToArray();
            allDset.skills = skillsDset;

            result[0] = Format(JsonSerializer.Serialize(allDset, options));

            return result;
        }
        public async Task<string[]> ShowBrowserFile(IBrowserFile browserFile)
        {
            var cont = await new StreamReader(browserFile.OpenReadStream())
                                        .ReadToEndAsync();
            var result = ShowCurrentDataSet();
            result[0] = cont;
            return result;
        }

        public string[] ValidateUpdate(string[] jsons, string[] newOnes = null) // delivers ChangeResult
        {
            infoMessages = new();
            errorMessages = new();


            var fieldsDataSet = JsonSerializer.Deserialize<string[]>(jsons[1], options);
            var newFields = fieldsDataSet.Select(x => new Field() { Name = x });
            if (newFields.Where(x => x.Name.Length > 40).Any())
                errorMessages.Add($"Brachennamen dürfen nicht länger als 40 Zeichen sein");


            OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
            return jsons;
        }
        public void Update(string[] json)
        {
            ValidateUpdate(json);
            if (errorMessages.Any())
                return;

            var fieldsDataSet = JsonSerializer.Deserialize<dataSet>(json[0], options).fields;
            var newFields = fieldsDataSet.Select(x => new Field() { Name = x.Trim() });

            var rolesDataSet = JsonSerializer.Deserialize<dataSet>(json[0], options).roles;

            var languagesDataSet = JsonSerializer.Deserialize<dataSet>(json[0], options).languages;
            var newLanguages = languagesDataSet.elements.Select(x => new Language() { Name = x.Trim() });
            var newLangLvl = languagesDataSet.levels;

            var dataSkillTree = JsonSerializer.Deserialize<dataSet>(json[0], options).skills;
            var newSkillTree = new SkillCategory();
            dataSkillTree.HardSkills.Name = "HardSkills";
            dataSkillTree.HardSkills.Parent = newSkillTree;
            var softSkills = new SkillCategory() { Name = "SoftSkills", Parent = newSkillTree };
            IEnumerable<SkillCategory> softChildren = dataSkillTree.SoftSkills.Select(x => new Skill() { Name = x, Category = softSkills });
            softSkills.Children = softChildren.ToList();
            newSkillTree.Children.Add(dataSkillTree.HardSkills);
            newSkillTree.Children.Add(softSkills);
            var newSkillLvl = dataSkillTree.hardSkillLevels;

            int addedRows, removedRows;
            (addedRows, removedRows) = _fieldService.UpdateAllFields(newFields);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} Brachen hinzugefügt/entfernt");

            (addedRows, removedRows) = _roleService.UpdateAllRoles(rolesDataSet);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} (Rollen,Lohn) hinzugefügt/entfernt");

            (addedRows, removedRows) = _languageService.UpdateAllLanguages(newLanguages);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} Sprachen hinzugefügt/entfernt");
            (addedRows, removedRows) = _languageService.UpdateAllLevels(newLangLvl);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} Sprachen Level Namen geändert");

            (var added, var removed) = _skillService.UpdateAllSkills(newSkillTree);
            infoMessages.Add($"Es wurden: {added[0]}/{removed[0]} Skills Kategorien hinzugefügt/entfernt");
            infoMessages.Add($"Es wurden: {added[1]}/{removed[1]} Skills hinzugefügt/entfernt");
            (addedRows, removedRows) = _skillService.UpdateAllLevels(newSkillLvl);
            infoMessages.Add($"Es wurden: {addedRows}/{removedRows} Skill Level hinzugefügt/entfernt");

            infoMessages = infoMessages.Concat(changeInfo.InfoMessages).ToList();
            errorMessages = errorMessages.Concat(changeInfo.ErrorMessages).ToList();
            OnChange(new() { SuccesMessage = "", InfoMessages = infoMessages });
        }



        // my alternative json intendation Style
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




        private ChangeResult changeInfo = new();

        private void OnSkillServ(object sender, ChangeResult e) => changeInfo = e;

        //-----------------------------------------------------------------------------------------
        private List<string> errorMessages = new();
        private List<string> infoMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }









    struct dataSet
    {
        public IEnumerable<string> fields;  //nur elements
        public IEnumerable<dataSetrole> roles;  //elements und wages
        public dataSetLanguages languages;  //elements und wages
        public dataSetSkills skills;  //elements und wages
    }

    public struct dataSetrole
    {
        public string name;
        public float[] wages;
    }
    struct dataSetLanguages
    {
        public string[] levels;
        public IEnumerable<string> elements;
    }

    struct dataSetSkills
    {
        public string[] hardSkillLevels;
        public SkillCategory HardSkills;
        public string[] SoftSkills;
    }
}
