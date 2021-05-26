using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team14.Data;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            //Bytestream um Document zu uebergeben nach Erstellung (an BlazorDownloadFile - Element)
            using (MemoryStream mem = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document))
                {

                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    // Dcumentstruktur und Text.
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    Paragraph para1 = body.AppendChild(new Paragraph());
                    Run run = para1.AppendChild(new Run());
                    RunProperties rp = run.AppendChild(new RunProperties());
                    rp.Bold = new Bold();
                    run.AppendChild(new Text("Mitglied:") { Space = SpaceProcessingModeValues.Preserve });

                    Paragraph para2 = body.AppendChild(new Paragraph());
                    Run run2 = para2.AppendChild(new Run());
                    run2.AppendChild(new Text($"Name: {mitarbeiter.Vorname}") { Space = SpaceProcessingModeValues.Preserve });

                    Paragraph para3 = body.AppendChild(new Paragraph());
                    Run run3 = para3.AppendChild(new Run());
                    run3.AppendChild(new Text($"Geburtsdatum: {mitarbeiter.Geburtstag.ToString("dd.MM.yyyy")}") { Space = SpaceProcessingModeValues.Preserve });

                    Paragraph para4 = body.AppendChild(new Paragraph());
                    Run run4 = para4.AppendChild(new Run());
                    run4.AppendChild(new Text("Projekte:") { Space = SpaceProcessingModeValues.Preserve });

                    // Paragraph Eigenschaften fuer unordered list fuer Projekte
                    SpacingBetweenLines sblUl = new SpacingBetweenLines() { After = "0" }; 
                    Indentation iUl = new Indentation() { Hanging = "360" };  
                    NumberingProperties npUl = new NumberingProperties(
                        new NumberingLevelReference() { Val = 1 },
                        new NumberingId() { Val = 2 }
                    );
                    ParagraphProperties ppUnordered = new ParagraphProperties(npUl, sblUl, iUl);
                    ppUnordered.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };

                    int length = mitarbeiter.Projekte.Count;
                    int i = 0;
                    Paragraph[] parAr = new Paragraph[length];

                    foreach (string p in mitarbeiter.Projekte)
                    {
                        parAr[i] = new Paragraph();
                        parAr[i].ParagraphProperties = new ParagraphProperties(ppUnordered.OuterXml);
                        parAr[i].Append(new Run(new Text(p)));
                        body.Append(parAr[i]);
                        i++;
                    }
                }
                return mem.ToArray(); //liefert Byte-Array
            }
        }
    }
}
