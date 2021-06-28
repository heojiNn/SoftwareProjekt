using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Employee
    {
        public string PersoNumber { get; set; } = "";
        public string Password { get; set; } = "";

        public ISet<AccessRole> AcRoles { get; set; } = new SortedSet<AccessRole>();


        [Required(ErrorMessage = "Der Vorname darf nicht leer sein"),
        MaxLength(10, ErrorMessage = "Die Vorname ist zu lang")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Der Nachname darf nicht leer sein"),
        MaxLength(10, ErrorMessage = "Die Nachname ist zu lang")]
        public string LastName { get; set; } = "";



        public string Description { get; set; } = "";
        public string Image { get; set; } = "musterPic.png";


        // bis der Mitarbeiter sich für ein Rate Card Level entschieden hat bleibet es NULL
        [Range(1, 7, ErrorMessage = "Das RCL muss zwischen 1 und 7 liegen")]
        public int? RCL { get; set; }


        //private TimeSpan _expirience = new();
        public float Expirience { get; set; }

        private DateTime _workingSince;
        public DateTime WorkingSince
        {
            get => _workingSince;
            init { _workingSince = value.Date; }
        }
        public bool MadeFirstChangesOnProfile = false;


        public ISet<Role> Roles { get; set; } = new SortedSet<Role>();
        public ISet<Field> Fields { get; set; } = new SortedSet<Field>();
        public ISet<Language> Languages { get; set; } = new SortedSet<Language>();
        public ISet<Skill> Abilities { get; set; } = new SortedSet<Skill>();
        public ISet<Project> Projects { get; set; } = new SortedSet<Project>();



       
        //public Dictionary<int, string> DocumentSettingsList { get; set; } = new Dictionary<int, string>();





        public override string ToString() => $"{PersoNumber}heißt \"{FirstName}\" \"{LastName}\"";
    }
}
