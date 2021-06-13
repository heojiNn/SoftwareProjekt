using System;
using System.Collections.Generic;




namespace Team14.Data
{
    public interface IAccountService
    {
        public IEnumerable<Employee> ShowAllProfiles();



        // Summary:
        //     Updates creats an Employye Account   if valid
        //
        // Raises:
        //   ChangeResult:
        //     Error:  Sie könne keinen Account überschreiben.
        //     Error:  Geben sie Vor-, Nachnamen und PersNr ein.
        // or
        //     Succes: Ein neuer Account wurde erstellt {PersoNumber}.
        // and
        //     Info:  Passwort und Personal-Number sollten gleich sein. Dies wurde koregiert.
        public void CreateAccount(Employee newAccount);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }
}
