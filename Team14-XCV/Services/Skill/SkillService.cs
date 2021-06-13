using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;



namespace Team14.Data
{
    public class SkillService : ISkillService
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<SkillService> log;
        public SkillService(IWebHostEnvironment environment, ILogger<SkillService> logger)
        {
            env = environment;
            log = logger;
        }


        //---------------------------------Buissines Logic-----------------------------------------
        //-----------------------------------------------------------------------------------------
        public IEnumerable<Skill> GetSkillsStarWith(string name)
        {
            IEnumerable<Skill> skills = new List<Skill>();
            skills = GetAllSkills().Where(x => x.Name.ToLower().StartsWith(name.ToLower()) || x.Name.ToUpper().StartsWith(name.ToUpper())).OrderBy(x => x.Name);
            return skills;
        }




        //-------------------------------------Persistence-----------------------------------------with json
        //-----------------------------------------------------------------------------------------
        private readonly string subPath = Path.Combine("jsonPersistierung");
        private readonly string fileName = "skills.json";
        private readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new JsonStringEnumConverter() } };

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Skill> GetAllSkills()
        {
            var file = env.ContentRootFileProvider.GetDirectoryContents(subPath)
                                     .Where(x => x.Name.Equals(fileName))
                                     .FirstOrDefault();
            if (file == null)
                throw new Exception($"Could not reach Persistence: {subPath}/{fileName} \n");
            return Deserialize(File.ReadAllText(file.PhysicalPath)); ;
        }
        private IEnumerable<Skill> Deserialize(string json) => JsonSerializer.Deserialize<IEnumerable<Skill>>(json, options);



        //---write --------------------------------------------------------------------------------
        public void UpdateAllSkills(IEnumerable<Skill> skills)
        {
            var json = Serialize(skills);
            var path = Path.Combine(env.ContentRootPath, subPath, fileName);
            File.WriteAllText(path, json, Encoding.UTF8);

            log.LogInformation($"All Skill updated  Persitence  {fileName}");
        }
        private string Serialize(IEnumerable<Skill> skills) => JsonSerializer.Serialize(skills, options);

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
