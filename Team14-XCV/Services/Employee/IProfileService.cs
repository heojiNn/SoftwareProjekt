using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace XCV.Data
{
    public interface IProfileService
    {

        // Summary:
        //     Returns the full instantiated Object from persistence if one exist
        // Returns:
        //     might be null
        //
        public Employee ShowProfile(string persNum);


        // Summary:
        //     1    everything
        //     2    if (firstName == "")only check lastName   and viseversa
        //                             otherwise check both  case-insesitive
        // Returns:
        //     A List of Emplyees that migth be empty
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
        // Raises:
        //   ChangeResult:
        //     Info:  Dein Vor- oder Nachname würde geändert werden
        //     Info:  Deine Beschreibung würde geändert werden
        //     Info:  Dein Rate-Card-Level würde geändert werden
        //     Info:  Deine  ("Soft Skils", "Hard Skills", "Rollen"
        //                    "Tätigeitfelder", "Sprachfähigkeiten") würden geändert werden
        // and
        //     Error: Mindesetens ein Sprache hat keine Level Angabe.
        //     Error: Mindesetens ein Skill hat keine Level Angabe.
        //     Error: and the DataAnotations from the Model
        public void ValidateUpdate(Employee newVersion);

        // Summary:
        //     Updates the Employee in persistence.
        //       uses  private_UpdatePersistence(Employee e)
        //
        // Raises:
        //   ChangeResult:
        //     Error:     as above
        //     Error:  Es trat ein Fehler in der Persistenz auf {e.Message}
        // or
        //     Succes: Keine Profil-Änderungen zu speichern.
        //     Succes: {FirstName},  deine Daten wurden gespeichert.
        public void Update(Employee toCommit);
        public Task Uploade(Employee toGetNum, IBrowserFile browserFile);


        public event EventHandler<NoResult> SearchEventHandel;
        public event EventHandler<ChangeResult> ChangeEventHandel;

    }
}
