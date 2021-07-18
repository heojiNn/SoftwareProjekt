using System;
using System.Collections.Generic;



namespace XCV.Data
{
    public interface ILanguageService
    {
        /// <summary>
        ///         GetAllLanguages from persistence.
        /// </summary>
        /// <remarks>
        ///         Catches all SqlExceptions, logs an Error: {e.Message}
        ///         and will just return an empty collection
        /// </remarks>
        ///
        /// <returns>
        ///          A collection of languages, that might be empty.
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
        ///         Adds one Language to persistence.
        /// </summary>
        /// <remarks>
        ///         Catches SqlExceptions and just logs them
        /// </remarks>
        ///
        /// <event cref="OnChange">
        ///         Succes: {toAdd}, wurde zur Liste der Sprachen hinzugefügt.
        /// or
        ///static   Error:  Name.Length war nicht (&lt50 &amp&amp &lt1) {newField}
        ///dyn      Error:  {toAdd}, kann nicht hinzugefügt werden, da es schon enthalten ist.
        /// </event>
        public int CreateLanguage(Language toAdd, bool justValidate = false);
        /// <summary>
        ///         Removes one Language to persistence.
        /// </summary>
        /// <remarks>
        ///         Catches SqlExceptions and just logs them
        /// </remarks>
        ///
        /// <event cref="OnChange">
        ///         Succes: {toRemove}, wurde aus Liste der Sprachen entfernt.
        /// or
        ///static   Error:  {toRemove}, kann nicht entfernt werden, da es nicht enthalten ist.
        /// </event>
        public int DeleteLanguage(Language toRemove);



        /// <summary>
        ///         UpdateAllLanguages in persistence.
        /// </summary>
        /// <remarks>
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
        ///     Error:  {languages}: see Create/Delete
        ///</event>
        public (int added, int removed) UpdateAllLanguages(IEnumerable<Language> languages, bool justValidate = false);

        /// <summary>
        ///         Updates(Renames)AllLevels in persistence.
        /// </summary>
        ///
        /// <param name="levels">
        ///         a correctly order array of size 6
        ///         for each element (.lenght &gt 1 &amp&&amp& lenght &lt 30)  &&  uniqe(caseinsensitve)
        /// </param>
        public int UpdateAllLevels(string[] levels, bool justValidate = false);



        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
