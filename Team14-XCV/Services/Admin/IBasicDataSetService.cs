using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;


namespace Team14.Data
{
    public interface IBasicDataSetService
    {
        // Summary:
        //     Checks the current file on the Server   and returns it.
        //     will do a  "round trip check"   1-deseriallise 2-check 3-searialise and 4-format
        //     before
        //
        // Returns:
        //     The .json form the server.
        // Raises:
        //   ChangeResultEvent:
        //     Info: Der Aktuelle file hat Duplikate die aber nicht stören
        //     Error: Aktuell ist eine Datein mit folgendem Fehler auf dem Server
        //     Error: Aktuell ist ein invalid file auf dem Server
        public string ShowCurrentDataSetAndCheck();


        // Summary:
        //     Reads the file and does a round trip validation check
        // Parameters:
        //   browserFile:
        //     A File which was uploaded.
        // Returns:
        //     the formated content of uploaded-file/
        // Raises:
        //   ChangeResultEvent:
        //     Info: Die ins Feld geladen Datei hat Duplikate die aber nicht stören
        //     Error: Die ins Feld geladen Datei hat folgende Fehler
        //     Error: Die Datei konnte einfach nicht seriallisiert werden
        public Task<string> ShowBrowserFileAsync(IBrowserFile browserFile);

        public string UpdateRoundtripCheck(string json);


        // Summary:
        //     Checks for consistensy
        // Parameters:
        //   treeRoot:
        //     A Root Category with all Children.
        // Returns:
        //     The Root Category with all Children.
        // Raises:
        //   ChangeResultEvent:
        //     Error: Änderung konnte nicht durchgeführt werden weil
        //     Error: Änderung wurde übernommen/durchgeführt im System.
        public void CommitUpdate(string json, bool newVers);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }

}
