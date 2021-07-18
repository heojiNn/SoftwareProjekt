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




        // for definition see   ILanguageService
        public IEnumerable<Language> GetAllLanguages()
        {
            IEnumerable<Language> languages = new List<Language>();
            //-------------------------------------------------------------------------MS-SQL-Query
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                languages = con.Query<Language>("Select * From [Language]");
            }
            catch (SqlException e)
            {
                log.LogError($"GetAllLanguages() persistence Error: \n{e.Message}\n"); //log Fail
            }
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            return languages;
        }
        // for definition see   ILanguageService
        public string[] GetAllLevel()
        {
            var lvl = Array.Empty<string>();
            //-------------------------------------------------------------------------MS-SQL-Query
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                lvl = con.Query<string>("Select [Name] From [Language_Level] Order By [Level]").ToArray();
            }
            catch (SqlException e)
            {
                log.LogError($"GetAllLevel() persistence Error: \n{e.Message}\n"); //log Fail
            }
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            return lvl;
        }

        // for definition see   ILanguageService
        public int CreateLanguage(Language toAdd, bool justValidate = false)
        {
            errorMessages = new();
            int changedRows = 0;
            //---------------------------------------------------------------------------Validation
            toAdd.Name = toAdd.Name.Trim();
            ValidateDataAno(toAdd);
            if (GetAllLanguages().Any(x => x.Name.ToLower() == toAdd.Name.ToLower()))
                errorMessages.Add($"({toAdd.Name}), kann nicht hinzugefügt werden, da sie schon enthalten ist.");

            if (errorMessages.Any() || justValidate)
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return 0;
            }
            //-------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------MS-SQL-Command
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                changedRows += con.Execute("Insert Into [Language] Values (@Name)", toAdd);
            }
            catch (SqlException e) { log.LogError($"CreateLanguage() persistence Error: \n{e.Message}\n"); } //log Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            OnChange(new() { SuccesMessage = $"({toAdd.Name}), wurde zu den Sprachen hinzugefügt." }); //SuccesM.
            return changedRows;
        }

        // for definition see   ILanguageService
        public int DeleteLanguage(Language toRemove)
        {
            int changedRows = 0;
            //---------------------------------------------------------------------------Validation
            if (!GetAllLanguages().Contains(toRemove))
            {
                OnChange(new() { ErrorMessages = new[] { $"({toRemove.Name}), kann nicht entfernt werden, da sie nicht (mehr) enthalten ist." } });
                return changedRows;
            }
            //-------------------------------------------------------------------------------------
            //-----------------------------------------------------------------------MS-SQL-Command
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                changedRows += con.Execute("Delete From [Language] Where [Name] = @Name", toRemove);
            }
            catch (SqlException e) { log.LogError($"DeleteLanguage() persistence Error: \n{e.Message}\n"); }   //log Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            OnChange(new() { SuccesMessage = $"({toRemove.Name}), wurde aus den Sprachen entfernt." }); //SuccesM.
            return changedRows;
        }



        // for definition see   ILanguageService
        public (int added, int removed) UpdateAllLanguages(IEnumerable<Language> languages, bool justValidate = false)
        {
            errorMessages = new();
            int addedRows = 0;
            int removedRows = 0;
            var oldLanguages = GetAllLanguages();
            var toAdd = languages.Except(oldLanguages);
            var toRemove = oldLanguages.Except(languages);
            //---------------------------------------------------------------------------Validation
            foreach (var language in toAdd)
            {
                language.Name = language.Name.Trim();
                ValidateDataAno(language);
            }
            if (errorMessages.Any() || justValidate)
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return (0, 0);
            }
            //-------------------------------------------------------------------------------------

            foreach (var a in toAdd)
                addedRows += CreateLanguage(a);
            foreach (var r in toRemove)
                removedRows += DeleteLanguage(r);


            var succes = "";
            if (addedRows > 5)
                succes += $"{string.Join(", ", toAdd.Select(x=>x.Name))} wurden hinzugefügt.\n";
            else if (addedRows > 0)
                succes += $"{addedRows} Sprachen wurden hinzugefügt.\n";
            if (removedRows > 5)
                succes += $"{string.Join(", ", toRemove.Select(x=>x.Name))} wurden entfernt.\n";
            else if (removedRows > 0)
                succes += $"{removedRows} Sprachen wurden entfernt.\n";
            OnChange(new() { SuccesMessage = succes });                             //SuccesMessage
            return (addedRows, removedRows);
        }

        public int UpdateAllLevels(string[] levels, bool justValidate = false)
        {
            errorMessages = new();
            int changed = 0;
            //---------------------------------------------------------------------------Validation
            if (levels.Select(x => x.Trim().ToLower()).Distinct().Count() != 6)
                errorMessages.Add( $"Diese Version ist auf 6 (unterscheidbare) Sprachlevel festgelegt.");
            if (levels.Any(x => x.Length > 30 || x.Length < 2))
                errorMessages.Add($"Alle Sprachlevel benötigen zwischen 2 und 30 Zeichen.");
            if (GetAllLevel().SequenceEqual(levels) || justValidate || errorMessages.Any())
            {
                OnChange(new() { ErrorMessages = errorMessages });
                return 0;
            }
            //-------------------------------------------------------------------------------------
            levels = levels.Select(x => x.Trim()).ToArray();
            //-----------------------------------------------------------------------MS-SQL-Command
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                for (int i = 0; i < levels.Length; i++)
                    con.Execute($"Update [Language_Level]  Set [Name]={i}  Where Level={i + 1}"); // if order changes avoids conflicts cause unique(name)
                for (int i = 0; i < levels.Length; i++)
                    changed += con.Execute($"Update [Language_Level]  Set [Name]=@Name  Where Level={i + 1}", new { Name = levels[i] });
            }
            catch (SqlException e) { log.LogError($"UpdateAllLevels() persitence Error: \n{e.Message}\n"); } //log Fail
            finally { con.Close(); }
            //-------------------------------------------------------------------------------------
            OnChange(new() { SuccesMessage = $"Alle Sprachlevel wurden aktualisiert." }); //SuccesM.
            return changed;
        }



        /// <summary>
        ///         Validates against DataAnotation(.Name.Length &lt50 &amp&amp &lt1)
        /// </summary>
        /// <remarks>
        ///         used by Update and Create
        /// </remarks>
        private IEnumerable<string> ValidateDataAno(Language language)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(language, new ValidationContext(language), results, true))
                foreach (var result in results)
                    errorMessages.Add($"{result.ErrorMessage} ({language.Name})");
            return errorMessages;
        }


        //-------------------------------------User Messaging--------------------------------------
        private List<string> errorMessages = new();
        public event EventHandler<ChangeResult> ChangeEventHandel;
        protected virtual void OnChange(ChangeResult e) => ChangeEventHandel?.Invoke(this, e);
    }
}
