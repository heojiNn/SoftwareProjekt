using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using XCV.Data;


namespace XCV.Pages.Admin
{
    public partial class ChangeJson
    {
        private string textarea = "";
        private bool validJson = true;


        protected override void OnInitialized()
        {
            dataSetService.ChangeEventHandel += OnChange;
            textarea = dataSetService.ShowCurrentDataSet();
        }
        private async Task ShowUpload(InputFileChangeEventArgs eventA)
        {
            textarea = await dataSetService.ShowBrowserFile(eventA.File);
            //StateHasChanged();
        }



        private void Validate()
        {
            dataSetService.JsonUpdate(textarea);// might show ChangeInfo and ChangeInfo.Error
        }
        private void Upload()
        {
            dataSetService.JsonUpdate(textarea, false); // shows either ChangeInfo.Error o. ChangeInfo.Succes
        }




        private ChangeResult changeInfo = new();
        private void OnChange(object sender, ChangeResult e)
        {
            changeInfo = e;
            validJson = (!e.ErrorMessages.Any());
        }
    }
}
