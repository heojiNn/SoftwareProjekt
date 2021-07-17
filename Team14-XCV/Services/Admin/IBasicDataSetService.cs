using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;


namespace XCV.Data
{
    public interface IBasicDataSetService
    {
        /// <summary>
        ///         Gets the data from the lookup-tables in the database
        ///         and shows it split up
        /// </summary>
        ///
        /// <returns>
        ///          An array of seriallised objects that the json contains
        ///          [0] is the whole databasis  [1] the fields .....
        /// </returns>
        public string ShowCurrentDataSet();


        /// <summary>
        ///         Reads the file and shows it split up
        /// </summary>
        public Task<string> ShowBrowserFile(IBrowserFile browserFile);


        /// <summary>
        ///         updates the lookup tables in the database with the json
        /// </summary>
        ///
        /// <remarks>
        ///         uses  UpdateAllFields(), UpdateAllRoles(), UpdateAllLang(), UpdateAllLangLevels()
        ///               UpdateAllSkills(), UpdateAllLevels(),  ValidateFields(), ValidateRoles()
        /// </remarks>
        ///
        /// <event cref="OnChange">
        ///     Succes: Änderungen in die Datenbank übernommen.
        /// and
        ///     Info:  SuccesM put through from FieldService
        ///     Info:  Es wurden: {changed} Löhne geändert.
        ///            Es wurden: {addedRows}/{removedRows} Rollen hinzugefügt/entfernt.
        ///     Info:  SuccesM put through from LanguageService
        ///     Info:   Es wurden: {cR}     Sprachen Level geändert
        ///     Info:  Es wurden: {aR}{rR}  SkillsKategorien hinzugefügt/entfernt.
        ///     Info:   Es wurden: {aR}{rR} Skills hinzugefügt/entfernt.
        ///     Info:   Es wurden: {cR}     Skill Level geändert.
        /// or
        ///     Error:  Json Format
        ///     Error:  ErrorM from FieldService
        ///     Error:  ErrorM from RoleService
        ///     Error:  ErrorM from LanguageService
        ///     Error:  ErrorM from Skillervice
        /// </event>
        public void JsonUpdate(string json, bool dryRun = true);



        public event EventHandler<ChangeResult> ChangeEventHandel;

    }
}
