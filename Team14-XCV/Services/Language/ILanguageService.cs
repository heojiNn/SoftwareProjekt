using System;
using System.Collections.Generic;



namespace Team14.Data
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


        // Summary:
        //     UpdateAllLanguages in Persistece.
        // Parameters:
        //   roles:
        //     The roles to replace the old ones.
        //
        // Loges:
        //   LogInformation:
        //     All Language updated  Persitence  {fileName}
        public void UpdateAllLanguages(IEnumerable<Language> roles);
    }
}
