using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;



namespace XCV.Data
{
    public class GenerateService : IGenerateService
    {
        private readonly IProjectService projectService;
        private readonly IOfferService offerService;
        private readonly IProfileService profileService;
         

        public GenerateService(IOfferService gofferService, IProjectService gprojectService, IProfileService gprofileService)
        {
            offerService = gofferService;
            projectService = gprojectService;
            profileService = gprofileService;
        }


        public byte[] GenerateSingleProfile(Employee e)
        {
            byte[] templateBytes = XCV.Properties.Resources.Einzelprofil; // Template (can be adapted without impact on the following (Additions are made to the first line after the last line of the template).

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
                        using (FileStream stream = new FileStream(Path.Combine(Environment.CurrentDirectory, @"wwwroot", e.Image), FileMode.Open))
                        {
                            imagePart.FeedData(stream);
                        }
                        AddImageToBody(body, mainPart.GetIdOfPart(imagePart));
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
                        run2.AppendChild(new Text($"Vorname: -") { Space = SpaceProcessingModeValues.Preserve });

                    // add LastName
                    Paragraph para3 = body.AppendChild(new Paragraph());
                    Run run3 = para3.AppendChild(new Run());
                    if (e.LastName.Length != 0)
                        run3.AppendChild(new Text($"Nachname: {e.LastName}") { Space = SpaceProcessingModeValues.Preserve });
                    else
                        run3.AppendChild(new Text($"Nachname: -") { Space = SpaceProcessingModeValues.Preserve });

                    // add Description
                    if (e.Description != "")
                    {
                        Paragraph para4 = body.AppendChild(new Paragraph());
                        Run run4 = para4.AppendChild(new Run());
                        if (e.Description.Length != 0)
                            run4.AppendChild(new Text($"Beschreibung: {e.Description}") { Space = SpaceProcessingModeValues.Preserve });
                        else
                            run4.AppendChild(new Text($"Beschreibung: {e.Description}") { Space = SpaceProcessingModeValues.Preserve });
                    }
                   

                    // add RCL
                    Paragraph para5 = body.AppendChild(new Paragraph());
                    Run run5 = para5.AppendChild(new Run());
                    if (e.RCL != 0)
                        run5.AppendChild(new Text($"RCL: {e.RCL}") { Space = SpaceProcessingModeValues.Preserve });
                    else
                        run5.AppendChild(new Text($"RCL: -") { Space = SpaceProcessingModeValues.Preserve });

                    // add EmployedSince
                    Paragraph para6 = body.AppendChild(new Paragraph());
                    Run run6 = para6.AppendChild(new Run());
                    run6.AppendChild(new Text($"Angestellt seit: {e.EmployedSince.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}") { Space = SpaceProcessingModeValues.Preserve });

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
                    }
                    else
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
                    }
                    else
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

                    Employee pre = profileService.ShowProfile(e.PersoNumber); //or adapt code where employee is given as input to generate.

                    if (pre.Projects.Count != 0 && pre.Projects != null)
                    {
                        int length = pre.Projects.Count();
                        int i = 0;
                        Paragraph[] parAr = new Paragraph[length];
                        foreach (var p in pre.Projects.Select(x => x.project).Distinct())
                        {
                            parAr[i] = new Paragraph();
                            parAr[i].ParagraphProperties = new ParagraphProperties(ppUnordered.OuterXml);
                            parAr[i].Append(new Run(new Text(projectService.ShowProject(p).Title + ": ")));
                            foreach (var pr in e.Projects.Where(x => x.project == p).ToHashSet())
                            {
                                var last = pre.Projects.Where(x => x.project == p).ToHashSet().Last();
                                if (!pr.Equals(last)) parAr[i].Append(new Run(new Text(pr.activity + ", ")));
                                else parAr[i].Append(new Run(new Text(pr.activity)));                                               
                                                                                                                                    
                            }
                            body.Append(parAr[i]);
                            i++;
                        }
                    }
                    else
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

