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
    public class FieldService : IFieldService
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<FieldService> log;
        public FieldService(IWebHostEnvironment environment, ILogger<FieldService> logger)
        {
            env = environment;
            log = logger;
        }


        //---------------------------------Buissines Logic-----------------------------------------
        // empt



        //-------------------------------------Persistence-----------------------------------------with json
        //-----------------------------------------------------------------------------------------
        private readonly string subPath = Path.Combine("jsonPersistierung");
        private readonly string fileName = "fields.json";
        private readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new JsonStringEnumConverter() } };

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Field> GetAllFields()
        {
            var file = env.ContentRootFileProvider.GetDirectoryContents(subPath)
                                     .Where(x => x.Name.Equals(fileName))
                                     .FirstOrDefault();
            if (file == null)
                throw new Exception($"Could not reach Persistence: {subPath}/{fileName} \n");
            return Deserialize(File.ReadAllText(file.PhysicalPath)); ;
        }
        private IEnumerable<Field> Deserialize(string json) => JsonSerializer.Deserialize<IEnumerable<Field>>(json, options);



        //---write --------------------------------------------------------------------------------
        public void UpdateAllFields(IEnumerable<Field> fields)
        {
            var json = Serialize(fields);
            var path = Path.Combine(env.ContentRootPath, subPath, fileName);
            File.WriteAllText(path, json, Encoding.UTF8);

            log.LogInformation($"All Field updated  Persitence  {fileName}");
        }
        private string Serialize(IEnumerable<Field> fields) => JsonSerializer.Serialize(fields, options);

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
