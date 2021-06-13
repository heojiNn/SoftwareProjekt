using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;


namespace Team14.Data
{
    public interface IBasicDataSetService
    {
        // Summary:
        //     Gets the current file from the Server
        //     and uses ValidateUpdate
        // Returns:
        //     The .json form the server.
        //
        // Raises:
        //   ChangeResultEvent:
        //       :  same as {ValidateUpdate(string json)}
        public string ShowCurrentDataSet();


        // Summary:
        //     Reads the file and does a round trip validation check
        //     and uses ValidateUpdate
        // Parameters:
        //   browserFile:
        //
        // Returns:
        //     The formated content of uploaded-file.
        //
        // Raises:
        //   ChangeResultEvent:
        //       :  same as {ValidateUpdate(string json)}
        public Task<string> ShowBrowserFileAsync(IBrowserFile browserFile);



        // Summary:
        //     Checks for consistensy (doubles  format error)
        // Returns:
        //     the fomated Version of the input
        //
        // Raises:
        //   ChangeResult:
        //     Info:  Akzeptables Dupllikat ...
        //     Info:  Fähigkeiten in .. Kategorien
        // and
        //     Error: Bei der Deserilisation fiel auf: ...
        //     Error: Inakzeptables Dupllikat ...
        public string ValidateUpdate(string json);

        // Summary:
        //     Checks for consistensy
        //     an strores a new Version on the Server  if somthing changed
        // Parameters:
        //   json:
        //     a new json
        //
        // Raises:
        //   ChangeResult:
        //     Error:  same as {ValidateUpdate(string json)}
        // or
        //     Succes: "Update erfolgreich. gespeichert unter: {name}."
        //             "Keine Änderungen zu übernehmen."
        public void Update(string json);


        public event EventHandler<ChangeResult> ChangeEventHandel;
    }

}
