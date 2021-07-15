using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace XCV.Data
{
    public class Employee
    {
        [MinLength(2, ErrorMessage = "Die Personalnummer muss länger als 2 Zeichen sein."),
        MaxLength(6, ErrorMessage = "Die Personalnummer darf nicht länger als 6 Zeichen sein."),
        RegularExpression(@"[a-zA-Z0-9_\-,.]*", ErrorMessage = "PersoNr. darf nur Buchstaben Zahlen oder -_,. enthalten.")] //some SQL might use it directly, so watch for (')
        public string PersoNumber { get; set; } = "";

        public string Password { get; set; } = "";

        public ISet<AccessRole> AcRoles { get; set; } = new SortedSet<AccessRole>();


        [Required(ErrorMessage = "Es muss eine Eingabe zum Vorname gemacht werden."),
        MaxLength(20, ErrorMessage = "Um Gestaltung und Marketing zu opimieren, \n" +
                                        "lässt das System keine Vornamen über 20 Zeichen zu.")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Es muss eine Eingabe zum Nachname gemacht werden."),
        MaxLength(20, ErrorMessage = "Um Gestaltung und Marketing zu optimieren, \n" +
                                        "lässt das System keine Nachname über 20 Zeichen zu")]
        public string LastName { get; set; } = "";



        [MaxLength(202, ErrorMessage = "Beschreibung nicht über 200 Zeichen")]
        public string Description { get; set; } = "";
        public string Image { get; set; } = "musterPic.png";

        // null, until explicitly set
        [Range(0, 8, ErrorMessage = "Das RCL muss zwischen 1 und 8 liegen")]
        public int RCL { get; set; }
        public DateTime? Experience { get; set; }

        public DateTime EmployedSince { get; init; }

        public ISet<Role> Roles { get; set; } = new SortedSet<Role>();
        public ISet<Field> Fields { get; set; } = new SortedSet<Field>();
        public ISet<Language> Languages { get; set; } = new SortedSet<Language>();
        public ISet<Skill> Abilities { get; set; } = new SortedSet<Skill>();

        public List<(int project, string activity)> Projects { get; set; } = new();


        public bool MadeFirstChangesOnProfile = false;

        [MaxLength(50, ErrorMessage = "Der Name der Rolle darf 50 Zeichen nicht überschreiten.")]
        public string offerRole { get; set; }
        [Range(0, 8, ErrorMessage = "Das RCL muss zwischen 1 und 8 liegen")]
        public int offerRCL { get; set; }
        public float offerWage { get; set; }
        public int hoursPerDay { get; set; }
        public int daysPerRun { get; set; }
        public int discount { get; set; }

        public override string ToString()
        {
            var pro = string.Join(", \n           ", Projects.Select(x => $"im Pro. Id:{x.project} :   ({x.activity})"));
            var ro = "keine Rollen";
            if (Roles.FirstOrDefault() != null)
                ro = string.Join(", \n           ", Roles.Select(x => $"(L{x.RCL}-{x.Name}:{x.Wage}€)"));

            var ab = "keine Skills";
            if (Abilities.FirstOrDefault() != null)
            {
                ab = $"\n {Abilities.Count}:Skills in  {Abilities.First().Category.GetRoot()}";
            }

            var result = $"{PersoNumber}: \"{FirstName}\" \"{LastName}\" Erfahrung{Experience}:\n"
                         + $"AcRoles:   {string.Join(", ", AcRoles)}\n\n"
                         + $"Projekte:  {pro}\n"
                         + $"Roles:     {ro}  \n\n"
                         + $"Fields:    {string.Join(", ", Fields)} \n"
                         + $"Languages: {string.Join(", ", Languages)} \n"
                         + $"{ab}";

            return result;
        }
    }
}
