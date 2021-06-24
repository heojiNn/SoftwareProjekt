using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace XCV.Data
{
    public class LanguageService : ILanguageService
    {
        private readonly string connectionString;
        private readonly ILogger<LanguageService> log;
        public LanguageService(IConfiguration config, ILogger<LanguageService> logger)
        {
            log = logger;
            connectionString = config.GetConnectionString("MyLocalConnection");
        }


        //---------------------------------Buissines Logic-----------------------------------------
        // empty







        //-------------------------------------Persistence-----------------------------------------
        //-----------------------------------------------------------------------------------------

        //--read   --------------------------------------------------------------------------------
        public IEnumerable<Language> GetAllLanguages()
        {
            IEnumerable<Language> languages = new List<Language>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                languages = con.Query<Language>("Select * From Language_Name");
                con.Close();
            }
            catch (SqlException e)
            {
                log.LogError($" retrieving Languages from database: {e.Message} \n");
            }
            return languages;
        }
        public string[] GetAllLevel()
        {
            var lvl = System.Array.Empty<string>();
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                lvl = con.Query<string>("Select Name From Language_Level Order By Level").ToArray();
                con.Close();
            }
            catch (SqlException e)
            {
                log.LogError($" retrieving Fields from database: {e.Message} \n");
            }
            return lvl;
        }

        //---write --------------------------------------------------------------------------------
        public (int added, int removed) UpdateAllLanguages(IEnumerable<Language> languages)
        {
            int addedRows = 0;
            int removedRows = 0;
            var newLanguages = languages.ToImmutableSortedSet();
            var oldLanguages = GetAllLanguages().ToImmutableSortedSet();
            var toAdd = newLanguages.Except(oldLanguages);
            var toRemove = oldLanguages.Except(newLanguages);

            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                foreach (var language in toAdd)
                    addedRows += con.Execute("Insert Into Language_Name Values (@Name)", new { language.Name });
                foreach (var language in toRemove)
                    removedRows += con.Execute("Delete From Language_Name Where Name = @Name", new { language.Name });
            }
            catch (SqlException e)
            {
                log.LogError($"Error updating Languages in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
            return (addedRows, removedRows);
        }
        public (int added, int removed) UpdateAllLevels(string[] levels)
        {
            int addedRows = 0;
            int removedRows = 0;
            if (GetAllLevel().SequenceEqual(levels))
                return (0, 0);
            using var con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                removedRows = con.Execute($"Delete From Language_Level");
                for (int i = 0; i < levels.Length; i++)
                    addedRows += con.Execute($"Insert Into Language_Level (Name, Level) Values ('{levels[i]}', {i})");
            }
            catch (SqlException e)
            {
                log.LogError($"Error updating Language Levels in database: {e.Message} \n");
            }
            finally
            {
                con.Close();
            }
            return (addedRows, removedRows);
        }
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
    }
}
