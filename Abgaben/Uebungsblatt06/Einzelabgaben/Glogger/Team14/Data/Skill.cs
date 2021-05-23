using System.ComponentModel.DataAnnotations;


namespace Team14.Data
{
    public class Skill
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(40, ErrorMessage = "Der Name ist zu lang")]
        [SkillNameConventionAttribut]
        public string Name { get; set; }
        public SkillCatgeory Skilltype { get; set; }

        public enum SkillCatgeory
        {
            Hardskill,
            Softskill,
        }
        public SprachenUndFrameworks SprachenundFrameworks { get; set; }
        public Tools Tools { get; set; }
        public MethodenUndProzesse MethodenundProzesse { get; set; }
        public string[] Datenbanken { get; set; }
        public BetriebssystemeCloudPlattformenHardware BetriebssystemeCloudPlattformenHardware { get; set; }
        public string[] SchnittstellenundProtokolle { get; set; }
        public string[] Expertise { get; set; }
    
}

    public class SprachenUndFrameworks
    {
        public string[] Sprachen { get; set; }
        public string[] Frameworks { get; set; }
        public string[] Bibliotheken { get; set; }
    }

    public class Tools
    {
        public string[] ProjektmanagementTools { get; set; }
        public string[] KonzeptundIdeenmanagement { get; set; }
        public string[] CICD { get; set; }
        public string[] IDEs { get; set; }
        public string[] Versionsverwaltung { get; set; }
        public string[] DigitalesDesign { get; set; }
        public string[] _3DVisualisierung { get; set; }
        public string[] Virtualisierung { get; set; }
        public string[] ServerundInfrastrukturManagement { get; set; }
        public string[] QA { get; set; }
        public string[] CAD { get; set; }
        public string[] PackageManagement { get; set; }
        public string[] BuildManagement { get; set; }
        public string[] Datenvisualisierunganalyse { get; set; }
        public string[] Simulation { get; set; }
        public string[] OptimierungtechnischeMathematik { get; set; }
        public string[] Datenbankverwaltung { get; set; }
    }

    public class MethodenUndProzesse
    {
        public string[] AgileMethoden { get; set; }
        public string[] AgileScaling { get; set; }
        public string[] UIUX { get; set; }
        public string[] SoftwareEngineering { get; set; }
        public string[] Modellierung { get; set; }
        public string[] Projektmanagement { get; set; }
    }

    public class BetriebssystemeCloudPlattformenHardware
    {
        public string[] Betriebssysteme { get; set; }
        public string[] Cloud { get; set; }
        public string[] Hardware { get; set; }
        public string[] Plattformen { get; set; }
    }

}
