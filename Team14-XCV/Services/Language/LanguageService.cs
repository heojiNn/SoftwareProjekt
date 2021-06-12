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
    public class LanguageService : ILanguageService
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<LanguageService> log;
        public LanguageService(IWebHostEnvironment environment, ILogger<LanguageService> logger)
        {
            env = environment;
            log = logger;
        }




        public void UpdateLanguage(IEnumerable<Language> languages)
        {
            var json = Serialize(languages);
            var path = Path.Combine(env.ContentRootPath, subPath, $"languages.json");
            log.LogWarning($"Language Persitenz wurde geschrieben  {path}");
            File.WriteAllText(path, json, Encoding.UTF8);
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------





        //-------------------------------------Persistence-----------------------------------------
        public IEnumerable<Language> GetAllLanguages()
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
                log.LogError($"IO Persistense Exception: \t{e.Message}");
                OnEmptyResult(new() { Message = "Languages konnten nicht geladen werden" });
            }
            catch (Exception e) { log.LogError($"UNEXPECTED Exception{e.Message}"); }

            return Deserialize(File.ReadAllText(path)); ;
        }

        private readonly string subPath = Path.Combine("jsonPersistierung");
        private readonly string fileName = "languages";
        readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, };
        //-------------------------------------------------------------------------------------------------------------
        private string Serialize(IEnumerable<Language> languages) => JsonSerializer.Serialize(languages, options);
        private IEnumerable<Language> Deserialize(string json) => JsonSerializer.Deserialize<IEnumerable<Language>>(json, options);

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
