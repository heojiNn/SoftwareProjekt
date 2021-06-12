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


        //-----------------------------------------------------------------------------------------
        //---------------------------------IBasicDataSetService------------------------------------
        // used by Layer above      for more information see    IBasicDataSetService Definition
        // ChangeResultEvent from  DeserializeCheck() gets   passed through
        public string ShowCurrentDataSetAndCheck()
        {
            var path = GetLatestPersistence().PhysicalPath;
            var content = File.ReadAllText(path);
            content = UpdateRoundtripCheck(content);
            return content;
        }

        // used by Layer above      for more information see    IBasicDataSetService
        // ChangeResultEvent   gets   passed through
        public async Task<string> ShowBrowserFileAsync(IBrowserFile browserFile)
        {
            var uncheckedString = await (new StreamReader(browserFile.OpenReadStream()))
                                        .ReadToEndAsync();
            var checkedString = UpdateRoundtripCheck(uncheckedString);
            return checkedString;
        }
        // used by Layer above      for more information see    IBasicDataSetService
        // ChangeResultEvent   gets   passed through
        public string UpdateRoundtripCheck(string json)
        {
            changeMessages = new();
            errorMessage = "";
            var cleanTree = DeserializeCheck(json);
            if (errorMessage.Equals(""))
                cleanTree = MarkDoubleCheck(cleanTree);
            if (errorMessage.Equals(""))
                SerializeAndFormat(cleanTree);

            OnChange(new() { InfoMessages = changeMessages, ErrorMessage = errorMessage });
            return json;
        }




        // catches Exceptions from the Persitence
        // and converts them to Events for the Layer above
        private BasicDataNode DeserializeCheck(string json)
        {
            BasicDataNode treeRoot = null;
            try
            {
                treeRoot = Deserialize(json);
            }
            catch (JsonException e)
            {
                var shortException = e.Message[..(e.Message.Length < 60 ? e.Message.Length : 60)];
                errorMessage = $"Beachten Sie:  {shortException}  im json";
            }
            catch (Exception e)
            {
                errorMessage = $"unerwartete Exception{e.GetType()}{e.Message}";
            }
            return treeRoot;
        }


        // my alternative json intedation Style
        private string SerializeAndFormat(BasicDataNode tree)
        {
            var s = Serialize(tree);
            var result = s;
            var pattern = @"(\[)\s*([^\n\]]*)\s*";
            var replacement = @"$1$2";
            for (int i = 0; i < 200; i++)
                result = Regex.Replace(result, pattern, replacement);
            return result;
        }



        //-----------------------------------------------------------------------------------------
        //----------------------------------  Validation   ----------------------------------------
        // will set the hasDouble bool
        private BasicDataNode MarkDoubleCheck(BasicDataNode treeRoot)
        {
            if (treeRoot == null)
                return treeRoot;

            List<(BasicDataLeaf pointer, string cat, IEnumerable<string> pos)> allskills = Combine(treeRoot);
            allskills = allskills.OrderBy(x => x.pointer.Name).ThenBy(x => x.cat).ToList();

            (BasicDataLeaf pointer, string cat, IEnumerable<string> pos) previous = (new BasicDataLeaf(), "", new List<string>());
            foreach (var item in allskills)
            {
                var previousName = previous.pointer.Name.ToLower();
                var currentName = item.pointer.Name.ToLower();
                if (previousName.Equals(currentName))
                {
                    if (item.cat.ToLower().Equals(previous.cat.ToLower()))
                    {
                        errorMessage = $"Dupllicat  \"{item.cat}-{currentName} \" und \"{previous.cat}-{previousName}\" inakzeptabel";
                        log.LogError($"duplicat");
                        return treeRoot;
                    }
                    else
                    {
                        changeMessages.Add($"Dupllicat  \"{item.cat}-{currentName} \"-\"{previous.cat} \" und \"{previousName}\" akzeptabel");
                        previous.pointer.HasDouble = true;
                        item.pointer.HasDouble = true;
                    }
                }
                previous = item;
            }
            return treeRoot;
        }

        //----------------------------------------------------------------------------------------
        private List<(BasicDataLeaf pointer, string cat, IEnumerable<string> pos)> Combine(BasicDataNode tree)
        {
            List<(BasicDataLeaf pointer, string cat, IEnumerable<string> pos)> allskills = new();
            if (tree.Children.First() is BasicDataNode)
                foreach (BasicDataNode node in tree.Children)
                {
                    //Console.WriteLine($"{node.LevelNames.Any()}nim {lenames}      sonst  {trnames}       {node.Name}");
                    node.LevelNames = node.LevelNames.Any() ? node.LevelNames : tree.LevelNames;
                    allskills = allskills.Concat(Combine(node)).ToList();
                }
            else
                foreach (BasicDataLeaf leaf in tree.Children)
                    allskills.Add((leaf, tree.Name, tree.LevelNames));
            return allskills;
        }



        //-----------------------------------------------------------------------------------------
        private void UpdateOtherServices(BasicDataNode treeRoot)
        {
            IEnumerable<Skill> hardSkills = new List<Skill>();
            IEnumerable<Skill> softSkills = new List<Skill>();
            IEnumerable<Field> fields = new List<Field>();
            IEnumerable<Language> languages = new List<Language>();
            IEnumerable<Role> roles = new List<Role>();


            foreach (BasicDataNode firstCategory in treeRoot.Children)
            {
                if (firstCategory.Name is "Skills")
                    hardSkills = Combine(firstCategory).Select(x => new Skill()
                    {
                        Name = x.pointer.Name,
                        Category = x.cat,
                        HasDouble = x.pointer.HasDouble,
                        PossibleLevels = x.pos.ToArray(),
                        Type = SkillGroup.Hardskill
                    });
                if (firstCategory.Name is "Softskills")
                    softSkills = Combine(firstCategory).Select(x => new Skill()
                    {
                        Name = x.pointer.Name,
                        Category = "menschliches",
                        HasDouble = x.pointer.HasDouble,
                        PossibleLevels = x.pos.ToArray(),
                        Type = SkillGroup.Softskill
                    });

                if (firstCategory.Name is "Felder")
                    fields = Combine(firstCategory).Select(x => new Field() { Name = x.pointer.Name });

                if (firstCategory.Name is "Sprachen")
                    languages = Combine(firstCategory).Select((x) => new Language()
                    {
                        Name = x.pointer.Name,
                        PossibleLevels = x.pos.ToArray(),
                    });
                if (firstCategory.Name is "Rollen")
                    roles = Combine(firstCategory).Select((x) => new Role() { Name = x.pointer.Name });


            }
            hardSkills = hardSkills.Concat(softSkills).ToList();
            _skillService.UpdateSkill(hardSkills); ;             // aktuallisiert  SkillServices
            _fildService.UpdateField(fields); ;                  // aktuallisiert  fields
            _languageService.UpdateLanguage(languages); ;        // aktuallisiert  languages
            _roleService.UpdateRole(roles); ;        // aktuallisiert  roles

        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------


        public void CommitUpdate(string json, bool newVers)
        {
            var cleanTree = DeserializeCheck(json);
            if (newVers)
            {
                var checkedDocument = SerializeAndFormat(cleanTree);
                if (!errorMessage.Equals(""))
                    return;
                var name = GetLatestPersistence().Name;
                int newNumber = Int32.Parse(name.Replace(prefix, "").Replace(".json", "")) + 1;
                name = $"{prefix}{newNumber}.json";
                var path = Path.Combine(env.ContentRootPath, subDirecory, name);
                File.WriteAllText(path, checkedDocument, Encoding.UTF8);
                OnChange(new() { SuccesMessage = $"Erfolg! die neue datenbasis heißt: {name}", InfoMessages = changeMessages });
            }
            UpdateOtherServices(cleanTree);
        }


        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------
        //---with json-----------------------------------------------------------------------------
        private readonly string subDirecory = "jsonPersistierung";
        private readonly string prefix = "datenbasisVers"; //datenbasisVers1..10.json
        readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, ReadCommentHandling = JsonCommentHandling.Skip, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new InnerNodeConverter(), new LeafConvert() } };

        private string Serialize(BasicDataNode tree) => JsonSerializer.Serialize(tree, options);
        private BasicDataNode Deserialize(string json) => JsonSerializer.Deserialize<BasicDataNode>(json, options);
        private IFileInfo GetLatestPersistence()
        {
            return env.ContentRootFileProvider.GetDirectoryContents(subDirecory)
                                                          .Where(x => x.Name.StartsWith(prefix))
                                                          .OrderByDescending(x => Int32.Parse(x.Name.Replace(prefix, "").Replace(".json", "")))
                                                          .First();
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------






        //----------------keeps delegates to send Events to the calling Layer above----------------
        //-----------------------------------------------------------------------------------------
        private string errorMessage = "";
        private List<string> changeMessages = new();

        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e)
        {
            EventHandler<ChangeResult> handler = ChangeEventHandel;
            handler?.Invoke(this, e);
        }
    }

}
