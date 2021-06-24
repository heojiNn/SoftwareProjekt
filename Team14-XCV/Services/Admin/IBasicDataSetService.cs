using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;


namespace XCV.Data
{
    public interface IBasicDataSetService
    {
        // Summary:
        //     Gets the data from the lookup-tables in the database
        //       uses ValidateUpdate
        // Returns:
        //     an array of seriallised objects
        //
        // Raises:
        //   ChangeResultEvent:
        //       :  same as {ValidateUpdate(string json)}
        public string[] ShowCurrentDataSet();


        // Summary:
        //     Reads the file and does a round trip validation check
        //       uses ValidateUpdate
        // Parameters:
        //   browserFile:
        //
        // Returns:
        //     an array of seriallised objects
        //
        // Raises:
        //   ChangeResultEvent:
        //       :  same as {ValidateUpdate(string json)}
        public Task<string[]> ShowBrowserFile(IBrowserFile browserFile);


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
        public string[] ValidateUpdate(string[] jsons, string[] newOnes = null);

        // Summary:
        //     Checks for consistensy
        //     an strores all ne Values in the database  if something changed
        //       uses ValidateUpdate
        // Parameters:
        //   json:
        //     a new json
        //
        // Raises:
        //   ChangeResult:
        //     Error:  same as {ValidateUpdate(string json)}
        // or
        //     Succes: "Update erfolgreich. gespeichert folgen de reihen ind data baae ceändert."
        //             "Keine Änderungen zu übernehmen."
        public void Update(string[] json);


        public event EventHandler<ChangeResult> ChangeEventHandel;


    }

}
