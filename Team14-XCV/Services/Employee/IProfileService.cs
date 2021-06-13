using System;
using System.Collections.Generic;



namespace Team14.Data
{
    public interface IProfileService
    {

        // Summary:
        //     might be null  but only if Interface isn't used properly
        public Employee ShowProfile(string persNum);


        // Summary:
        //     1    everything
        //     2    (firstName == "")only check lastName  and viseversa     otherwise check both  case-insesitive
        //
        // Returns:
        //     A List of Emplyees that migth be empty (but never null)
        //
        // Raises:
        //   NoResult:
        //    Message:       Keine Ergebnis für "{firstName}" "{lastName}".
        public IEnumerable<Employee> ShowAllProfiles();
        public IEnumerable<Employee> SearchProfiles(string firstName, string lastName);



        // Summary:
        //     Checkes Updates against Constrains
        //
        // Parameters:
        //   Profile:
        //     that contains  Profile.PersoNumber for reference
        //                    and the  new data for replacement
        //
        // Raises:
        //   ChangeResult:
        //     Info:  Dei Vor oder Nachname würde geändert werden
        //     Info:  Deine Beschreibung würde geändert werden
        //     Info:  Dein Rate-Card-Level würde geändert werde
        //     Info:  Deine  ("Soft Skils", "Hard Skills", "Rollen"
        //                    "Tätigeitfelder", "Sprachfähigkeiten") würden geändert werden
        // and
        //     Error: Bei der Profile-Erstellung muss mindestens eine Primärsprache auswählen werden.
        //     Error: Mindesetens ein Sprache hat keine Level Angabe.
        //     Error: Mindesetens ein Skill hat keine Level Angabe.
        public void ValidateUpdate(Employee newVersion);

        // Summary:
        //     Updates the Profile    if valid
        //
        // Raises:
        //   ChangeResult:
        //     Error:     as above
        //     Error:  Es trat ein Fehler in der Persistenz auf {e.Message}
        // or
        //     Succes: Keine Profil-Änderungen zu speichern.
        //     Succes: {FirstName},  deine Daten wurden gespeichert.
        public void Update(Employee toCommit);



        public event EventHandler<NoResult> SearchEventHandel;
        public event EventHandler<ChangeResult> ChangeEventHandel;

    }
}
