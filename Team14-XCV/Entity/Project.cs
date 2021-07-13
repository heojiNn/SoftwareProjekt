using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace XCV.Data
{
    public class Project
    {
        public int Id { get; set; } = 0;

        [Required(ErrorMessage = "Der Titel darf nicht leer sein."),
        MaxLength(30, ErrorMessage = "Der Titel darf nicht länger als 30 Zeichen sein.")]
        public string Title { get; set; } = "";

        [MaxLength(1000, ErrorMessage = "Die Beschreibung darf nicht länger als 1000 Zeichen sein.")]
        public string Description { get; set; } = "";
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;

        public List<string> Purpose { get; set; } = new();

        public string Field { get; set; }


        public Dictionary<string, (List<string> persNr, IEnumerable<Skill> requirements)> Activities { get; set; } = new();

    }
}
