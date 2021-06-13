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
    public class RoleService : IRoleService
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<RoleService> log;
        public RoleService(IWebHostEnvironment environment, ILogger<RoleService> logger)
        {
            env = environment;
            log = logger;
        }


        //---------------------------------Buissines Logic-----------------------------------------
        // empt



        //-------------------------------------Persistence-----------------------------------------with json
        //-----------------------------------------------------------------------------------------
        private readonly string subPath = Path.Combine("jsonPersistierung");
        private readonly string fileName = "roles.json";
        private readonly JsonSerializerOptions options = new() { IgnoreNullValues = true, IgnoreReadOnlyProperties = true, IncludeFields = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true, Converters = { new JsonStringEnumConverter() } };

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Role> GetAllRoles()
        {
            var file = env.ContentRootFileProvider.GetDirectoryContents(subPath)
                                     .Where(x => x.Name.Equals(fileName))
                                     .FirstOrDefault();
            if (file == null)
                throw new Exception($"Could not reach Persistence: {subPath}/{fileName} \n");
            return Deserialize(File.ReadAllText(file.PhysicalPath)); ;
        }
        private IEnumerable<Role> Deserialize(string json) => JsonSerializer.Deserialize<IEnumerable<Role>>(json, options);



        //---write --------------------------------------------------------------------------------
        public void UpdateAllRoles(IEnumerable<Role> roles)
        {
            var json = Serialize(roles);
            var path = Path.Combine(env.ContentRootPath, subPath, fileName);
            File.WriteAllText(path, json, Encoding.UTF8);

            log.LogInformation($"All Role updated  Persitence  {fileName}");
        }
        private string Serialize(IEnumerable<Role> roles) => JsonSerializer.Serialize(roles, options);

        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
