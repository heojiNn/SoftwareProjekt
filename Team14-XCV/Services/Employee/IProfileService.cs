using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace XCV.Data
{
    public interface IProfileService
    {
        /// <summary>
        ///         Returns the full instantiated object from persistence, if one exist
        /// </summary>
        ///
        /// <returns>
        ///         the Employye or null
        /// </returns>
        public Employee ShowProfile(string persNum);


        /// <summary>
        ///         Returns all Employees that exist in the persitence
        /// </summary>
        ///
        /// <returns>
        ///          A collection which got at least one element under normal condition
        /// </returns>
        public IEnumerable<Employee> ShowAllProfiles();


        /// <summary>
        ///         Returns all Employyees that match the criteria.
        /// </summary>
        /// <remarks>
        /// <para>  if (firstName == "")only check lastName   and  viseversa  </para>
        /// <para>  otherwise checks both  (case-insesitive)   </para>
        /// </remarks>
        ///
        /// <returns>
        ///         A collection of Emplyees that migth be empty
        /// </returns>
        ///
        /// <event cref="OnEmptyResult">
        ///         Keine Ergebnis für "{firstName}" "{lastName}".
        /// </event>
        public IEnumerable<Employee> SearchProfiles(string firstName, string lastName);



        /// <summary>
        ///         Checkes <paramref name="newVersion"/> against constraints
        /// </summary>
        ///
        /// <remarks>
        /// <para>  Informs via a  ChangeEvent about:  </para>
        /// <para>  Info: what will be changed   </para>
        /// <para>  Error: throgh the constraints in the method  </para>
        /// <para>  Error: throgh the DataAnoation on the model  </para>
        /// </remarks>
        /// <param name="newVersion">
        ///     the .PersoNumber for reference, and the rest is the data for replacement
        /// </param>
        ///
        /// <event cref="OnChange">
        ///     Info:  Dein Vor- oder Nachname würde geändert werden
        ///     Info:  Deine Beschreibung würde geändert werden.
        ///     Info:  Dein Berufserfahrung würde geändert werden.
        ///     Info:  Deine  ("Soft Skils", "Hard Skills", "Rollen"
        ///                   "Tätigeitfelder", "Sprachfähigkeiten") würden geändert werden.
        ///
        ///     Error: Mindesetens ein Sprache hat keine Level Angabe.
        ///     Error: Mindesetens ein Skill hat keine Level Angabe.
        ///     Error: and the DataAnotations from the Employee-Model
        /// </event>
        public void ValidateUpdate(Employee newVersion);


        /// <summary>
        ///         Updates <paramref name="newVersion"/> the Employee in the persistence.
        /// </summary>
        ///
        /// <remarks>
        ///         uses  ValidateUpdate(e)
        ///           and private_UpdatePersistence(e)
        /// </remarks>
        /// <param name="newVersion">
        ///     the .PersoNumber for reference, and the rest is the data for replacement
        ///</param>
        /// <event cref="OnChange">
        ///     Succes: Keine Profil-Änderungen zu speichern.
        /// or
        ///     Succes: {FirstName},  deine Daten wurden gespeichert.
        /// or
        ///     Error:  as in ValidateUpdate(e)
        ///     Error:  Es trat ein Fehler in der Persistenz auf {e.Message}.
        ///</event>
        public void UpdateProfile(Employee newVersion);


        /// <summary>
        ///         Uploades the Image to the Server
        /// </summary>
        ///
        public Task UploadImage(Employee e, IBrowserFile image);
        // -------------TODO Validation----------------------



        public event EventHandler<NoResult> SearchEventHandel;
        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
