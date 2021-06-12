using System;
using System.Collections.Generic;



namespace Team14.Data
{
    public interface ILanguageService
    {
        public IEnumerable<Language> GetAllLanguages();


        public void UpdateLanguage(IEnumerable<Language> skills);


        public event EventHandler<NoResult> SearchEventHandel;
    }

}
