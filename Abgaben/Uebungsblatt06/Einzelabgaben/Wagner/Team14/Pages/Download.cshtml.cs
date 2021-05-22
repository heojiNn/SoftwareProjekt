using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BlazorDownloadFile;
using Microsoft.AspNetCore.Components;

namespace Team14.Pages
{

    public class DownloadModel : PageModel
    {
        [Inject] IBlazorDownloadFileService BlazorDownloadFileService { get; set; }

        private readonly IWebHostEnvironment _env;

        public DownloadModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult OnGet(string name)
        {
            return File(CreateWordprocessingDocument(), "application/octet-stream", name);
        }

        public byte[] CreateWordprocessingDocument()
        {

            MemoryStream stream = new MemoryStream();

            // Create a document by supplying the filepath. 
            using (WordprocessingDocument wordDocument =
            WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document(
                  new Body(
                     new Paragraph(
                        new Run(
                           new Text("Create text in body - CreateWordprocessingDocument")))));
                return stream.GetBuffer();
            }
        }
    }
}

