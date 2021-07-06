using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    /// <inheritdoc/>
    public class LanguageService : ILanguageService
    {
        private readonly string connectionString;
        private readonly ILogger<LanguageService> log;
        public LanguageService(IConfiguration config, ILogger<LanguageService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MS_SQL_Connection");
        }

        //-------------------------------------Business Logic--------------------------------------
        //-----------------------------------------------------------------------------------------
        // for definition see   ILanguageService
        public IEnumerable<string> ValidateLanguages(IEnumerable<Language> languages)
        {
            errorMessages = new();
            foreach (var language in languages)
            {
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(language, new ValidationContext(language), results, true))
                    foreach (var result in results)
                        errorMessages.Add($"{result.ErrorMessage} {language.Name}");
            }
            return errorMessages;
        }





        //-------------------------------------Persistence-----------------------------------------
        //--read   --------------------------------------------------------------------------------
        // for definition see   ILanguageService
        public IEnumerable<Language> GetAllLanguages()
        {
            IEnumerable<Language> languages = new List<Language>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                languages = con.Query<Language>("Select * From [Language]");
            }
            catch (SqlException e)
            {
                log.LogError($"GetAllLanguages() persistence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            return languages;
        }
        // for definition see   ILanguageService
        public string[] GetAllLevel()
        {
            var lvl = Array.Empty<string>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                lvl = con.Query<string>("Select [Name] From [Language_Level] Order By [Level]").ToArray();
            }
            catch (SqlException e)
            {
                log.LogError($"GetAllLevel() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            return lvl;
        }
        //---write --------------------------------------------------------------------------------
        public (int added, int removed) UpdateAllLanguages(IEnumerable<Language> languages)
        {
            int addedRows = 0;
            int removedRows = 0;
            var oldLanguages = GetAllLanguages();
            var toAdd = languages.Except(oldLanguages);
            var toRemove = oldLanguages.Except(languages);
            ValidateLanguages(toAdd);
            if (errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return (0, 0);    // if a new one(name) is invalid, nothing changes
            }
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                removedRows += con.Execute("Delete From [Language] Where [Name] = @Name", toRemove);
                addedRows += con.Execute("Insert Into [Language] Values (@Name)", toAdd);
            }
            catch (SqlException e)
            {
                log.LogError($"UpdateAllLanguages() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            var succes = "";
            if (toAdd.Any())
                succes += $"hinzugefügt wurden: {string.Join(", ", toAdd.Select(x => x.Name))} .";
            if (toRemove.Any())
                succes += $"entfernt wurden: {string.Join(", ", toRemove.Select(x => x.Name))} .";
            OnChange(new() { SuccesMessage = succes });
            return (addedRows, removedRows);
        }

        public (int added, int removed) UpdateAllLevels(string[] levels)
        {
            int addedRows = 0;
            int removedRows = 0;
            if (GetAllLevel().SequenceEqual(levels))
                return (0, 0);  // only update if someting is different

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                for (int i = 0; i < levels.Length; i++)
                    if (con.ExecuteScalar<int>("Select COUNT(*) From [Language_Level] Where [Name] = @Name ", new { Name = levels[i] }) > 0)
                        con.Execute($"Update [Language_Level]  Set [Level]={i + 100}  Where [Name] = @Name ", new { Name = levels[i] });
                    // move the ones to keep  (to not loos refences and avoid (unique)number-conflicts)
                    else
                        addedRows += con.Execute("Insert Into [Language_Level]  Values (@Level, @Name)", new { Level = i + 100, Name = levels[i] });
                removedRows = con.Execute("Delete From [Language_Level] Where [Level] BETWEEN 1 AND 99");
                for (int i = 0; i < levels.Length; i++)
                    con.Execute($"Update [Language_Level]  Set [Level]={i + 1}  Where [Name] = @Name ", new { Name = levels[i] });
                // and moves them back down
            }
            catch (SqlException e)
            {
                log.LogError($"UpdateAllLevels() persitence Error: \n{e.Message}\n");
            }
            finally { con.Close(); }
            return (addedRows, removedRows);
        }


        //-------------------------------------User Messaging--------------------------------------
        private List<string> errorMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
}
