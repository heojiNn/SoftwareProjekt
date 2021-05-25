using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team14.Data;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Team14.Shared
{
    public partial class EmployeeShortcard
    {
        //Default fuer Mitarbeiterkarten
        private Employee standard = new Employee
        {
            Vorname = "",
            Nachname = "",
            Geburtstag = new DateTime(2000, 01, 01),
            Projekte = new List<string> { "" }
        };

        //DocumentFormat.OpenXml - Variante
        public byte[] CreateWordprocessingDocument()
        {
            MemoryStream stream = new MemoryStream();
            using (WordprocessingDocument wordDocument =
            WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document(
                  new Body(
                     new Paragraph(
                        new Run(
                           new Text(mitarbeiter.ToString())))));    //hier soll Mitarbeiter übergeben werden
                return stream.ToArray();
            }
        }

    }
}
