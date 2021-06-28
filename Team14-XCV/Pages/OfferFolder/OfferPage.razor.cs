using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;




namespace XCV.Pages.OfferNamespace
{
    public partial class OfferPage
    {
        
        //===========================================================================================================================================//

        // Dokumentenkonfiguration: TODO: (needs: List of offers, List of employees contained in an offer)

        // Access:
        // Use access role to identify user. -> Access role returns employee-instance.
        // Employee instance contains: "DocumentConfigurationList".
        // Display the Configurations in a HTML element on the offer with the latest chosen Config being preselected.
        // How to implement latest chosen Config? Persist in Employee as attribute?

        // Functions:
        // User can selected/deselect/delete/create/copy/rename a configuration
        // select: activates this configuration to be "active" (for generating the document); implies "deselect" for the prior config
        // deselect: deactivate the prior config -> in this state no config is selected
        // delete: removes an item from the "DocumentConfigurationList"
        // create: creates a new DocumentConfig instance
        // copy: copies a certain DocumentConfig with a default description of "<name> (1)" or "<name> (2)", as long as the previous copynumber doesn't exist/ is unique
        // rename: change of the description of DocumentConfig, must be unique

        // States:
        // User must have at least one DocumentConfig in order to generate a Document(else no Input can be processed) [Alternative: Define a copy of the generate Function that prints the defaults{all Employees with all data}] or to enter the OfferEmployeeConfigEdit page.
        // User with at least one DocumentConfig must have a DocumentConfig selected in order to download/edit it aswell.


        //===========================================================================================================================================//

        // Dokumentengeneration: TODO: (needs: Dokumentenkonfiguration)

        // Parameter:
        // DocumentConfig config, contains all the EmployeeConfigs of the document which are to be iterated through individually and displayed in the document.

        // Content:
        // Good looking document structure for EmployeeConfigs
        // "Good looking" := Information-display of individual Employees shoudln't bn displayed cross-page.

        /// <summary>
        /// Converts template from byte[] to stream, modifies it with Config-elements and returns the stream. <para></para>
        /// - .docx-template included as project-resource. <para></para>
        /// - Template can be modified easily, additional content (body.Append(...)) appears one line after the last line of the template (in the .xml body tag after last element). <para></para>
        /// </summary>
        /// <param name="projects"></param>
        /// <returns></returns>

        public byte[] OpenAndModfiyWordprocessingDocument(Boolean projects)
        {
            byte[] templateBytes = XCV.Properties.Resources.Vorlage1; // Template.

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