        public byte[] GenerateDocumentConfig(Offer o, DocumentConfig config)
        {
            if (o == null || config == null)
                return null;

            byte[] templateBytes = XCV.Properties.Resources.Angebot1_3;

            using (MemoryStream templateStream = new MemoryStream())
            {
                templateStream.Write(templateBytes, 0, (int)templateBytes.Length);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(templateStream, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    Body body = doc.MainDocumentPart.Document.Body; // Reference to the existing body.

                    //==================='Selection' of Worddocument's content.=========================//

                    Paragraph parax = body.AppendChild(new Paragraph());
                    ParagraphProperties pPrx = parax.AppendChild(new ParagraphProperties());
                    pPrx.ParagraphStyleId = new ParagraphStyleId() { Val = "Subtitle" };
                    Run runx = parax.AppendChild(new Run());
                    runx.AppendChild(new Text($"Angebot: {o.Title}") { Space = SpaceProcessingModeValues.Preserve });
                    runx.AppendChild(new Break());

                    Paragraph paray = body.AppendChild(new Paragraph());
                    ParagraphProperties pPry = parax.AppendChild(new ParagraphProperties());
                    pPry.ParagraphStyleId = new ParagraphStyleId() { Val = "Subtitle" };
                    Run runy = parax.AppendChild(new Run());
                    runy.AppendChild(new Text($"Laufzeit: {o.Start.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))} - {o.End.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}") { Space = SpaceProcessingModeValues.Preserve });
                    runy.AppendChild(new Break());
                    runy.AppendChild(new Break());


                    if (config.employeeConfigs.Count() == 0)
                    {
                        Paragraph para0 = body.AppendChild(new Paragraph());
                        Run run0 = para0.AppendChild(new Run());
                        run0.AppendChild(new Text($"Keine Mitarbeiter oder Leere Konfigurationen!") { Space = SpaceProcessingModeValues.Preserve });
                        run0.AppendChild(new Break());
                    }
                    else
                    {
                        foreach (EmployeeConfig e in config.employeeConfigs)
                        {
                            Employee oe = offerService.ShowOfferEmployee(o.Id, e.PersNr);
                            Paragraph para = body.AppendChild(new Paragraph());
                            ParagraphProperties pPr = para.AppendChild(new ParagraphProperties());
                            pPr.ParagraphStyleId = new ParagraphStyleId() { Val = "Heading1" };
                            Run run = para.AppendChild(new Run());
                            run.AppendChild(new Text($"{oe.offerRole}:") { Space = SpaceProcessingModeValues.Preserve });
                            run.AppendChild(new Break());

                            for (int n=0; n < 5; ++n) 
                            {
                                switch (e.order[n]) // Determines the order
                                {
                                    case 1:
                                        if (e.Image != null)
                                        {
                                            if (e.Image.Length != 0)
                                            {
                                                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                                                using (FileStream stream = new FileStream(Path.Combine(Environment.CurrentDirectory, @"wwwroot", e.Image), FileMode.Open))
                                                {
                                                    imagePart.FeedData(stream);
                                                }
                                                AddImageToBody(body, mainPart.GetIdOfPart(imagePart));
                                            }
                                        }

                                        if (e.FirstName != null)
                                        {
                                            Paragraph para2 = body.AppendChild(new Paragraph());
                                            Run run2 = para2.AppendChild(new Run());
                                            if (e.FirstName.Length != 0)
                                                run2.AppendChild(new Text($"Vorname: {e.FirstName}") { Space = SpaceProcessingModeValues.Preserve });
                                            else
                                                run2.AppendChild(new Text($"Vorname: -") { Space = SpaceProcessingModeValues.Preserve });
                                        }

                                        if (e.LastName != null)
                                        {
                                            Paragraph para3 = body.AppendChild(new Paragraph());
                                            Run run3 = para3.AppendChild(new Run());
                                            if (e.LastName.Length != 0)
                                                run3.AppendChild(new Text($"Nachname: {e.LastName}") { Space = SpaceProcessingModeValues.Preserve });
                                            else
                                                run3.AppendChild(new Text($"Nachname: -") { Space = SpaceProcessingModeValues.Preserve });
                                        }

                                        if (e.Description != null)
                                        {
                                            Paragraph para4 = body.AppendChild(new Paragraph());
                                            Run run4 = para4.AppendChild(new Run());
                                            if (e.Description.Length != 0)
                                                run4.AppendChild(new Text($"Beschreibung: {e.Description}") { Space = SpaceProcessingModeValues.Preserve });
                                            else
                                                run4.AppendChild(new Text($"Beschreibung: {e.Description}") { Space = SpaceProcessingModeValues.Preserve });
                                        }



                                        if (e.Experience.HasValue)
                                        {
                                            Paragraph para6 = body.AppendChild(new Paragraph());
                                            Run run6 = para6.AppendChild(new Run());
                                            run6.AppendChild(new Text($"Berufserfahrung: {e.EmployedSince.Value.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}") { Space = SpaceProcessingModeValues.Preserve });
                                        }

                                        if (e.EmployedSince.HasValue)
                                        {
                                            Paragraph para7 = body.AppendChild(new Paragraph());
                                            Run run7 = para7.AppendChild(new Run());
                                            run7.AppendChild(new Text($"Angestellt seit: {e.EmployedSince.Value.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}") { Space = SpaceProcessingModeValues.Preserve });
                                        }
                                        break;
                                    case 2:
                                        if (e.selectedFields != null && e.selectedFields.Count != 0)
                                        {
                                            Paragraph para9 = body.AppendChild(new Paragraph());
                                            Run run9 = para9.AppendChild(new Run());
                                            run9.AppendChild(new Text($"Branchenwissen: "));
                                            if ((e.selectedFields.Count() != 0) && e.selectedFields != null)
                                            {
                                                Field lastF = e.selectedFields.Last();
                                                foreach (Field f in e.selectedFields)
                                                {
                                                    if (!f.Equals(lastF))
                                                        run9.AppendChild(new Text($"{f.Name}, "));
                                                    else
                                                        run9.AppendChild(new Text($"{f.Name}") { Space = SpaceProcessingModeValues.Preserve });
                                                }
                                            }
                                            else
                                            {
                                                run9.AppendChild(new Text($"-"));
                                            }
                                        }
                                        break;
                                    case 3:
                                        if (e.selectedSoftSkills != null && e.selectedSoftSkills.Count != 0)
                                        {
                                            Paragraph paraA = body.AppendChild(new Paragraph());
                                            Run runA = paraA.AppendChild(new Run());
                                            runA.AppendChild(new Text($"Softskills: "));
                                            List<Skill> temp = e.selectedSoftSkills.Where(s => s.Type == SkillGroup.Softskill).ToList();
                                            Skill lastSS = temp.Last();
                                            if ((temp.Count() != 0) && temp != null)
                                                foreach (Skill s in temp)
                                                {
                                                    if (!s.Equals(lastSS))
                                                        runA.AppendChild(new Text($"{s.Name}, "));
                                                    else
                                                        runA.AppendChild(new Text($"{s.Name}") { Space = SpaceProcessingModeValues.Preserve });
                                                }
                                            else
                                            {
                                                runA.AppendChild(new Text($"-"));
                                            }
                                        }
                                        break;
                                    case 4:
                                        if (e.selectedHardSkills != null && e.selectedHardSkills.Count != 0)
                                        {
                                            Paragraph paraA = body.AppendChild(new Paragraph());
                                            Run runA = paraA.AppendChild(new Run());
                                            runA.AppendChild(new Text($"Hardskills: "));
                                            List<Skill> temp = e.selectedHardSkills.Where(s => s.Type == SkillGroup.Hardskill).ToList();
                                            Skill lastSS = temp.Last();
                                            if ((temp.Count() != 0) && temp != null)
                                                foreach (Skill s in temp)
                                                {
                                                    if (!s.Equals(lastSS))
                                                        runA.AppendChild(new Text($"{s.Name}, "));
                                                    else
                                                        runA.AppendChild(new Text($"{s.Name}") { Space = SpaceProcessingModeValues.Preserve });
                                                }
                                            else
                                            {
                                                runA.AppendChild(new Text($"-"));
                                            }
                                        }
                                        break;
                                    case 5:
                                        if (e.selectedProjects != null && e.selectedProjects.Count != 0)
                                        {

                                            // Projects - Bulletpointlist:
                                            Paragraph paraB = body.AppendChild(new Paragraph());
                                            Run runB = paraB.AppendChild(new Run());
                                            runB.AppendChild(new Text("Projekte: "));
                                            SpacingBetweenLines sblUl = new SpacingBetweenLines() { After = "0" };
                                            Indentation iUl = new Indentation() { Hanging = "360" };
                                            NumberingProperties npUl = new NumberingProperties(
                                                new NumberingLevelReference() { Val = 0 },
                                                new NumberingId() { Val = 1 }
                                            );
                                            ParagraphProperties ppUnordered = new ParagraphProperties(npUl, sblUl, iUl);
                                            ppUnordered.ParagraphStyleId = new ParagraphStyleId() { Val = "ListParagraph" };
                                            int length = e.selectedProjects.Count();
                                            int j = 0;
                                            Paragraph[] parAr = new Paragraph[length];

                                            foreach (int p in e.selectedProjects.Select(x => x.project).Distinct())
                                            {
                                                parAr[j] = new Paragraph();
                                                parAr[j].ParagraphProperties = new ParagraphProperties(ppUnordered.OuterXml);
                                                parAr[j].Append(new Run(new Text(projectService.ShowProject(p).Title + ": ")));
                                                foreach (var pr in e.selectedProjects.Where(x => x.project == p).ToHashSet())
                                                {
                                                    var last = e.selectedProjects.Where(x => x.project == p).ToHashSet().Last();
                                                    if (!pr.Equals(last)) parAr[j].Append(new Run(new Text(pr.activity + ", ")));      
                                                    else parAr[j].Append(new Run(new Text(pr.activity)));                                                   
                                                }                                                                                                           
                                                body.Append(parAr[j]);
                                                j++;
                                            }
                                            new NumberingRestart();
                                        }
                                        
                                        break;
                                }
                            }
                            AddPageBreak(body);
                        }

                        //================= @@TEAMMIX-KALKULATION@@ =====================//

                        // Create an empty table.
                        Table table = new();
                        // Create a TableProperties object and specify its border information.
                        TableProperties tblProp = new TableProperties(
                            new TableBorders(
                                new TopBorder()
                                {
                                    Val =
                                    new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                                    Size = 8
                                },
                                new BottomBorder()
                                {
                                    Val =
                                    new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                                    Size = 8
                                },
                                new LeftBorder()
                                {
                                    Val =
                                    new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                                    Size = 8
                                },
                                new RightBorder()
                                {
                                    Val =
                                    new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                                    Size = 8
                                },
                                new InsideHorizontalBorder()
                                {
                                    Val =
                                    new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                                    Size = 8
                                },
                                new InsideVerticalBorder()
                                {
                                    Val =
                                    new EnumValue<BorderValues>(BorderValues.BasicThinLines),
                                    Size = 8
                                }
                            )
                        );

                        // Append the TableProperties object to the empty table.
                        table.AppendChild<TableProperties>(tblProp);

                        //5x5 table:

                        // Create a row.
                        TableRow tr1 = new TableRow();

                        // Create 5 cells.
                        TableCell tc1 = new TableCell();
                        tc1.Append(new Paragraph(new Run(new Text($"Titel: {o.Title}"))));
                        tc1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        TableCell tc2 = new TableCell(new Paragraph(new Run(new Text(""))));
                        tc2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        TableCell tc3 = new TableCell(new Paragraph(new Run(new Text(""))));
                        tc3.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        TableCell tc4 = new TableCell(new Paragraph(new Run(new Text(""))));
                        tc4.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        TableCell tc5 = new TableCell();
                        tc5.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc5.Append(new Paragraph(new Run(new Text($"Laufzeit {(o.End - o.Start).Days} Tage"))));

                        tr1.Append(tc1, tc2, tc3, tc4, tc5);
                        table.Append(tr1);

                        // Create another row.
                        TableRow tr2 = new TableRow();

                        // Create 5 cells.
                        TableCell tc6 = new TableCell();
                        tc6.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc6.Append(new Paragraph(new Run(new Text($"Rolle, RCL"))));
                        TableCell tc7 = new TableCell();
                        tc7.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc7.Append(new Paragraph(new Run(new Text($"Rabatt"))));
                        TableCell tc8 = new TableCell();
                        tc8.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc8.Append(new Paragraph(new Run(new Text($"Sätze"))));
                        TableCell tc9 = new TableCell();
                        tc9.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc9.Append(new Paragraph(new Run(new Text($"Kapazität"))));
                        TableCell tcA = new TableCell();
                        tcA.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcA.Append(new Paragraph(new Run(new Text($"Zeit und Kosten"))));

                        tr2.Append(tc6, tc7, tc8, tc9, tcA);
                        table.Append(tr2);

                        
                        int cntDays = 0;
                        double cost = 0;
                        double feePt = 0;

                        foreach (Employee e in o.participants)
                        {
                           // Create a row.
                           TableRow trx = new TableRow();

                            TableCell tca = new TableCell();
                            tca.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                            tca.Append(new Paragraph(new Run(new Text($"{e.offerRole}"))));
                            tca.Append(new Paragraph(new Run(new Text($"{e.offerRCL}"))));

                            TableCell tcb = new TableCell();
                            tcb.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                            if (e.discount != 0)
                            {
                                Paragraph para = tcb.AppendChild(new Paragraph());
                                Run run = para.AppendChild(new Run());
                                RunProperties runProperties = run.AppendChild(new RunProperties());
                                Strike strike = new Strike();
                                strike.Val = true;
                                runProperties.AppendChild(strike);
                                run.AppendChild(new Text($"{(e.offerWage * e.hoursPerDay)}€/PT"));
                                tcb.Append(new Paragraph(new Run(new Text($"{e.discount}%"))));
                            }
                            else tcb.Append(new Paragraph(new Run(new Text(""))));

                            TableCell tcc = new TableCell();
                            tcc.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                            tcc.Append(new Paragraph(new Run(new Text($"{((e.offerWage - (e.offerWage * e.discount * 1 / 100)) * e.hoursPerDay)}€/PT"))));
                            tcc.Append(new Paragraph(new Run(new Text($"{e.offerWage}€/h"))));

                            TableCell tcd = new TableCell();
                            tcd.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                            tcd.Append(new Paragraph(new Run(new Text($"{e.hoursPerDay}Std/Tag"))));
                            tcd.Append(new Paragraph(new Run(new Text($"{e.daysPerRun}Tage/Projektlaufzeit"))));

                            TableCell tce = new TableCell();
                            tce.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                            tce.Append(new Paragraph(new Run(new Text($"Kosten: {(e.offerWage - (e.discount * (1 / 100) * e.offerWage)) * e.hoursPerDay * e.daysPerRun}€"))));

                            trx.Append(tca, tcb, tcc, tcd, tce);
                            table.Append(trx);

                            feePt += (e.offerWage - (e.offerWage * e.discount * 1 / 100)) * e.hoursPerDay;
                            cost += (e.offerWage - (e.discount / 100 * e.offerWage)) * e.hoursPerDay * e.daysPerRun;
                            cntDays += e.daysPerRun;
                        }


                        TableRow tr_ = new TableRow();
                        TableCell tc_1 = new TableCell();
                        tc_1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc_1.Append(new Paragraph(new Run(new Text(""))));
                        TableCell tc_2 = new TableCell();
                        tc_2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc_2.Append(new Paragraph(new Run(new Text(""))));
                        TableCell tc_3 = new TableCell();
                        tc_3.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc_3.Append(new Paragraph(new Run(new Text(""))));
                        TableCell tc_4 = new TableCell();
                        tc_4.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc_4.Append(new Paragraph(new Run(new Text(""))));
                        TableCell tc_5 = new TableCell();
                        tc_5.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tc_5.Append(new Paragraph(new Run(new Text(""))));

                        tr_.Append(tc_1, tc_2, tc_3, tc_4, tc_5);
                        table.Append(tr_);



                        TableRow tr4 = new TableRow();
                        TableCell tcG = new TableCell();
                        tcG.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcG.Append(new Paragraph(new Run(new Text($"Gesamt:"))));
                        TableCell tcH = new TableCell();
                        tcH.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcH.Append(new Paragraph(new Run(new Text($""))));
                        TableCell tcI = new TableCell();
                        tcI.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcI.Append(new Paragraph(new Run(new Text($"Ø {feePt/o.participants.Count}"))));
                        TableCell tcJ = new TableCell();
                        tcJ.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcJ.Append(new Paragraph(new Run(new Text($""))));
                        TableCell tcK = new TableCell();
                        tcK.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcK.Append(new Paragraph(new Run(new Text(cntDays + " PT"))));
                        Paragraph paraF = tcK.AppendChild(new Paragraph());
                        Run runF = paraF.AppendChild(new Run());
                        RunProperties runPropertiesF = runF.AppendChild(new RunProperties());
                        Bold boldF = new Bold();
                        Color colorF = new Color() { Val = "000000" }; //black (val in hex)
                        runPropertiesF.AppendChild(boldF);
                        runPropertiesF.AppendChild(colorF);
                        runF.AppendChild(new Text($"{cost} €"));

                        tr4.Append(tcG, tcH, tcI, tcJ, tcK);
                        table.Append(tr4);

                        TableRow tr3 = new TableRow();
                        TableCell tcB = new TableCell();
                        tcB.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcB.Append(new Paragraph(new Run(new Text($"Listenpreis"))));
                        tcB.Append(new Paragraph(new Run(new Text($"Summe der Rabatte"))));
                        TableCell tcC = new TableCell();
                        tcC.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcC.Append(new Paragraph(new Run(new Text($""))));
                        TableCell tcD = new TableCell();
                        tcD.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcD.Append(new Paragraph(new Run(new Text($""))));
                        TableCell tcE = new TableCell();
                        tcE.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        tcE.Append(new Paragraph(new Run(new Text($""))));
                        TableCell tcF = new TableCell();
                        tcF.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = "2400" }));
                        double discounts = 0;
                        foreach (Employee e in o.participants)
                        {
                            discounts += e.offerWage * e.hoursPerDay - (e.offerWage - (e.offerWage * e.discount / 100)) * e.hoursPerDay;
                        }
                        tcF.Append(new Paragraph(new Run(new Text($"{cost + discounts} €"))));                    
                        tcF.Append(new Paragraph(new Run(new Text($"-{Math.Round(discounts)} €"))));

                        tr3.Append(tcB, tcC, tcD, tcE, tcF);
                        table.Append(tr3);

                        // Append the table to the document.
                        body.Append(table);


                        //========== @@(Graphische) Gegenüberstellung@@ ==================//

                    }
                    // Returns the stream.
                    mainPart.Document.Save();
                    doc.Close(); // Closes the handle explicitly.
                    templateStream.Position = 0;
                    return templateStream.ToArray();
                }
            }
        }

        public void AddPageBreak(Body body)
        {
            Paragraph PageBreakParagraph = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
            body.Append(PageBreakParagraph);
        }

        // Refers to the Image - https://docs.microsoft.com/en-us/office/open-xml/how-to-insert-a-picture-into-a-word-processing-document?redirectedfrom=MSDN
        public void AddImageToBody(Body body, string relationshipId)
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
            body.AppendChild(new Paragraph(new Run(element)));
        }

        public byte[] Anything(Object e)
        {
            byte[] templateBytes = XCV.Properties.Resources.Einzelprofil; // Adapt for Template.

            using (MemoryStream templateStream = new MemoryStream())
            {
                templateStream.Write(templateBytes, 0, (int)templateBytes.Length);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(templateStream, true))
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
                    run.AppendChild(new Text($"First Paragraph") { Space = SpaceProcessingModeValues.Preserve });
                    run.AppendChild(new Break());

                    //==================================================================================//

                    // Returns the stream.
                    mainPart.Document.Save();
                    doc.Close(); // Closes the handle explicitly.
                    templateStream.Position = 0;
                    return templateStream.ToArray();
                }
            }
        }

      
        //class
    }
    //ns
}


