using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface ILanguageService
    {
        /// <summary>
        ///         Validates against DataAnotations on the  Language-Entity
        /// </summary>
        ///
        /// <returns>
        ///          ErrorMessages
        /// </returns>
        public IEnumerable<string> ValidateLanguages(IEnumerable<Language> languages);



        /// <summary>
        ///         GetAllLanguages from persistence.
        /// </summary>
        /// <remarks>
        ///         Catches all SqlExceptions, logs an Error: {e.Message}
        ///         and will just return an empty collection
        /// </remarks>
        ///
        /// <returns>
        ///          A collection of Languages, that might be empty.
        /// </returns>
        public IEnumerable<Language> GetAllLanguages();


        /// <summary>
        ///         GetAllLevel correctly orderd from persistence.
        /// </summary>
        ///
        /// <returns>
        ///          In case of SqlExcep., an empty array
        /// </returns>
        public string[] GetAllLevel();


        /// <summary>
        ///         UpdateAllLanguages in persistence.
        /// </summary>
        /// <remarks>
        ///         uses ValidateLanguages(roles),
        ///         catches SqlExceptions and logs them
        /// </remarks>
        ///
        /// <param name="languages">
        ///         The languages to replace the old ones.
        /// </param>
        ///
        /// <event cref="OnChange">
        ///     Succes: {languages}: wurde entfernt\hinzugefügt.
        /// or
        ///     Error:  {languages}: Sprachen benötigen einen Namen / darf 50 Zeichen ...
        ///</event>
        public (int added, int removed) UpdateAllLanguages(IEnumerable<Language> languages);


        /// <summary>
        ///         UpdateAllLevels in persistence.
        /// </summary>
        ///
        /// <param name="levels">
        ///         The ordered levels
        /// </param>
        public (int added, int removed) UpdateAllLevels(string[] levels);



        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
