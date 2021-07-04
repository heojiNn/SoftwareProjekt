using System;
using System.Collections.Generic;


namespace XCV.Data
{
    public interface IAccountService
    {
        public IEnumerable<Employee> ShowAllProfiles();


        /// <summary>
        ///         Creates an Employye Account
        /// </summary>
        /// <remarks>
        ///         uses  private_InsertEmployee(Employee e)
        /// </remarks>
        ///
        /// <event cref="OnChange">
        ///     Succes: Ein neuer Account wurde erstellt {PersoNumber}.
        /// and
        ///     Info: Passwort und Personal-Number sollten gleich sein, dies wurde korregiert.
        /// or
        ///     Error:  Sie können keinen Account überschreiben.
        ///     Error:  + Entity-DataAnnotations
        ///     Error:  Es trat ein Fehler in der Persistenz auf {SQL ex.Message}.
        /// </event>
        public void CreateAccount(Employee newAccount);


        /// <summary>
        ///         Deletes the employee with the PersoNumber: <paramref name="IdToDelete"/>
        ///         and all references on him in the persistence.
        /// </summary>
        /// <remarks>
        ///         uses  private_DeleteEmployee(string id)
        /// </remarks>
        ///
        /// <param name="IdToDelete">
        ///     the .PersoNumber  which is the primary key/unique identifer  for Employyes
        /// </param>
        /// <event cref="OnChange">
        ///     Succes: Account:{IdToDelete} wurde gelöscht.
        /// or
        ///     Error:  Es trat ein Fehler in der Persistenz auf {ex.Message}.
        /// </event>
        public void DeleteAccount(string IdToDelete);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
