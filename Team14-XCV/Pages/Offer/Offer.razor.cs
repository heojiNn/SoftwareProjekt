using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Team14.Data;




namespace Team14.Pages.Offer
{
    public partial class Offer
    {
        // Create Employee.
        public Employee emp;

        // Define selection Booleans. Should later inherit from DocumentConfig.
        public Boolean a { get; set; } = true;
        public Boolean b { get; set; } = true;
        public Boolean c { get; set; } = true;
        public Boolean d { get; set; } = true;
        public Boolean e { get; set; } = true;

        /*
        * Converts template from byte[] to stream, modify it and return the stream.
        * - .docx-template included as project-resource.
        * - Template can be modified easily, additional content (body.Append(...)) appears one line after the last line of the template (in the .xml body tag after last element).
        * 
        * @param: projekte: true if included in Document, false else
        * @param: ...
       */

        public byte[] OpenAndModfiyWordprocessingDocument(Boolean projects)
        {
            byte[] templateBytes = Team14.Properties.Resources.Vorlage1; // Template.

            using (MemoryStream templateStream = new MemoryStream())
            {
                templateStream.Write(templateBytes, 0, (int)templateBytes.Length); // 'WordprocessingDocument.Open' takes a stream as input in this implementation.
                using (WordprocessingDocument doc = WordprocessingDocument.Open(templateStream, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    Body body = doc.MainDocumentPart.Document.Body; // Reference to the existing body.


                    // 'Selection' of Worddocument's content.

                    // Mandatory: Employee.
                    Paragraph para1 = body.AppendChild(new Paragraph());
                    ParagraphProperties pPr = para1.AppendChild(new ParagraphProperties());
                    pPr.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
                    Run run = para1.AppendChild(new Run());
                    run.AppendChild(new Text("MitarbeiterIn:") { Space = SpaceProcessingModeValues.Preserve });
                    run.AppendChild(new Break());

                    Paragraph para2 = body.AppendChild(new Paragraph());
                    Run run2 = para2.AppendChild(new Run());
                    run2.AppendChild(new Text($"Vorname: [templatename]") { Space = SpaceProcessingModeValues.Preserve });

                    Paragraph para5 = body.AppendChild(new Paragraph());
                    Run run5 = para5.AppendChild(new Run());
                    run5.AppendChild(new Text($"Nachname: [templatename]") { Space = SpaceProcessingModeValues.Preserve });

                    Paragraph para3 = body.AppendChild(new Paragraph());
                    Run run3 = para3.AppendChild(new Run());
                    run3.AppendChild(new Text($"Geburtsdatum: [dd.MM.yyyy]") { Space = SpaceProcessingModeValues.Preserve });


                    /*
                    // Optional selection: projects.
                    if (projects)
                    {
                        Paragraph para4 = body.AppendChild(new Paragraph());
                        Run run4 = para4.AppendChild(new Run());
                        run4.AppendChild(new Text("Projekte:") { Space = SpaceProcessingModeValues.Preserve });

                        // Unordered list.
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
                    */


                    // Returns the stream.
                    mainPart.Document.Save();
                    doc.Close(); // Closes the handle explicitly.
                    templateStream.Position = 0;
                    return templateStream.ToArray();
                }
            }
        }









    }









}
