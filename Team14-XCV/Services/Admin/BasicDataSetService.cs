using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Team14.Data
{
    public class BasicDataSetService : IBasicDataSetService
    {
        private readonly ILogger<BasicDataSetService> log;
        private readonly IWebHostEnvironment env;

        private readonly ISkillService _skillService;
        private readonly ILanguageService _languageService;
        private readonly IFieldService _fildService;
        private readonly IRoleService _roleService;

        public BasicDataSetService(ILogger<BasicDataSetService> logger, IWebHostEnvironment environment,
                                   ISkillService skillService,
                                   ILanguageService languageService,
                                   IRoleService roleService,
                                   IFieldService fildService)
        {
            log = logger;
            env = environment;
            _skillService = skillService;
            _languageService = languageService;
            _fildService = fildService;
            _roleService = roleService;
        }


        //---------------------------------IBasicDataSetService------------------------------------
        //-----------------------------------------------------------------------------------------
        //---------------------------------Buissines Logic-----------------------------------------
        //-----------------------------------------------------------------------------------------
        // used by Layer above      for more information see    IBasicDataSetService Definition
        // ChangeResultEvent from  ValidateUpdate() gets   passed through
        public string ShowCurrentDataSet()
        {
            var path = GetLatestPersistence().PhysicalPath;
            var content = File.ReadAllText(path);

            content = ValidateUpdate(content);
            return content;
        }


        public async Task<string> ShowBrowserFileAsync(IBrowserFile browserFile)
        {
            var uncheckedString = await (new StreamReader(browserFile.OpenReadStream()))
                                        .ReadToEndAsync();
            var checkedString = ValidateUpdate(uncheckedString);
            return checkedString;
        }


        public string ValidateUpdate(string json)
        {
            infoMessages = new();
            errorMessages = new();

            var tree = Deserialize(json);
            if (!errorMessages.Any())
                tree = MarkDouble(tree);
            if (!errorMessages.Any())
                json = Format(Serialize(tree));

            OnChange(new() { InfoMessages = infoMessages, ErrorMessages = errorMessages });
            return json;
        }


        public void Update(string json)
        {
            json = ValidateUpdate(json);
            var oldVersion = File.ReadAllText(GetLatestPersistence().PhysicalPath);
            if (errorMessages.Any())
                return;

            if (json != oldVersion)
                UpdatePersistence(json);
            else
                OnChange(new() { SuccesMessage = "Keine Änderungen zu übernehmen." });

            UpdateOtherServices(json);
        }



        // my alternative json intedation Style
        private static string Format(string json)
        {
            var result = json;
            var pattern = @"(\[)\s*([^\n\]]*)\s*";
            var replacement = @"$1$2";
            for (int i = 0; i < 200; i++)
                result = Regex.Replace(result, pattern, replacement);
            return result;
        }

        //-----------------------------------------------------------------------------------------
        //----------------------------------  Validation   ----------------------------------------
        // will set the hasDouble bool
        private BasicDataNode MarkDouble(BasicDataNode treeRoot)
        {
            if (treeRoot == null)
            {
                log.LogCritical($"Internal Service faliure while Double Check \n"); //called with null
                return treeRoot;
            }
            List<(BasicDataLeaf pointer, string cat)> allSkills = ListAll(treeRoot);
            allSkills = allSkills.OrderBy(x => x.pointer.Name).ThenBy(x => x.cat).ToList();

            (BasicDataLeaf pointer, string cat) previous = allSkills.Last();
            foreach (var item in allSkills)
            {
                var previousName = previous.pointer.Name;
                var currentName = item.pointer.Name;
                if (item.cat.Length > 35)
                    errorMessages.Add($"Für Kategorien nicht mehr als 35 Zeichen: {item.cat}");
                if (currentName.Length > 45)
                    errorMessages.Add($"Das Design beschränkt Skills auf 45 Zeichen: {currentName}");

                if (previousName.ToLower().Equals(currentName.ToLower()))
                {
                    if (item.cat.ToLower().Equals(previous.cat.ToLower()))
                        errorMessages.Add($"Inakzeptabels Dupllikat \n\t\"{item.cat}\" \"{currentName}\" \n\t\"{previous.cat}\" \"{previousName}\" ");
                    else
                    {
                        infoMessages.Add($"Akzeptabel Dupllicat \n\t\"{item.cat}\" \"{currentName}\" \n\t\"{previous.cat}\" \"{previousName}\" ");
                        previous.pointer.HasDouble = true;
                        item.pointer.HasDouble = true;
                    }
                }
                previous = item;
            }

            var allCat = allSkills.Select(x => x.cat).ToHashSet();
            infoMessages.Add($"{allSkills.Count} Fähigkeiten in {allCat.Count} Kategorien.");
            return treeRoot;
        }

        // combines a BasicDataNode-Tree to alist of Skills
        private List<(BasicDataLeaf pointer, string cat)> ListAll(BasicDataNode tree)
        {
            List<(BasicDataLeaf pointer, string cat)> allSkills = new();

            if (tree.Children.First() is BasicDataNode)     // if recursion
                foreach (BasicDataNode node in tree.Children)
                    allSkills = allSkills.Concat(ListAll(node)).ToList();
            else
                foreach (BasicDataLeaf leaf in tree.Children)
                    allSkills.Add((leaf, tree.Name));
            return allSkills;
        }

        // same as above but the (inputParameter: tree) will be  modified afterwards
        private List<(BasicDataLeaf pointer, string cat, string[] posibLvl)> ListAndHeritage(BasicDataNode tree)
        {
            List<(BasicDataLeaf pointer, string cat, string[] posibLvl)> allskills = new();

            if (tree.Children.First() is BasicDataNode)     // if recursion
                foreach (BasicDataNode node in tree.Children)
                {
                    node.LevelNames = node.LevelNames.Any() ? node.LevelNames : tree.LevelNames;
                    allskills = allskills.Concat(ListAndHeritage(node)).ToList();
                }
            else
                foreach (BasicDataLeaf leaf in tree.Children)
                    allskills.Add((leaf, tree.Name, tree.LevelNames));
            return allskills;
        }
        //----------------------------------------------------------------------------------------


        //-----------------------------------------------------------------------------------------
        private void UpdateOtherServices(string json)
        {
            var treeRoot = Deserialize(json);
            IEnumerable<Skill> hardSkills = new List<Skill>();
            IEnumerable<Skill> softSkills = new List<Skill>();
            IEnumerable<Field> fields = new List<Field>();
            IEnumerable<Language> languages = new List<Language>();
            IEnumerable<Role> roles = new List<Role>();

            treeRoot = MarkDouble(treeRoot);

            foreach (BasicDataNode firstCategory in treeRoot.Children)
            {
                if (firstCategory.Name is "Skills")
                    hardSkills = ListAndHeritage(firstCategory).Select(x => new Skill()
                    {
                        Name = x.pointer.Name,
                        Category = x.cat,
                        HasDouble = x.pointer.HasDouble,
                        PossibleLevels = x.posibLvl,
                        Type = SkillGroup.Hardskill
                    });
                if (firstCategory.Name is "Softskills")
                    softSkills = ListAndHeritage(firstCategory).Select(x => new Skill()
                    {
                        Name = x.pointer.Name,
                        HasDouble = x.pointer.HasDouble,
                        PossibleLevels = x.posibLvl,
                        Type = SkillGroup.Softskill
                    });

                if (firstCategory.Name is "Felder")
                    fields = ListAndHeritage(firstCategory).Select(x => new Field() { Name = x.pointer.Name });

                if (firstCategory.Name is "Sprachen")
                    languages = ListAndHeritage(firstCategory).Select((x) => new Language()
                    {
                        Name = x.pointer.Name,
                        PossibleLevels = x.posibLvl,
                    });
                if (firstCategory.Name is "Rollen")
                    roles = ListAndHeritage(firstCategory).Select((x) => new Role() { Name = x.pointer.Name });
            }
            hardSkills = hardSkills.Concat(softSkills).ToList();
            _skillService.UpdateAllSkills(hardSkills);

            _fildService.UpdateAllFields(fields);
            _languageService.UpdateAllLanguages(languages);
            _roleService.UpdateAllRoles(roles);

        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------





        //-------------------------------------Persistence-----------------------------------------with json
        //-----------------------------------------------------------------------------------------
        private readonly string subDirecory = "jsonPersistierung";
        private readonly string prefix = "datenbasisV";
        private readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new InnerNodeConverter(), new LeafConvert() } };

        //--read   --------------------------------------------------------------------------------
        // might return null   uses the custom JsonConverter and  tries to deserialize the input
        private BasicDataNode Deserialize(string json)
        {
            BasicDataNode treeRoot = null;
            try
            {
                treeRoot = JsonSerializer.Deserialize<BasicDataNode>(json, options);
            }
            catch (Exception e)
            {
                var shortException = e.Message[..(e.Message.Length < 50 ? e.Message.Length : 50)];
                errorMessages.Add($"Es fiel auf:  {shortException} ");
            }
            return treeRoot;
        }
        private IFileInfo GetLatestPersistence()
        {
            var file = env.ContentRootFileProvider.GetDirectoryContents(subDirecory)
                                                         .Where(x => x.Name.StartsWith(prefix))
                                                         .OrderByDescending(x => Int32.Parse(x.Name.Replace(prefix, "").Replace(".json", "")))
                                                         .FirstOrDefault();
            if (file == null)
                throw new Exception("Not one BasicDataSet in Persistence \n");
            return file;
        }


        //---write --------------------------------------------------------------------------------
        private string Serialize(BasicDataNode tree) => JsonSerializer.Serialize(tree, options);
        public void UpdatePersistence(string json)
        {
            var name = GetLatestPersistence().Name;
            int newNumber = Int32.Parse(name.Replace(prefix, "").Replace(".json", "")) + 1;
            name = $"{prefix}{newNumber}.json";
            var path = Path.Combine(env.ContentRootPath, subDirecory, name);
            File.WriteAllText(path, json, Encoding.UTF8);
            OnChange(new() { SuccesMessage = $"Update erfolgreich. - Gespeichert unter: {name}", InfoMessages = infoMessages });
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------






        //----------------keeps delegates to send Events to the calling Layer above----------------
        //-----------------------------------------------------------------------------------------
        private List<string> errorMessages = new();
        private List<string> infoMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }

}
