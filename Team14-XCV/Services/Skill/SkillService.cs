using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;



namespace Team14.Data
{
    public class SkillService : ISkillService
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<SkillService> _logger;
        public SkillService(IWebHostEnvironment environment, ILogger<SkillService> logger)
        {
            env = environment;
            _logger = logger;
        }


        public IEnumerable<Skill> GetSkillsStarWith(string name)
        {
            var skills = GetAllSkills().Where(x => x.Name.StartsWith(name.ToLower()) || x.Name.StartsWith(name.ToUpper())).OrderBy(x => x.Name); ;
            if (!skills.Any())
                OnEmptyResult(new() { Message = $"kein Skill erfülllt das Kriterium \"{name}\"" });
            return skills;
        }



        public void UpdateSkill(IEnumerable<Skill> skills)
        {
            var json = Serialize(skills);
            var path = Path.Combine(env.ContentRootPath, subPath, $"skills.json");
            _logger.LogWarning($"Skill Persitenz wurde geschrieben  {path}");
            File.WriteAllText(path, json, Encoding.UTF8);
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------






        //-------------------------------------Persistence-----------------------------------------
        //---with json-----------------------------------------------------------------------------
        //-------------------------------------Persistence-----------------------------------------
        public IEnumerable<Skill> GetAllSkills()
        {
            string path = "";
            try
            {
                path = env.ContentRootFileProvider.GetDirectoryContents(subPath)
                                         .Where(x => x.Name.StartsWith(fileName))
                                         .First()
                                         .PhysicalPath;
            }
            catch (IOException e)
            {
                _logger.LogError($"IO Persistense Exception: \t{e.Message}");
                OnEmptyResult(new() { Message = "Skill s konnten nicht geladen werden" });
            }
            catch (Exception e) { _logger.LogError($"UNEXPECTED Exception{e.Message}"); }

            return Deserialize(File.ReadAllText(path)); ;
        }
        //-------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------
        private readonly string subPath = Path.Combine("jsonPersistierung");
        private readonly string fileName = "skills";
        readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, };
        //-------------------------------------------------------------------------------------------------------------
        private string Serialize(IEnumerable<Skill> skills) => JsonSerializer.Serialize(skills, options);
        private IEnumerable<Skill> Deserialize(string json) => JsonSerializer.Deserialize<IEnumerable<Skill>>(json, options);

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------




        public event EventHandler<NoResult> SearchEventHandel;
        protected virtual void OnEmptyResult(NoResult e)
        {
            EventHandler<NoResult> handler = SearchEventHandel;
            handler?.Invoke(this, e);
        }

    }
}
