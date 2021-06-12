using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Team14.Data
{
    public class Employee
    {
        public string _image;

        [MaxLength(10, ErrorMessage = "Die Personalnummer ist zu lang!")]
        public string PersoNumber { get; set; }

        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z]).+$", ErrorMessage = "ihr Passwort muss Groß und klein")] scheint bei "arnold" fehlerhaft zu funktionieren
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Das Passwort besteht nur aus Buchstaben und Zahlen!")]
        public string Password { get; set; }

        public IEnumerable<AccessRole> Roles { get; set; }

        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Tippe eine Namen ein")] -> Fehlernachricht bei Login, da Required und das gesamt Model überprüft wird...(funktioniert trotzdem ist blos unschön)
        public string LastName { get; set; }

        public string Image
        {
            get => _image;
            set
            {
                if (!!Path.IsPathRooted(value))
                    throw new ArgumentException(" Das Bild muss auf dem Server gespeicher werden");
                _image = value;
            }
        }

        // Bis der Mitarbeiter sich für ein Rate Card Level entschieden hat bleibet es NULL
        [Range(1, 7, ErrorMessage = "Das RCL muss zwischen 1 und 7 liegen")]
        public int? RCL { get; set; }


        public PersSkillTree SkillTree { get; set; }

    }



    public enum AccessRole
    {
        Sales,
        Admin,
        SuperAdmin
    }
}
