using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCV.Data;

using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;



namespace XCV.Services.DocumentConfig
{
    public class ConfigService : IConfigService
    {
        public byte[] GenerateSingleProfile(Employee e)
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

                    //add Image (opt)/TODO only for SinlgeProfile
                    /*
                    Paragraph pImg = new Paragraph();
                    ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                    string imgPath = "Absolute/URL/Of/picture.png";
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(imgPath);
                    req.UseDefaultCredentials = true;
                    req.PreAuthenticate = true;
                    req.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    imagePart.FeedData(resp.GetResponseStream());

                    // 1500000 and 1092000 are img width and height
                    Run rImg = new Run(DrawingManager(mainPart.GetIdOfPart(imagePart), "PictureName", 1500000, 1092000, string.Empty));
                    pImg.Append(rImg);
                    docBody.Append(pImg);
                    */

                    // add Heading 
                    Paragraph para = body.AppendChild(new Paragraph());
                    ParagraphProperties pPr = para.AppendChild(new ParagraphProperties());
                    pPr.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text($"MitarbeiterIn:") { Space = SpaceProcessingModeValues.Preserve });
                    run.AppendChild(new Break());

                    // add PersoNumber
                    Paragraph para1 = body.AppendChild(new Paragraph());
                    Run run1 = para1.AppendChild(new Run());
                    run1.AppendChild(new Text($"Vorname: {e.PersoNumber}") { Space = SpaceProcessingModeValues.Preserve });

                    // add FirstName
                    Paragraph para2 = body.AppendChild(new Paragraph());
                    Run run2 = para2.AppendChild(new Run());
                    run2.AppendChild(new Text($"Vorname: {e.FirstName}") { Space = SpaceProcessingModeValues.Preserve });

                    // add LastName 
                    Paragraph para5 = body.AppendChild(new Paragraph());
                    Run run5 = para5.AppendChild(new Run());
                    run5.AppendChild(new Text($"Nachname: {e.LastName}") { Space = SpaceProcessingModeValues.Preserve });

                    // add Heading 
                    Paragraph para3 = body.AppendChild(new Paragraph());
                    Run run3 = para3.AppendChild(new Run());
                    run3.AppendChild(new Text($"Beschreibung: {e.Description}") { Space = SpaceProcessingModeValues.Preserve });

                    // add RCL (general, independent from specific offers) 
                    Paragraph para4 = body.AppendChild(new Paragraph());
                    Run run4 = para4.AppendChild(new Run());
                    run4.AppendChild(new Text($"Beschreibung: {e.RCL}") { Space = SpaceProcessingModeValues.Preserve });




                    /*
                    // Optional selection: projects.
                    if (projects)
                    {
                        Paragraph paraX = body.AppendChild(new Paragraph());
                        Run runX = paraX.AppendChild(new Run());
                        runX.AppendChild(new Text("Projekte:") { Space = SpaceProcessingModeValues.Preserve });

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

                    //==================================================================================//

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
