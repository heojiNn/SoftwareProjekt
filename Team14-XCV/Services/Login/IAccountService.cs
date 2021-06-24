using System;
using System.Collections.Generic;




namespace XCV.Data
{
    public interface IAccountService
    {
        public IEnumerable<Employee> ShowAllProfiles();



        // Summary:
        //     Creates an Employye Account
        //       uses  private_CreatePersistence(Employee e)
        //
        // Parameters:
        //   Employee: The new one.
        //
        // Raises:
        //   ChangeResult:
        //     Error:  Sie könne keinen Account überschreiben.
        //     Error:  Geben sie Vor-, Nachnamen und PersNr ein.
        // or
        //
        //     Succes: Ein neuer Account wurde erstellt {PersoNumber}.
        // and
        //     Info:  Passwort und Personal-Number sollten gleich sein. Dies wurde koregiert.
        public void CreateAccount(Employee newAccount);

        // Summary:
        //     Deletes the Employee in persistence.
        public void Delete(Employee toDelete);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
