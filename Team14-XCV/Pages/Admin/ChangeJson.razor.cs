using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using XCV.Data;


namespace XCV.Pages.Admin
{
    public partial class ChangeJson
    {
        private string[] textareas = new string[5];
        private bool validJson = true;


        protected override void OnInitialized()
        {
            dataSetService.ChangeEventHandel += OnChange;
            textareas = dataSetService.ShowCurrentDataSet();
        }
        private async Task ShowUpload(InputFileChangeEventArgs eventA)
        {
            textareas = await dataSetService.ShowBrowserFile(eventA.File);
            //StateHasChanged();
        }



        private void Validate()
        {
            dataSetService.JsonUpdate(textareas[0]);// might show ChangeInfo and ChangeInfo.Error
        }
        private void Upload()
        {
            dataSetService.JsonUpdate(textareas[0], false); // shows either ChangeInfo.Error o. ChangeInfo.Succes
        }




        private ChangeResult changeInfo = new();
        private void OnChange(object sender, ChangeResult e)
        {
            changeInfo = e;
            validJson = (!e.ErrorMessages.Any());
        }
    }
}
