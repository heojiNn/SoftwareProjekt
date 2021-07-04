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
        public string[] ShowCurrentDataSet();


        /// <summary>
        ///         Reads the file and shows it split up
        /// </summary>
        public Task<string[]> ShowBrowserFile(IBrowserFile browserFile);


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
        ///     Info:  Es wurden: {addRows}/{removoRows} Brachen hinzugefügt/entfernt.
        ///     Info:  Es wurden: {aR}{cR}{rR} (Rollen mit Lohn) hinzugefügt/geändert/entfernt
        ///     Info:  Es wurden: {aR}{rR}  Sprachen hinzugefügt/entfernt"
        ///     Info:   Es wurden: {cR}     Sprachen Level geändert
        ///     Info:  Es wurden: {aR}{rR}  SkillsKategorien hinzugefügt/entfernt.
        ///     Info:   Es wurden: {aR}{rR} Skills hinzugefügt/entfernt.
        ///     Info:   Es wurden: {cR}     Skill Level geändert.
        /// or
        ///     Error:  {Name} Brachen brauchen einen Namen./Der Name muss unter 50 Zeichen sein.
        ///     Error:  {Name} Rollen brauchen einen Namen./Der Name muss unter 50 Zeichen sein.
        ///     Error:  {Name} Sprachen brauchen einen Namen./Der Name muss unter 50 Zeichen sein.
        ///     Error:  {Name} Skill brauchen einen Namen./Der Name muss unter 50 Zeichen sein.
        ///     Error:  {Name} SkillCategory brauchen einen Namen./Der Name muss unter 40 Zeichen sein.
        ///     Error:  Es trat ein Fehler in der Persistenz auf {ex.Message}.
        /// </event>
        public void JsonUpdate(string json, bool dryRun = true);



        public event EventHandler<ChangeResult> ChangeEventHandel;

    }
}
