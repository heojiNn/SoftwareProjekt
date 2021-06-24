using System.Collections.Generic;



namespace XCV.Data
{
    public interface ILanguageService
    {
        // Summary:
        //     GetAllLanguages from Persistece.
        // Returns:
        //     A List of Languages that might be  empty
        //
        // Exceptions:
        //   Exception:
        //     Could not reach Persistence: {subPath}/{fileName}
        public IEnumerable<Language> GetAllLanguages();
        public string[] GetAllLevel();

        // Summary:
        //     UpdateAllLanguages in Persistece.
        // Parameters:
        //   languages:
        //     The languages to replace the old ones.
        //
        // Loges:
        //   LogInformation:
        //     All Language updated  Persitence  {fileName}
        public (int added, int removed) UpdateAllLanguages(IEnumerable<Language> languages);
        public (int added, int removed) UpdateAllLevels(string[] levels);
    }
}
