using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;


namespace XCV.Data
{
    public class ConfigService : IConfigService
    {
        public byte[] GenerateSingleProfile(Employee e)
        {
            byte[] templateBytes = XCV.Properties.Resources.Vorlage1; // Template (can be adapted without impact on the following (Additions are made to the first line after the last line of the template).

            using (MemoryStream templateStream = new MemoryStream())
            {
                templateStream.Write(templateBytes, 0, (int)templateBytes.Length); 
                using (WordprocessingDocument doc = WordprocessingDocument.Open(templateStream, true)) // 'WordprocessingDocument.Open' takes a stream as input in this implementation.
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    Body body = doc.MainDocumentPart.Document.Body; // Reference to the existing body.

                    //==================='Selection' of Worddocument's content.=========================//

                    // -------------------Single Input:------------------------------

                    // add Heading 
                    Paragraph para = body.AppendChild(new Paragraph());
                    ParagraphProperties pPr = para.AppendChild(new ParagraphProperties());
                    pPr.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text($"MitarbeiterIn:") { Space = SpaceProcessingModeValues.Preserve });
                    run.AppendChild(new Break());

                    //add Image
                    if (e.Image.Length != 0)
                    {
                        ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                        using (FileStream stream = new FileStream(Path.Combine(Environment.CurrentDirectory, @"wwwroot\", e.Image), FileMode.Open))
                        {
                            imagePart.FeedData(stream);
                        }
                        AddImageToBody(doc, mainPart.GetIdOfPart(imagePart));
                    }

                    // add PersoNumber
                    Paragraph para1 = body.AppendChild(new Paragraph());
                    Run run1 = para1.AppendChild(new Run());
                    if (e.PersoNumber.Length != 0)
                        run1.AppendChild(new Text($"Personalnummer: {e.PersoNumber}") { Space = SpaceProcessingModeValues.Preserve });
                    else
                        run1.AppendChild(new Text($"Personalnummer: -") { Space = SpaceProcessingModeValues.Preserve });

                    // add FirstName
                    Paragraph para2 = body.AppendChild(new Paragraph());
                    Run run2 = para2.AppendChild(new Run());
                    if (e.FirstName.Length != 0)
                        run2.AppendChild(new Text($"Vorname: {e.FirstName}") { Space = SpaceProcessingModeValues.Preserve });
                    else
                        run1.AppendChild(new Text($"Vorname: -") { Space = SpaceProcessingModeValues.Preserve });

                    // add LastName 
                    Paragraph para3 = body.AppendChild(new Paragraph());
                    Run run3 = para3.AppendChild(new Run());
                    if (e.LastName.Length != 0)
                        run3.AppendChild(new Text($"Nachname: {e.LastName}") { Space = SpaceProcessingModeValues.Preserve });
                    else
                        run3.AppendChild(new Text($"Nachname: -") { Space = SpaceProcessingModeValues.Preserve });

                    // add Description
                    Paragraph para4 = body.AppendChild(new Paragraph());
                    Run run4 = para4.AppendChild(new Run());
                    if (e.Description.Length != 0)
                        run4.AppendChild(new Text($"Beschreibung: {e.Description}") { Space = SpaceProcessingModeValues.Preserve });
                    else
                        run4.AppendChild(new Text($"Beschreibung: {e.Description}") { Space = SpaceProcessingModeValues.Preserve });

                    // add RCL
                    Paragraph para5 = body.AppendChild(new Paragraph());
                    Run run5 = para5.AppendChild(new Run());
                    if (e.RCL.HasValue)
                        run5.AppendChild(new Text($"RCL: {e.RCL}") { Space = SpaceProcessingModeValues.Preserve });
                    else
                        run5.AppendChild(new Text($"RCL: -") { Space = SpaceProcessingModeValues.Preserve });
                    
                    // add WorkingSince 
                    Paragraph para6 = body.AppendChild(new Paragraph());
                    Run run6 = para6.AppendChild(new Run());
                    run6.AppendChild(new Text($"Beschreibung: {e.WorkingSince.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}") { Space = SpaceProcessingModeValues.Preserve });
                    
                    //
                    // ----------------------Multiple Inputs:------------------------
                    //
                    //Simple Enumeration of Contents in new Appends as Strings (Alternative: i.e. Bullet Lists)


                    // Roles
                    Paragraph para7 = body.AppendChild(new Paragraph());
                    Run run7 = para7.AppendChild(new Run());
                    run7.AppendChild(new Text($"Rollen: "));
                    if ((e.Roles.Count() != 0) && e.Roles != null)
                    {
                        
                        Role lastR = e.Roles.Last();
                        foreach (Role r in e.Roles)
                        {
                            if (!r.Equals(lastR))
                                run7.AppendChild(new Text($"{r.Name}, "));
                            else
                                run7.AppendChild(new Text($"{r.Name}") { Space = SpaceProcessingModeValues.Preserve });
                        }
                    } else
                    {
                        run7.AppendChild(new Text($"-"));
                    }

                    // Fields
                    Paragraph para8 = body.AppendChild(new Paragraph());
                    Run run8 = para8.AppendChild(new Run());
                    run8.AppendChild(new Text($"Branchenwissen: "));
                    if ((e.Fields.Count() != 0) && e.Fields != null)
                    {
                        Field lastF = e.Fields.Last();
                        foreach (Field f in e.Fields)
                        {
                            if (!f.Equals(lastF))
                                run8.AppendChild(new Text($"{f.Name}, "));
                            else
                                run8.AppendChild(new Text($"{f.Name}") { Space = SpaceProcessingModeValues.Preserve });
                        }
                    } else
                    {
                        run8.AppendChild(new Text($"-"));
                    }


                    /*
                    // Languages
                    Paragraph para9 = body.AppendChild(new Paragraph());
                    Run run9 = para9.AppendChild(new Run());
                    run9.AppendChild(new Text($"Sprachen: "));
                    Language lastL = e.Languages.Last();
                    foreach (Language l in e.Languages)
                    {
                        if (!l.Equals(lastL))
                            run9.AppendChild(new Text($"{l.Name}, "));
                        else
                            run9.AppendChild(new Text($"{l.Name}") { Space = SpaceProcessingModeValues.Preserve });
                    }

                    // Softskills
                    Paragraph para10 = body.AppendChild(new Paragraph());
                    Run run10 = para10.AppendChild(new Run());
                    run10.AppendChild(new Text($"Rollen: "));
                    List<Skill> temp = e.Abilities.Where(s => s.Type == SkillGroup.Softskill).ToList();
                    Skill lastSS = temp.Last();
                    foreach (Skill s in temp)
                    {
                        if (!s.Equals(lastSS))
                            run10.AppendChild(new Text($"{s.Name}, "));
                        else
                            run10.AppendChild(new Text($"{s.Name}") { Space = SpaceProcessingModeValues.Preserve });
                    }

                    // Hardskills
                    Paragraph para11 = body.AppendChild(new Paragraph());
                    Run run11 = para10.AppendChild(new Run());
                    run10.AppendChild(new Text($"Rollen: "));
                    List<Skill> temp2 = e.Abilities.Where(s => s.Type == SkillGroup.Hardskill).ToList();
                    Skill lastHS = temp2.Last();
                    foreach (Skill s in temp2)
                    {
                        if (!s.Equals(lastSS))
                            run11.AppendChild(new Text($"{s.Name}, "));
                        else
                            run11.AppendChild(new Text($"{s.Name}") { Space = SpaceProcessingModeValues.Preserve });
                    }
                    */

                    // Projects - Bulletpointlist(unordered)-example:
                    Paragraph para12 = body.AppendChild(new Paragraph());
                    Run run12 = para12.AppendChild(new Run());
                    run12.AppendChild(new Text("Projekte: "));
                    SpacingBetweenLines sblUl = new SpacingBetweenLines() { After = "0" };
                    Indentation iUl = new Indentation() { Hanging = "360" };
                    NumberingProperties npUl = new NumberingProperties(
                        new NumberingLevelReference() { Val = 1 },
                        new NumberingId() { Val = 2 }
                    );
                    ParagraphProperties ppUnordered = new ParagraphProperties(npUl, sblUl, iUl);
                    ppUnordered.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
                    if (e.Projects.Count != 0 && e.Projects != null)
                    {
                        int length = e.Projects.Count();
                        int i = 0;
                        Paragraph[] parAr = new Paragraph[length];

                        foreach (Project p in e.Projects)
                        {
                            parAr[i] = new Paragraph();
                            parAr[i].ParagraphProperties = new ParagraphProperties(ppUnordered.OuterXml);
                            parAr[i].Append(new Run(new Text(p.Title)));
                            body.Append(parAr[i]);
                            i++;
                        }
                    } else
                    {
                        run12.AppendChild(new Text("-") { Space = SpaceProcessingModeValues.Preserve });
                    }
                    
                    

                    //==================================================================================//

                    // Returns the stream.
                    mainPart.Document.Save();
                    doc.Close(); // Closes the handle explicitly.
                    templateStream.Position = 0;
                    return templateStream.ToArray();
                }
            }
        }

        public byte[] GenerateDocumentConfig(DocumentConfig config)
        {
            byte[] templateBytes = XCV.Properties.Resources.Vorlage1; // Template (can be adapted without impact on the following/Additions are made to the first line after the last line of the template)

            using (MemoryStream templateStream = new MemoryStream())
            {
                templateStream.Write(templateBytes, 0, (int)templateBytes.Length);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(templateStream, true)) // 'WordprocessingDocument.Open' takes a stream as input in this implementation.
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    Body body = doc.MainDocumentPart.Document.Body; // Reference to the existing body.

                    //==================='Selection' of Worddocument's content.=========================//

                    foreach (EmployeeConfig ec in config.employeeConfigs)
                    {
                        // Add the selected Properties of Employee X.
                    }

                    // Table with Wages - Blueprint:
                    /*
                    Table table = new Table();
                    TableRow tr1 = new TableRow();

                    TableCell tc11 = new TableCell();  
                    Paragraph p11 = new Paragraph(new Run(new text("A")));  
                    tc11.Append(p11);  
                    tr1.Append(tc11);

                    TableCell tc12 = new TableCell();  
                    Paragraph p12 = new Paragraph();  
                    Run r12 = new Run();  
                    RunProperties rp12 = new RunProperties();  
                    rp12.Bold = new Bold();  
                    r12.Append(rp12);  
                    r12.Append(new Text("Nice"));  
                    p12.Append(r12);  
                    tc12.Append(p12);

                    tr1.Append(tc12);  
                    table.Append(tr1);

                    TableRow tr2 = new TableRow();


                    TableCell tc21 = new TableCell();  
                    Paragraph p21 = new Paragraph(new Run(new text("Little")));  
                    tc21.Append(p21);  
                    tr2.Append(tc21);

                    TableCell tc22 = new TableCell();  
                    Paragraph p22 = new Paragraph();  
                    ParagraphProperties pp22 = new ParagraphProperties();  
                    pp22.Justification = new Justification() { Val = JustificationValues.Center };  
                    p22.Append(pp22);  
                    p22.Append(new Run(new Text("Table")));  
                    tc22.Append(p22);

                    tr2.Append(tc22);  
                    table.Append(tr2);

                    // Add your table to docx body
                    docBody.Append(table);  
                     */
                    //==================================================================================//

                    // Returns the stream.
                    mainPart.Document.Save();
                    doc.Close(); // Closes the handle explicitly.
                    templateStream.Position = 0;
                    return templateStream.ToArray();
                }
            };
        }

        // Refers to the Image - https://docs.microsoft.com/en-us/office/open-xml/how-to-insert-a-picture-into-a-word-processing-document?redirectedfrom=MSDN
        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }
    }
}
